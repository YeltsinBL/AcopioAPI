using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionResultDto
    {
        public int LiquidacionId { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo { get; set; }
        public required string ProveedorUT { get; set; }

        public DateOnly LiquidacionFechaInicio { get; set; }
        public DateOnly LiquidacionFechaFin { get; set; }
        public decimal LiquidacionPesoNeto { get; set; }
        public decimal LiquidacionPesoBruto { get; set; }
        public decimal LiquidacionToneladaTotal { get; set; }
        public decimal? LiquidacionFinanciamientoACuenta { get; set; }
        public decimal? LiquidacionAdicionalTotal { get; set; }
        public decimal LiquidacionPagar { get; set; }
        public required string LiquidacionEstadoDescripcion { get; set; }
        public decimal? LiquidacionToneladaPrecioCompra { get; set; }

    }
}

