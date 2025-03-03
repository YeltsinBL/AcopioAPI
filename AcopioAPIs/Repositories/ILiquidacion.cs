using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Liquidacion;

namespace AcopioAPIs.Repositories
{
    public interface ILiquidacion
    {
        Task<List<EstadoResultDto>> GetEstadoResult();
        Task<List<LiquidacionResultDto>> GetLiquidacionResult(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, int? estadoId);
        Task<LiquidacionDto> GetLiquidacionById(int liquidacionId);
        Task<ResultDto<LiquidacionResultDto>> SaveLiquidacion(LiquidacionInsertDto liquidacionInsertDto, List<IFormFile>? imagenes);
        Task<ResultDto<LiquidacionResultDto>> UpdateLiquidacion(LiquidacionUpdateDto liquidacionUpdateDto, List<IFormFile>? imagenes);
        Task<ResultDto<int>> DeleteLiquidacion(LiquidacionDeleteDto liquidacionDeleteDto);
        Task<List<LiquidacionCorteResultDto>> LiquidacionCorteResult();
        Task<List<PersonaResultDto>> GetProveedorLiquidacion();
    }
}
