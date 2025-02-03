namespace AcopioAPIs.DTOs.Compra
{
    public class CompraDto
    {
        public int CompraId { get; set; }
        public DateTime CompraFecha { get; set; }
        public int TipoComprobanteId { get; set; }
        public required string TipoComprobanteNombre { get; set; }
        public required string CompraNumeroComprobante { get; set; }
        public int DistribuidorId { get; set; }
        public required string DistribuidorNombre { get; set; }
        public decimal CompraTotal { get; set; }
        public bool CompraStatus { get; set; }
        public required List<CompraDetalleDto> CompraDetalles { get; set; }
    }
}
