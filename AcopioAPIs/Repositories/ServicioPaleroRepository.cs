using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ServicioPaleroRepository : IServicioPalero
    {
        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;


        public ServicioPaleroRepository(DbacopioContext dbacopioContext, IConfiguration configuration)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
        }

        public async Task<List<ServicioResultDto>> ListServiciosPalero(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            try
            {
                return await GetServiciosPaleroQuery(
                    fechaDesde, fechaHasta, carguilloId, estadoId, null)
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ServicioDto>> GetServicioPalero(int servicioPaleroId)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_ServicioPaleroGetById", new { ServicioPaleroId = servicioPaleroId },
                    commandType: CommandType.StoredProcedure);

                var master = multi.Read<ServicioDto>().FirstOrDefault();
                var detail = multi.Read<ServicioDetailDto>().AsList();
                if (master == null) throw new KeyNotFoundException("Servicio Palero no encontrado");
                master.ServicioDetails = detail;
                return ReturnData(master, "Servicio Palero recuperado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ServicioResultDto>> SaveServicioPalero(ServicioPaleroInsertDto servicioInsertDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();

            try
            {
                if (servicioInsertDto == null)
                    throw new Exception("No se enviaron datos para guardar el Servicio Palero");
                if (servicioInsertDto.ServicioDetail.Count == 0)
                    throw new Exception("No se enviaron tickets para guardar el Servicio Palero");
                var carguillo = await _dbacopioContext.Carguillos.FindAsync(servicioInsertDto.CarguilloId)
                    ?? throw new Exception("Carguillo no encontrado");
                var estado = await GetEstado(
                    "activo", _dbacopioContext.ServicioTransporteEstados,
                    "ServicioTransporteEstadoDescripcion")
                    ?? throw new Exception("Estado del Servicio Palero no encontrado");

                var servicio = new ServicioPalero
                {
                    ServicioPaleroFecha = servicioInsertDto.ServicioFecha,
                    CarguilloId = servicioInsertDto.CarguilloId,
                    ServicioPaleroPrecio = servicioInsertDto.ServicioPrecio,
                    ServicioPaleroPesoBruto = servicioInsertDto.ServicioPesoBruto,
                    ServicioPaleroTotal = servicioInsertDto.ServicioTotal,
                    ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId,
                    UserCreatedAt = servicioInsertDto.UserCreatedAt,
                    UserCreatedName = servicioInsertDto.UserCreatedName
                };
                foreach (var item in servicioInsertDto.ServicioDetail)
                {
                    var dto = await _dbacopioContext.Tickets.FindAsync(item.TicketId)
                        ?? throw new Exception("Ticket no encontrado");
                    var detail = new ServicioPaleroDetalle
                    {
                        TicketId = item.TicketId,
                        ServicioPaleroDetalleStatus = true,
                        UserCreatedAt = servicioInsertDto.UserCreatedAt,
                        UserCreatedName = servicioInsertDto.UserCreatedName
                    };
                    servicio.ServicioPaleroDetalles.Add(detail);
                    dto.EnServicioPalero = true;
                }

                _dbacopioContext.Add(servicio);
                await _dbacopioContext.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = await GetServiciosPaleroQuery(
                    null, null, null, null, servicio.ServicioPaleroId)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception("");
                return ReturnData(response, "Servicio Palero guardado");
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();
                throw;
            }        
        }

        public async Task<ResultDto<ServicioResultDto>> UpdateServicioPalero(ServicioUpdateDto servicioUpdateDto)
        {
            try
            {
                var existing = await _dbacopioContext.ServicioPaleros
                    .FindAsync(servicioUpdateDto.ServicioId)
                    ?? throw new Exception("Servicio  no encontrado");
                var estado = await GetEstado(
                    "activo", _dbacopioContext.ServicioTransporteEstados,
                    "ServicioTransporteEstadoDescripcion")
                    ?? throw new Exception("Estado del Servicio Palero no encontrado");
                if (estado.ServicioTransporteEstadoDescripcion != servicioUpdateDto.ServicioEstadoDescripcion)
                    throw new Exception("El Servicio Palero no se encuentra activo");
                existing.ServicioPaleroPrecio = servicioUpdateDto.ServicioPrecio;
                existing.ServicioPaleroTotal = servicioUpdateDto.ServicioTotal;
                existing.UserModifiedAt = servicioUpdateDto.UserModifiedAt;
                existing.UserModifiedName = servicioUpdateDto.UserModifiedName;
                await _dbacopioContext.SaveChangesAsync();
                
                var response = await GetServiciosPaleroQuery(
                    null, null, null, null, servicioUpdateDto.ServicioId)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception("");
                return ReturnData(response, "Servicio Palero actualizado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<int>> DeleteServicioPalero(ServicioDeleteDto servicioDeleteDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();

            try
            {
                var estado = await GetEstado(
                    "anulado", _dbacopioContext.ServicioTransporteEstados,
                    "ServicioTransporteEstadoDescripcion")
                    ?? throw new Exception("Estado del Servicio Palero no encontrado");
                var existing = await _dbacopioContext.ServicioPaleros
                    .Include(c => c.ServicioPaleroDetalles)
                    .FirstOrDefaultAsync(c => c.ServicioPaleroId== servicioDeleteDto.ServicioId)
                    ?? throw new Exception("Servicio  no encontrado");
                var estadoTicketPagado = await GetEstado(
                    "pagado", _dbacopioContext.TicketEstados, "TicketEstadoDescripcion")
                    ?? throw new Exception("Estado de Ticket Pagado no encontrado");

                foreach (var item in existing.ServicioPaleroDetalles)
                {
                    var ticket = await _dbacopioContext.Tickets.FindAsync(item.TicketId)
                        ?? throw new Exception("Ticket no encontrado");
                    ticket.EnServicioPalero = null;


                    item.ServicioPaleroDetalleStatus = false;
                    item.UserModifiedAt = servicioDeleteDto.UserModifiedAt;
                    item.UserModifiedName = servicioDeleteDto.UserModifiedName;
                }
                existing.ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId;
                existing.UserModifiedAt = servicioDeleteDto.UserModifiedAt;
                existing.UserModifiedName = servicioDeleteDto.UserModifiedName;
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ReturnData(servicioDeleteDto.ServicioId, "Servicio Palero eliminado");
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();

                throw;
            }
        }

        private static ResultDto<T> ReturnData<T>(T? data, string message)
        {
            return new ResultDto<T>
            {
                Result = true,
                ErrorMessage = message,
                Data = data
            };
        }
        private IQueryable<ServicioResultDto> GetServiciosPaleroQuery(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId, int? servicioPaleroId)
        {
            return from servicio in _dbacopioContext.ServicioPaleros
                   join estado in _dbacopioContext.ServicioTransporteEstados
                       on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                   join carguillo in _dbacopioContext.Carguillos
                       on servicio.CarguilloId equals carguillo.CarguilloId
                   where (fechaDesde == null || servicio.ServicioPaleroFecha >= fechaDesde)
                   && (fechaHasta == null || servicio.ServicioPaleroFecha <= fechaHasta)
                   && (carguilloId == null || servicio.CarguilloId == carguilloId)
                   && (estadoId == null || servicio.ServicioTransporteEstadoId == estadoId)
                   && (servicioPaleroId == null || servicio.ServicioPaleroId == servicioPaleroId)
                   select new ServicioResultDto
                   {
                       ServicioId = servicio.ServicioPaleroId,
                       ServicioFecha = servicio.ServicioPaleroFecha,
                       ServicioCarguilloTitular = carguillo.CarguilloTitular,
                       ServicioPrecio = servicio.ServicioPaleroPrecio,
                       ServicioPesoBruto = servicio.ServicioPaleroPesoBruto,
                       ServicioTotal = servicio.ServicioPaleroTotal,
                       ServicioEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                   };
        }
        private static async Task<T?> GetEstado<T>(string descripcion, DbSet<T> estados, string columna) where T : class
        {
            return await estados
                .FirstOrDefaultAsync(e => EF.Property<string>(e, columna).Equals(descripcion));
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
