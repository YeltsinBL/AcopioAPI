using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.DTOs.InformeIngresoGasto;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeIngresoGastoController : ControllerBase
    {
        private readonly IInformeIngresoGasto _informeIngresoGasto;

        public InformeIngresoGastoController(IInformeIngresoGasto informeIngresoGasto)
        {
            _informeIngresoGasto = informeIngresoGasto;
        }

        [HttpGet]
        public async Task<ActionResult<List<InformeResultDto>>> GetIngresoGasto(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, bool? estadoId)
        {
            var results = await _informeIngresoGasto.GetAll(fechaDesde, fechaHasta, proveedorId, estadoId);
            return Ok(results);
        }
        [HttpGet("Cortes/{tierraId}")]
        public async Task<ActionResult<List<CorteResultDto>>> GetCorte(int tierraId)
        {
            var results = await _informeIngresoGasto.GetCorteResults(tierraId);
            return Ok(results);
        }
        [HttpGet("Paleros")]
        public async Task<ActionResult<List<ServicioResultDto>>> GetPaleros()
        {
            var results = await _informeIngresoGasto.GetServiciosPalero();
            return Ok(results);
        }
        [HttpGet("Transportes")]
        public async Task<ActionResult<List<ServicioResultDto>>> GetTransportes()
        {
            var results = await _informeIngresoGasto.GetServiciosTransporte();
            return Ok(results);
        }
        [HttpGet("Liquidaciones/{personaId}")]
        public async Task<ActionResult<List<LiquidacionResultDto>>> GetLiquidaciones(int personaId)
        {
            var results = await _informeIngresoGasto.GetLiquidacions(personaId);
            return Ok(results);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<InformeDto>>> GetInforme(int id)
        {
            try
            {
                var response = await _informeIngresoGasto.GetInformeById(id);
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
        [HttpPost]
        public async Task<ActionResult<ResultDto<InformeResultDto>>> Save([FromBody] InformeInsertDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var response = await _informeIngresoGasto.Save(dto);
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
        [HttpDelete]
        public async Task<ActionResult<ResultDto<bool>>> Anular([FromBody] InformeDeleteDto informeDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var response = await _informeIngresoGasto.Delete(informeDeleteDto);
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
