using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Corte
{
    public class CorteUpdateDto: UpdateDto
    {
        public int CorteId { get; set; }
        public decimal CortePrecio { get; set; }
        public decimal CorteTotal { get; set; }
        public required string CorteEstadoDescripcion { get; set; }
    }
}
