namespace AcopioAPIs.DTOs.Producto
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
        public int ProductoCantidad { get; set; }
        public decimal ProductoPrecioVenta { get; set; }
        public bool ProductoStatus { get; set; }
        public int ProductoTipoId { get; set; }
        public required string ProductoTipoDetalle { get; set; }
    }
}
