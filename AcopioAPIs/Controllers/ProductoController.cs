using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Producto;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProducto _producto;
        public ProductoController(IProducto producto)
        {
            _producto = producto;
        }

        [HttpGet("Tipos")]
        public async Task<ActionResult<List<TipoResultDto>>> GetTiposAll()
        {
            var productos = await _producto.GetTipos();
            return Ok(productos);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetAll(string? nombre, bool? estado, bool? stock)
        {
            var productos = await _producto.GetAll(nombre,estado, stock);
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<ProductoDto>>> GetById(int id)
        {
            try
            {
                var producto = await _producto.GetById(id);
                return Ok(producto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<ProductoDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<ProductoDto>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }   
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto<ProductoDto>>> Create([FromBody] ProductoInsertDto productoInsertDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _producto.Insert(productoInsertDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpPut]
        public async Task<ActionResult<ResultDto<ProductoDto>>> Update([FromBody] ProductoUpdateDto productoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _producto.Update(productoUpdateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto<bool>>> Delete([FromBody] ProductoDeleteDto productoDeleteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _producto.Delete(productoDeleteDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<int>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
