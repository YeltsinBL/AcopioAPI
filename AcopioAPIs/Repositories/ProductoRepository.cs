using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Producto;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class ProductoRepository : IProducto
    {
        private readonly DbacopioContext _dbacopioContext;

        public ProductoRepository(DbacopioContext dbacopioContext)
        {
            _dbacopioContext = dbacopioContext;
        }

        public async Task<List<ProductoDto>> GetAll(string? nombre, bool? estado, bool? stock)
        {
            var query = from product in _dbacopioContext.Productos
                        join tipo in _dbacopioContext.ProductoTipos
                            on product.ProductoTipoId equals tipo.ProductoTipoId into tipoList
                        from tipo in tipoList.DefaultIfEmpty() // LEFT JOIN
                        where (nombre == null || product.ProductoNombre.Contains(nombre))
                        && (estado == null || product.ProductoStatus == estado)
                        && (stock == null || (stock == true && product.ProductoCantidad > 0) || (stock==false && product.ProductoCantidad==0))
                        select new ProductoDto
                        {
                            ProductoId = product.ProductoId,
                            ProductoNombre = product.ProductoNombre,
                            ProductoStock = product.ProductoCantidad,
                            ProductoPrecioVenta = product.ProductoPrecioVenta,
                            ProductoTipoDetalle = tipo.ProductoTipoDetalle,
                            ProductoStatus = product.ProductoStatus
                        };

            return await query.ToListAsync();
        }

        public async Task<ResultDto<ProductoDto>> GetById(int id)
        {
            try
            {
                var producto = await _dbacopioContext.Productos
                    .Include(c => c.ProductoTipo)
                    .FirstOrDefaultAsync(c => c.ProductoId == id)
                    ?? throw new KeyNotFoundException("Producto no encontrado");
                return new ResultDto<ProductoDto>
                {
                    Result = true,
                    ErrorMessage = "Producto recuperado",
                    Data = new ProductoDto
                    {
                        ProductoId = producto.ProductoId,
                        ProductoNombre = producto.ProductoNombre,
                        ProductoStock = producto.ProductoCantidad,
                        ProductoPrecioVenta = producto.ProductoPrecioVenta,
                        ProductoTipoId = producto.ProductoTipoId ?? 0,
                        ProductoTipoDetalle = producto.ProductoTipo?.ProductoTipoDetalle ?? "",
                        ProductoStatus = producto.ProductoStatus
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ProductoDto>> Insert(ProductoInsertDto producto)
        {
            try
            {
                if (producto == null)
                    throw new Exception("No se enviaron datos para guardar el producto");
                ProductoTipo? tipo = null;
                if(producto.ProductoTipoId != 0)
                {
                    tipo = await _dbacopioContext.ProductoTipos.FindAsync(producto.ProductoTipoId)
                        ?? throw new KeyNotFoundException("Tipo de Producto no encontrado");
                }
                
                var exist = await _dbacopioContext.Productos.AnyAsync(p => p.ProductoNombre.Equals(producto.ProductoNombre));
                if (exist) throw new Exception("El producto ya existe");
                var product = new Producto
                {
                    ProductoNombre = producto.ProductoNombre,
                    ProductoCantidad = producto.ProductoStock,
                    ProductoPrecioVenta = producto.ProductoPrecioVenta,
                    ProductoTipoId = producto.ProductoTipoId != 0 ? producto.ProductoTipoId : null,
                    ProductoStatus = true,
                    UserCreatedAt = producto.UserCreatedAt,
                    UserCreatedName = producto.UserCreatedName                    
                };
                _dbacopioContext.Productos.Add(product);
                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<ProductoDto>
                {
                    Result = true,
                    ErrorMessage = "Producto guardado",
                    Data = new ProductoDto
                    {
                        ProductoId = product.ProductoId,
                        ProductoNombre = product.ProductoNombre,
                        ProductoStock = product.ProductoCantidad,
                        ProductoPrecioVenta = product.ProductoPrecioVenta,
                        ProductoTipoDetalle = producto.ProductoTipoId != 0 ? tipo!.ProductoTipoDetalle: "",
                        ProductoStatus = product.ProductoStatus
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ProductoDto>> Update(ProductoUpdateDto producto)
        {
            try
            {
                ProductoTipo? tipo = null;
                if (producto.ProductoTipoId != 0)
                {
                    tipo = await _dbacopioContext.ProductoTipos.FindAsync(producto.ProductoTipoId)
                        ?? throw new KeyNotFoundException("Tipo de Producto no encontrado");
                }
                var product = await _dbacopioContext.Productos.FindAsync(producto.ProductoId)
                    ?? throw new KeyNotFoundException("Producto no encontrado");
                var exist = await _dbacopioContext.Productos.AnyAsync(p => p.ProductoNombre.Equals(producto.ProductoNombre)
                    && p.ProductoId != producto.ProductoId);
                if (exist) throw new Exception("El producto ya existe");
                product.ProductoNombre = producto.ProductoNombre;
                product.ProductoPrecioVenta = producto.ProductoPrecioVenta;
                product.ProductoCantidad = producto.ProductoStock;
                product.ProductoTipoId = producto.ProductoTipoId != 0 ? producto.ProductoTipoId : null;
                product.ProductoStatus = true;
                product.UserModifiedAt = producto.UserModifiedAt;
                product.UserModifiedName = producto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();

                return new ResultDto<ProductoDto>
                {
                    Result = true,
                    ErrorMessage = "Producto actualizado",
                    Data = new ProductoDto
                    {
                        ProductoId = producto.ProductoId,
                        ProductoNombre = producto.ProductoNombre,
                        ProductoStock = product.ProductoCantidad,
                        ProductoPrecioVenta = product.ProductoPrecioVenta,
                        ProductoTipoDetalle = producto.ProductoTipoId != 0 ? tipo!.ProductoTipoDetalle : "",
                        ProductoStatus = true
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<bool>> Delete(ProductoDeleteDto producto)
        {
            try
            {
                var product = await _dbacopioContext.Productos.FindAsync(producto.ProductoId)
                    ?? throw new KeyNotFoundException("Producto no encontrado");
                if(!product.ProductoStatus) throw new Exception("El producto ya está inactivo");
                product.ProductoStatus = false;
                product.UserModifiedAt = producto.UserModifiedAt;
                product.UserModifiedName = producto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<bool>
                {
                    Result = true,
                    ErrorMessage = "Producto inactivo",
                    Data = true
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TipoResultDto>> GetTipos()
        {
            try
            {
                var query = from tipos in _dbacopioContext.ProductoTipos
                            where tipos.ProductoTipoStatus == true
                            select new TipoResultDto { 
                                Id = tipos.ProductoTipoId,
                                Nombre = tipos.ProductoTipoDetalle 
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
