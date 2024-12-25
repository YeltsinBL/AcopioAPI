using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraUpdateDto:UserUpdateDto
    {
        public int AsignarTierraId { get; set; }

        public int AsignarTierraProveedorId { get; set; }

        public int AsignarTierraTierraId { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }
    }
}
