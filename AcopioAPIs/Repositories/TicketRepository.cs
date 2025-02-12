using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Ticket;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class TicketRepository : ITicket
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public TicketRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<List<TicketEstadoResultDto>> GetEstadoResults()
        {
            var query = from ticketStado in _context.TicketEstados
                        select new TicketEstadoResultDto
                        {
                            TicketEstadoId = ticketStado.TicketEstadoId,
                            TicketEstadoDescripcion = ticketStado.TicketEstadoDescripcion,
                            TicketEstadoStatus = ticketStado.TicketEstadoStatus
                        };
            return await query.ToListAsync();
        }

        public async Task<List<TicketResultDto>> GetTicketResults(string? ingenio, int? carguilloId, string? viaje, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId)
        {
            try
            {
                using var conexion = GetConnection();
                var tickets = await conexion.QueryAsync<TicketResultDto>(
                    "usp_TicketList", 
                    new {
                        TicketIngenio=ingenio,
                        CarguilloId = carguilloId,
                        TicketViaje=viaje,
                        TicketFechaDesde =fechaDesde,
                        TicketFechaHasta =fechaHasta,
                        EstadoId =estadoId
                    }, 
                    commandType: CommandType.StoredProcedure);
                return tickets.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TicketDto> GetTicket(int id)
        {
            var query = from ticket in _context.Tickets
                        join ticketStado in _context.TicketEstados
                            on ticket.TicketEstadoId equals ticketStado.TicketEstadoId
                        join carguillo in _context.Carguillos
                            on ticket.CarguilloId equals carguillo.CarguilloId
                        join camion in _context.CarguilloDetalles
                            on ticket.CarguilloDetalleCamionId equals camion.CarguilloDetalleId into carguilloCamion
                        from camion in carguilloCamion.DefaultIfEmpty()
                        join vehiculo in _context.CarguilloDetalles
                            on ticket.CarguilloDetalleVehiculoId equals vehiculo.CarguilloDetalleId into carguilloVehiculo
                        from vehiculo in carguilloVehiculo.DefaultIfEmpty()
                        where ticket.TicketId == id
                        select new TicketDto
                        {
                            TicketId = ticket.TicketId,
                            TicketIngenio = ticket.TicketIngenio,
                            TicketViaje = ticket.TicketViaje,
                            CarguilloId = ticket.CarguilloId,
                            TicketChofer = ticket.TicketChofer,
                            TicketFecha = ticket.TicketFecha,
                            CarguilloDetalleCamionId = ticket.CarguilloDetalleCamionId,
                            TicketCamionPeso = ticket.TicketCamionPeso,
                            CarguilloDetalleVehiculoId = ticket.CarguilloDetalleVehiculoId,
                            TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                            TicketUnidadPeso = ticket.TicketUnidadPeso,
                            TicketPesoBruto = ticket.TicketPesoBruto,
                            TicketEstadoDescripcion = ticketStado.TicketEstadoDescripcion,
                            TicketCamion = camion.CarguilloDetallePlaca!,
                            TicketTransportista=carguillo.CarguilloTitular,
                            TicketVehiculo = vehiculo.CarguilloDetallePlaca!,
                            TicketCampo = ticket.TicketCampo,
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Ticket no encontrada.");
        }

        public async Task<TicketResultDto> Save(TicketInsertDto ticketInsertDto)
        {
            var estadosTicket = from estados in _context.TicketEstados
                                where estados.TicketEstadoDescripcion.Equals("activo")
                                select estados;
            var estado = await estadosTicket.FirstOrDefaultAsync()
                ?? throw new Exception("Estados de Tickets no encontrados");
            var newTicket = new Ticket 
            {
                TicketIngenio = ticketInsertDto.TicketIngenio,
                TicketViaje = ticketInsertDto.TicketViaje,
                TicketCampo = ticketInsertDto.TicketCampo,
                CarguilloId = ticketInsertDto.CarguilloId,
                TicketChofer = ticketInsertDto.TicketChofer,
                TicketFecha = ticketInsertDto.TicketFecha,
                CarguilloDetalleCamionId= ticketInsertDto.CarguilloDetalleCamionId,
                TicketCamionPeso = ticketInsertDto.TicketCamionPeso,
                CarguilloDetalleVehiculoId= ticketInsertDto.CarguilloDetalleVehiculoId,
                TicketVehiculoPeso = ticketInsertDto.TicketVehiculoPeso,
                TicketUnidadPeso = ticketInsertDto.TicketUnidadPeso,
                TicketPesoBruto = ticketInsertDto.TicketPesoBruto,
                TicketEstadoId = estado.TicketEstadoId,
                UserCreatedAt = ticketInsertDto.UserCreatedAt,
                UserCreatedName = ticketInsertDto.UserCreatedName
            };
            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();
            return await GetTicketResult(newTicket.TicketId);
        }

        public async Task<TicketResultDto> Update(TicketUpdateDto ticketUpdateDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (ticketUpdateDto == null) throw new Exception("No se enviaron datos para guardar el ticket");
                var ticket = await _context.Tickets.FindAsync(ticketUpdateDto.TicketId)
                    ?? throw new Exception("Ticket no encontrado");
                var historial = new TicketHistorial
                {
                    TicketId = ticketUpdateDto.TicketId,
                    TicketIngenio = ticket.TicketIngenio,
                    TicketCampo = ticket.TicketCampo,
                    TicketViaje = ticket.TicketViaje,
                    CarguilloId = ticket.CarguilloId,
                    TicketChofer = ticket.TicketChofer,
                    TicketFecha = ticket.TicketFecha,
                    CarguilloDetalleCamionId = ticket.CarguilloDetalleCamionId,
                    TicketCamionPeso = ticket.TicketCamionPeso,
                    CarguilloDetalleVehiculoId = ticket.CarguilloDetalleVehiculoId,
                    TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                    TicketUnidadPeso = ticket.TicketUnidadPeso,
                    TicketPesoBruto = ticket.TicketPesoBruto,
                    TicketEstadoId = ticket.TicketEstadoId,
                    UserModifiedName = ticketUpdateDto.UserModifiedName,
                    UserModifiedAt = ticketUpdateDto.UserModifiedAt
                };
                _context.Add(historial);

                ticket.TicketIngenio = ticketUpdateDto.TicketIngenio;
                ticket.TicketCampo = ticketUpdateDto.TicketCampo;
                ticket.TicketViaje = ticketUpdateDto.TicketViaje;
                ticket.CarguilloId = ticketUpdateDto.CarguilloId;
                ticket.TicketChofer = ticketUpdateDto.TicketChofer;
                ticket.TicketFecha = ticketUpdateDto.TicketFecha;
                ticket.CarguilloDetalleCamionId = ticketUpdateDto.CarguilloDetalleCamionId;
                ticket.TicketCamionPeso = ticketUpdateDto.TicketCamionPeso;
                ticket.CarguilloDetalleVehiculoId = ticketUpdateDto.CarguilloDetalleVehiculoId;
                ticket.TicketVehiculoPeso = ticketUpdateDto.TicketVehiculoPeso;
                ticket.TicketUnidadPeso = ticketUpdateDto.TicketUnidadPeso;
                ticket.TicketPesoBruto = ticketUpdateDto.TicketPesoBruto;
                ticket.UserModifiedAt = ticketUpdateDto.UserModifiedAt;
                ticket.UserModifiedName = ticketUpdateDto.UserModifiedName;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetTicketResult(ticketUpdateDto.TicketId);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(DeleteDto deleteDto)
        {
            try
            {
                using var conexion = GetConnection();
                await conexion.ExecuteAsync(
                    "usp_TicketDelete", deleteDto,
                    commandType: CommandType.StoredProcedure
                );
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<TicketResultDto> GetTicketResult(int ticketId)
        {
            try
            {
                var query = from ticket in _context.Tickets
                            join ticketStado in _context.TicketEstados
                                on ticket.TicketEstadoId equals ticketStado.TicketEstadoId
                            join carguillo in _context.Carguillos
                                on ticket.CarguilloId equals carguillo.CarguilloId
                            join camion in _context.CarguilloDetalles
                                on ticket.CarguilloDetalleCamionId equals camion.CarguilloDetalleId into carguilloCamion
                            from camion in carguilloCamion.DefaultIfEmpty()
                            join vehiculo in _context.CarguilloDetalles
                                on ticket.CarguilloDetalleVehiculoId equals vehiculo.CarguilloDetalleId into carguilloVehiculo
                            from vehiculo in carguilloVehiculo.DefaultIfEmpty()
                            where ticket.TicketId == ticketId
                            select new TicketResultDto
                            {
                                TicketId = ticket.TicketId,
                                TicketIngenio = ticket.TicketIngenio,
                                TicketViaje = ticket.TicketViaje,
                                TicketChofer = ticket.TicketChofer,
                                TicketFecha = ticket.TicketFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                                TicketCamionPeso = ticket.TicketCamionPeso,
                                TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                                TicketUnidadPeso = ticket.TicketUnidadPeso,
                                TicketPesoBruto = ticket.TicketPesoBruto,
                                TicketEstadoDescripcion = ticketStado.TicketEstadoDescripcion,
                                TicketCamion = camion.CarguilloDetallePlaca!,
                                TicketTransportista = carguillo.CarguilloTitular,
                                TicketVehiculo = vehiculo.CarguilloDetallePlaca!,
                                TicketCampo = ticket.TicketCampo
                            };
                return await query.FirstOrDefaultAsync() ??
                    throw new KeyNotFoundException("Ticket no encontrada."); ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<TicketResultDto>> GetTicketsByCarguilllo(int carguilloId)
        {
            try
            {
                var estados = from est in _context.TicketEstados
                              where est.TicketEstadoDescripcion.Equals("archivado")
                              select est;
                var estad = await estados.FirstOrDefaultAsync()
                    ?? throw new Exception("Estado de Tickets no encontrado");

                var query = from ticket in _context.Tickets
                            join estado in _context.TicketEstados
                                on ticket.TicketEstadoId equals estado.TicketEstadoId
                            join carguillo in _context.Carguillos
                                on ticket.CarguilloId equals carguillo.CarguilloId
                            join carguilloVehiculo in _context.CarguilloDetalles
                                on ticket.CarguilloDetalleVehiculoId equals carguilloVehiculo.CarguilloDetalleId into Vehiculo
                            from carguilloVehiculo in Vehiculo.DefaultIfEmpty()
                            join carguilloCamion in _context.CarguilloDetalles
                                on ticket.CarguilloDetalleCamionId equals carguilloCamion.CarguilloDetalleId into Camion
                            from carguilloCamion in Camion.DefaultIfEmpty()
                            where (ticket.CarguilloId == carguilloId ) && 
                            (ticket.TicketEstadoId == estad.TicketEstadoId)
                            select new TicketResultDto
                            {
                                TicketId = ticket.TicketId,
                                TicketIngenio = ticket.TicketIngenio,
                                TicketViaje = ticket.TicketViaje,
                                TicketChofer = ticket.TicketChofer,
                                TicketFecha = ticket.TicketFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                                TicketCamionPeso = ticket.TicketCamionPeso,
                                TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                                TicketUnidadPeso = ticket.TicketUnidadPeso,
                                TicketPesoBruto = ticket.TicketPesoBruto,
                                TicketEstadoDescripcion = estado.TicketEstadoDescripcion,
                                TicketCamion = carguilloCamion.CarguilloDetallePlaca!,
                                TicketTransportista = carguillo.CarguilloTitular,
                                TicketVehiculo = carguilloVehiculo.CarguilloDetallePlaca!,
                                TicketCampo = ticket.TicketCampo
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<TicketResultDto>> GetTicketsByProveedor(int proveedorId)
        {
            try
            {
                using var conexion = GetConnection();
                var tickets = await conexion.QueryAsync<TicketResultDto>(
                    "usp_LiquidacionGetTicketsByProveedor", new { ProveedorId = proveedorId},
                    commandType: CommandType.StoredProcedure);
                return tickets.ToList();
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
