using AcopioAPIs.DTOs.Cliente;
using AcopioAPIs.DTOs.Common;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ICliente _cliente;

        public ClienteController(ICliente cliente)
        {
            _cliente = cliente;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> GetAll(string? nombre, bool? estado)
        {
            var clientes = await _cliente.GetAll(nombre, estado);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<ClienteDto>>> GetById(int id)
        {
            try
            {
                var cliente = await _cliente.GetById(id);
                return Ok(cliente);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<ClienteDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ClienteDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto<ClienteDto>>> Save([FromBody] ClienteInsertDto ClienteInsertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cliente.Insert(ClienteInsertDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ClienteDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResultDto<ClienteDto>>> Update([FromBody] ClienteUpdateDto ClienteUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cliente.Update(ClienteUpdateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<ClienteDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ClienteDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto<ClienteDto>>> Delete([FromBody] ClienteDeleteDto ClienteDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cliente.Delete(ClienteDeleteDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
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
