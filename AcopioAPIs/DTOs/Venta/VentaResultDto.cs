namespace AcopioAPIs.DTOs.Venta
{
    public class VentaResultDto
    {
        public int VentaId { get; set; }
        public DateOnly VentaFecha { get; set; }
        public required string TipoComprobanteDescripcion { get; set; }
        public required string VentaNumeroDocumento { get; set; }
        public required string PersonaNombre { get; set; }
        public required string VentaTipoNombre { get; set; }
        public DateOnly? VentaFechaVence { get; set; }
        public decimal VentaTotal { get; set; }
        public required string VentaEstadoNombre { get; set; }
    }
}
