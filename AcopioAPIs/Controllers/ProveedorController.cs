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
        public async Task<ActionResult<ProveedorResultDto>> GetById(int id)
        {
            var proveedores = await _proveedor.Get(id);
            return Ok(proveedores);
        }
        [HttpGet]
        [Route("Asignar/Available")]
        public async Task<ActionResult<List<ProveedorResultDto>>> GetAvailable()
        {
            var proveedores = await _proveedor.GetAvailableProveedor();
            return Ok(proveedores);
        }
        [HttpPost]
        public async Task<ActionResult<ProveedorResultDto>> Add([FromBody] ProveedorInsertDto proveedorInsertDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var proveedor = await _proveedor.Save(proveedorInsertDto);
            return Ok(proveedor);
        }
        [HttpPut]
        public async Task<ActionResult<ProveedorResultDto>> Update([FromBody] ProveedorUpdateDto proveedorUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existProveedor = await _proveedor.Get(proveedorUpdateDto.ProveedorId);
            if (existProveedor == null) return NotFound("Proveedor no encontrado");
            var proveedor = await _proveedor.Update(proveedorUpdateDto);
            return Ok(proveedor);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete([FromBody] ProveedorDeleteDto proveedorDeleteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return await _proveedor.Delete(proveedorDeleteDto);
        }

    }
}
