using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionCorteResultDto
    {
        public int CorteId { get; set; }
        public int TierraId { get; set; }
        public required string TierraUC { get; set; }
        public required string TierraCampo { get; set; }
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public decimal CortePesoBrutoTotal { get; set; }
        public decimal CorteTotal { get; set; }

    }
}
