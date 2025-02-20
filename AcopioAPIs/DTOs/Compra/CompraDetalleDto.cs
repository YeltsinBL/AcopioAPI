namespace AcopioAPIs.DTOs.Compra
{
    public class CompraDetalleDto
    {
        public int CompraDetalleId { get; set; }
        public int CompraId { get; set; }
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
        public int CompraDetalleCantidad { get; set; }
        public decimal CompraDetallePrecio { get; set; }
        public int CompraDetalleRecogidos { get; set; }
        public int CompraDetallePendientes { get; set; }
    }
}
