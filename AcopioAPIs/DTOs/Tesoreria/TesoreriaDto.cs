namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaDto
    {
        public int TesoreriaId { get; set; }
        public int LiquidacionId { get; set; }
        public DateTime TesoreriaFecha { get; set; }
        public decimal TesoreriaMonto { get; set; }
        public decimal TesoreriaPendientePagar { get; set; }
        public decimal TesoreriaPagado { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo { get; set; }
        public required string ProveedorUT { get; set; }
        public required List<TesoreriaDetallePagoResultDto> TesoreriaDetallePagos { get; set; }
    }
}
