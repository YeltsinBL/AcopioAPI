using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaUpdateDto:UpdateDto
    {
        public int TesoreriaId { get; set; }
        public decimal TesoreriaPendientePagar { get; set; }
        public decimal TesoreriaPagado { get; set; }
        public required List<TesoreriaDetallePagoInsertDto> TesoreriaDetallePagos { get; set; }
    }
}
