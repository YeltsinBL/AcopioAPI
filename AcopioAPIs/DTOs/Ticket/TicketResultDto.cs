namespace AcopioAPIs.DTOs.Ticket
{
    public class TicketResultDto
    {
        public int TicketId { get; set; }
        public required string TicketIngenio { get; set; }
        public required string TicketViaje { get; set; }
        public required int CarguilloId { get; set; }
        public required string TicketTransportista { get; set; }

        public required string TicketChofer { get; set; }
        public DateTime TicketFecha { get; set; }
        public required int CarguilloDetalleCamionId{ get; set; }
        public required string TicketCamion { get; set; }

        public decimal TicketCamionPeso { get; set; }
        public required int CarguilloDetalleVehiculoId{ get; set; }
        public required string TicketVehiculo { get; set; }

        public decimal TicketVehiculoPeso { get; set; }
        public required string TicketUnidadPeso { get; set; }
        public decimal TicketPesoBruto { get; set; }
        public required string TicketEstadoDescripcion { get; set; }
    }
}
