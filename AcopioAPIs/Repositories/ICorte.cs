using AcopioAPIs.DTOs.Corte;

namespace AcopioAPIs.Repositories
{
    public interface ICorte
    {
        Task<List<CorteEstadoDto>> GetCorteEstados();
        Task<List<CorteResultDto>> GetAll(int? tierraId, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId);
        Task<CorteDto> GetById(int id);
        Task<CorteResultDto> Save(CorteInsertDto corteInsertDto);
        Task<bool> DeleteById(int id);
    }
}
