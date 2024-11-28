namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraInsertDto
    {
        public int AsignarTierraProveedorId { get; set; }

        public int AsignarTierraTierraId { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }

        public required string UserCreatedName { get; set; }

        public DateOnly UserCreatedAt { get; set; }
    }
}
