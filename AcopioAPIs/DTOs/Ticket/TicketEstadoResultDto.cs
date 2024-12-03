namespace AcopioAPIs.DTOs.Ticket
{
    public class TicketEstadoResultDto
    {
        public int TicketEstadoId { get; set; }
        public required string TicketEstadoDescripcion { get; set; }
        public bool TicketEstadoStatus { get; set; }
    }
}
