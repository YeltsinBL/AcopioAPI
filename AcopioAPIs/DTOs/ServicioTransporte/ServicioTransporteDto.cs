namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteDto
    {
        public int ServicioTransporteId { get; set; }
        public DateTime ServicioTransporteFecha { get; set; }
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public decimal ServicioTransporteTotal { get; set; }
        public required string ServicioTransporteEstadoDescripcion { get; set; }
        public required List<ServicioTransporteDetailDto> ServicioTransporteDetails { get; set; }
    }
}
