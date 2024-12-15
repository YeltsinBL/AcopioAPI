using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.ServicioTransporte;

namespace AcopioAPIs.Repositories
{
    public interface IServicioTransporte
    {
        Task<List<EstadoResultDto>> ListEstados();
        Task<List<ServicioTransporteResultDto>> ListServiciosTransporte(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId);
        Task<ServicioTransporteDto> GetServicioTransporte(int servicioTransporteId);
        Task<ServicioTransporteResultDto> SaveServicioTransporte(ServicioTransporteInsertDto servicioTransporteInsertDto);
        Task<ServicioTransporteResultDto> UpdateServicioTransporte(ServicioTransporteUpdateDto servicioTransporteUpdateDto);
        Task<bool> DeleteServicioTransporte(ServicioTransporteDeleteDto servicioTransporteDeleteDto);
    }
}
