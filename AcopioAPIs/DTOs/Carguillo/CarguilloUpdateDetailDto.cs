namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloUpdateDetailDto
    {
        public int CarguilloDetalleId { get; set; }
        public int CarguilloId { get; set; }
        public int CarguilloTipoId { get; set; }
        public required string CarguilloDetallePlaca { get; set; }
        public bool CarguilloDetalleEstado { get; set; }
    }
}
