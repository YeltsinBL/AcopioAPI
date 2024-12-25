namespace AcopioAPIs.DTOs.Corte
{
    public class CorteResultDto
    {

        public int CorteId { get; set; }
        public DateTime CorteFecha {  get; set; }
        public required string TierraUC {  get; set; }
        public required string TierraCampo { get; set; }
        public double CortePrecio { get; set; }
        public int CorteCantidadTicket { get; set; }
        public required string CorteEstadoDescripcion {  get; set; }
        public double CortePesoBrutoTotal {  get; set; }
        public double CorteTotal {  get; set; }
    }
}
