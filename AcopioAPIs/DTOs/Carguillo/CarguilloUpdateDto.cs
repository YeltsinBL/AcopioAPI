namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloUpdateDto
    {
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public int CarguilloTipoId { get; set; }
        public required string UserModifiedName { get; set; }
        public DateTime UserModifiedAt { get; set; }
        
        public List<CarguilloUpdateDetailDto>? CarguilloDetalle { get; set; }
    }
}
