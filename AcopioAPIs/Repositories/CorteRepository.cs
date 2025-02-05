using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CorteRepository : ICorte
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public CorteRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<CorteEstadoDto>> GetCorteEstados()
        {
            var query = from tipo in _context.CorteEstados
                        select new CorteEstadoDto
                        {
                            CorteTipoId = tipo.CorteEstadoId,
                            CorteDescripcion = tipo.CorteEstadoDescripcion
                        };
            return await query.ToListAsync();
        }
        public async Task<List<CorteResultDto>> GetAll(DateOnly? fechaDesde,
            DateOnly? fechaHasta, int? tierraId, int? estadoId)
        {
            try
            {
                IQueryable<CorteResultDto> query = GetCorteResults(
                    fechaDesde, fechaHasta, tierraId, estadoId, null);
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CorteDto> GetById(int id)
        {
            try
            {
                using (var conexion = GetConnection())
                {
                    using (var multi = await conexion.QueryMultipleAsync(
                        "usp_CorteGetById", 
                        new { CorteId = id }, 
                        commandType: CommandType.StoredProcedure))
                    {
                        var master = multi.Read<CorteDto>().FirstOrDefault();
                        var details = multi.Read<CorteDetailDto>().AsList();
                        if (master == null)
                        {
                            throw new Exception("No se encontró el Corte");
                        }
                        master.CorteDetail = details;
                        return master;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<CorteResultDto>> Save(CorteInsertDto corteInsertDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (corteInsertDto == null)
                    throw new Exception("No se enviaron datos para guardar el corte");
                if(corteInsertDto.CorteDetail.Count == 0)
                    throw new Exception("No se enviaron tickets para guardar el corte");
                var estadosCorte = from est in _context.CorteEstados
                                   where est.CorteEstadoDescripcion.Equals("activo")
                                   select est;
                var estadoCorte = await estadosCorte.FirstOrDefaultAsync()
                    ?? throw new Exception("Estados del Corte no encontrados");
                var estadosTicket = from est in _context.TicketEstados
                              where est.TicketEstadoDescripcion.Equals("archivado")
                              select est;
                var estadoTicket = await estadosTicket.FirstOrDefaultAsync()
                    ?? throw new Exception("Estados del Ticket no encontrados");
                
                foreach (var ticket in corteInsertDto.CorteDetail)
                {
                    var dto = await _context.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId)
                            ?? throw new Exception("Cosecha no encontrada");
                    var historyTicket = new TicketHistorial
                    {
                        TicketId = dto.TicketId,
                        TicketIngenio = dto.TicketIngenio,
                        TicketCampo = dto.TicketCampo,
                        TicketViaje = dto.TicketViaje,
                        CarguilloId = dto.CarguilloId,
                        TicketChofer = dto.TicketChofer,
                        TicketFecha = dto.TicketFecha,
                        CarguilloDetalleCamionId = dto.CarguilloDetalleCamionId,
                        TicketCamionPeso = dto.TicketCamionPeso,
                        CarguilloDetalleVehiculoId = dto.CarguilloDetalleVehiculoId,
                        TicketVehiculoPeso = dto.TicketVehiculoPeso,
                        TicketUnidadPeso = dto.TicketUnidadPeso,
                        TicketPesoBruto = dto.TicketPesoBruto,
                        TicketEstadoId = dto.TicketEstadoId,                            
                        UserModifiedAt = corteInsertDto.UserCreatedAt,
                        UserModifiedName = corteInsertDto.UserCreatedName
                    };
                    _context.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = estadoTicket.TicketEstadoId; // Archivado
                    dto.UserModifiedAt = corteInsertDto.UserCreatedAt;
                    dto.UserModifiedName = corteInsertDto.UserCreatedName;
                }
                var corte = new Corte
                {
                    CorteFecha = corteInsertDto.CorteFecha,
                    TierraId = corteInsertDto.TierraId,
                    CortePrecio = corteInsertDto.CortePrecio,
                    CorteEstadoId = estadoCorte.CorteEstadoId, // Activo
                    CortePesoBrutoTotal = corteInsertDto.CortePesoBrutoTotal,
                    CorteTotal = corteInsertDto.CorteTotal,
                    UserCreatedAt = corteInsertDto.UserCreatedAt,
                    UserCreatedName = corteInsertDto.UserCreatedName
                };
                foreach (var item in corteInsertDto.CorteDetail)
                    {
                        var detail = new CorteDetalle
                        {
                            TicketId = item.TicketId,
                            UserCreatedAt = corteInsertDto.UserCreatedAt,
                            UserCreatedName = corteInsertDto.UserCreatedName,
                        };
                        corte.CorteDetalles.Add(detail);
                    }
                _context.Cortes.Add(corte);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                IQueryable<CorteResultDto> query = GetCorteResults(
                    null, null, null, null, corte.CorteId);
                var response = await query.FirstOrDefaultAsync() ??
                        throw new Exception("Corte guardado pero no encontrado.");
                return new ResultDto<CorteResultDto>
                {
                    Result = true,
                    ErrorMessage = "Corte guardado",
                    Data = response
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
        public async Task<ResultDto<CorteResultDto>> Update(CorteUpdateDto corteUpdateDto)
        {
            try
            {
                if (corteUpdateDto == null) 
                    throw new Exception("No se enviaron datos para actualizar el corte");
                var corte = await _context.Cortes.FindAsync(corteUpdateDto.CorteId)
                    ?? throw new Exception("Corte no encontrado");
                var estado = await GetCorteEstado("activo")
                    ?? throw new Exception("Corte Estado Activo no encontrado");
                if (estado.CorteEstadoDescripcion != corteUpdateDto.CorteEstadoDescripcion)
                    throw new Exception("El corte no esta activo");
                corte.CortePrecio = corteUpdateDto.CortePrecio;
                corte.CorteTotal = corteUpdateDto.CorteTotal;
                corte.UserModifiedAt = corteUpdateDto.UserModifiedAt;
                corte.UserModifiedName = corteUpdateDto.UserModifiedName;
                await _context.SaveChangesAsync();
                IQueryable<CorteResultDto> query = GetCorteResults(
                    null, null, null, null, corte.CorteId);
                var response = await query.FirstOrDefaultAsync() ??
                        throw new Exception("Corte actualizado");
                return new ResultDto<CorteResultDto>
                {
                    Result = true,
                    ErrorMessage = "Corte actualizado",
                    Data = response
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<int>> Delete(CorteDeleteDto corteDelete)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var corte = await _context.Cortes
                    .Include(c => c.CorteDetalles)
                    .FirstOrDefaultAsync(c => c.CorteId == corteDelete.CorteId)
                    ?? throw new Exception("Corte no encontrado");
                var estadoCorte = await GetCorteEstado("anulado")//
                    ?? throw new Exception("Estado del Corte Anulado no encontrados");
                if (corte.CorteEstadoId == estadoCorte.CorteEstadoId)
                    throw new Exception("El Corte ya esta anulado");
                var estadoTicket = await GetTicketEstado("activo")
                    ?? throw new Exception("Estado de Ticket Activo no encontrado");
                var estadoTicketArchivado = await GetTicketEstado("archivado")
                    ?? throw new Exception("Estado de Ticket Archivado no encontrado");
                foreach (var item in corte.CorteDetalles)
                {
                    var dto = await _context.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == item.TicketId)
                            ?? throw new Exception("Ticket no encontrada");
                    if (dto.TicketEstadoId != estadoTicketArchivado.TicketEstadoId)
                        throw new Exception("Debe de anular el Servicio Transporte");
                    var historyTicket = new TicketHistorial
                    {
                        TicketId = dto.TicketId,
                        TicketIngenio = dto.TicketIngenio,
                        TicketCampo = dto.TicketCampo,
                        TicketViaje = dto.TicketViaje,
                        CarguilloId = dto.CarguilloId,
                        TicketChofer = dto.TicketChofer,
                        TicketFecha = dto.TicketFecha,
                        CarguilloDetalleCamionId = dto.CarguilloDetalleCamionId,
                        TicketCamionPeso = dto.TicketCamionPeso,
                        CarguilloDetalleVehiculoId = dto.CarguilloDetalleVehiculoId,
                        TicketVehiculoPeso = dto.TicketVehiculoPeso,
                        TicketUnidadPeso = dto.TicketUnidadPeso,
                        TicketPesoBruto = dto.TicketPesoBruto,
                        TicketEstadoId = dto.TicketEstadoId,
                        UserModifiedAt = corteDelete.UserModifiedAt,
                        UserModifiedName = corteDelete.UserModifiedName
                    };
                    _context.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = estadoTicket.TicketEstadoId;
                    dto.UserModifiedAt = corteDelete.UserModifiedAt;
                    dto.UserModifiedName = corteDelete.UserModifiedName;

                    item.CorteDetalleStatus = false;
                    item.UserModifiedAt = corteDelete.UserModifiedAt;
                    item.UserModifiedName = corteDelete.UserModifiedName;
                }
                corte.CorteEstadoId = estadoCorte.CorteEstadoId;
                corte.UserModifiedAt = corteDelete.UserModifiedAt;
                corte.UserModifiedName = corteDelete.UserModifiedName;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultDto<int>
                {
                    Result = true,
                    ErrorMessage = "Corte eliminado",
                    Data = corteDelete.CorteId
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private IQueryable<CorteResultDto> GetCorteResults(DateOnly? fechaDesde,
            DateOnly? fechaHasta, int? tierraId, int? estadoId, int? corteId)
        {
            return from cortes in _context.Cortes
                   join tierra in _context.Tierras
                       on cortes.TierraId equals tierra.TierraId
                    join estado in _context.CorteEstados
                        on cortes.CorteEstadoId equals estado.CorteEstadoId
                   where (tierraId == null || cortes.TierraId == tierraId)
                   && (fechaDesde == null || cortes.CorteFecha >= fechaDesde)
                   && (fechaHasta == null || cortes.CorteFecha <= fechaHasta)
                   && (estadoId == null || cortes.CorteEstadoId == estadoId)
                   && (corteId == null || cortes.CorteId == corteId)
                   select new CorteResultDto
                   {
                       CorteId = cortes.CorteId,
                       CorteFecha = cortes.CorteFecha,
                       TierraUC = tierra.TierraUc,
                       CortePrecio = cortes.CortePrecio,
                       CorteCantidadTicket = cortes.CorteDetalles.Count,
                       CorteEstadoDescripcion = estado.CorteEstadoDescripcion,
                       CortePesoBrutoTotal = cortes.CortePesoBrutoTotal,
                       CorteTotal = cortes.CorteTotal,
                       TierraCampo = tierra.TierraCampo
                   };
        }
        private async Task<TicketEstado?> GetTicketEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _context.TicketEstados
                            where estado.TicketEstadoDescripcion.Equals(estadoDescripcion)
                            select estado;
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<CorteEstado?> GetCorteEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _context.CorteEstados
                            where estado.CorteEstadoDescripcion.Equals(estadoDescripcion)
                            select estado;
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private SqlConnection GetConnection() 
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
