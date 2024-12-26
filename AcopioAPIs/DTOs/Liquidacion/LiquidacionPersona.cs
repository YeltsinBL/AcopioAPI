namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionPersona
    {
        public int PersonId { get; set; }
        public required string ProveedorNombre { get; set; }
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public int TierraId { get; set; }
        public required string TierraCampo { get; set; }

    }
}
