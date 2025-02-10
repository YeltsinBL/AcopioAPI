using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionUpdateDto: UpdateDto
    {
        public int LiquidacionId { get; set; }
        public DateOnly LiquidacionFechaInicio { get; set; }
        public DateOnly LiquidacionFechaFin { get; set; }
        public decimal LiquidacionPesoNeto { get; set; }

        public decimal LiquidacionToneladaPrecioCompra { get; set; }
        public decimal LiquidacionToneladaTotal { get; set; }

        public decimal? LiquidacionFinanciamientoACuenta { get; set; }
        public decimal? LiquidacionAdicionalTotal { get; set; }
        public decimal LiquidacionPagar { get; set; }
        public List<LiquidacionUpdateFinanciamientoDto>? LiquidacionFinanciamientos { get; set; }
        public List<LiquidacionUpdateAdicionalesDto>? LiquidacionAdicionales { get; set; }

    }
    public class LiquidacionUpdateFinanciamientoDto: LiquidacionInsertFinanciamientoDto
    {
        public int LiquidacionFinanciamientoId { get; set; }
        public bool LiquidacionFinanciamientoStatus { get; set; }

    }
    public class LiquidacionUpdateAdicionalesDto: LiquidacionAdicionalesDto
    {
        public int LiquidacionAdicionalId { get; set; }
        public bool LiquidacionAdicionalStatus { get; set; }
    }
}
