namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaUpdateDto
    {
        public int CosechaId { get; set; }

        public decimal? CosechaHas { get; set; }

        public decimal? CosechaSac { get; set; }

        public decimal? CosechaRed { get; set; }

        public decimal? CosechaHumedad { get; set; }

        public int CosechaCosechaTipoId { get; set; }

        public string? UserModifiedName { get; set; }

        public DateOnly? UserModifiedAt { get; set; }
    }
}
