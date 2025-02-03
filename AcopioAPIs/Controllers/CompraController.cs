using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Compra;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ICompra _compraService;

        public CompraController(ICompra compraService)
        {
            _compraService = compraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompraResultDto>>> GetCompraResults(DateOnly? fechaDesde, DateOnly? fechaHasta, 
            int? tipoComprobanteId, string? numeroComprobante, bool? estadoId)
        {
            var result = await _compraService.GetCompraResults(fechaDesde, fechaHasta, tipoComprobanteId, numeroComprobante, estadoId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<CompraDto>>> GetCompra(int id)
        {
            try
            {
                var result = await _compraService.GetCompra(id);
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
        public async Task<ActionResult<ResultDto<CompraResultDto>>> InsertCompra([FromBody] CompraInsertDto compra)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _compraService.InsertCompra(compra);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<CompraResultDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto<bool>>> DeleteCompra([FromBody] CompraDeleteDto compra)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _compraService.DeleteCompra(compra);
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
