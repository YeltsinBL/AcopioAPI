using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Compra
{
    public class CompraInsertDto: InsertDto
    {
        public DateOnly CompraFecha { get; set; }
        public int TipoComprobanteId { get; set; }
        public required string CompraNumeroComprobante { get; set; }
        public int DistribuidorId { get; set; }
        public decimal CompraTotal { get; set; }
        public int? PendienteRecojo { get; set; }

        public required List<CompraDetalleInsertDto> CompraDetalles { get; set; }
    }   
}
