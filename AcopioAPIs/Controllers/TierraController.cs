using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TierraController : ControllerBase
    {
        private readonly ITierra _tierra;

        public TierraController(ITierra tierra)
        {
            _tierra = tierra;
        }
        [HttpGet]
        public async Task<ActionResult<List<TierraResultDto>>>GetAll()
        {
            var tierras = await _tierra.GetTierras();
            return Ok(tierras);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TierraResultDto>> GetById(int id)
        {
            var tierra = await _tierra.GetTierraById(id);
            if (tierra == null) return NotFound("Tierra no encontrada");
            return Ok(tierra);
        }
        [HttpGet]
        [Route("Asignar/Available")]
        public async Task<ActionResult<List<TierraResultDto>>> GetAvailable()
        {
            var tierras = await _tierra.GetAvailableTierras();
            return Ok(tierras);
        }

        [HttpPost]
        public async Task<ActionResult<TierraResultDto>> CreateTierra([FromBody] TierraInsertDto tierraInsertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _tierra.Save(tierraInsertDto);
            return CreatedAtAction(nameof(GetById), new { id = result.TierraId }, result);
        }
        [HttpPut]
        public async Task<ActionResult<TierraResultDto>> UpdateTierra([FromBody] TierraUpdateDto tierraUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _tierra.Update(tierraUpdateDto);
            return CreatedAtAction(nameof(GetById), new { id = result.TierraId }, result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTierra(int id)
        {
            var result = await _tierra.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }


    }
}
