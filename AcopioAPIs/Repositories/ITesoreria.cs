using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Tesoreria;

namespace AcopioAPIs.Repositories
{
    public interface ITesoreria
    {
        Task<List<TesoreriaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId);
        Task<TesoreriaDto> GetById(int id);
        Task<ResultDto<TesoreriaResultDto>> Save(TesoreriaInsertDto tesoreriaInsertDto);
        Task<ResultDto<TesoreriaResultDto>> Update(TesoreriaUpdateDto tesoreriaUpdateDto);


    }
}
