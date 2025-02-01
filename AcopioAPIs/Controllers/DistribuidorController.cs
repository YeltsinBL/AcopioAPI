using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Distribuidor;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistribuidorController : ControllerBase
    {
        private readonly IDistribuidor _distribuidor;

        public DistribuidorController(IDistribuidor distribuidor)
        {
            _distribuidor = distribuidor;
        }

        [HttpGet]
        public async Task<ActionResult<List<DistribuidorDto>>> GetAll(string? ruc, string? nombre, bool? estado)
        {
            var distribuidores = await _distribuidor.GetAll(ruc, nombre, estado);
            return Ok(distribuidores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<DistribuidorDto>>> GetById(int id)
        {
            try
            {
                var distribuidor = await _distribuidor.GetById(id);
                return Ok(distribuidor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<DistribuidorDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<DistribuidorDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto<DistribuidorDto>>> Save([FromBody] DistribuidorInsertDto distribuidorInsertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _distribuidor.Insert(distribuidorInsertDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<DistribuidorDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResultDto<DistribuidorDto>>> Update([FromBody] DistribuidorUpdateDto distribuidorUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _distribuidor.Update(distribuidorUpdateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<DistribuidorDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<DistribuidorDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto<DistribuidorDto>>> Delete([FromBody] DistribuidorDeleteDto distribuidorDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _distribuidor.Delete(distribuidorDeleteDto);
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
