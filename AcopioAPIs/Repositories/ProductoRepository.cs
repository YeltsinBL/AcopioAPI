﻿using AcopioAPIs.DTOs.Common;
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

        public async Task<List<ProductoDto>> GetAll(string? nombre, bool? estado)
        {
            var query = from product in _dbacopioContext.Productos
                        where (nombre == null || product.ProductoNombre.Contains(nombre))
                        && (estado == null || product.ProductoStatus == estado)
                        select new ProductoDto
                        {
                            ProductoId = product.ProductoId,
                            ProductoNombre = product.ProductoNombre,
                            ProductoCantidad = product.ProductoCantidad,
                            ProductoPrecioVenta = product.ProductoPrecioVenta,
                            ProductoStatus = product.ProductoStatus
                        };

            return await query.ToListAsync();
        }

        public async Task<ResultDto<ProductoDto>> GetById(int id)
        {
            try
            {
                var producto = await _dbacopioContext.Productos.FindAsync(id)
                    ?? throw new KeyNotFoundException("Producto no encontrado");
                return new ResultDto<ProductoDto>
                {
                    Result = true,
                    ErrorMessage = "Producto recuperado",
                    Data = new ProductoDto
                    {
                        ProductoId = producto.ProductoId,
                        ProductoNombre = producto.ProductoNombre,
                        ProductoCantidad = producto.ProductoCantidad,
                        ProductoPrecioVenta = producto.ProductoPrecioVenta,
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
                var exist = await _dbacopioContext.Productos.AnyAsync(p => p.ProductoNombre.Equals(producto.ProductoNombre));
                if (exist) throw new Exception("El producto ya existe");
                var product = new Producto
                {
                    ProductoNombre = producto.ProductoNombre,
                    ProductoCantidad = 0,
                    ProductoPrecioVenta = producto.ProductoPrecioVenta,
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
                        ProductoCantidad = product.ProductoCantidad,
                        ProductoPrecioVenta = product.ProductoPrecioVenta,
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
                var product = await _dbacopioContext.Productos.FindAsync(producto.ProductoId)
                    ?? throw new KeyNotFoundException("Producto no encontrado");
                var exist = await _dbacopioContext.Productos.AnyAsync(p => p.ProductoNombre.Equals(producto.ProductoNombre)
                    && p.ProductoId != producto.ProductoId);
                if (exist) throw new Exception("El producto ya existe");
                product.ProductoNombre = producto.ProductoNombre;
                product.ProductoPrecioVenta = producto.ProductoPrecioVenta;
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
                        ProductoCantidad = product.ProductoCantidad,
                        ProductoPrecioVenta = product.ProductoPrecioVenta,
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
                if(!product.ProductoStatus) throw new Exception("El producto ya está desactivado");
                product.ProductoStatus = false;
                product.UserModifiedAt = producto.UserModifiedAt;
                product.UserModifiedName = producto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<bool>
                {
                    Result = true,
                    ErrorMessage = "Producto desactivado",
                    Data = true
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
