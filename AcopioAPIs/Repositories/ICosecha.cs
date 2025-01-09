using AcopioAPIs.DTOs.Cosecha;

namespace AcopioAPIs.Repositories
{
    public interface ICosecha
    {
        Task<List<CosechaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta,
            string? tierraUC, string? proveedotUT, int? tipoCosechaId);
        Task<CosechaDto> GetById(int id);
        Task<CosechaResultDto> Save(CosechaInsertDto insert);
        Task<CosechaResultDto> Update(CosechaUpdateDto update);
        Task<List<CosechaTipoDto>> GetTipo();
    }
}
