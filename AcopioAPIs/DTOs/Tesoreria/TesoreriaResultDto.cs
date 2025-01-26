namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaResultDto
    {
        public int TesoreriaId { get; set; }
        public DateOnly TesoreriaFecha { get; set; }
        public decimal TesoreriaMonto { get; set; }
        public decimal TesoreriaPendientePagar { get; set; }
        public decimal TesoreriaPagado { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo { get; set; }
        public required string ProveedorUT { get; set; }
    }
}
