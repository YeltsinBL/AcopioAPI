namespace AcopioAPIs.DTOs.Corte
{
    public class CorteResultDto
    {

        public int CorteId { get; set; }
        public DateOnly CorteFecha {  get; set; }
        public required string TierraUC {  get; set; }
        public required string TierraCampo { get; set; }
        public decimal CortePrecio { get; set; }
        public int CorteCantidadTicket { get; set; }
        public required string CorteEstadoDescripcion {  get; set; }
        public decimal CortePesoBrutoTotal {  get; set; }
        public decimal CorteTotal {  get; set; }
    }
}
