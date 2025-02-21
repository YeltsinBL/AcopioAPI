using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Pago;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioUpdateDto:UpdateDto
    {
        public int ServicioId { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal ServicioTotal { get; set; }

        public decimal ServicioPendientePagar { get; set; }
        public decimal ServicioPagado { get; set; }
        public required string ServicioEstadoDescripcion { get; set; }
        public required List<PagoInsertDto> DetallePagos { get; set; }
    }
}
