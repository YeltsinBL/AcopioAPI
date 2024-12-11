using AcopioAPIs.DTOs.Common;
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
                            on ticket.CarguilloDetalleCamionId equals camion.CarguilloDetalleId
                        join vehiculo in _context.CarguilloDetalles
                            on ticket.CarguilloDetalleVehiculoId equals vehiculo.CarguilloDetalleId
                        where ticket.TicketId == id
                        select new TicketDto
                        {
                            TicketId = ticket.TicketId,
                            TicketIngenio = ticket.TicketIngenio,
                            TicketViaje = ticket.TicketViaje,
                            CarguilloId = ticket.CarguilloId,
                            TicketChofer = ticket.TicketChofer,
                            TicketFecha = ticket.TicketFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                            CarguilloDetalleCamionId = ticket.CarguilloDetalleCamionId,
                            TicketCamionPeso = ticket.TicketCamionPeso,
                            CarguilloDetalleVehiculoId = ticket.CarguilloDetalleVehiculoId,
                            TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                            TicketUnidadPeso = ticket.TicketUnidadPeso,
                            TicketPesoBruto = ticket.TicketPesoBruto,
                            TicketEstadoDescripcion = ticketStado.TicketEstadoDescripcion,
                            TicketCamion = camion.CarguilloDetallePlaca!,
                            TicketTransportista=carguillo.CarguilloTitular,
                            TicketVehiculo = vehiculo.CarguilloDetallePlaca!
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Ticket no encontrada.");
        }

        public async Task<TicketResultDto> Save(TicketInsertDto ticketInsertDto)
        {
            var newTicket = new Ticket 
            {
                TicketIngenio = ticketInsertDto.TicketIngenio,
                TicketViaje = ticketInsertDto.TicketViaje,
                CarguilloId = ticketInsertDto.CarguilloId,
                TicketChofer = ticketInsertDto.TicketChofer,
                TicketFecha = ticketInsertDto.TicketFecha,
                CarguilloDetalleCamionId= ticketInsertDto.CarguilloDetalleCamionId,
                TicketCamionPeso = ticketInsertDto.TicketCamionPeso,
                CarguilloDetalleVehiculoId= ticketInsertDto.CarguilloDetalleVehiculoId,
                TicketVehiculoPeso = ticketInsertDto.TicketVehiculoPeso,
                TicketUnidadPeso = ticketInsertDto.TicketUnidadPeso,
                TicketPesoBruto = ticketInsertDto.TicketPesoBruto,
                TicketEstadoId = 1,
                UserCreatedAt = ticketInsertDto.UserCreatedAt,
                UserCreatedName = ticketInsertDto.UserCreatedName
            };
            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();
            return await GetTicketResult(newTicket.TicketId);
        }

        public async Task<TicketResultDto> Update(TicketUpdateDto ticketUpdateDto)
        {
            try
            {
                using var conexion = GetConnection();
                await conexion.ExecuteAsync(
                    "usp_TicketUpdate", ticketUpdateDto, commandType: CommandType.StoredProcedure);
                return await GetTicketResult(ticketUpdateDto.TicketId);
            }
            catch (Exception)
            {

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
                                on ticket.CarguilloDetalleCamionId equals camion.CarguilloDetalleId
                            join vehiculo in _context.CarguilloDetalles
                                on ticket.CarguilloDetalleVehiculoId equals vehiculo.CarguilloDetalleId
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
                                TicketVehiculo = vehiculo.CarguilloDetallePlaca!
                            };
                return await query.FirstOrDefaultAsync() ??
                    throw new KeyNotFoundException("Ticket no encontrada."); ;
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
