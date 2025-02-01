namespace AcopioAPIs.DTOs.Producto
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
        public bool ProductoStatus { get; set; }
    }
}
