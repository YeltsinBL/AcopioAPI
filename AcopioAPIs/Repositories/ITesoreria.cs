using AcopioAPIs.DTOs.Tesoreria;

namespace AcopioAPIs.Repositories
{
    public interface ITesoreria
    {
        Task<List<TesoreriaResultDto>> GetAll(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId);
        Task<TesoreriaDto> GetById(int id);
        Task<TesoreriaResultDto> Save(TesoreriaInsertDto tesoreriaInsertDto);
        
    }
}
