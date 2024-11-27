using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Models;

namespace AcopioAPIs.Repositories
{
    public interface ITierra
    {
        Task<List<TierraResultDto>> GetTierras();
        Task<TierraResultDto> GetTierraById(int id);
        Task<TierraResultDto> Save(TierraInsertDto tierraInsertDto);
        Task<TierraResultDto> Update(TierraUpdateDto tierraUpdateDto);
        Task<bool> Delete(int id);

    }
}
