using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.ServicioTransporte
{
    public class ServicioTransporteInsertDto: UserInsertDto
    {
        public DateOnly ServicioTransporteFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioTransportePrecio { get; set; }
        public int ServicioTransporteTicketCantidad { get; set; }
    }
}
