using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionFinanciamientoDto
    {
        public int LiquidacionFinanciamientoId { get; set; }
        public int LiquidacionId {get; set; }
        public DateTime LiquidacionFinanciamientoFecha {get; set; }
        public decimal LiquidacionFinanciamientoACuenta {get; set; }
        public required string LiquidacionFinanciamientoTiempo {get; set; }
        public decimal LiquidacionFinanciamientoInteresMes {get; set; }
        public decimal LiquidacionFinanciamientoInteres {get; set; }
        public decimal LiquidacionFinanciamientoTotal {get; set; }
        public bool LiquidacionFinanciamientoStatus {get; set; }
        public string? LiquidacionFinanciamientoImagen { get; set; }
        public string? LiquidacionFinanciamientoComentario { get; set; }

    }
}
