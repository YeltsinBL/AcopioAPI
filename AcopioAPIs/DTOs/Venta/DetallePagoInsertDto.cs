using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Venta
{
    public class DetallePagoInsertDto
    {
        public DateOnly DetallePagoFecha { get; set; }
        public bool DetallePagoEfectivo { get; set; }
        public string? DetallePagoBanco { get; set; }
        public string? DetallePagoCtaCte { get; set; }
        public decimal DetallePagoPagado { get; set; }

    }
}
