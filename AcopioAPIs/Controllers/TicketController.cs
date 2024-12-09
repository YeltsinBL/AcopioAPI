using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Ticket;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;
        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<TicketResultDto>>> GetAll(
            string? ingenio, int? carguilloId, string? viaje, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId)
        {
            var tickets = await _ticket.GetTicketResults(ingenio, carguilloId, viaje, fechaDesde, fechaHasta,estadoId);
            return Ok(tickets);
        }
        [HttpGet]
        [Route("TipoEstado")]
        public async Task<ActionResult<List<TicketEstadoResultDto>>> GetAllType()
        {
            var estado = await _ticket.GetEstadoResults();
            return Ok(estado);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResultDto>> GetById(int id)
        {
            var ticket = await _ticket.GetTicket(id);
            return Ok(ticket);
        }
        [HttpPost]
        public async Task<ActionResult<TicketResultDto>> Create([FromBody] TicketInsertDto ticketInsertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _ticket.Save(ticketInsertDto);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<TicketResultDto>> Update([FromBody] TicketUpdateDto ticketUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _ticket.Update(ticketUpdate);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete([FromBody] DeleteDto deleteDto)
        {
            var result = await _ticket.Delete(deleteDto);
            if(!result) return NotFound();
            return Ok(result);
        }
    }
}
