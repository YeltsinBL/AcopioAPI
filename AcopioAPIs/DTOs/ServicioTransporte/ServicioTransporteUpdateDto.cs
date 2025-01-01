using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteUpdateDto:UpdateDto
    {
        public int ServicioTransporteId { get; set; }
        public DateOnly ServicioTransporteFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public decimal ServicioTransporteTotal { get; set; }
        public required string ServicioTransporteEstadoDescripcion { get; set; }
        public int? CarguilloIdPalero { get; set; }
        public decimal? CarguilloPaleroPrecio { get; set; }
    }
}
