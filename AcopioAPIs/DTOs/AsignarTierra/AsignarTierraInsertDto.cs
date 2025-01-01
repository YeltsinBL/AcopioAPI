using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraInsertDto:InsertDto
    {
        public int AsignarTierraProveedorId { get; set; }

        public int AsignarTierraTierraId { get; set; }

        public DateOnly AsignarTierraFecha { get; set; }
    }
}
