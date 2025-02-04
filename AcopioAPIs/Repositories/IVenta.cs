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
            int? tipoComprobanteId, int? numeroComprobante, int? estadoId);
        Task<ResultDto<VentaDto>> GetVenta(int id);
        Task<ResultDto<VentaResultDto>> InsertVenta(VentaInsertDto ventaDto);
        Task<ResultDto<bool>> DeleteVenta(DeleteDto ventaDto);
    }
}
