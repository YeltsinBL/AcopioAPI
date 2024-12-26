using AcopioAPIs.DTOs.Common;
using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionDto
    {
        public int LiquidacionId { get; set; }
        public int PersonaId { get; set; }
        public required string PersonaNombre { get; set; }
        public int TierraId { get; set; }
        public required string TierraCampo { get; set; }
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public DateTime LiquidacionFechaInicio { get; set; }
        public DateTime LiquidacionFechaFin { get; set; }
        public decimal LiquidacionPesoBruto { get; set; }
        public decimal LiquidacionPesoNeto { get; set; }
        public decimal LiquidacionTotalPesoBruto { get; set; }
        public decimal LiquidacionTotalPesoNeto { get; set; }
        public decimal LiquidacionToneladaPrecioCompra { get; set; }
        public decimal LiquidacionToneladaTotal { get; set; }

        public decimal? LiquidacionFinanciamientoACuenta { get; set; }
        public decimal LiquidacionPagar { get; set; }
        public int LiquidacionEstadoId { get; set; }
        public decimal? LiquidacionAdicionalTotal { get; set; }
        public required string LiquidacionEstadoDescripcion { get; set; }
        public required List<LiquidacionTicketDto> LiquidacionTickets { get; set; }
        public List<LiquidacionFinanciamientoDto>? LiquidacionFinanciamiento { get; set; }
        public List<LiquidacionAdicionalDto>? LiquidacionAdicionals { get; set; }
    }
    public class LiquidacionTicketDto:EsperaTicketDto
    {
        public int LiquidacionTicketId { get; set; }
    }
    public class LiquidacionAdicionalDto
    {
        public int LiquidacionAdicionalId { get; set; }
        public int LiquidacionId {get; set;}
        public required string LiquidacionAdicionalMotivo {get; set;}
        public decimal LiquidacionAdicionalTotal {get; set;}

    }
}

