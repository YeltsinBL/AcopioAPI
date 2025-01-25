using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedor _proveedor;

        public ProveedorController(IProveedor proveedor)
        {
            _proveedor = proveedor;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProveedorGroupedDto>>> GetAll(string? ut, string? nombre, bool? estado)
        {
            var proveedores = await _proveedor.List(ut, nombre, estado);
            return Ok(proveedores);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<ProveedorDTO>>> GetById(int id)
        {
            try
            {
                var proveedores = await _proveedor.Get(id);
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ProveedorDTO> { 
                    Result = false, ErrorMessage = ex.Message });
            }
        }
        [HttpGet]
        [Route("Asignar/Available")]
        public async Task<ActionResult<List<ProveedorResultDto>>> GetAvailable()
        {
            var proveedores = await _proveedor.GetAvailableProveedor();
            return Ok(proveedores);
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<ProveedorResultDto>>> Add([FromBody] ProveedorInsertDto proveedorInsertDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var proveedor = await _proveedor.Save(proveedorInsertDto);
                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ProveedorResultDto> { 
                    Result = false, ErrorMessage = ex.Message });
            }
        }
        [HttpPut]
        public async Task<ActionResult<ResultDto<ProveedorResultDto>>> Update([FromBody] ProveedorUpdateDto proveedorUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var existProveedor = await _proveedor.Get(proveedorUpdateDto.ProveedorId);
                if (existProveedor == null) return NotFound("Proveedor no encontrado");
                var proveedor = await _proveedor.Update(proveedorUpdateDto);
                return Ok(proveedor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<ProveedorResultDto> { 
                    Result = false, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ProveedorResultDto> { 
                    Result = false, ErrorMessage = ex.Message });
            }

        }
        [HttpDelete]
        public async Task<ActionResult<ResultDto<int>>> Delete([FromBody] ProveedorDeleteDto proveedorDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var response = await _proveedor.Delete(proveedorDeleteDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<int> { 
                    Result = false, ErrorMessage = ex.Message });
            }
        }

    }
}
