namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraDeleteDto
    {
        public int AsignarTierraId { get; set; }

        public required string UserModifiedName { get; set; }

        public DateTime UserModifiedAt { get; set; }
    }
}
