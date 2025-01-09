using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Tierra
{
    public class TierraUpdateDto: UpdateDto
    {
        public int TierraId { get; set; }

        public string TierraUc { get; set; } = null!;

        public string TierraCampo { get; set; } = null!;

        public string TierraSector { get; set; } = null!;

        public string TierraValle { get; set; } = null!;

        public string TierraHa { get; set; } = null!;
    }
}
