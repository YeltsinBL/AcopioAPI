namespace AcopioAPIs.DTOs.Pago
{
    public class PagoInsertDto
    {
        public DateOnly PagoFecha { get; set; }
        public bool PagoEfectivo { get; set; }
        public string? PagoBanco { get; set; }
        public string? PagoCtaCte { get; set; }
        public decimal PagoPagado { get; set; }
    }
}
