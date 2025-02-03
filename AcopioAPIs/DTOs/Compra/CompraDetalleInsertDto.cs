namespace AcopioAPIs.DTOs.Compra
{
    public class CompraDetalleInsertDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
