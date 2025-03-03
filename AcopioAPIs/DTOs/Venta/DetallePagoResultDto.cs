namespace AcopioAPIs.DTOs.Venta
{
    public class DetallePagoResultDto
    {
        public int VentaDetallePagoId { get; set; }
        public DateTime VentaDetallePagoFecha { get; set; }
        public bool VentaDetallePagoEfectivo { get; set; }
        public string? VentaDetallePagoBanco { get; set; }
        public string? VentaDetallePagoCtaCte { get; set; }
        public decimal VentaDetallePagoPagado { get; set; }
        public string? VentaDetallePagoImagen { get; set; }
        public string? VentaDetallePagoComentario { get; set; }
    }
}
