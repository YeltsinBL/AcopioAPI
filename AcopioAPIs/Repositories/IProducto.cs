using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Producto;

namespace AcopioAPIs.Repositories
{
    public interface IProducto
    {
        Task<List<TipoResultDto>> GetTipos();
        Task<List<ProductoDto>> GetAll(string? nombre, bool? estado, bool? stock);
        Task<ResultDto<ProductoDto>> GetById(int id);
        Task<ResultDto<ProductoDto>> Insert(ProductoInsertDto producto);
        Task<ResultDto<ProductoDto>> Update(ProductoUpdateDto producto);
        Task<ResultDto<bool>> Delete(ProductoDeleteDto producto);
    }
}
