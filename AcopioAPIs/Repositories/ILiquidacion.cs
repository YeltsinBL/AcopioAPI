using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Liquidacion;

namespace AcopioAPIs.Repositories
{
    public interface ILiquidacion
    {
        Task<List<EstadoResultDto>> GetEstadoResult();
        Task<List<LiquidacionResultDto>> GetLiquidacionResult(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, int? estadoId);
        Task<LiquidacionDto> GetLiquidacionById(int liquidacionId);
        Task<LiquidacionResultDto> SaveLiquidacion(LiquidacionInsertDto liquidacionInsertDto);
        Task<LiquidacionResultDto> UpdateLiquidacion(LiquidacionUpdateDto liquidacionUpdateDto);
        Task<bool> DeleteLiquidacion(LiquidacionDeleteDto liquidacionDeleteDto);
        Task<List<LiquidacionCorteResultDto>> LiquidacionCorteResult();
        Task<List<LiquidacionPersona>> GetProveedorLiquidacion();
    }
}
