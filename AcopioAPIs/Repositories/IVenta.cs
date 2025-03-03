using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Venta;

namespace AcopioAPIs.Repositories
{
    public interface IVenta
    {
        Task<List<TipoResultDto>> GetTipoVenta();
        Task<List<TipoResultDto>> GetVentaEstado();
        Task<List<TipoResultDto>> GetVentaCliente();
        Task<List<VentaResultDto>> GetVentaResults(DateOnly? fechaDesde, DateOnly? fechaHasta,
            int? tipoVentaId, int? numeroComprobante, int? estadoId);
        Task<ResultDto<VentaDto>> GetVenta(int id);
        Task<ResultDto<VentaResultDto>> InsertVenta(VentaInsertDto ventaDto, List<IFormFile>? imagenes);
        Task<ResultDto<VentaResultDto>> UpdateVenta(VentaUpdateDto ventaDto, List<IFormFile>? imagenes);
        Task<ResultDto<bool>> DeleteVenta(DeleteDto ventaDto);
    }
}
