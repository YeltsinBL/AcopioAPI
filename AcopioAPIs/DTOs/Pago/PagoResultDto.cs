namespace AcopioAPIs.DTOs.Pago
{
    public class PagoResultDto
    {
        public int PagoId { get; set; }
        public DateTime PagoFecha { get; set; }
        public bool PagoEfectivo { get; set; }
        public string? PagoBanco { get; set; }
        public string? PagoCtaCte { get; set; }
        public decimal PagoPagado { get; set; }
    }
}
