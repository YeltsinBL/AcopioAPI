namespace AcopioAPIs.DTOs.FacturaVenta
{
    public class FacturaVentaDto
    {
        public int FacturaVentaId { get; set; }
        public required string FacturaVentaNumero { get; set; }
        public DateTime facturaVentaFecha { get; set; }
        public decimal FacturaVentaCantidad { get; set; }
        public required string FacturaVentaUnidadMedida { get; set; }
        public decimal FacturaVentaImporte { get; set; }
        public decimal FacturaVentaDetraccion { get; set; }
        public decimal FacturaVentaPendientePago { get; set; }
        public int FacturaVentaEstadoId { get; set; }
        public required string FacturaVentaEstado { get; set; }
        public required List<FacturaVentaPersonaDto> FacturaVentaPersonas { get; set; }
    }
    public class FacturaVentaPersonaDto
    {
        public int FacturaVentaPersonaId { get; set; }
        public int PersonaId { get; set; }
        public required string PersonaNombre { get; set; }

    }
}
