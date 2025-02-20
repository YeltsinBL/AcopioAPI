using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Compra
{
    public class CompraUpdateDto:UpdateDto
    {
        public int CompraId { get; set; }
        public DateOnly CompraFecha { get; set; }
        public int TipoComprobanteId { get; set; }
        public required string CompraNumeroComprobante { get; set; }
        public int DistribuidorId { get; set; }
        public int? PendienteRecojo { get; set; }
        public required List<CompraDetalleRecojoRegisterDto> CompraDetallesRecojo { get; set; }
    }
}
