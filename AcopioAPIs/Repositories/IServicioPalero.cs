using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Servicio;

namespace AcopioAPIs.Repositories
{
    public interface IServicioPalero
    {
        Task<List<ServicioResultDto>> ListServiciosPalero(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId);
        Task<ServicioDto> GetServicioPalero(int servicioPaleroId);
        Task<ResultDto<ServicioResultDto>> SaveServicioPalero(ServicioPaleroInsertDto servicioTransporteInsertDto);
        Task<ResultDto<ServicioResultDto>> UpdateServicioPalero(ServicioUpdateDto servicioTransporteUpdateDto);
        Task<ResultDto<int>> DeleteServicioPalero(ServicioDeleteDto servicioTransporteDeleteDto);
        Task<List<ServicioDto>> GetListServicioTransporteAvailable();
    }
}
