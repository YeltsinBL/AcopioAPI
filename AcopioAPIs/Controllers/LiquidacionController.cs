using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiquidacionController : ControllerBase
    {
        private readonly ILiquidacion _liquidacion;

        public LiquidacionController(ILiquidacion liquidacion)
        {
            _liquidacion = liquidacion;
        }

        [HttpGet]
        [Route("Estados")]
        public async Task<ActionResult<List<EstadoResultDto>>> GetLiquidacionEstados()
        {
            var estados = await _liquidacion.GetEstadoResult();
            return Ok(estados);
        }
        [HttpGet]
        public async Task<ActionResult<List<LiquidacionResultDto>>> GetLiquidacionList(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, int? estadoId)
        {
            var results = await _liquidacion.GetLiquidacionResult(fechaDesde, fechaHasta, proveedorId, estadoId);
            return Ok(results);
        }
        [HttpGet]
        [Route("{liquidacionId}")]
        public async Task<ActionResult<LiquidacionDto>> GetLiquidacionById(int liquidacionId)
        {
            var liquidacion = await _liquidacion.GetLiquidacionById(liquidacionId);
            return Ok(liquidacion);
        }
        [HttpGet]
        [Route("CorteAvailable")]
        public async Task<ActionResult<List<LiquidacionCorteResultDto>>> GetLiquidacionCorteAvailable()
        {
            var corte = await _liquidacion.LiquidacionCorteResult();
            return Ok(corte);
        }

        [HttpGet]
        [Route("ALiquidar")]
        public async Task<ActionResult<List<ProveedorResultDto>>> GetProveedorLiquidacion()
        {
            var proveedores = await _liquidacion.GetProveedorLiquidacion();
            return Ok(proveedores);
        }
        [HttpPost]
        public async Task<ActionResult<LiquidacionResultDto>> SaveLiquidacion([FromBody]LiquidacionInsertDto liquidacionInsertDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var liquidacion = await _liquidacion.SaveLiquidacion(liquidacionInsertDto);
            return Ok(liquidacion);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteLiquidacion([FromBody] LiquidacionDeleteDto liquidacionDeleteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _liquidacion.DeleteLiquidacion(liquidacionDeleteDto);
            return Ok(result);
        }
    }
}
