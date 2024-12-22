using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcopioAPIs.Repositories
{
    public interface IProveedor
    {
        Task<List<ProveedorResultDto>> List(string? ut, string? nombre, bool? estado);

        Task<List<ProveedorGroupedDto>> ListNew(string? ut, string? nombre, bool? estado);
        Task<ProveedorDTO> Get(int id);
        Task<ProveedorResultDto> Save(ProveedorInsertDto proveedor);
        Task<ProveedorResultDto> Update(ProveedorUpdateDto proveedor);
        Task<bool> Delete(ProveedorDeleteDto deleteDto);
        Task<List<ProveedorResultDto>> GetAvailableProveedor();
    }
}
