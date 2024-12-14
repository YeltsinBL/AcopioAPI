using AcopioAPIs.DTOs.Recojo;
using AcopioAPIs.Models;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecojoController : ControllerBase
    {
        private readonly IRecojo _recojo;

        public RecojoController(IRecojo recojo)
        {
            _recojo = recojo;
        }

        [HttpGet]
        [Route("Estados")]
        public async Task<ActionResult<List<RecojoEstadoResultDto>>> RecojoEstadoList()
        {
            var estados = await _recojo.ListRecojoEstado();
            return Ok(estados);
        }
        [HttpGet]
        public async Task<ActionResult<List<RecojoResultDto>>> RecojoList(DateOnly? fechaDesde, DateOnly? fechaHasta, int? estadoId)
        {
            var recojos = await _recojo.ListRecojo(fechaDesde, fechaHasta, estadoId);
            return Ok(recojos);
        }
        [HttpGet]
        [Route("{recojoId}")]
        public async Task<ActionResult<RecojoDto>> RecojoGeyById(int recojoId)
        {
            var recojo = await _recojo.GetRecojoById(recojoId);
            return Ok(recojo);
        }
        [HttpPost]
        public async Task<ActionResult<RecojoResultDto>> RecojoSave([FromBody] RecojoInsertDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var recojo = await _recojo.SaveRecojo(dto);
                return Ok(recojo);

            }
            catch (Exception)
            {
                Console.WriteLine("Error Controller");
                throw;
            }
        }
        [HttpPut]
        public async Task<ActionResult<RecojoResultDto>> RecojoUpdate([FromBody] RecojoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var recojo = await _recojo.UpdateRecojo(dto);
            return Ok(recojo);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> RecojoDelete([FromBody] RecojoDeleteDto deleteDto)
        {
            var delete = await _recojo.DeleteRecojo(deleteDto);
            return Ok(delete);
        }
    }
}
