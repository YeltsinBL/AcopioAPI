using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Venta;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVenta _VentaService;

        public VentaController(IVenta VentaService)
        {
            _VentaService = VentaService;
        }

        [HttpGet("VentaTipo")]
        public async Task<ActionResult<List<TipoResultDto>>> GetVentaTipoList()
        {
            var result = await _VentaService.GetTipoVenta();
            return Ok(result);
        }
        [HttpGet("VentaEstado")]
        public async Task<ActionResult<List<TipoResultDto>>> GetVentaEstadoList()
        {
            var result = await _VentaService.GetVentaEstado();
            return Ok(result);
        }
        [HttpGet("VentaCliente")]
        public async Task<ActionResult<List<TipoResultDto>>> GetVentaClienteList()
        {
            var result = await _VentaService.GetVentaCliente();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<VentaResultDto>>> GetVentaResults(DateOnly? fechaDesde, DateOnly? fechaHasta,
            int? tipoVentaId, int? numeroComprobante, int? estadoId)
        {
            var result = await _VentaService.GetVentaResults(fechaDesde, fechaHasta, tipoVentaId, numeroComprobante, estadoId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<VentaDto>>> GetVenta(int id)
        {
            try
            {
                var result = await _VentaService.GetVenta(id);
                return Ok(result);

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
        [HttpPost]
        public async Task<ActionResult<ResultDto<VentaResultDto>>> InsertVenta(
            [FromForm] VentaInsertDto Venta,
            [FromForm] List<IFormFile>? imagenes)
        {
            try
            {
                
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _VentaService.InsertVenta(Venta, imagenes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<VentaResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResultDto<VentaResultDto>>> UpdateVenta(
            [FromForm] VentaUpdateDto ventaUpdateDto,
            [FromForm] List<IFormFile>? imagenes
            )
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _VentaService.UpdateVenta(ventaUpdateDto, imagenes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<VentaResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto<bool>>> DeleteVenta([FromBody] DeleteDto Venta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _VentaService.DeleteVenta(Venta);
                return Ok(result);
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
    }
}
