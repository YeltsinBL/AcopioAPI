using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Cosecha;
using AcopioAPIs.Repositories;
using AcopioAPIs.Utils;
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
        public async Task<ActionResult<ResultDto<List<CosechaResultDto>>>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta,
            string? tierraUC, string? proveedotUT, int? tipoCosechaId)
        {
            try
            {
                var cosecha = await _cosecha.GetAll(fechaDesde, fechaHasta, tierraUC, proveedotUT, tipoCosechaId);
                return Ok(ResponseHelper.ReturnData(cosecha, "Cosechas encontradas"));

            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<CosechaDto>>> GetById(int id)
        {
            try
            {
                var cosecha = await _cosecha.GetById(id);
                return Ok(ResponseHelper.ReturnData(cosecha, "Cosecha recuperada"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<CosechaResultDto>>> Create([FromBody] CosechaInsertDto insertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cosecha.Save(insertDto);
                return Ok(ResponseHelper.ReturnData(result, "Cosecha guardada"));

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpPut]
        public async Task<ActionResult<ResultDto<CosechaResultDto>>> Updte([FromBody] CosechaUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _cosecha.Update(updateDto);
                return Ok(ResponseHelper.ReturnData(result, "Cosecha actualizada"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet]
        [Route("Tipo")]
        public async Task<ActionResult<List<CosechaTipoDto>>> GetTipo()
        {
            var cosecha = await _cosecha.GetTipo();
            return Ok(cosecha);
        }
    }
}
