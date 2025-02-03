using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Compra;

namespace AcopioAPIs.Repositories
{
    public interface ICompra
    {
        Task<List<CompraResultDto>> GetCompraResults(DateOnly? fechaDesde, DateOnly? fechaHasta, 
            int? tipoComprobanteId, string? numeroComprobante, bool? estadoId);
        Task<ResultDto<CompraDto>> GetCompra(int id);
        Task<ResultDto<CompraResultDto>> InsertCompra(CompraInsertDto compra);
        Task<ResultDto<bool>> DeleteCompra(CompraDeleteDto compra);
    }
}
