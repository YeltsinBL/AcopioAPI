namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaDto
    {
        public int TesoreriaId { get; set; }
        public int LiquidacionId { get; set; }
        public string? TesoreriaBanco { get; set; }
        public required string TesoreriaCtaCte { get; set; }
        public DateTime TesoreriaFecha { get; set; }
        public decimal TesoreriaMonto { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo { get; set; }
        public required string ProveedorUT { get; set; }
    }
}
