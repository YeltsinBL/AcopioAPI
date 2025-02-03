using AcopioAPIs.DTOs.Tipos;

namespace AcopioAPIs.Repositories
{
    public interface ITipos
    {
        Task<List<TipoCompronteResultDto>> GetTipoComprontes();
    }
}
