namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraResultDto
    {
        public int AsignarTierraId { get; set; }

        public bool AsignarTierraStatus { get; set; }

        public required string AsignarTierraProveedorUT { get; set; }

        public required string AsignarTierraTierraUC {  get; set; }

        public DateOnly AsignarTierraFecha { get; set; }
        public required string TierraCampo { get; set; }
    }
}
