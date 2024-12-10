using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloDetalleDto
    {
        public int CarguilloDetalleId { get; set; }
        public int CarguilloId { get; set; }
        public int CarguilloTipoId { get; set; }
        public required string CarguilloTipoDescripcion { get; set; }
        public required string CarguilloDetallePlaca { get; set; }
        public bool CarguilloDetalleEstado { get; set; }
    }
}
