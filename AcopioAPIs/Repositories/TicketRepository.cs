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

        public async Task<List<TicketResultDto>> GetTicketResults(string? ingenio, string? transportista, string? viaje, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId)
        {
            try
            {
                Console.WriteLine($"Ingenio: {ingenio}");
                Console.WriteLine($"Transportista: {transportista}");
                Console.WriteLine($"Viaje: {viaje}");
                Console.WriteLine($"FechaDesde: {fechaDesde}");
                Console.WriteLine($"FechaHasta: {fechaHasta}");
                Console.WriteLine($"EstadoId: {estadoId}");

                using var conexion = GetConnection();
                var tickets = await conexion.QueryAsync<TicketResultDto>(
                    "usp_TicketList", 
                    new {
                        TicketIngenio=ingenio,
                        TicketTransportista=transportista,
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

        public async Task<TicketResultDto> GetTicket(int id)
        {
            var query = from ticket in _context.Tickets
                        join ticketStado in _context.TicketEstados
                            on ticket.TicketEstadoId equals ticketStado.TicketEstadoId
                        where ticket.TicketId == id
                        select new TicketResultDto
                        {
                            TicketId = ticket.TicketId,
                            TicketIngenio = ticket.TicketIngenio,
                            TicketViaje = ticket.TicketViaje,
                            TicketTransportista = ticket.TicketTransportista,
                            TicketChofer = ticket.TicketChofer,
                            TicketFecha = ticket.TicketFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                            TicketCamion = ticket.TicketCamion,
                            TicketCamionPeso = ticket.TicketCamionPeso,
                            TicketVehiculo = ticket.TicketVehiculo,
                            TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                            TicketUnidadPeso = ticket.TicketUnidadPeso,
                            TicketPesoBruto = ticket.TicketPesoBruto,
                            TicketEstadoDescripcion = ticketStado.TicketEstadoDescripcion
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Ticket no encontrada."); ;
        }

        public async Task<TicketResultDto> Save(TicketInsertDto ticketInsertDto)
        {
            var newTicket = new Ticket 
            {
                TicketIngenio = ticketInsertDto.TicketIngenio,
                TicketViaje = ticketInsertDto.TicketViaje,
                TicketTransportista = ticketInsertDto.TicketTransportista,
                TicketChofer = ticketInsertDto.TicketChofer,
                TicketFecha = ticketInsertDto.TicketFecha,
                TicketCamion = ticketInsertDto.TicketCamion,
                TicketCamionPeso = ticketInsertDto.TicketCamionPeso,
                TicketVehiculo = ticketInsertDto.TicketVehiculo,
                TicketVehiculoPeso = ticketInsertDto.TicketVehiculoPeso,
                TicketUnidadPeso = ticketInsertDto.TicketUnidadPeso,
                TicketPesoBruto = ticketInsertDto.TicketPesoBruto,
                TicketEstadoId = 1,
                UserCreatedAt = ticketInsertDto.UserCreatedAt,
                UserCreatedName = ticketInsertDto.UserCreatedName
            };
            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();
            return await GetTicket(newTicket.TicketId);
        }

        public async Task<TicketResultDto> Update(TicketUpdateDto ticketUpdateDto)
        {
            try
            {
                using var conexion = GetConnection();
                await conexion.ExecuteAsync(
                    "usp_TicketUpdate", ticketUpdateDto, commandType: CommandType.StoredProcedure);
                return await GetTicket(ticketUpdateDto.TicketId);
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

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
