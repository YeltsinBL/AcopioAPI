using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.Models;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorteController : ControllerBase
    {
        private readonly ICorte _corte;
        public CorteController(ICorte corte) 
        {
            _corte = corte;
        }
        [HttpGet]
        [Route("Estados")]
        public async Task<ActionResult<List<CorteEstadoDto>>> GetCorteEstado()
        {
            var estados = await _corte.GetCorteEstados();
            return Ok(estados);
        }
        [HttpGet]
        public async Task<ActionResult<List<CorteResultDto>>> GetCorteAll(DateOnly? fechaDesde,
            DateOnly? fechaHasta, int? tierraId, int? estadoId)
        {
            var tickets = await _corte.GetAll(fechaDesde, fechaHasta, tierraId, estadoId);
            return Ok(tickets);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CorteDto>> GetCorteById(int id)
        {
            var ticket = await _corte.GetById(id);
            return Ok(ticket);
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<CorteResultDto>>> Save([FromBody] CorteInsertDto corteInsert)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var corte = await _corte.Save(corteInsert);
                return Ok(corte);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpDelete]
        public async Task<ActionResult<ResultDto<int>>> Delete([FromBody] CorteDeleteDto corteDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var cortes = await _corte.Delete(corteDeleteDto);
                return Ok(cortes);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
