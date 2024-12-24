using AcopioAPIs.DTOs.Proveedor;

namespace AcopioAPIs.Repositories
{
    public interface IProveedor
    {
        Task<List<ProveedorGroupedDto>> List(string? ut, string? nombre, bool? estado);
        Task<ProveedorDTO> Get(int id);
        Task<ProveedorResultDto> Save(ProveedorInsertDto proveedor);
        Task<ProveedorResultDto> Update(ProveedorUpdateDto proveedor);
        Task<bool> Delete(ProveedorDeleteDto deleteDto);
        Task<List<ProveedorResultDto>> GetAvailableProveedor();
    }
}
