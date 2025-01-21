using System.Data;
using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AcopioAPIs.Repositories
{
    public class LiquidacionRepository : ILiquidacion
    {
        private readonly DbacopioContext _dacopioContext;
        private readonly IConfiguration _configuration;

        public LiquidacionRepository(DbacopioContext dacopioContext, IConfiguration configuration)
        {
            _dacopioContext = dacopioContext;
            _configuration = configuration;
        }


        public async Task<List<LiquidacionPersona>> GetProveedorLiquidacion()
        {
            try
            {
                using var conexion = GetConnection();
                var cortes = await conexion.QueryAsync<LiquidacionPersona>(
                    "usp_LiquidacionGetProveedoresTickets",
                    commandType: CommandType.StoredProcedure);
                return cortes.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<EstadoResultDto>> GetEstadoResult()
        {
            try
            {
                var query = from estado in _dacopioContext.LiquidacionEstados
                            where estado.LiquidacionEstadoStatus == true
                            select new EstadoResultDto
                            {
                                EstadoId = estado.LiquidacionEstadoId,
                                EstadoDescripcion = estado.LiquidacionEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<LiquidacionResultDto>> GetLiquidacionResult(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, int? estadoId)
        {
            try
            {
                var query = from liquid in _dacopioContext.Liquidacions
                            join estado in _dacopioContext.LiquidacionEstados
                                on liquid.LiquidacionEstadoId equals estado.LiquidacionEstadoId
                            join tierra in _dacopioContext.Tierras
                                on liquid.TierraId equals tierra.TierraId
                            join proveedor in _dacopioContext.Proveedors
                                on liquid.ProveedorId equals proveedor.ProveedorId
                            join persona in _dacopioContext.Persons
                                on liquid.PersonaId equals persona.PersonId
                            where (fechaDesde == null || liquid.LiquidacionFechaInicio >= fechaDesde)
                            && (fechaHasta == null || liquid.LiquidacionFechaFin <= fechaHasta)
                            && (proveedorId == null || liquid.ProveedorId == proveedorId)
                            && (estadoId == null || liquid.LiquidacionEstadoId == estadoId)
                            select new LiquidacionResultDto
                            {
                                LiquidacionId = liquid.LiquidacionId,
                                PersonaNombre = (persona.PersonName + ' '+persona.PersonPaternalSurname+' '+persona.PersonMaternalSurname),
                                TierraCampo = tierra.TierraCampo,
                                ProveedorUT = proveedor.ProveedorUt,
                                LiquidacionFechaInicio = liquid.LiquidacionFechaInicio,
                                LiquidacionFechaFin = liquid.LiquidacionFechaFin,
                                LiquidacionPesoNeto = liquid.LiquidacionPesoNeto,
                                LiquidacionPesoBruto = liquid.LiquidacionPesoBruto,
                                LiquidacionToneladaTotal = liquid.LiquidacionToneladaTotal,
                                LiquidacionFinanciamientoACuenta = liquid.LiquidacionFinanciamientoAcuenta,
                                LiquidacionPagar = liquid.LiquidacionPagar,
                                LiquidacionAdicionalTotal = liquid.LiquidacionAdicionalTotal,
                                LiquidacionEstadoDescripcion = estado.LiquidacionEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LiquidacionDto> GetLiquidacionById(int liquidacionId)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_LiquidacionGetById", new { LiquidacionId = liquidacionId }, commandType: CommandType.StoredProcedure);
                var master = (await multi.ReadAsync<LiquidacionDto>()).FirstOrDefault()
                    ?? throw new Exception("Liquidación no encontrada");
                var tickets = (await multi.ReadAsync<LiquidacionTicketDto>()).AsList();
                var financia = (await multi.ReadAsync<LiquidacionFinanciamientoDto>()).AsList();
                var adicional = (await multi.ReadAsync<LiquidacionAdicionalDto>()).AsList();
                master.LiquidacionTickets = tickets;
                master.LiquidacionFinanciamiento = financia;
                master.LiquidacionAdicionals = adicional;
                return master;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LiquidacionResultDto> SaveLiquidacion(LiquidacionInsertDto liquidacionInsertDto)
        {
            using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (liquidacionInsertDto == null) throw new Exception("No se enviaron datos para guardar la liquidación");
                if(liquidacionInsertDto.LiquidacionTickets == null) throw new Exception("No se enviaron los tickets para guardar la liquidación");
                var estados = from est in _dacopioContext.LiquidacionEstados
                              where est.LiquidacionEstadoDescripcion.Equals("activo")
                              select est;
                var estado = await estados.FirstOrDefaultAsync()
                    ?? throw new Exception("Estado de Liquidación no encontrado");
                var estadosTicket = from est in _dacopioContext.TicketEstados
                                    where est.TicketEstadoDescripcion.Equals("tesoreria")
                                    select est;
                var estadoTicket = await estadosTicket.FirstOrDefaultAsync()
                    ?? throw new Exception("Estados del Ticket no encontrados");
                var liquidacion = new Liquidacion
                {
                    PersonaId = liquidacionInsertDto.PersonaId,
                    TierraId = liquidacionInsertDto.TierraId,
                    ProveedorId = liquidacionInsertDto.ProveedorId,
                    LiquidacionFechaInicio = liquidacionInsertDto.LiquidacionFechaInicio,
                    LiquidacionFechaFin = liquidacionInsertDto.LiquidacionFechaFin,
                    LiquidacionPesoBruto = liquidacionInsertDto.LiquidacionPesoBruto,
                    LiquidacionPesoNeto = liquidacionInsertDto.LiquidacionPesoNeto,
                    LiquidacionToneladaPrecioCompra = liquidacionInsertDto.LiquidacionToneladaPrecioCompra,
                    LiquidacionToneladaTotal = liquidacionInsertDto.LiquidacionToneladaTotal,
                    LiquidacionFinanciamientoAcuenta = liquidacionInsertDto.LiquidacionFinanciamientoACuenta,
                    LiquidacionPagar = liquidacionInsertDto.LiquidacionPagar,
                    LiquidacionEstadoId = estado.LiquidacionEstadoId,
                    LiquidacionAdicionalTotal = liquidacionInsertDto.LiquidacionAdicionalTotal,
                    UserCreatedName = liquidacionInsertDto.UserCreatedName,
                    UserCreatedAt = liquidacionInsertDto.UserCreatedAt
                };
                foreach (var ticket in liquidacionInsertDto.LiquidacionTickets)
                {
                    var dto = await _dacopioContext.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId)
                            ?? throw new Exception("Ticket no encontrado");
                    var historyTicket = new TicketHistorial
                    {
                        TicketId = dto.TicketId,
                        TicketIngenio = dto.TicketIngenio,
                        TicketViaje = dto.TicketViaje,
                        CarguilloId = dto.CarguilloId,
                        TicketChofer = dto.TicketChofer,
                        TicketFecha = dto.TicketFecha,
                        CarguilloDetalleCamionId = dto.CarguilloDetalleCamionId,
                        TicketCamionPeso = dto.TicketCamionPeso,
                        CarguilloDetalleVehiculoId = dto.CarguilloDetalleVehiculoId,
                        TicketVehiculoPeso = dto.TicketVehiculoPeso,
                        TicketUnidadPeso = dto.TicketUnidadPeso,
                        TicketPesoBruto = dto.TicketPesoBruto,
                        TicketEstadoId = dto.TicketEstadoId,
                        UserModifiedAt = liquidacionInsertDto.UserCreatedAt,
                        UserModifiedName = liquidacionInsertDto.UserCreatedName
                    };
                    _dacopioContext.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = estadoTicket.TicketEstadoId; // Tesoreria
                    dto.UserModifiedAt = liquidacionInsertDto.UserCreatedAt;
                    dto.UserModifiedName = liquidacionInsertDto.UserCreatedName;
                }
                foreach (var item in liquidacionInsertDto.LiquidacionTickets)
                {
                    var detail = new LiquidacionTicket
                    {
                        TicketId = item.TicketId,
                        UserCreatedAt = liquidacionInsertDto.UserCreatedAt,
                        UserCreatedName = liquidacionInsertDto.UserCreatedName,                        
                    };
                    liquidacion.LiquidacionTickets.Add(detail);
                }
                if (liquidacionInsertDto.LiquidacionFinanciamientos != null)
                {
                    foreach (var financia in liquidacionInsertDto.LiquidacionFinanciamientos)
                    {
                        var financiamiento = new LiquidacionFinanciamiento
                        {
                            LiquidacionFinanciamientoFecha = financia.LiquidacionFinanciamientoFecha,
                            LiquidacionFinanciamientoAcuenta = financia.LiquidacionFinanciamientoACuenta,
                            LiquidacionFinanciamientoTiempo = financia.LiquidacionFinanciamientoTiempo,
                            LiquidacionFinanciamientoInteresMes = financia.LiquidacionFinanciamientoInteresMes,
                            LiquidacionFinanciamientoInteres = financia.LiquidacionFinanciamientoInteres,
                            LiquidacionFinanciamientoTotal = financia.LiquidacionFinanciamientoTotal,
                            LiquidacionFinanciamientoStatus = true,
                            UserCreatedName = liquidacionInsertDto.UserCreatedName,
                            UserCreatedAt = liquidacionInsertDto.UserCreatedAt
                        };
                        liquidacion.LiquidacionFinanciamientos.Add(financiamiento);
                    }
                }
                if(liquidacionInsertDto.LiquidacionAdicionales != null)
                {
                    foreach (var adicionales in liquidacionInsertDto.LiquidacionAdicionales)
                    {
                        var adicional = new LiquidacionAdicional
                        {
                            LiquidacionAdicionalMotivo = adicionales.LiquidacionAdicionalMotivo,
                            LiquidacionAdicionalTotal = adicionales.LiquidacionAdicionalTotal,
                            UserCreatedName = liquidacionInsertDto.UserCreatedName,
                            UserCreatedAt = liquidacionInsertDto.UserCreatedAt
                        };
                        liquidacion.LiquidacionAdicionals.Add(adicional);
                    }
                }
                _dacopioContext.Liquidacions.Add(liquidacion);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return await GetLiquidacion(liquidacion.LiquidacionId)
                    ?? throw new Exception("Liquidación guardada pero no encontrada");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<LiquidacionResultDto> UpdateLiquidacion(LiquidacionUpdateDto liquidacionUpdateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteLiquidacion(LiquidacionDeleteDto liquidacionDeleteDto)
        {
            try
            {
                if (liquidacionDeleteDto == null) throw new Exception("No se enviaron datos para eliminar la liquidación");
                var exist = await _dacopioContext.Liquidacions.FirstOrDefaultAsync(c => c.LiquidacionId == liquidacionDeleteDto.LiquidacionId)
                    ?? throw new Exception("Liquidación no encontrada");
                var estados = from est in _dacopioContext.LiquidacionEstados
                              where est.LiquidacionEstadoDescripcion.Equals("anulado")
                              select est;
                var estado = await estados.FirstOrDefaultAsync()
                    ?? throw new Exception("Estado de Liquidación no encontrado");
                exist.LiquidacionEstadoId = estado.LiquidacionEstadoId;
                exist.UserModifiedAt = liquidacionDeleteDto.UserModifiedAt;
                exist.UserModifiedName = liquidacionDeleteDto.UserModifiedName;
                await _dacopioContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LiquidacionResultDto?> GetLiquidacion(int id)
        {
            try
            {
                var query = from liquid in _dacopioContext.Liquidacions
                            join estado in _dacopioContext.LiquidacionEstados
                                on liquid.LiquidacionEstadoId equals estado.LiquidacionEstadoId
                            join tierra in _dacopioContext.Tierras
                                on liquid.TierraId equals tierra.TierraId
                            join proveedor in _dacopioContext.Proveedors
                                on liquid.ProveedorId equals proveedor.ProveedorId
                            join persona in _dacopioContext.Persons
                                on liquid.PersonaId equals persona.PersonId
                            where liquid.LiquidacionId == id
                            select new LiquidacionResultDto
                            {
                                LiquidacionId = liquid.LiquidacionId,
                                PersonaNombre = (persona.PersonName + ' ' + persona.PersonPaternalSurname + ' ' + persona.PersonMaternalSurname),
                                TierraCampo = tierra.TierraCampo,
                                ProveedorUT = proveedor.ProveedorUt,
                                LiquidacionFechaInicio = liquid.LiquidacionFechaInicio,
                                LiquidacionFechaFin = liquid.LiquidacionFechaFin,
                                LiquidacionPesoNeto = liquid.LiquidacionPesoNeto,
                                LiquidacionToneladaTotal = liquid.LiquidacionToneladaTotal,
                                LiquidacionFinanciamientoACuenta = liquid.LiquidacionFinanciamientoAcuenta,
                                LiquidacionPagar = liquid.LiquidacionPagar,
                                LiquidacionEstadoDescripcion = estado.LiquidacionEstadoDescripcion
                            };
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<LiquidacionCorteResultDto>> LiquidacionCorteResult()
        {
            try
            {
                var query = from corte in _dacopioContext.Cortes                                
                            join tierra in _dacopioContext.Tierras
                                on corte.TierraId equals tierra.TierraId
                            join at in _dacopioContext.AsignarTierras
                                on tierra.TierraId equals at.AsignarTierraTierra
                            join proveedor in _dacopioContext.Proveedors
                                on at.AsignarTierraProveedor equals proveedor.ProveedorId
                            select new LiquidacionCorteResultDto
                            {
                                CorteId = corte.CorteId,
                                TierraId = corte.TierraId,
                                TierraUC = tierra.TierraUc,
                                TierraCampo = tierra.TierraCampo,
                                ProveedorId = at.AsignarTierraProveedor,
                                ProveedorUT = proveedor.ProveedorUt,
                                CortePesoBrutoTotal = corte.CortePesoBrutoTotal,
                                CorteTotal = corte.CorteTotal
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }

    }
}
