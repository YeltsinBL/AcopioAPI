using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.FacturaVenta;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaVentaController : ControllerBase
    {
        private readonly IFacturaVenta _facturaVenta;

        public FacturaVentaController(IFacturaVenta facturaVenta)
        {
            _facturaVenta = facturaVenta;
        }

        [HttpGet("Estados")]
        public async Task<ActionResult<List<TipoResultDto>>> GetAvailable()
        {
            var proveedores = await _facturaVenta.GetAllEstados();
            return Ok(proveedores);
        }

        [HttpGet]
        public async Task<ActionResult<List<FacturaVentaResultDto>>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, string? numero, int? estadoId)
        {
            var proveedores = await _facturaVenta.GetAll(fechaDesde, fechaHasta, numero, estadoId);
            return Ok(proveedores);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<FacturaVentaDto>>> GetById(int id)
        {
            try
            {
                var proveedores = await _facturaVenta.GetById(id);
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<FacturaVentaDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<FacturaVentaResultDto>>> Add([FromBody] FacturaVentaInsertDto insertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var proveedor = await _facturaVenta.Save(insertDto);
                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<FacturaVentaResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
