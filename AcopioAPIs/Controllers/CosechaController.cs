using AcopioAPIs.DTOs.Cosecha;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosechaController : ControllerBase
    {
        private readonly ICosecha _cosecha;

        public CosechaController(ICosecha cosecha)
        {
            _cosecha = cosecha;
        }

        [HttpGet]
        public async Task<ActionResult<List<CosechaResultDto>>> GetAll()
        {
            var cosecha = await _cosecha.GetAll();
            return Ok(cosecha);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CosechaResultDto>> GetById(int id)
        {
            var cosecha = await _cosecha.GetById(id);
            return Ok(cosecha);
        }
        [HttpPost]
        public async Task<ActionResult<CosechaResultDto>> Create([FromBody] CosechaInsertDto insertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _cosecha.Save(insertDto);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<CosechaResultDto>> Updte([FromBody] CosechaUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _cosecha.Update(updateDto);
            return Ok(result);
        }
    }
}
