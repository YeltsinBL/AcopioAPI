using AcopioAPIs.DTOs.AsignarTierra;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignaTierraController : ControllerBase
    {
        private readonly IAsignarTierra _asignarTierra;

        public AsignaTierraController(IAsignarTierra asignarTierra)
        {
            _asignarTierra = asignarTierra;
        }
        [HttpGet]
        public async Task<ActionResult<List<AsignarTierraDto>>> GetAll()
        {
            var asignaTierra = await _asignarTierra.GetAll();
            return Ok(asignaTierra);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AsignarTierraDto>> GetById(int id)
        {
            var asignaTierra = await _asignarTierra.GetById(id);
            if (asignaTierra == null) return NotFound("Tierra Asignada no encontrada");
            return Ok(asignaTierra);
        }
        [HttpPost]
        public async Task<ActionResult<AsignarTierraResultDto>> Create([FromBody] AsignarTierraInsertDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _asignarTierra.Save(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.AsignarTierraId }, result);
        }
        [HttpPut]
        public async Task<ActionResult<AsignarTierraResultDto>> Update([FromBody] AsignarTierraUpdateDto dto)
        {
            if (!ModelState.IsValid)  return BadRequest(ModelState);

            var result = await _asignarTierra.Update(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.AsignarTierraId }, result);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteTierra([FromBody] AsignarTierraDeleteDto asignarTierraDeleteDto)
        {
            var result = await _asignarTierra.Delete(asignarTierraDeleteDto);

            if (!result) return NotFound();
            return Ok(result);
        }
    }
}
