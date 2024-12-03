namespace AcopioAPIs.DTOs.Ticket
{
    public class TicketInsertDto
    {
        public required string TicketIngenio { get; set; }
        public required string TicketViaje { get; set; }
        public required string TicketTransportista { get; set; }
        public required string TicketChofer { get; set; }
        public DateOnly TicketFecha { get; set; }
        public required string TicketCamion { get; set; }
        public decimal TicketCamionPeso { get; set; }
        public required string TicketVehiculo { get; set; }
        public decimal TicketVehiculoPeso { get; set; }
        public required string TicketUnidadPeso { get; set; }
        public decimal TicketPesoBruto { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public required string UserCreatedName { get; set; }
    }
}
