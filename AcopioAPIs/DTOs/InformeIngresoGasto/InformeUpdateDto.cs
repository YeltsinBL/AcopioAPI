using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.InformeIngresoGasto
{
    public class InformeUpdateDto: UpdateDto
    {
        public int InformeId { get; set; }
        public decimal InformeFacturaTotal { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public decimal InformeTotal { get; set; }
        public string? InformeResultado { get; set; }
    }
}
