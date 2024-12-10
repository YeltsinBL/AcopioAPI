using AcopioAPIs.DTOs.Carguillo;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarguilloController : ControllerBase
    {
        private readonly ICarguillo _carguillo;

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
        public async Task<ActionResult<List<CarguilloResultDto>>> GetCarguillo(int tipoCarguilloId, string? titular, bool? estado)
        {            
            var carguillos = await _carguillo.GetCarguillos(tipoCarguilloId, titular, estado);
            return Ok(carguillos);
        }
        [HttpGet("{carguilloId}")]
        public async Task<ActionResult<CarguilloDto>> GetCarguilloById(int carguilloId)
        {
            if (carguilloId <= 0)
                return BadRequest("El parámetro 'carguilloId' es obligatorio y debe ser mayor a 0.");

            var carguillo = await _carguillo.GetCarguilloById(carguilloId);
            return Ok(carguillo);
        }
        [HttpPost]
        public async Task<ActionResult<CarguilloResultDto>> SaveCarguillo([FromBody]CarguilloInsertDto carguilloInsertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var carguillo = await _carguillo.SaveCarguillo(carguilloInsertDto);
            return Ok(carguillo);
        }
        [HttpPut]
        public async Task<ActionResult<CarguilloResultDto>> UpdateCarguillo([FromBody] CarguilloUpdateDto updateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var carguillo = await _carguillo.UpdateCarguillo(updateDto);
            return Ok(carguillo);
        }
    }
}
