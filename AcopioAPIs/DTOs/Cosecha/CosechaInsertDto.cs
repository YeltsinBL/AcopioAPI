namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaInsertDto
    {
        public DateOnly CosechaFecha { get; set; }

        public string? CosechaSupervisor { get; set; }

        public decimal? CosechaHas { get; set; }

        public decimal? CosechaSac { get; set; }

        public decimal? CosechaRed { get; set; }

        public decimal? CosechaHumedad { get; set; }

        public int CosechaTierraId { get; set; }

        public int CosechaProveedorId { get; set; }

        public int CosechaCosechaTipoId { get; set; }
        public string UserCreatedName { get; set; }

        public DateOnly UserCreatedAt { get; set; }
    }
}
