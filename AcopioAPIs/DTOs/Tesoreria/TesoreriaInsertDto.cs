using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaInsertDto: InsertDto
    {
        public int LiquidacionId { get; set; }
        public string? TesoreriaBanco { get; set; }
        public required string TesoreriaCtaCte { get; set; }
        public DateTime TesoreriaFecha { get; set; }
        public decimal TesoreriaMonto { get; set; }
    }
}
