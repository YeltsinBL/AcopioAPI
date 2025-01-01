using AcopioAPIs.DTOs.TipoUsuario;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuario _tipoUsuario;

        public TipoUsuarioController(ITipoUsuario tipoUsuario)
        {
            _tipoUsuario = tipoUsuario;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoUsuarioDto>>> ListTipos(string? nombre, bool? estado)
        {
            try
            {
                var tipos = await _tipoUsuario.ListTipos(nombre, estado);
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{tipoUsuarioId}")]
        public async Task<ActionResult<TipoUsuarioDto>> GetTipo(int tipoUsuarioId)
        {
            try
            {
                var tipo = await _tipoUsuario.GetTipo(tipoUsuarioId);
                return Ok(tipo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<TipoUsuarioDto>> InsertTipo([FromBody] TipoUsuarioInsertDto tipoUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var tipo = await _tipoUsuario.InsertTipo(tipoUsuario);
                return Ok(tipo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<TipoUsuarioDto>> UpdateTipo([FromBody] TipoUsuarioUpdateDto tipoUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var tipo = await _tipoUsuario.UpdateTipo(tipoUsuario);
                return Ok(tipo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteTipo([FromBody] TipoUsuarioDeleteDto tipoUsuario)
        {
            try
            {
                var delete = await _tipoUsuario.DeleteTipo(tipoUsuario);
                return Ok(delete);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
