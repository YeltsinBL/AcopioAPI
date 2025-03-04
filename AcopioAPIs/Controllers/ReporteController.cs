using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Reporte;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReporte _reporte;

        public ReporteController(IReporte reporte)
        {
            _reporte = reporte;
        }
        [HttpGet("Gastos")]
        public async Task<ActionResult<ResultDto<List<ReporteGastoResult>>>> GetListReporteGasto(int? PersonaId, DateTime? FechaDesde, DateTime? FechaHasta)
        {
            try
            {
                var response = await _reporte.GetResultGasto(PersonaId, FechaDesde, FechaHasta);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<LiquidacionResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
