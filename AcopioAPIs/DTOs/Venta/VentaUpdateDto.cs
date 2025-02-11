using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Venta
{
    public class VentaUpdateDto:UpdateDto
    {
        public int VentaId { get; set; }
        public DateOnly VentaFecha { get; set; }
        public int TipoComprobanteId { get; set; }
        public int PersonaId { get; set; }
        public int VentaTipoId { get; set; }
        public int VentaDia { get; set; }
        public int VentaEstadoId { get; set; }
        public decimal VentaPendientePagar { get; set; }
        public decimal VentaPagado { get; set; }
        public required List<DetallePagoInsertDto> DetallePagos { get; set; }
    }
}
