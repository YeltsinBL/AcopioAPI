using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Compra;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CompraRepository : ICompra
    {
        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;

        public CompraRepository(DbacopioContext dbacopioContext, IConfiguration configuration)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
        }

        public async Task<List<CompraResultDto>> GetCompraResults(DateOnly? fechaDesde, DateOnly? fechaHasta, int? tipoComprobanteId, string? numeroComprobante, bool? estadoId)
        {
            var query = from compra in _dbacopioContext.Compras
                        join tipoComprobante in _dbacopioContext.TipoComprobantes 
                            on compra.TipoComprobanteId equals tipoComprobante.TipoComprobanteId
                        join distribuidor in _dbacopioContext.Distribuidors 
                            on compra.DistribuidorId equals distribuidor.DistribuidorId
                        where (fechaDesde == null || compra.CompraFecha >= fechaDesde) &&
                            (fechaHasta == null || compra.CompraFecha <= fechaHasta) &&
                            (tipoComprobanteId == null || compra.TipoComprobanteId == tipoComprobanteId) &&
                            (numeroComprobante == null || compra.CompraNumeroComprobante.Contains(numeroComprobante)) &&
                            (estadoId == null || compra.CompraStatus == estadoId)
                        select new CompraResultDto
                        {
                            CompraId = compra.CompraId,
                            CompraFecha = compra.CompraFecha,
                            TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                            CompraNumeroComprobante = compra.CompraNumeroComprobante,
                            DistribuidorNombre = distribuidor.DistribuidorNombre,
                            CompraTotal = compra.CompraTotal,
                            CompraStatus = compra.CompraStatus
                        };
            return await query.ToListAsync();
        }

        public async Task<ResultDto<CompraDto>> GetCompra(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_CompraGetById", 
                    new { CompraId = id }, 
                    commandType: CommandType.StoredProcedure);
                var compra = await multi.ReadFirstOrDefaultAsync<CompraDto>()
                    ?? throw new Exception("No se encontró la compra");
                compra.CompraDetalles = (await multi.ReadAsync<CompraDetalleDto>()).ToList();
                return new ResultDto<CompraDto>
                {
                    Result = true,
                    ErrorMessage = "Compra encontrada",
                    Data = compra
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<CompraResultDto>> InsertCompra(CompraInsertDto compraDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (compraDto == null) throw new Exception("No se enviaron datos para guardar la compra");
                if(compraDto.CompraDetalles == null || compraDto.CompraDetalles.Count == 0)
                    throw new Exception("La compra no tiene detalles");
                var tipoComprobante = await GetTipoComprobante(compraDto.TipoComprobanteId)
                    ?? throw new Exception("No se encontró el tipo de comprobante.");
                var distribuidor = await GetDistribuidor(compraDto.DistribuidorId)
                    ?? throw new Exception("No se encontró el distribuidor.");
                var compra = new Compra
                {
                    CompraFecha = compraDto.CompraFecha,
                    TipoComprobanteId = compraDto.TipoComprobanteId,
                    CompraNumeroComprobante = compraDto.CompraNumeroComprobante,
                    DistribuidorId = compraDto.DistribuidorId,
                    CompraTotal = compraDto.CompraTotal,
                    CompraStatus = true,
                    UserCreatedAt = compraDto.UserCreatedAt,
                    UserCreatedName = compraDto.UserCreatedName
                };
                foreach (var detalle in compraDto.CompraDetalles)
                {
                    var producto = await GetProducto(detalle.ProductoId)
                        ?? throw new Exception("No se encontró el producto.");
                    compra.CompraDetalles.Add(new CompraDetalle
                    {
                        ProductoId = detalle.ProductoId,
                        CompraDetalleCantidad = detalle.Cantidad,
                        CompraDetallePrecio = detalle.Precio,
                        CompraDetalleStatus = true,
                        UserCreatedName = compraDto.UserCreatedName,
                        UserCreatedAt = compraDto.UserCreatedAt
                    });       
                    
                    producto.ProductoCantidad += detalle.Cantidad;
                    producto.UserModifiedAt = compraDto.UserCreatedAt;
                    producto.UserModifiedName = compraDto.UserCreatedName;
                }
                _dbacopioContext.Compras.Add(compra);
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<CompraResultDto>
                {
                    Result = true,
                    ErrorMessage = "Compra guardada",
                    Data = new CompraResultDto
                    {
                        CompraId = compra.CompraId,
                        CompraFecha = compra.CompraFecha,
                        TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                        CompraNumeroComprobante = compra.CompraNumeroComprobante,
                        DistribuidorNombre = distribuidor.DistribuidorNombre,
                        CompraTotal = compra.CompraTotal
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ResultDto<CompraResultDto>> UpdateCompra(CompraUpdateDto compraDto)
        {
            try
            {
                if (compraDto == null) throw new Exception("No se enviaron datos para modificar la compra");
                
                var tipoComprobante = await GetTipoComprobante(compraDto.TipoComprobanteId)
                    ?? throw new Exception("No se encontró el Tipo de Comprobante.");
                var distribuidor = await GetDistribuidor(compraDto.DistribuidorId)
                    ?? throw new Exception("No se encontró el Distribuidor.");
                var compra = await _dbacopioContext.Compras
                    .FirstOrDefaultAsync(c => c.CompraId == compraDto.CompraId)
                    ?? throw new Exception("No se encontró la compra");
                compra.CompraFecha = compraDto.CompraFecha;
                compra.TipoComprobanteId = compraDto.TipoComprobanteId;
                compra.CompraNumeroComprobante = compraDto.CompraNumeroComprobante;
                compra.DistribuidorId = compraDto.DistribuidorId;
                compra.UserModifiedAt = compraDto.UserModifiedAt;
                compra.UserModifiedName = compraDto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<CompraResultDto>
                {
                    Result = true,
                    ErrorMessage = "Compra modificada",
                    Data = new CompraResultDto
                    {
                        CompraId = compra.CompraId,
                        CompraFecha = compra.CompraFecha,
                        TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                        CompraNumeroComprobante = compra.CompraNumeroComprobante,
                        DistribuidorNombre = distribuidor.DistribuidorNombre,
                        CompraTotal = compra.CompraTotal
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<bool>> DeleteCompra(CompraDeleteDto compraDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (compraDto == null) throw new Exception("No se enviaron datos para anular la compra");
                var compra = await _dbacopioContext.Compras
                    .Include(c => c.CompraDetalles)
                    .FirstOrDefaultAsync(c => c.CompraId == compraDto.CompraId)
                    ?? throw new Exception("No se encontró la compra");
                compra.CompraStatus = false;
                compra.UserModifiedAt = compraDto.UserModifiedAt;
                compra.UserModifiedName = compraDto.UserModifiedName;
                foreach (var detalle in compra.CompraDetalles)
                {
                    var producto = await _dbacopioContext.Productos.FindAsync(detalle.ProductoId)
                        ?? throw new Exception("No se encontró el producto.");
                    if(producto.ProductoCantidad < detalle.CompraDetalleCantidad)
                        throw new Exception("No hay suficiente cantidad de producto para anular la compra");
                    producto.ProductoCantidad -= detalle.CompraDetalleCantidad;
                    producto.UserModifiedAt = compraDto.UserModifiedAt;
                    producto.UserModifiedName = compraDto.UserModifiedName;

                    detalle.CompraDetalleStatus = false;
                    detalle.UserModifiedAt= compraDto.UserModifiedAt;
                    detalle.UserModifiedName= compraDto.UserModifiedName;
                }
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<bool>
                {
                    Result = true,
                    ErrorMessage = "Compra anulada",
                    Data = true
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<TipoComprobante?> GetTipoComprobante(int tipocomprobanteId)
        {
            return await _dbacopioContext.TipoComprobantes.FindAsync(tipocomprobanteId);
        }
        private async Task<Distribuidor?> GetDistribuidor(int distribuidorId)
        {
            return await _dbacopioContext.Distribuidors.FindAsync(distribuidorId);
        }
        private async Task<Producto?> GetProducto(int productoId)
        {
            return await _dbacopioContext.Productos.FindAsync(productoId);
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
