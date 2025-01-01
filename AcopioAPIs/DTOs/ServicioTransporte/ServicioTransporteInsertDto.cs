using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteInsertDto: InsertDto
    {
        public DateOnly ServicioTransporteFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public decimal ServicioTransporteTotal { get; set; }
        public int? CarguilloIdPalero { get; set; }
        public decimal? CarguilloPaleroPrecio { get; set; }
        public required List<InsertDetailTicketDto> ServicioTransporteDetail { get; set; }
    }
}
