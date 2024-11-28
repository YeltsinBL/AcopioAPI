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
        public async Task<ActionResult<List<ProveedorResultDto>>> GetAll()
        {
            var proveedores = await _proveedor.List();
            return Ok(proveedores);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorResultDto>> GetById(int id)
        {
            var proveedores = await _proveedor.Get(id);
            return Ok(proveedores);
        }
        [HttpGet]
        [Route("Available")]
        public async Task<ActionResult<List<ProveedorResultDto>>> GetAvailable()
        {
            var proveedores = await _proveedor.GetAvailableProveedor();
            return Ok(proveedores);
        }
        [HttpPost]
        public async Task<ActionResult<ProveedorResultDto>> Add(ProveedorInsertDto proveedorInsertDto)
        {
            var proveedor = await _proveedor.Save(proveedorInsertDto);
            return Ok(proveedor);
        }
        [HttpPut]
        public async Task<ActionResult<ProveedorResultDto>> Update(ProveedorUpdateDto proveedorUpdateDto)
        {
            var existProveedor = await _proveedor.Get(proveedorUpdateDto.ProveedorId);
            if (existProveedor == null) return NotFound("Proveedor no encontrado");
            var proveedor = await _proveedor.Update(proveedorUpdateDto);
            return Ok(proveedor);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var existProveedor = await _proveedor.Get(id);
            if (existProveedor == null) return NotFound("Proveedor no encontrado");
            return await _proveedor.Delete(id);
        }

    }
}
