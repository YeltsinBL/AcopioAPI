namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloDto
    {
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public int CarguilloTipoId { get; set; }
        public required string CarguilloTipoDescripcion { get; set; }
        public bool CarguilloEstado { get; set; }
        public List<CarguilloDetalleDto>? CarguilloDetalle { get; set; }
    }
}
