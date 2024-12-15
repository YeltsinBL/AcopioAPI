using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.ServicioTransporte;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioTransporteController : ControllerBase
    {
        private readonly IServicioTransporte _servicioTransporte;

        public ServicioTransporteController(IServicioTransporte servicioTransporte)
        {
            _servicioTransporte = servicioTransporte;
        }

        [HttpGet]
        [Route("Estados")]
        public async Task<ActionResult<List<EstadoResultDto>>> ServicioTransporteEstadoList()
        {
            var estado = await _servicioTransporte.ListEstados();
            return Ok(estado);
        }
        [HttpGet]
        public async Task<ActionResult<List<ServicioTransporteResultDto>>> ServicioTransporteList(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            var servicios = await _servicioTransporte.ListServiciosTransporte(fechaDesde, fechaHasta, carguilloId, estadoId);
            return Ok(servicios);
        }
        [HttpGet]
        [Route("{servicioTransporteId}")]
        public async Task<ActionResult<ServicioTransporteDto>> ServicioTransporte(int servicioTransporteId)
        {
            var servicio = await _servicioTransporte.GetServicioTransporte(servicioTransporteId);
            return Ok(servicio);
        }
        [HttpPost]
        public async Task<ActionResult<ServicioTransporteResultDto>> ServicioTransporteSave([FromBody] ServicioTransporteInsertDto insertDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var servicio = await _servicioTransporte.SaveServicioTransporte(insertDto);
            return Ok(servicio);
        }
        [HttpPut]
        public async Task<ActionResult<ServicioTransporteResultDto>> ServicioTransporteUpdate([FromBody] ServicioTransporteUpdateDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var servicio = await _servicioTransporte.UpdateServicioTransporte(updateDto);
            return Ok(servicio);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> ServicioTransporteDelete([FromBody] ServicioTransporteDeleteDto deleteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var servicio = await _servicioTransporte.DeleteServicioTransporte(deleteDto);
            return Ok(servicio);
        }
    }
}
