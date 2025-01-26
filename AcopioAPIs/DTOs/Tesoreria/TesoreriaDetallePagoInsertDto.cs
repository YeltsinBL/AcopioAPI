using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaDetallePagoInsertDto
    {
        public DateOnly TesoreriaDetallePagoFecha { get; set; }
        public bool TesoreriaDetallePagoEfectivo { get; set; }
        public string? TesoreriaDetallePagoBanco { get; set; }
        public string? TesoreriaDetallePagoCtaCte { get; set; }
        public decimal TesoreriaDetallePagoPagado { get; set; }

    }
}
