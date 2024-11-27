namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraUpdateDto
    {
        public int AsignarTierraId { get; set; }

        public int AsignarTierraProveedor { get; set; }

        public int AsignarTierraTierra { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }

        public string UserModifiedName { get; set; }

        public DateOnly UserModifiedAt { get; set; }
    }
}
