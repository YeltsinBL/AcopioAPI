using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Ticket;

namespace AcopioAPIs.Repositories
{
    public interface ITicket
    {
        Task<List<TicketResultDto>> GetTicketResults(string? ingenio, int? carguilloId, string? viaje, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId);
        Task<TicketDto> GetTicket(int id);
        Task<TicketResultDto> Save(TicketInsertDto ticketInsertDto);
        Task<TicketResultDto> Update(TicketUpdateDto ticketUpdateDto);
        Task<bool> Delete(DeleteDto deleteDto);
        Task<List<TicketEstadoResultDto>> GetEstadoResults();
        Task<List<TicketResultDto>> GetTicketsByCarguilllo(int carguilloId);
        Task<List<TicketResultDto>> GetTicketsByProveedor(int proveedorId);
    }
}
