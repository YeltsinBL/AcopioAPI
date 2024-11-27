namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaUpdateDto
    {
        public int CosechaId { get; set; }

        public double? CosechaHas { get; set; }

        public double? CosechaSac { get; set; }

        public double? CosechaRed { get; set; }

        public double? CosechaHumedad { get; set; }

        public int CosechaCosechaTipoId { get; set; }

        public string? UserModifiedName { get; set; }

        public DateOnly? UserModifiedAt { get; set; }
    }
}
