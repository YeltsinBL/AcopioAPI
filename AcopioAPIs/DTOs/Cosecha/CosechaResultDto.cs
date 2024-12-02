namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaResultDto
    {
        public int CosechaId { get; set; }

        public DateTime CosechaFecha { get; set; }

        public int CosechaTierraId { get; set; }

        public string CosechaTierraUC { get; set; }

        public string CosechaTierraValle { get; set; }

        public string CosechaTierraSector { get; set; }

        public int CosechaProveedorId { get; set; }

        public string CosechaProveedorUT { get; set; }

        public string? CosechaSupervisor { get; set; }

        public string CosechaTierraCampo { get; set; }

        public decimal? CosechaHAS { get; set; }

        public decimal? CosechaSac { get; set; }

        public decimal? CosechaRed { get; set; }

        public decimal? CosechaHumedad { get; set; }

        public int CosechaCosechaId { get; set; }

        public string CosechaCosechaTipo { get; set; }
    }
}
