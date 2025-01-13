using AcopioAPIs.DTOs.Tesoreria;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class TesoreriaRepository : ITesoreria
    {

        private readonly DbacopioContext _dacopioContext;

        public TesoreriaRepository(DbacopioContext dacopioContext)
        {
            _dacopioContext = dacopioContext;
        }

        public async Task<List<TesoreriaResultDto>> GetAll(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId)
        {
            return await GetTesoreria(fechaDesde, fechaHasta, proveedorId, null)
                .ToListAsync(); 
        }

        public async Task<TesoreriaDto> GetById(int id)
        {
            try
            {
                var query = from tesoreria in _dacopioContext.Tesoreria
                            join liquidacion in _dacopioContext.Liquidacions
                                on tesoreria.LiquidacionId equals liquidacion.LiquidacionId
                            join proveedor in _dacopioContext.Proveedors
                                on liquidacion.ProveedorId equals proveedor.ProveedorId
                            join persona in _dacopioContext.Persons
                                on liquidacion.PersonaId equals persona.PersonId
                            join tierra in _dacopioContext.Tierras
                                on liquidacion.TierraId equals tierra.TierraId
                            where tesoreria.TesoreriaId == id
                            select new TesoreriaDto
                            {
                                TesoreriaId = tesoreria.TesoreriaId,
                                LiquidacionId = liquidacion.LiquidacionId,
                                TesoreriaBanco = tesoreria.TesoreriaBanco,
                                TesoreriaCtaCte = tesoreria.TesoreriaCtaCte,
                                TesoreriaFecha = tesoreria.TesoreriaFecha,
                                TesoreriaMonto = tesoreria.TesoreriaMonto,
                                ProveedorUT = proveedor.ProveedorUt,
                                PersonaNombre = persona.PersonName,
                                TierraCampo = tierra.TierraCampo
                            };
                return await query
                    .FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Tesoreria no encontrada");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TesoreriaResultDto> Save(TesoreriaInsertDto tesoreriaInsertDto)
        {
             using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (tesoreriaInsertDto == null) 
                    throw new Exception("No se enviaron datos para guardar la tesoreria");
                var liquidacion = await _dacopioContext.Liquidacions
                    .FindAsync(tesoreriaInsertDto.LiquidacionId)
                    ?? throw new KeyNotFoundException("Liquidación no encontrada");

                var liquidacionEstado = await GetEstado("Pagado", _dacopioContext.LiquidacionEstados,
                    "LiquidacionEstadoDescripcion")
                    ?? throw new Exception("Estado de Liquidación no encontrado");
                var ticketEstadoLiq = await GetEstado("Liquidación", _dacopioContext.TicketEstados,
                    "TicketEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Tesoreria no encontrado");
                var ticketEstadoPago = await GetEstado("Pagado", _dacopioContext.TicketEstados,
                    "TicketEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Pagado no encontrado");
                var corteEstadoPagando = await GetEstado("Pagando", _dacopioContext.CorteEstados,
                    "CorteEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Tesoreria no encontrado");
                var corteEstadoPago = await GetEstado("Pagado", _dacopioContext.CorteEstados,
                    "CorteEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Tesoreria no encontrado");

                // Procesar tickets y actualizar estados
                await ProcesarTickets(tesoreriaInsertDto, ticketEstadoPago);

                // Procesar cortes y actualizar estados
                await ProcesarCortes(tesoreriaInsertDto, ticketEstadoLiq, corteEstadoPagando, corteEstadoPago);
                
                liquidacion.LiquidacionEstadoId = liquidacionEstado.LiquidacionEstadoId;
                liquidacion.UserModifiedAt = tesoreriaInsertDto.UserCreatedAt;
                liquidacion.UserModifiedName = tesoreriaInsertDto.UserCreatedName;

                var tesoreria = new Tesorerium
                {
                    LiquidacionId = tesoreriaInsertDto.LiquidacionId,
                    TesoreriaBanco = tesoreriaInsertDto.TesoreriaBanco,
                    TesoreriaCtaCte = tesoreriaInsertDto.TesoreriaCtaCte,
                    TesoreriaFecha = tesoreriaInsertDto.TesoreriaFecha,
                    TesoreriaMonto = tesoreriaInsertDto.TesoreriaMonto,
                    UserCreatedAt = tesoreriaInsertDto.UserCreatedAt,
                    UserCreatedName = tesoreriaInsertDto.UserCreatedName
                };
                _dacopioContext.Add(tesoreria);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return await GetTesoreria(null, null, null,tesoreria.TesoreriaId).FirstOrDefaultAsync()
                    ?? throw new Exception();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private IQueryable<TesoreriaResultDto> GetTesoreria(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId, int? tesoreiaId)
        {
            return from tesoreria in _dacopioContext.Tesoreria
                   join liquidacion in _dacopioContext.Liquidacions
                       on tesoreria.LiquidacionId equals liquidacion.LiquidacionId
                   join proveedor in _dacopioContext.Proveedors
                       on liquidacion.ProveedorId equals proveedor.ProveedorId
                   join persona in _dacopioContext.Persons
                       on liquidacion.PersonaId equals persona.PersonId
                    join tierra in _dacopioContext.Tierras
                        on liquidacion.TierraId equals tierra.TierraId
                   where (fechaDesde == null || tesoreria.TesoreriaFecha >= fechaDesde)
                   && (fechaHasta == null || tesoreria.TesoreriaFecha <= fechaHasta)
                   && (proveedorId == null || liquidacion.ProveedorId == proveedorId)
                   && (tesoreiaId == null || tesoreria.TesoreriaId == tesoreiaId)
                   select new TesoreriaResultDto
                   {
                       TesoreriaId = tesoreria.TesoreriaId,
                       TesoreriaFecha = DateOnly.FromDateTime(tesoreria.TesoreriaFecha),
                       TesoreriaMonto = tesoreria.TesoreriaMonto,
                       ProveedorUT = proveedor.ProveedorUt,
                       PersonaNombre = persona.PersonName,
                       TierraCampo = tierra.TierraCampo
                   };
        }
        private static async Task<T?> GetEstado<T>(string descripcion, DbSet<T> estados, string columna) where T : class
        {
            return await estados.FirstOrDefaultAsync(e => EF.Property<string>(e, columna) == descripcion);
        }
        private async Task ProcesarTickets(TesoreriaInsertDto dto, TicketEstado ticketEstadoPago)
        {
            var ticketIds = await _dacopioContext.LiquidacionTickets
                .Where(lt => lt.LiquidacionId == dto.LiquidacionId)
                .Select(lt => lt.TicketId)
                .ToListAsync();

            var tickets = await _dacopioContext.Tickets
                .Where(t => ticketIds.Contains(t.TicketId))
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                var historyTicket = new TicketHistorial
                {
                    TicketId = ticket.TicketId,
                    TicketIngenio = ticket.TicketIngenio,
                    TicketViaje = ticket.TicketViaje,
                    CarguilloId = ticket.CarguilloId,
                    TicketChofer = ticket.TicketChofer,
                    TicketFecha = ticket.TicketFecha,
                    CarguilloDetalleCamionId = ticket.CarguilloDetalleCamionId,
                    TicketCamionPeso = ticket.TicketCamionPeso,
                    CarguilloDetalleVehiculoId = ticket.CarguilloDetalleVehiculoId,
                    TicketVehiculoPeso = ticket.TicketVehiculoPeso,
                    TicketUnidadPeso = ticket.TicketUnidadPeso,
                    TicketPesoBruto = ticket.TicketPesoBruto,
                    TicketEstadoId = ticket.TicketEstadoId,
                    UserModifiedAt = dto.UserCreatedAt,
                    UserModifiedName = dto.UserCreatedName
                };

                _dacopioContext.TicketHistorials.Add(historyTicket);

                ticket.TicketEstadoId = ticketEstadoPago.TicketEstadoId;
                ticket.UserModifiedAt = dto.UserCreatedAt;
                ticket.UserModifiedName = dto.UserCreatedName;
            }

            await _dacopioContext.SaveChangesAsync();
        }

        private async Task ProcesarCortes(TesoreriaInsertDto dto, TicketEstado ticketEstadoLiq, CorteEstado corteEstadoPagando, CorteEstado corteEstadoPago)
        {
            var cortes = await _dacopioContext.Cortes
                .Where(c => c.CorteDetalles
                    .Any(cd => cd.Ticket.LiquidacionTickets
                        .Any(lt => lt.LiquidacionId == dto.LiquidacionId)))
                .ToListAsync();

            foreach (var corte in cortes)
            {
                var tieneTicketsPendientes = await _dacopioContext.CorteDetalles
                    .Where(cd => cd.CorteId == corte.CorteId)
                    .AnyAsync(cd => cd.Ticket.TicketEstadoId == ticketEstadoLiq.TicketEstadoId);
                
                var historyCorte = new CorteHistorial
                {
                    CorteId = corte.CorteId,
                    CorteFecha = corte.CorteFecha,
                    TierraId = corte.TierraId,
                    CortePrecio = corte.CortePrecio,
                    CorteEstadoId = corte.CorteEstadoId,
                    CortePesoBrutoTotal = corte.CortePesoBrutoTotal,
                    CorteTotal = corte.CorteTotal,
                    UserModifiedAt = dto.UserCreatedAt,
                    UserModifiedName = dto.UserCreatedName
                };

                _dacopioContext.CorteHistorials.Add(historyCorte);

                corte.CorteEstadoId = tieneTicketsPendientes ? 
                    corteEstadoPagando.CorteEstadoId : corteEstadoPago.CorteEstadoId;
                corte.UserModifiedAt = dto.UserCreatedAt;
                corte.UserModifiedName = dto.UserCreatedName;
            }

            await _dacopioContext.SaveChangesAsync();
        }
    }
}
