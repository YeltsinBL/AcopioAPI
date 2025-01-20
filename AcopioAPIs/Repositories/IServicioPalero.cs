using AcopioAPIs.DTOs.Servicio;

namespace AcopioAPIs.Repositories
{
    public interface IServicioPalero
    {
        Task<List<ServicioResultDto>> ListServiciosPalero(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId);
        Task<ServicioDto> GetServicioPalero(int servicioPaleroId);
        Task<ServicioResultDto> SaveServicioPalero(ServicioPaleroInsertDto servicioTransporteInsertDto);
        Task<ServicioResultDto> UpdateServicioPalero(ServicioUpdateDto servicioTransporteUpdateDto);
        Task<bool> DeleteServicioPalero(ServicioDeleteDto servicioTransporteDeleteDto);
        Task<List<ServicioDto>> GetListServicioTransporteAvailable();
    }
}
