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

        public double? CosechaHAS { get; set; }

        public double? CosechaSac { get; set; }

        public double? CosechaRed { get; set; }

        public double? CosechaHumedad { get; set; }

        public int CosechaCosechaId { get; set; }

        public string CosechaCosechaTipo { get; set; }
    }
}
