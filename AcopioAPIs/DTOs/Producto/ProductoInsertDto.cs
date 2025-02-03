using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Producto
{
    public class ProductoInsertDto: InsertDto
    {
        public required string ProductoNombre { get; set; }
        public decimal ProductoPrecioVenta { get; set; }
    }
}
