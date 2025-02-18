namespace AcopioAPIs.DTOs.FacturaVenta
{
    public class FacturaVentaResultDto
    {
        public int FacturaVentaId { get; set; }
        public required string FacturaNumero { get; set; }
        public DateOnly FacturaVentaFecha { get; set; }
        public decimal FacturaCantidad { get; set; }
        public required string FacturaUnidadMedida { get; set; }
        public decimal FacturaImporteTotal { get; set; }
        public decimal FacturaDetraccion { get; set; }
        public decimal FacturaPendientePago { get; set; }
        public required string FacturaVentaEstado { get; set; }
    }
}
