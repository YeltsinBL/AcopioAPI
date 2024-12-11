namespace AcopioAPIs.DTOs.Corte
{
    public class CorteDto
    {
        public int CorteId { get; set; }
        public DateTime CorteFecha { get; set; }
        public int TierraId { get; set; }
        public required string TierraUC { get; set; }
        public double CortePrecio { get; set; }
        public required string CorteEstadoDescripcion { get; set; }
        public double CortePesoBrutoTotal { get; set; }
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public double CarguilloPrecio { get; set; }
        public double CorteTotal { get; set; }
        public required List<CorteDetailDto> CorteDetail { get; set; }
    }
}
