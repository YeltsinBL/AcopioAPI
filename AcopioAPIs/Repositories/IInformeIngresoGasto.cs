using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.DTOs.InformeIngresoGasto;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Servicio;

namespace AcopioAPIs.Repositories
{
    public interface IInformeIngresoGasto
    {
        Task<List<InformeResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, bool? estadoId);
        Task<ResultDto<InformeDto>> GetInformeById(int informeId);
        Task<ResultDto<InformeResultDto>> Save(InformeInsertDto informeInsertDto);
        Task<ResultDto<InformeResultDto>> Update(InformeUpdateDto informeUpdateDto);
        Task<ResultDto<bool>> Delete(InformeDeleteDto informeDeleteDto);
        Task<List<LiquidacionResultDto>> GetLiquidacions(int personaId);
        Task<List<ServicioResultDto>> GetServiciosTransporte();
        Task<List<ServicioResultDto>> GetServiciosPalero();
        Task<List<CorteResultDto>> GetCorteResults(int tierraId);
    }
}
