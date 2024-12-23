﻿namespace AcopioAPIs.DTOs.AsignarTierra
{
    public class AsignarTierraUpdateDto
    {
        public int AsignarTierraId { get; set; }

        public int AsignarTierraProveedorId { get; set; }

        public int AsignarTierraTierraId { get; set; }

        public DateTime AsignarTierraFecha { get; set; }

        public required string UserModifiedName { get; set; }

        public DateTime UserModifiedAt { get; set; }
    }
}
