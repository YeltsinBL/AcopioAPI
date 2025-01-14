﻿namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaResultDto
    {
        public int CosechaId { get; set; }
        public DateOnly CosechaFecha { get; set; }
        public required string CosechaTierraUC { get; set; }
        public required string CosechaTierraValle { get; set; }
        public required string CosechaTierraSector { get; set; }
        public required string CosechaProveedorUT { get; set; }
        public required string CosechaTierraCampo { get; set; }
        public required string CosechaCosechaTipo { get; set; }
    }
}
