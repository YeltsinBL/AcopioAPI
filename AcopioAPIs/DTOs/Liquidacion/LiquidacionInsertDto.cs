using AcopioAPIs.DTOs.Common;
using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Liquidacion
{
    public class LiquidacionInsertDto: UserInsertDto
    {
        public int PersonaId { get; set; }
        public int TierraId {get; set; }
        public int ProveedorId {get; set; }

        public DateOnly LiquidacionFechaInicio {get; set; }
        public DateOnly LiquidacionFechaFin {get; set; }
        public decimal LiquidacionPesoBruto {get; set; }
        public decimal LiquidacionPesoNeto {get; set; }

        public decimal LiquidacionToneladaPrecioCompra {get; set; }
        public decimal LiquidacionToneladaTotal {get; set; }

        public decimal? LiquidacionFinanciamientoACuenta {get; set; }

        public decimal LiquidacionPagar {get; set; }
        //public int LiquidacionEstadoId {get; set; }
        public decimal? LiquidacionAdicionalTotal { get; set; }
        public required List<LiquidacionTicketsDto> LiquidacionTickets {get; set; }
        public List<LiquidacionInsertFinanciamientoDto>? LiquidacionFinanciamientos { get; set; }
        public List<LiquidacionAdicionalesDto>? LiquidacionAdicionales { get; set; }

    }
    public class LiquidacionTicketsDto
    {
        public int TicketId { get; set; }
    }
    public class LiquidacionAdicionalesDto
    {
        public required string LiquidacionAdicionalMotivo { get; set; }
        public decimal LiquidacionAdicionalTotal { get; set; }
    }

}
