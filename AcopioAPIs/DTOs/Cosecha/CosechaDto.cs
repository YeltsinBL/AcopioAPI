namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaDto
    {
        public int CosechaId { get; set; }

        public DateOnly CosechaFecha { get; set; }

        public int CosechaTierraId { get; set; }

        public required string CosechaTierraUC { get; set; }

        public required string CosechaTierraValle { get; set; }

        public required string CosechaTierraSector { get; set; }

        public int CosechaProveedorId { get; set; }

        public required string CosechaProveedorUT { get; set; }

        public string? CosechaSupervisor { get; set; }

        public required string CosechaTierraCampo { get; set; }

        public decimal? CosechaHAS { get; set; }

        public decimal? CosechaSac { get; set; }

        public decimal? CosechaRed { get; set; }

        public decimal? CosechaHumedad { get; set; }

        public int CosechaCosechaId { get; set; }

        public required string CosechaCosechaTipo { get; set; }
    }
}
