namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteResultDto
    {
        public int ServicioTransporteId { get; set; }
        public DateOnly ServicioTransporteFecha { get; set; }
        public required string ServicioTransporteCarguilloTitular { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public int ServicioTransporteTicketCantidad { get; set; }
        public required string ServicioTransporteEstadoDescripcion { get; set; }
    }
}
