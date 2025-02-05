using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioTransporte _servicioTransporte;
        private readonly IServicioPalero _servicioPalero;

        public ServicioController(IServicioTransporte servicioTransporte, IServicioPalero servicioPalero)
        {
            _servicioTransporte = servicioTransporte;
            _servicioPalero = servicioPalero;
        }

        [HttpGet]
        [Route("Estados")]
        public async Task<ActionResult<List<EstadoResultDto>>> ServicioTransporteEstadoList()
        {
            var estado = await _servicioTransporte.ListEstados();
            return Ok(estado);
        }
        [HttpGet("Transporte")]
        public async Task<ActionResult<List<ServicioResultDto>>> ServicioTransporteList(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            var servicios = await _servicioTransporte.ListServiciosTransporte(fechaDesde, fechaHasta, carguilloId, estadoId);
            return Ok(servicios);
        }
        [HttpGet]
        [Route("Transporte/{servicioTransporteId}")]
        public async Task<ActionResult<ServicioDto>> ServicioTransporte(int servicioTransporteId)
        {
            var servicio = await _servicioTransporte.GetServicioTransporte(servicioTransporteId);
            return Ok(servicio);
        }
        [HttpPost("Transporte")]
        public async Task<ActionResult<ResultDto<ServicioResultDto>>> ServicioTransporteSave([FromBody] ServicioInsertDto insertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioTransporte.SaveServicioTransporte(insertDto);
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ServicioResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpPut("Transporte")]
        public async Task<ActionResult<ServicioResultDto>> ServicioTransporteUpdate([FromBody] ServicioUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioTransporte.UpdateServicioTransporte(updateDto);
                return Ok(servicio);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpDelete("Transporte")]
        public async Task<ActionResult<bool>> ServicioTransporteDelete([FromBody] ServicioDeleteDto deleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioTransporte.DeleteServicioTransporte(deleteDto);
                return Ok(servicio);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        // Palero
        [HttpGet("Palero")]
        public async Task<ActionResult<List<ServicioResultDto>>> ServicioPaleroList(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            var servicios = await _servicioPalero.ListServiciosPalero(fechaDesde, fechaHasta, carguilloId, estadoId);
            return Ok(servicios);
        }
        [HttpGet]
        [Route("Palero/{servicioPaleroId}")]
        public async Task<ActionResult<ServicioDto>> ServicioPalero(int servicioPaleroId)
        {
            var servicio = await _servicioPalero.GetServicioPalero(servicioPaleroId);
            return Ok(servicio);
        }
        [HttpPost("Palero")]
        public async Task<ActionResult<ResultDto<ServicioResultDto>>> ServicioPaleroSave([FromBody] ServicioPaleroInsertDto insertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioPalero.SaveServicioPalero(insertDto);
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpPut("Palero")]
        public async Task<ActionResult<ServicioResultDto>> ServicioPaleroUpdate([FromBody] ServicioUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioPalero.UpdateServicioPalero(updateDto);
                return Ok(servicio);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpDelete("Palero")]
        public async Task<ActionResult<ResultDto<int>>> ServicioPaleroDelete([FromBody] ServicioDeleteDto deleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var servicio = await _servicioPalero.DeleteServicioPalero(deleteDto);
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpGet("Palero/ServicioTransporteAvailable")]
        public async Task<ActionResult<List<ServicioDto>>> ServicioPaleroListServicioTransporte()
        {
            var servicios = await _servicioPalero.GetListServicioTransporteAvailable();
            return Ok(servicios);
        }
    }
}
