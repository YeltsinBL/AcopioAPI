using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionInsertFinanciamientoDto
    {
        public DateOnly LiquidacionFinanciamientoFecha { get; set; }
        public decimal LiquidacionFinanciamientoACuenta {get; set; }
        public int LiquidacionFinanciamientoTiempo {get; set; }
        public decimal LiquidacionFinanciamientoInteresMes {get; set; }
        public decimal LiquidacionFinanciamientoInteres {get; set; }
        public decimal LiquidacionFinanciamientoTotal {get; set; }

    }
}