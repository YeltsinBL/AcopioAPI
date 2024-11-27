namespace AcopioAPIs.DTOs.Tierra
{
    public class TierraInsertDto
    {
        public string TierraUc { get; set; } = null!;

        public string TierraCampo { get; set; } = null!;

        public string TierraSector { get; set; } = null!;

        public string TierraValle { get; set; } = null!;

        public string TierraHa { get; set; } = null!;

        public bool TierraStatus { get; set; }

        public string UserCreatedName { get; set; } = null!;

        public DateOnly UserCreatedAt { get; set; }
    }
}
