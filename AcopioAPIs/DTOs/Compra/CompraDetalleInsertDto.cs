namespace AcopioAPIs.DTOs.Compra
{
    public class CompraDetalleInsertDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int CompraDetalleRecogidos { get; set; }
        public int CompraDetallePendientes { get; set; }
        public DateOnly CompraDetalleRecojoFecha { get; set; }
        public string? CompraDetalleRecojoGuia { get; set; }
    }
}
