using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Reporte;

namespace AcopioAPIs.Repositories
{
    public interface IReporte
    {
        Task<ResultDto<List<ReporteGastoResult>>> GetResultGasto(int? PersonaId, DateTime? FechaDesde, DateTime? FechaHasta);
    }
}
