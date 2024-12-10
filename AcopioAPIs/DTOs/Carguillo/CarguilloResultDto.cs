namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloResultDto
    {
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public required string CarguilloTipoDescripcion { get; set; }
        public bool CarguilloEstado { get; set; }
    }
}
