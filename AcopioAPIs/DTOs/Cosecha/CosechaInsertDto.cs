namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaInsertDto
    {
        public DateOnly CosechaFecha { get; set; }

        public string? CosechaSupervisor { get; set; }

        public double? CosechaHas { get; set; }

        public double? CosechaSac { get; set; }

        public double? CosechaRed { get; set; }

        public double? CosechaHumedad { get; set; }

        public int CosechaTierraId { get; set; }

        public int CosechaProveedorId { get; set; }

        public int CosechaCosechaTipoId { get; set; }
        public string UserCreatedName { get; set; }

        public DateOnly UserCreatedAt { get; set; }
    }
}
