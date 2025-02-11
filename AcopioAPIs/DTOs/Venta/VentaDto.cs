namespace AcopioAPIs.DTOs.Venta
{
    public class VentaDto
    {
        public int VentaId { get; set; }
        public DateTime VentaFecha { get; set; }
        public int TipoComprobanteId { get; set; }
        public required string TipoComprobanteNombre { get; set; }
        public required string VentaNumeroDocumento { get; set; }
        public int VentaTipoId { get; set; }
        public required string VentaTipoNombre { get; set; }
        public int PersonaId { get; set; }
        public required string PersonaNombre { get; set; }
        public int? VentaDia { get; set; }
        public DateTime? VentaFechaVence { get; set; }
        public decimal VentaTotal { get; set; }
        public decimal VentaPendientePagar { get; set; }
        public decimal VentaPagado { get; set; }
        public int VentaEstadoId { get; set; }
        public required string VentaEstadoNombre { get; set; }
        public required List<VentaDetalleDto> VentaDetalles { get; set; }
        public List<DetallePagoResultDto>? DetallePagos { get; set; }

    }
}
