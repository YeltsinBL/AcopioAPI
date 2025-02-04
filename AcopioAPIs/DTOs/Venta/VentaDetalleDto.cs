namespace AcopioAPIs.DTOs.Venta
{
    public class VentaDetalleDto
    {
        public int VentaDetalleId { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
        public int VentaDetalleCantidad { get; set; }
        public decimal VentaDetallePrecio { get; set; }
    }
}
