using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteUpdateDto:UserUpdateDto
    {
        public int ServicioTransporteId { get; set; }
        public DateOnly ServicioTransporteFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public int ServicioTransporteTicketCantidad { get; set; }
        public required string ServicioTransporteEstadoDescripcion { get; set; }
    }
}
