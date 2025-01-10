using AcopioAPIs.DTOs.AsignarTierra;

namespace AcopioAPIs.Repositories
{
    public interface IAsignarTierra
    {
        Task<List<AsignarTierraDto>> GetAll(string? tierraUC, string? proveedorUT, DateOnly? fechaDesde, DateOnly? fechaHasta);
        Task<AsignarTierraDto> GetById(int id);
        Task<AsignarTierraResultDto> Save(AsignarTierraInsertDto asignarTierraInsertDto);
        Task<AsignarTierraResultDto> Update(AsignarTierraUpdateDto asignarTierraUpdateDto);
        Task<bool> Delete(AsignarTierraDeleteDto asignarTierraDeleteDto);
    }
}
