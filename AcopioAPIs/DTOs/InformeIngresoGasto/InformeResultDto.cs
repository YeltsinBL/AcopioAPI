namespace AcopioAPIs.DTOs.InformeIngresoGasto
{
    public class InformeResultDto
    {
        public int InformeId { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo { get; set; }
        public DateOnly InformeFecha { get; set; }
        public decimal InformeFacturaTotal { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public decimal InformeTotal { get; set; }
        public bool InformeStatus { get; set; }
    }
}
