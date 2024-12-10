
namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloInsertDto
    {
       
        public required string CarguilloTitular { get; set; }
        public int CarguilloTipoId { get; set; }
        public required string UserCreatedName { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public List<CarguilloInsertDetailDto>? CarguilloDetalle { get; set; }
    }
}
