namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraDto
    {
        public int AsignarTierraId { get; set; }

        public int AsignarTierraProveedorId { get; set; }

        public string? AsignarTierraProveedorUT { get; set; }

        public int AsignarTierraTierraId { get; set; }
        public string? AsignarTierraTierraUC { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }
        public bool AsignarTierraStatus { get; set; }
        public required string TierraCampo { get; set; }
    }
}
