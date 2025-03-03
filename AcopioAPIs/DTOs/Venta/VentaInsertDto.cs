using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Venta
{
    public class VentaInsertDto: InsertDto
    {
        public DateOnly VentaFecha { get; set;  }
        public int TipoComprobanteId {get; set; }
        public int VentaTipoId {get; set; }
        public int PersonaId {get; set; }
        public int VentaDia {get; set; }
        public decimal VentaTotal {get; set; }
        public int VentaEstadoId {get; set; }

        public decimal VentaPendientePagar { get; set; }
        public decimal VentaPagado { get; set; }
        public required List<VentaInsertDetalleDto> VentaDetalles { get; set; }
        public List<DetallePagoInsertDto>? DetallePagos { get; set;}
    }
}
