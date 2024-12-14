using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Recojo
{
    public class RecojoInsertDto:UserInsertDto
    {
        public DateOnly RecojoFechaInicio { get; set; }
        public DateOnly RecojoFechaFin { get; set; }
        public int RecojoTicketCantidad { get; set; }
        public int RecojoCamionesCantidad { get; set; }
        public decimal RecojoCamionesPrecio { get; set; }
        public int RecojoDiasCantidad { get; set; }
        public decimal RecojoDiasPrecio { get; set; }
        public decimal RecojoTotalPrecio { get; set; }
    }
}
