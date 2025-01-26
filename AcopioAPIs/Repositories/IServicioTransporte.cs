using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Servicio;

namespace AcopioAPIs.Repositories
{
    public interface IServicioTransporte
    {
        Task<List<EstadoResultDto>> ListEstados();
        Task<List<ServicioResultDto>> ListServiciosTransporte(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId);
        Task<ServicioDto> GetServicioTransporte(int servicioTransporteId);
        Task<ResultDto<ServicioResultDto>> SaveServicioTransporte(ServicioInsertDto servicioTransporteInsertDto);
        Task<ServicioResultDto> UpdateServicioTransporte(ServicioUpdateDto servicioTransporteUpdateDto);
        Task<ResultDto<int>> DeleteServicioTransporte(ServicioDeleteDto servicioTransporteDeleteDto);
    }
}
