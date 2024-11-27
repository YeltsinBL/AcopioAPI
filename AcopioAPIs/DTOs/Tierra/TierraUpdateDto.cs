namespace AcopioAPIs.DTOs.Tierra
{
    public class TierraUpdateDto
    {
        public int TierraId { get; set; }

        public string TierraUc { get; set; } = null!;

        public string TierraCampo { get; set; } = null!;

        public string TierraSector { get; set; } = null!;

        public string TierraValle { get; set; } = null!;

        public string TierraHa { get; set; } = null!;

        public bool TierraStatus { get; set; }

        public string UserModifiedName { get; set; }

        public DateOnly UserModifiedAt { get; set; }
    }
}
