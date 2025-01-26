namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaDetallePagoResultDto
    {
        public int TesoreriaDetallePagoId { get; set; }
        public DateTime TesoreriaDetallePagoFecha { get; set; }
        public bool TesoreriaDetallePagoEfectivo { get; set; }
        public string? TesoreriaDetallePagoBanco { get; set; }
        public string? TesoreriaDetallePagoCtaCte { get; set; }
        public decimal TesoreriaDetallePagoPagado { get; set; }
    }
}
