namespace AcopioAPIs.DTOs.Recojo
{
    public class RecojoResultDto
    {
        public int RecojoId { get; set; }
        public DateOnly RecojoFechaInicio { get; set; }
        public DateOnly RecojoFechaFin { get; set; }
        public decimal RecojoCamionesPrecio { get; set; }
        public decimal RecojoDiasPrecio { get; set; }
        public decimal RecojoTotalPrecio { get; set; }
        public required string RecojoEstadoDescripcion { get; set; }
        public string? RecojoCampo { get; set; }
    }
}
