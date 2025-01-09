using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Models;

namespace AcopioAPIs.Repositories
{
    public interface ITierra
    {
        Task<List<TierraResultDto>> GetTierras(string? tierraUC, string? tierraCampo, string? tierraSector, string? tierraValle);
        Task<TierraResultDto> GetTierraById(int id);
        Task<TierraResultDto> Save(TierraInsertDto tierraInsertDto);
        Task<TierraResultDto> Update(TierraUpdateDto tierraUpdateDto);
        Task<bool> Delete(TierraDeleteDto tierraDeleteDto);
        Task<List<TierraResultDto>> GetAvailableTierras();

    }
}
