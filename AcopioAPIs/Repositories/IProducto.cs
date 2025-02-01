using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Producto;

namespace AcopioAPIs.Repositories
{
    public interface IProducto
    {
        Task<List<ProductoDto>> GetAll(string? nombre, bool? estado);
        Task<ResultDto<ProductoDto>> GetById(int id);
        Task<ResultDto<ProductoDto>> Insert(ProductoInsertDto producto);
        Task<ResultDto<ProductoDto>> Update(ProductoUpdateDto producto);
        Task<ResultDto<ProductoDto>> Delete(ProductoDeleteDto producto);
    }
}
