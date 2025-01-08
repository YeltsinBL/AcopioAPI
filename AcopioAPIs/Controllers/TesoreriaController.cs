using AcopioAPIs.DTOs.Tesoreria;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesoreriaController : ControllerBase
    {
        private readonly ITesoreria _tesoreria;

        public TesoreriaController(ITesoreria tesoreria)
        {
            _tesoreria = tesoreria;
        }

        [HttpGet]
        public async Task<ActionResult<List<TesoreriaResultDto>>> GetTesorerias(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId)
        {
            var results = await _tesoreria.GetAll(fechaDesde, fechaHasta, proveedorId);
            return Ok(results);
        }

        [HttpGet("{tesoreriaId}")]
        public async Task<ActionResult<TesoreriaDto>> GetTesoreria(int tesoreriaId)
        {
            try
            {
                var result = await _tesoreria.GetById(tesoreriaId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<TesoreriaResultDto>> SaveTesoreria([FromBody] TesoreriaInsertDto dto)
        {
            try
            {
                var result = await _tesoreria.Save(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
