using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Tesoreria;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class TesoreriaRepository : ITesoreria
    {

        private readonly DbacopioContext _dacopioContext;
        private readonly IConfiguration _configuration;
        private const string Nombre = "Pago Liquidación";

        public TesoreriaRepository(DbacopioContext dacopioContext, IConfiguration configuration)
        {
            _dacopioContext = dacopioContext;
            _configuration = configuration;
        }

        public async Task<List<TesoreriaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, int? personaId)
        {
            return await GetTesoreria(fechaDesde, fechaHasta, personaId, null)
                .ToListAsync(); 
        }

        public async Task<TesoreriaDto> GetById(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_TesoreriaGetById", new { TesoreriaId = id },
                    commandType: CommandType.StoredProcedure);

                var master = multi.Read<TesoreriaDto>().FirstOrDefault();
                var detail = multi.Read<TesoreriaDetallePagoResultDto>().AsList();
                if (master == null) throw new Exception(Nombre + " no encontrada");
                master.TesoreriaDetallePagos = detail;
                return master;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<TesoreriaResultDto>> Save(TesoreriaInsertDto tesoreriaInsertDto)
        {
             using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (tesoreriaInsertDto == null) 
                    throw new Exception("No se enviaron datos para guardar el "+ Nombre);
                var liquidacion = await _dacopioContext.Liquidacions
                    .FindAsync(tesoreriaInsertDto.LiquidacionId)
                    ?? throw new KeyNotFoundException("Liquidación no encontrada");

                var liquidacionEstado = await GetEstado("Pagado", _dacopioContext.LiquidacionEstados,
                    "LiquidacionEstadoDescripcion")
                    ?? throw new Exception("Estado de Liquidación Pagado no encontrado");
                var ticketEstadoLiq = await GetEstado("Liquidación", _dacopioContext.TicketEstados,
                    "TicketEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Liquidación no encontrado");
                var ticketEstadoPago = await GetEstado("Pagado", _dacopioContext.TicketEstados,
                    "TicketEstadoDescripcion")
                    ?? throw new Exception("Estado del Ticket Pagado no encontrado");
                var corteEstadoPagando = await GetEstado("Pagando", _dacopioContext.CorteEstados,
                    "CorteEstadoDescripcion")
                    ?? throw new Exception("Estado del Corte Pagando no encontrado");
                var corteEstadoPago = await GetEstado("Pagado", _dacopioContext.CorteEstados,
                    "CorteEstadoDescripcion")
                    ?? throw new Exception("Estado de Corte Pagado no encontrado");

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
                    TesoreriaFecha = tesoreriaInsertDto.TesoreriaFecha,
                    TesoreriaMonto = tesoreriaInsertDto.TesoreriaMonto,
                    TesoreriaPendientePagar = tesoreriaInsertDto.TesoreriaPendientePagar,
                    TesoreriaPagado = tesoreriaInsertDto.TesoreriaPagado,
                    UserCreatedAt = tesoreriaInsertDto.UserCreatedAt,
                    UserCreatedName = tesoreriaInsertDto.UserCreatedName
                };
                foreach (var item in tesoreriaInsertDto.TesoreriaDetallePagos)
                {
                    var detalle = new TesoreriaDetallePago
                    {
                        TesoreriaDetallePagoFecha = item.TesoreriaDetallePagoFecha,
                        TesoreriaDetallePagoEfectivo = item.TesoreriaDetallePagoEfectivo,
                        TesoreriaDetallePagoBanco = item.TesoreriaDetallePagoBanco,
                        TesoreriaDetallePagoCtaCte = item.TesoreriaDetallePagoCtaCte,
                        TesoreriaDetallePagoPagado = item.TesoreriaDetallePagoPagado,
                        UserCreatedAt = tesoreriaInsertDto.UserCreatedAt,
                        UserCreatedName = tesoreriaInsertDto.UserCreatedName
                    };
                    tesoreria.TesoreriaDetallePagos.Add(detalle);
                }
                _dacopioContext.Add(tesoreria);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var result = await GetTesoreria(null, null, null,tesoreria.TesoreriaId).FirstOrDefaultAsync()
                    ?? throw new Exception();
                return new ResultDto<TesoreriaResultDto>
                {
                    Result = true,
                    ErrorMessage = Nombre +" guardada",
                    Data = result
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<ResultDto<TesoreriaResultDto>> Update(TesoreriaUpdateDto tesoreriaUpdateDto)
        {
            using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (tesoreriaUpdateDto == null)
                    throw new Exception("No se enviaron datos para actualizar el "+ Nombre);
                var tesoreria = await _dacopioContext.Tesoreria
                    .FindAsync(tesoreriaUpdateDto.TesoreriaId)
                    ?? throw new KeyNotFoundException(Nombre +" no encontrada");

                tesoreria.TesoreriaPendientePagar = tesoreriaUpdateDto.TesoreriaPendientePagar;
                tesoreria.TesoreriaPagado = tesoreriaUpdateDto.TesoreriaPagado;
                tesoreria.UserModifiedAt = tesoreriaUpdateDto.UserModifiedAt;
                tesoreria.UserModifiedName = tesoreriaUpdateDto.UserModifiedName;

                foreach (var item in tesoreriaUpdateDto.TesoreriaDetallePagos)
                {
                    var detalle = new TesoreriaDetallePago
                    {
                        TesoreriaDetallePagoFecha = item.TesoreriaDetallePagoFecha,
                        TesoreriaDetallePagoEfectivo = item.TesoreriaDetallePagoEfectivo,
                        TesoreriaDetallePagoBanco = item.TesoreriaDetallePagoBanco,
                        TesoreriaDetallePagoCtaCte = item.TesoreriaDetallePagoCtaCte,
                        TesoreriaDetallePagoPagado = item.TesoreriaDetallePagoPagado,
                        UserCreatedAt = tesoreriaUpdateDto.UserModifiedAt,
                        UserCreatedName = tesoreriaUpdateDto.UserModifiedName
                    };
                    tesoreria.TesoreriaDetallePagos.Add(detalle);
                }
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var result = await GetTesoreria(null, null, null, tesoreria.TesoreriaId).FirstOrDefaultAsync()
                    ?? throw new Exception();
                return new ResultDto<TesoreriaResultDto>
                {
                    Result = true,
                    ErrorMessage = Nombre +" actualizada",
                    Data = result
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private IQueryable<TesoreriaResultDto> GetTesoreria(DateOnly? fechaDesde, DateOnly? fechaHasta, int? personaId, int? tesoreiaId)
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
                   && (personaId == null || liquidacion.PersonaId == personaId)
                   && (tesoreiaId == null || tesoreria.TesoreriaId == tesoreiaId)
                   orderby tesoreria.TesoreriaFecha
                   select new TesoreriaResultDto
                   {
                       TesoreriaId = tesoreria.TesoreriaId,
                       TesoreriaFecha = tesoreria.TesoreriaFecha,
                       TesoreriaMonto = tesoreria.TesoreriaMonto,
                       TesoreriaPendientePagar = tesoreria.TesoreriaPendientePagar ?? 0,
                       TesoreriaPagado = tesoreria.TesoreriaPagado ?? 0,
                       ProveedorUT = proveedor.ProveedorUt,
                       PersonaNombre = persona.PersonName + " " + persona.PersonPaternalSurname + " " + persona.PersonMaternalSurname,
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
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }

    }
}
