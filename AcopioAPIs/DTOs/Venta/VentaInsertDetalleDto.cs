namespace AcopioAPIs.DTOs.Venta
{
    public class VentaInsertDetalleDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
