using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Proveedor;

namespace AcopioAPIs.Repositories
{
    public interface IProveedor
    {
        Task<List<ProveedorGroupedDto>> List(string? ut, string? nombre, bool? estado);
        Task<ResultDto<ProveedorDTO>> Get(int id);
        Task<ResultDto<ProveedorResultDto>> Save(ProveedorInsertDto proveedor);
        Task<ResultDto<ProveedorResultDto>> Update(ProveedorUpdateDto proveedor);
        Task<ResultDto<int>> Delete(ProveedorDeleteDto deleteDto);
        Task<List<ProveedorResultDto>> GetAvailableProveedor();
        Task<List<PersonaResultDto>> GetPersonaResults();
    }
}
