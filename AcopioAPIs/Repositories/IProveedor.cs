using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;

namespace AcopioAPIs.Repositories
{
    public interface IProveedor
    {
        Task<List<ProveedorResultDto>> List();
        Task<Proveedor> Get(int id);
        Task<ProveedorResultDto> Save(ProveedorInsertDto proveedor);
        Task<ProveedorResultDto> Update(ProveedorUpdateDto proveedor);
        Task<bool> Delete(int id);
    }
}
