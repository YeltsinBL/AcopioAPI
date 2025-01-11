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
        public async Task<ActionResult<CorteResultDto>> Save([FromBody] CorteInsertDto corteInsert)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var corte = await _corte.Save(corteInsert);
            return Ok(corte);
        }
    }
}
