namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraInsertDto
    {
        public int AsignarTierraProveedor { get; set; }

        public int AsignarTierraTierra { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }

        public string UserCreatedName { get; set; }

        public DateOnly UserCreatedAt { get; set; }
    }
}
