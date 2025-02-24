using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioDetailDto:EsperaTicketDto
    {
        public int ServicioId { get; set; }
        public int ServicioDetalleId { get; set; }
    }
}
