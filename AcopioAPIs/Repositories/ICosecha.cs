using AcopioAPIs.DTOs.Cosecha;

namespace AcopioAPIs.Repositories
{
    public interface ICosecha
    {
        Task<List<CosechaResultDto>> GetAll();
        Task<CosechaResultDto> GetById(int id);
        Task<CosechaResultDto> Save(CosechaInsertDto insert);
        Task<CosechaResultDto> Update(CosechaUpdateDto update);
        Task<List<CosechaTipoDto>> GetTipo();
    }
}
