using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Pago;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioPaleroInsertDto: InsertDto
    {
        public DateOnly ServicioFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal? ServicioPesoBruto { get; set; }
        public decimal ServicioTotal { get; set; }
        public decimal ServicioPendientePagar { get; set; }
        public decimal ServicioPagado { get; set; }
        public required List<ServicioInsertDetailDto> ServicioDetail { get; set; }
        public required List<PagoInsertDto> DetallePagos { get; set; }
    }
}
