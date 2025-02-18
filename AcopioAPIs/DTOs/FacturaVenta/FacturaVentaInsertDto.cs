using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.FacturaVenta
{
    public class FacturaVentaInsertDto: InsertDto
    {
        public int FacturaVentaEstadoId { get; set; }
        public required string FacturaNumero { get; set; }
        public DateOnly FacturaVentaFecha { get; set; }
        public decimal FacturaCantidad { get; set; }
        public required string FacturaUnidadMedida { get; set; }
        public decimal FacturaImporteTotal { get; set; }
        public decimal FacturaDetraccion { get; set; }
        public decimal FacturaPendientePago { get; set; }
        public required List<FacturaVentaPersonaInsertDto> FacturaVentaPersonas { get; set; }
    }
    public class FacturaVentaPersonaInsertDto
    {
        public int PersonaId { get; set; }
    }
}
