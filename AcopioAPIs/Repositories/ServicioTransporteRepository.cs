using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Pago;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Models;
using AcopioAPIs.Utils;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ServicioTransporteRepository : IServicioTransporte
    {
        private readonly DbacopioContext _acopioContext;
        private readonly IConfiguration _configuration;
        private const string TipoReferencia = "Transporte";

        public ServicioTransporteRepository(DbacopioContext acopioContext, IConfiguration configuration)
        {
            _acopioContext = acopioContext;
            _configuration = configuration;
        }

        public async Task<List<EstadoResultDto>> ListEstados()
        {
            try
            {
                var query = from estado in _acopioContext.ServicioTransporteEstados
                            where estado.ServicioTransporteEstadoStatus == true
                            select new EstadoResultDto
                            {
                                EstadoId = estado.ServicioTransporteEstadoId,
                                EstadoDescripcion = estado.ServicioTransporteEstadoDescripcion,
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ServicioResultDto>> ListServiciosTransporte(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            try
            {
                return await GetServiciosTransporteQuery(
                    fechaDesde, fechaHasta, carguilloId, estadoId, null)
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ServicioDto> GetServicioTransporte(int servicioTransporteId)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_ServicioTransporteGetById", new { ServicioTransporteId = servicioTransporteId },
                    commandType: CommandType.StoredProcedure);

                var master = multi.Read<ServicioDto>().FirstOrDefault();
                var detail = multi.Read<ServicioDetailDto>().AsList();
                var detailPago = multi.Read<PagoResultDto>().AsList();
                if (master == null) throw new Exception("Servicio Transporte no encontrado");
                master.ServicioDetails = detail;
                master.DetallePagos = detailPago;
                return master;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ServicioResultDto>> SaveServicioTransporte(ServicioInsertDto servicioTransporteInsertDto)
        {
            using var transaction = await _acopioContext.Database.BeginTransactionAsync();
                
            try
            {
                if (servicioTransporteInsertDto == null)
                    throw new Exception("No se enviaron datos para guardar el Servicio Transporte");
                if (servicioTransporteInsertDto.ServicioDetail.Count == 0)
                    throw new Exception("No se enviaron tickets para guardar el Servicio Transporte");

                var estado = await GetServicioTransporteEstado("activo") 
                    ?? throw new Exception("Estado del Servicio Transporte no encontrado");
                var carguillo = await _acopioContext.Carguillos.FindAsync(servicioTransporteInsertDto.CarguilloId)
                    ?? throw new Exception("Carguillo no encontrado");
                var ticketEstados = await GetTicketEstado("liquidación")
                    ?? throw new Exception("Estado de Tickets no encontrado");
                foreach (var ticket in servicioTransporteInsertDto.ServicioDetail)
                {
                    var dto = await _acopioContext.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId)
                            ?? throw new Exception("Ticket no encontrado");
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
                        UserModifiedAt = servicioTransporteInsertDto.UserCreatedAt,
                        UserModifiedName = servicioTransporteInsertDto.UserCreatedName
                    };
                    _acopioContext.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = ticketEstados.TicketEstadoId;
                    dto.UserModifiedAt = servicioTransporteInsertDto.UserCreatedAt;
                    dto.UserModifiedName = servicioTransporteInsertDto.UserCreatedName;
                }
                var servicio = new ServicioTransporte
                {
                    ServicioTransporteFecha = servicioTransporteInsertDto.ServicioFecha,
                    CarguilloId = servicioTransporteInsertDto.CarguilloId,
                    ServicioTransportePrecio = servicioTransporteInsertDto.ServicioPrecio,
                    ServicioTransportePesoBruto = servicioTransporteInsertDto.ServicioPesoBruto,
                    ServicioTransporteTotal = servicioTransporteInsertDto.ServicioTotal,
                    ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId,
                    ServicioTransportePendientePagar = servicioTransporteInsertDto.ServicioPendientePagar,
                    ServicioTransportePagado = servicioTransporteInsertDto.ServicioPagado,                    
                    UserCreatedAt = servicioTransporteInsertDto.UserCreatedAt,
                    UserCreatedName = servicioTransporteInsertDto.UserCreatedName
                };
                foreach (var item in servicioTransporteInsertDto.ServicioDetail)
                {
                    var detail = new ServicioTransporteDetalle
                    {
                        TicketId = item.TicketId,
                        ServicioTransporteDetalleStatus = true,
                        UserCreatedAt = servicioTransporteInsertDto.UserCreatedAt,
                        UserCreatedName = servicioTransporteInsertDto.UserCreatedName
                    };
                    servicio.ServicioTransporteDetalles.Add(detail);
                }

                _acopioContext.ServicioTransportes.Add(servicio);
                await _acopioContext.SaveChangesAsync();

                var pagoService = new PagoRepository(_acopioContext);
                foreach (var item in servicioTransporteInsertDto.DetallePagos)
                {
                    await pagoService.CrearPagoAsync(
                        servicio.ServicioTransporteId, TipoReferencia, servicioTransporteInsertDto.UserCreatedAt,
                        servicioTransporteInsertDto.UserCreatedName, item);
                }

                await _acopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var response = await GetServiciosTransporteQuery(
                    null, null, null, null, servicio.ServicioTransporteId)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception("");
                return ResponseHelper.ReturnData(response, true, "Servicio Transporte guardado");
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResultDto<ServicioResultDto>> UpdateServicioTransporte(ServicioUpdateDto servicioTransporteUpdateDto)
        {
            try
            {
                var existing = await _acopioContext.ServicioTransportes.FindAsync(servicioTransporteUpdateDto.ServicioId)
                    ?? throw new Exception("Servicio Transporte no encontrado");
                var estado = await GetServicioTransporteEstado("activo")
                    ?? throw new Exception("Estado del Servicio Transporte no encontrado");
                if (estado.ServicioTransporteEstadoDescripcion != servicioTransporteUpdateDto.ServicioEstadoDescripcion)
                    throw new Exception("El Servicio Transporte no se encuentra activo");
                var estadoPagado = await GetServicioTransporteEstado("pagado")
                    ?? throw new Exception("Estado Pagado del Servicio Transporte no encontrado");

                existing.ServicioTransportePrecio = servicioTransporteUpdateDto.ServicioPrecio;
                existing.ServicioTransporteTotal = servicioTransporteUpdateDto.ServicioTotal;
                existing.ServicioTransportePendientePagar = servicioTransporteUpdateDto.ServicioPendientePagar;
                existing.ServicioTransportePagado = servicioTransporteUpdateDto.ServicioPagado;
                if (servicioTransporteUpdateDto.ServicioPendientePagar == 0
                    && existing.ServicioTransporteTotal == servicioTransporteUpdateDto.ServicioPagado)
                    existing.ServicioTransporteEstadoId = estadoPagado.ServicioTransporteEstadoId;
                existing.UserModifiedAt = servicioTransporteUpdateDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteUpdateDto.UserModifiedName;

                var pagoService = new PagoRepository(_acopioContext);
                foreach (var item in servicioTransporteUpdateDto.DetallePagos)
                {
                    await pagoService.CrearPagoAsync(
                        existing.ServicioTransporteId, TipoReferencia, servicioTransporteUpdateDto.UserModifiedAt,
                        servicioTransporteUpdateDto.UserModifiedName, item);
                }
                await _acopioContext.SaveChangesAsync();

                var response = await GetServiciosTransporteQuery(
                    null, null, null, null, servicioTransporteUpdateDto.ServicioId)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception("");
                return ResponseHelper.ReturnData(response, true, "Servicio Transporte actualizado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<int>> DeleteServicioTransporte(ServicioDeleteDto servicioTransporteDeleteDto)
        {
            using var transaction = await _acopioContext.Database.BeginTransactionAsync();

            try
            {
                var estadoTicket = await GetTicketEstado("archivado")
                    ?? throw new Exception("Estado de Tickets Archivado no encontrado");
                var estadoTicketTesoreria = await GetTicketEstado("tesoreria")
                    ?? throw new Exception("Estado de Ticket Tesoreria no encontrado");
                var estadoTicketPagado = await GetTicketEstado("pagado")
                    ?? throw new Exception("Estado de Ticket Pagado no encontrado");
                var estado = await GetServicioTransporteEstado("anulado")
                    ?? throw new Exception("Estado de Servicio Transporte Anulado no encontrado");
                var existing = await _acopioContext.ServicioTransportes.FindAsync(servicioTransporteDeleteDto.ServicioId)
                    ?? throw new Exception("Servicio Transporte no encontrado");
                var querySevTransTicket = from servTransDet in _acopioContext.ServicioTransporteDetalles                                        
                                    where servTransDet.ServicioTransporteId == servicioTransporteDeleteDto.ServicioId
                                    select servTransDet;
                var sevTransTicketList = await querySevTransTicket.ToListAsync()
                    ?? throw new Exception("Tickets no encontrados en Servicio Transporte");
                foreach (var detalle in sevTransTicketList)
                {
                    var dto = await _acopioContext.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == detalle.TicketId)
                            ?? throw new Exception("Ticket no encontrado");
                    if (dto.TicketEstadoId == estadoTicketTesoreria.TicketEstadoId)
                        throw new Exception("Debe anular la Liquidación");
                    if (dto.TicketEstadoId == estadoTicketPagado.TicketEstadoId)
                        throw new Exception("No se puede anular el Servicio Transporte, el Ticket ya ha sido pagado");
                    var historyTicket = new TicketHistorial
                    {
                        TicketId = dto.TicketId,
                        TicketIngenio = dto.TicketIngenio,
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
                        UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt,
                        UserModifiedName = servicioTransporteDeleteDto.UserModifiedName
                    };
                    _acopioContext.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = estadoTicket.TicketEstadoId;
                    dto.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                    dto.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;

                    var servTranDet = await _acopioContext.ServicioTransporteDetalles
                        .FirstOrDefaultAsync(d =>d.ServicioTransporteDetalleId == detalle.ServicioTransporteDetalleId)
                        ?? throw new Exception("Detalle Servicio Transporte no encontrado"); ;
                    servTranDet.ServicioTransporteDetalleStatus = false;
                    servTranDet.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                    servTranDet.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;
                }

                existing.ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId;
                existing.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;

                var pagoService = new PagoRepository(_acopioContext);
                await pagoService.AnularPago(
                    servicioTransporteDeleteDto.ServicioId, TipoReferencia, 
                    servicioTransporteDeleteDto.UserModifiedAt, 
                    servicioTransporteDeleteDto.UserModifiedName
                );
                await _acopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<int>
                {
                    Result = true,
                    ErrorMessage = "Servicio Transporte anulado",
                    Data = servicioTransporteDeleteDto.ServicioId
                }
                ;
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();

                throw;
            }
        }
        private IQueryable<ServicioResultDto> GetServiciosTransporteQuery(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId, int? servicioTransporteId)
        {
            return from servicio in _acopioContext.ServicioTransportes
                   join estado in _acopioContext.ServicioTransporteEstados
                       on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                   join carguillo in _acopioContext.Carguillos
                       on servicio.CarguilloId equals carguillo.CarguilloId
                   where (fechaDesde == null || servicio.ServicioTransporteFecha >= fechaDesde)
                   && (fechaHasta == null || servicio.ServicioTransporteFecha <= fechaHasta)
                   && (carguilloId == null || servicio.CarguilloId == carguilloId)
                   && (estadoId == null || servicio.ServicioTransporteEstadoId == estadoId)
                   && (servicioTransporteId == null || servicio.ServicioTransporteId == servicioTransporteId)
                   select new ServicioResultDto
                   {
                       ServicioId = servicio.ServicioTransporteId,
                       ServicioFecha = servicio.ServicioTransporteFecha,
                       ServicioCarguilloTitular = carguillo.CarguilloTitular,
                       ServicioPrecio = servicio.ServicioTransportePrecio,
                       ServicioPesoBruto = servicio.ServicioTransportePesoBruto,
                       ServicioTotal = servicio.ServicioTransporteTotal,
                       ServicioEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                   };
        }
        private async Task<ServicioTransporteEstado?> GetServicioTransporteEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _acopioContext.ServicioTransporteEstados
                            where estado.ServicioTransporteEstadoDescripcion.Equals(estadoDescripcion)
                            select estado;
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<TicketEstado?> GetTicketEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _acopioContext.TicketEstados
                            where estado.TicketEstadoDescripcion.Equals(estadoDescripcion)
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
        //private async Task<T?> GetEntityByDescriptionAsync<T>(string estadoDescripcion, Expression<Func<T, string>> descripcionSelector) where T : class
        //{
        //    try
        //    {
        //        var query = _acopioContext.Set<T>()
        //            .Where(e => 
        //            EF.Property<string>(e, descripcionSelector.Parameters[0].Name).Equals(estadoDescripcion));
        //        return await query.FirstOrDefaultAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}
