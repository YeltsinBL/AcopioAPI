using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.FacturaVenta;
using AcopioAPIs.DTOs.InformeIngresoGasto;

namespace AcopioAPIs.Repositories
{
    public interface IFacturaVenta
    {
        Task<List<TipoResultDto>> GetAllEstados();
        Task<List<FacturaVentaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, string? numero, int? estadoId);
        Task<ResultDto<FacturaVentaDto>> GetById(int id);
        Task<ResultDto<FacturaVentaResultDto>> Save(FacturaVentaInsertDto insertDto);
        Task<ResultDto<FacturaVentaResultDto>> Update(FacturaVentaUpdateDto updateDto);


    }
}
