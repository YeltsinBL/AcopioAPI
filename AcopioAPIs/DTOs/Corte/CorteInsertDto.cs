using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Corte
{
    public class CorteInsertDto:InsertDto
    {
        public CorteInsertDto() 
        {
            CorteDetail = [];
        }
        public DateOnly CorteFecha { get; set; }
        public int TierraId { get; set; }
        public decimal CortePrecio { get; set; }
        public decimal CortePesoBrutoTotal { get; set; }
        public decimal CorteTotal { get; set; }
        public required List<CorteInsertDetailDto> CorteDetail { get; set; }
    }
}
