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
        public int? PendienteRecojo { get; set; }
        public required List<CompraDetalleDto> CompraDetalles { get; set; }
        public required List<CompraDetalleRecojoDto> CompraDetallesRecojo { get; set; }
    }
    public class CompraDetalleRecojoDto
    {
        public int CompraDetalleRecojoId { get; set; }
        public int CompraId { get; set; }
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
        public int CompraDetallePorRecoger { get; set; }
        public int CompraDetalleRecogidos { get; set; }
        public int CompraDetallePendientes { get; set; }
        public DateTime? CompraDetalleRecojoFecha { get; set; }
        public string? CompraDetalleRecojoGuia { get; set; }
    }
}
