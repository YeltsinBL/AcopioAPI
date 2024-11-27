namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraResultDto
    {
        public int AsignarTierraId { get; set; }

        public bool AsignarTierraStatus { get; set; }

        public int AsignarTierraProveedor { get; set; }

        public int AsignarTierraTierra { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }
    }
}
