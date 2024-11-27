using AcopioAPIs.DTOs.AsignarTierra;

namespace AcopioAPIs.Repositories
{
    public interface IAsignarTierra
    {
        Task<List<AsignarTierraResultDto>> GetAll();
        Task<AsignarTierraResultDto> GetById(int id);
        Task<AsignarTierraResultDto> Save(AsignarTierraInsertDto asignarTierraInsertDto);
        Task<AsignarTierraResultDto> Update(AsignarTierraUpdateDto asignarTierraUpdateDto);
        Task<bool> Delete(int id);
    }
}
