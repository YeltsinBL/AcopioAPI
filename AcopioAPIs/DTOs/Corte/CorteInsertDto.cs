namespace AcopioAPIs.DTOs.Corte
{
    public class CorteInsertDto
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
        public required string UserCreatedName { get; set; }

        public DateTime UserCreatedAt { get; set; }
        public required List<CorteInsertDetailDto> CorteDetail { get; set; }
    }
}
