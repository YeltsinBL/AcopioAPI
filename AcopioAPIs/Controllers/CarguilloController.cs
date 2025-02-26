using AcopioAPIs.DTOs.Carguillo;
using AcopioAPIs.DTOs.Common;
using AcopioAPIs.Repositories;
using AcopioAPIs.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarguilloController : ControllerBase
    {
        private readonly ICarguillo _carguillo;
        private const string Nombre = "Carguillo";

        public CarguilloController(ICarguillo carguillo)
        {
            _carguillo = carguillo;
        }

        [HttpGet]
        [Route("CarguilloTipos")]
        public async Task<ActionResult<List<CarguilloTipoResultDto>>> GetCarguiilloTipo(bool isCarguillo= true)
        {
            var tipo = await _carguillo.GetCarguilloTipos(isCarguillo);
            return Ok(tipo);
        }
        [HttpGet]
        public async Task<ActionResult<ResultDto<List<CarguilloResultDto>>>> GetCarguillo(int? tipoCarguilloId, string? titular, bool? estado)
        {
            try
            {
                var carguillos = await _carguillo.GetCarguillos(tipoCarguilloId, titular, estado);
                return Ok(ResponseHelper.ReturnData(carguillos, Nombre+"s encontrados"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet("{carguilloId}")]
        public async Task<ActionResult<ResultDto<CarguilloDto>>> GetCarguilloById(int carguilloId)
        {
            try
            {
                if (carguilloId <= 0)
                    return BadRequest(ResponseHelper.ReturnData(false, "El parámetro 'carguilloId' es obligatorio y debe ser mayor a 0.", false));

                var carguillo = await _carguillo.GetCarguilloById(carguilloId);
                return Ok(ResponseHelper.ReturnData(carguillo, Nombre+" recuperado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet]
        [Route("{carguilloId}/Tipo")]
        public async Task<ActionResult<CarguilloPlacasResultDto>> GetCarguilloPlacas(int carguilloId)
        {
            var placas = await _carguillo.GetCarguilloDetalles(carguilloId);
            return Ok(placas);
        }
        [HttpGet]
        [Route("InAllTickets")]
        public async Task<ActionResult<List<CarguilloResultDto>>> GetCarguillosEnTicket()
        {
            var carguillos = await _carguillo.GetCarguillosTicket();
            return Ok(carguillos);
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<CarguilloResultDto>>> SaveCarguillo([FromBody]CarguilloInsertDto carguilloInsertDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var carguillo = await _carguillo.SaveCarguillo(carguilloInsertDto);
                return Ok(ResponseHelper.ReturnData(carguillo, Nombre + " guardado"));
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
        public async Task<ActionResult<ResultDto<CarguilloResultDto>>> UpdateCarguillo([FromBody] CarguilloUpdateDto updateDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                var carguillo = await _carguillo.UpdateCarguillo(updateDto);
                return Ok(ResponseHelper.ReturnData(carguillo, Nombre + " actualizado"));
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
    }
}
