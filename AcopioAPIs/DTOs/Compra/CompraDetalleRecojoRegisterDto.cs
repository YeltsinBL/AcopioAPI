namespace AcopioAPIs.DTOs.Compra
{
    public class CompraDetalleRecojoRegisterDto
    {
        public int CompraDetalleRecojoId { get; set; }
        public int ProductoId { get; set; }
        public int CompraDetallePorRecoger { get; set; }
        public int CompraDetalleRecogidos { get; set; }
        public int CompraDetallePendientes { get; set; }
        public DateOnly CompraDetalleRecojoFecha { get; set; }
        public string? CompraDetalleRecojoGuia { get; set; }
    }
}
