namespace AcopioAPIs.DTOs.Compra
{
    public class CompraResultDto
    {
        public int CompraId { get; set; }
        public DateOnly CompraFecha { get; set; }
        public required string TipoComprobanteDescripcion { get; set; }
        public required string CompraNumeroComprobante { get; set; }
        public required string DistribuidorNombre { get; set; }
        public decimal CompraTotal { get; set; }
        public bool CompraStatus { get; set; }
    }
}
