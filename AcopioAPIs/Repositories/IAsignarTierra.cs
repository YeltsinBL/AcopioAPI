using AcopioAPIs.DTOs.AsignarTierra;

namespace AcopioAPIs.Repositories
{
    public interface IAsignarTierra
    {
        Task<List<AsignarTierraResultDto>> GetAll();
        Task<AsignarTierraDto> GetById(int id);
        Task<AsignarTierraResultDto> Save(AsignarTierraInsertDto asignarTierraInsertDto);
        Task<AsignarTierraResultDto> Update(AsignarTierraUpdateDto asignarTierraUpdateDto);
        Task<bool> Delete(AsignarTierraDeleteDto asignarTierraDeleteDto);
    }
}
