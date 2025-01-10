namespace AcopioAPIs.DTOs.Carguillo
{
    public class CarguilloPlacasResultDto
    {
        public List<CarguilloDetalleDto>? CarguilloTipoCamion {  get; set; }
        public List<CarguilloDetalleDto>? CarguilloTipoVehiculo { get; set; }
    }
}
