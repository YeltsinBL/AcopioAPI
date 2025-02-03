using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController : ControllerBase
    {
        private readonly ITipos _tipos;

        public TiposController(ITipos tipos)
        {
            _tipos = tipos;
        }

        [HttpGet("TipoComprobante")]
        public async Task<IActionResult> Get()
        {
            var result = await _tipos.GetTipoComprontes();
            return Ok(result);
        }
    }
}
