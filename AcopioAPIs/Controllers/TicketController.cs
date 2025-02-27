using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Ticket;
using AcopioAPIs.Repositories;
using AcopioAPIs.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;
        private const string Nombre = "Ticket";
        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResultDto<List<TicketResultDto>>>> GetAll(
            string? ingenio, int? carguilloId, string? viaje, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId)
        {
            try
            {
                var tickets = await _ticket.GetTicketResults(ingenio, carguilloId, viaje, fechaDesde, fechaHasta,estadoId);
                return Ok(ResponseHelper.ReturnData(tickets, Nombre + " encontrados"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet]
        [Route("TipoEstado")]
        public async Task<ActionResult<List<TicketEstadoResultDto>>> GetAllType()
        {
            var estado = await _ticket.GetEstadoResults();
            return Ok(estado);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<TicketResultDto>>> GetById(int id)
        {
            try
            {
                var ticket = await _ticket.GetTicket(id);
                return Ok(ResponseHelper.ReturnData(ticket, Nombre + " recuperado"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpGet]
        [Route("Corte/Carguillo")]
        public async Task<ActionResult<List<TicketResultDto>>> GetTiketsByCarguillo(int carguilloId)
        {
            var estado = await _ticket.GetTicketsByCarguilllo(carguilloId);
            return Ok(estado);
        }
        [HttpGet]
        [Route("Liquidacion/Proveedor")]
        public async Task<ActionResult<List<TicketResultDto>>> GetTiketsByProveedor(int proveedorId)
        {
            var estado = await _ticket.GetTicketsByProveedor(proveedorId);
            return Ok(estado);
        }
        [HttpGet]
        [Route("Palero")]
        public async Task<ActionResult<List<TicketResultDto>>> GetTiketsForPalero()
        {
            var estado = await _ticket.GetTicketsForPaleros();
            return Ok(estado);
        }
        [HttpPost]
        public async Task<ActionResult<ResultDto<TicketResultDto>>> Create([FromBody] TicketInsertDto ticketInsertDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _ticket.Save(ticketInsertDto);
                return Ok(ResponseHelper.ReturnData(result, Nombre + " guardado"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpPut]
        public async Task<ActionResult<ResultDto<TicketResultDto>>> Update([FromBody] TicketUpdateDto ticketUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _ticket.Update(ticketUpdate);
                return Ok(ResponseHelper.ReturnData(result, Nombre + " actualizado"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseHelper.ReturnData(false, ex.Message, false));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
        [HttpDelete]
        public async Task<ActionResult<ResultDto<bool>>> Delete([FromBody] DeleteDto deleteDto)
        {
            try
            {
                var result = await _ticket.Delete(deleteDto);
                if(!result) return NotFound();
                return Ok(ResponseHelper.ReturnData(result, Nombre + " eliminado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.ReturnData(false, ex.Message, false));
            }
        }
    }
}
