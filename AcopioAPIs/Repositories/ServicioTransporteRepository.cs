﻿using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.ServicioTransporte;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ServicioTransporteRepository : IServicioTransporte
    {
        private readonly DbacopioContext _acopioContext;
        private readonly IConfiguration _configuration;

        public ServicioTransporteRepository(DbacopioContext acopioContext, IConfiguration configuration)
        {
            _acopioContext = acopioContext;
            _configuration = configuration;
        }

        public async Task<List<EstadoResultDto>> ListEstados()
        {
            try
            {
                var query = from estado in _acopioContext.ServicioTransporteEstados
                            where estado.ServicioTransporteEstadoStatus == true
                            select new EstadoResultDto
                            {
                                EstadoId = estado.ServicioTransporteEstadoId,
                                EstadoDescripcion = estado.ServicioTransporteEstadoDescripcion,
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ServicioTransporteResultDto>> ListServiciosTransporte(DateOnly? fechaDesde, DateOnly? fechaHasta, int? carguilloId, int? estadoId)
        {
            try
            {
                var query = from servicio in _acopioContext.ServicioTransportes
                            join estado in _acopioContext.ServicioTransporteEstados
                                on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                            join carguillo in _acopioContext.Carguillos
                                on servicio.CarguilloId equals carguillo.CarguilloId
                            where (fechaDesde == null || servicio.ServicioTransporteFecha >= fechaDesde)
                            && (fechaHasta == null || servicio.ServicioTransporteFecha <= fechaHasta)
                            && (carguilloId == null || servicio.CarguilloId == carguilloId)
                            && (estadoId == null || servicio.ServicioTransporteEstadoId == estadoId)
                            select new ServicioTransporteResultDto
                            {
                                ServicioTransporteId                = servicio.ServicioTransporteId,
                                ServicioTransporteFecha             = servicio.ServicioTransporteFecha,
                                ServicioTransporteCarguilloTitular  = carguillo.CarguilloTitular,
                                ServicioTransportePrecio            = servicio.ServicioTransportePrecio,
                                ServicioTransporteTotal             = servicio.ServicioTransporteTotal,
                                ServicioTransporteEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ServicioTransporteDto> GetServicioTransporte(int servicioTransporteId)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_ServicioTransporteGetById", new { ServicioTransporteId = servicioTransporteId },
                    commandType: CommandType.StoredProcedure);

                var master = multi.Read<ServicioTransporteDto>().FirstOrDefault();
                var detail = multi.Read<ServicioTransporteDetailDto>().AsList();
                if (master == null) throw new Exception("Servicio Transporte no encontrado");
                master.ServicioTransporteDetails = detail;
                return master;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ServicioTransporteResultDto> SaveServicioTransporte(ServicioTransporteInsertDto servicioTransporteInsertDto)
        {
            using var transaction = await _acopioContext.Database.BeginTransactionAsync();
                
            try
            {
                if (servicioTransporteInsertDto == null)
                    throw new Exception("No se enviaron datos para guardar el Servicio Transporte");
                if (servicioTransporteInsertDto.ServicioTransporteDetail.Count == 0)
                    throw new Exception("No se enviaron tickets para guardar el Servicio Transporte");

                var estado = await GetServicioTransporteEstado("activo") 
                    ?? throw new Exception("Estado del Servicio Transporte no encontrado");
                var carguillo = await _acopioContext.Carguillos.FindAsync(servicioTransporteInsertDto.CarguilloId)
                    ?? throw new Exception("Carguillo no encontrado");
                var ticketEstados = await GetTicketEstado("liquidación")
                    ?? throw new Exception("Estado de Tickets no encontrado");
                foreach (var ticket in servicioTransporteInsertDto.ServicioTransporteDetail)
                {
                    var dto = await _acopioContext.Tickets
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
                        UserModifiedAt = servicioTransporteInsertDto.UserCreatedAt,
                        UserModifiedName = servicioTransporteInsertDto.UserCreatedName
                    };
                    _acopioContext.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = ticketEstados.TicketEstadoId;
                    dto.UserModifiedAt = servicioTransporteInsertDto.UserCreatedAt;
                    dto.UserModifiedName = servicioTransporteInsertDto.UserCreatedName;
                }
                var servicio = new ServicioTransporte
                {
                    ServicioTransporteFecha = servicioTransporteInsertDto.ServicioTransporteFecha,
                    CarguilloId = servicioTransporteInsertDto.CarguilloId,
                    ServicioTransportePrecio = servicioTransporteInsertDto.ServicioTransportePrecio,
                    ServicioTransporteTotal = servicioTransporteInsertDto.ServicioTransporteTotal,
                    ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId,
                    UserCreatedAt = servicioTransporteInsertDto.UserCreatedAt,
                    UserCreatedName = servicioTransporteInsertDto.UserCreatedName
                };
                foreach (var item in servicioTransporteInsertDto.ServicioTransporteDetail)
                {
                    var detail = new ServicioTransporteDetalle
                    {
                        TicketId = item.TicketId,
                        ServicioTransporteDetalleStatus = true,
                        UserCreatedAt = servicioTransporteInsertDto.UserCreatedAt,
                        UserCreatedName = servicioTransporteInsertDto.UserCreatedName
                    };
                    servicio.ServicioTransporteDetalles.Add(detail);
                }

                _acopioContext.ServicioTransportes.Add(servicio);
                await _acopioContext.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = new ServicioTransporteResultDto
                {
                    ServicioTransporteId = servicio.ServicioTransporteId,
                    ServicioTransporteFecha = servicioTransporteInsertDto.ServicioTransporteFecha,
                    ServicioTransporteCarguilloTitular = carguillo.CarguilloTitular,
                    ServicioTransportePrecio = servicioTransporteInsertDto.ServicioTransportePrecio,
                    ServicioTransporteTotal = servicioTransporteInsertDto.ServicioTransporteTotal,
                    ServicioTransporteEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion,
                };
                return response;
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ServicioTransporteResultDto> UpdateServicioTransporte(ServicioTransporteUpdateDto servicioTransporteUpdateDto)
        {
            try
            {
                var existing = await _acopioContext.ServicioTransportes.FindAsync(servicioTransporteUpdateDto.ServicioTransporteId)
                    ?? throw new Exception("Servicio Transporte no encontrado");
                var carguillo = await _acopioContext.Carguillos.FindAsync(servicioTransporteUpdateDto.CarguilloId)
                    ?? throw new Exception("Carguillo no encontrado");

                existing.ServicioTransporteFecha = servicioTransporteUpdateDto.ServicioTransporteFecha;
                existing.CarguilloId = servicioTransporteUpdateDto.CarguilloId;
                existing.ServicioTransportePrecio = servicioTransporteUpdateDto.ServicioTransportePrecio;
                existing.ServicioTransporteTotal = servicioTransporteUpdateDto.ServicioTransporteTotal;
                existing.UserModifiedAt = servicioTransporteUpdateDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteUpdateDto.UserModifiedName;
                await _acopioContext.SaveChangesAsync();

                var response = new ServicioTransporteResultDto
                {
                    ServicioTransporteId = servicioTransporteUpdateDto.ServicioTransporteId,
                    ServicioTransporteFecha = servicioTransporteUpdateDto.ServicioTransporteFecha,
                    ServicioTransporteCarguilloTitular = carguillo.CarguilloTitular,
                    ServicioTransportePrecio = servicioTransporteUpdateDto.ServicioTransportePrecio,
                    ServicioTransporteTotal = servicioTransporteUpdateDto.ServicioTransporteTotal,
                    ServicioTransporteEstadoDescripcion = servicioTransporteUpdateDto.ServicioTransporteEstadoDescripcion,
                    
                };
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteServicioTransporte(ServicioTransporteDeleteDto servicioTransporteDeleteDto)
        {
            using var transaction = await _acopioContext.Database.BeginTransactionAsync();

            try
            {
                var estadoTicket = await GetTicketEstado("archivado")
                    ?? throw new Exception("Estado de Tickets no encontrado");

                var estado = await GetServicioTransporteEstado("anulado")
                    ?? throw new Exception("Estado de Servicio Transporte no encontrado");
                var existing = await _acopioContext.ServicioTransportes.FindAsync(servicioTransporteDeleteDto.ServicioTransporteId)
                    ?? throw new Exception("Servicio Transporte no encontrado");
                var querySevTransTicket = from servTransDet in _acopioContext.ServicioTransporteDetalles                                        
                                    where servTransDet.ServicioTransporteId == servicioTransporteDeleteDto.ServicioTransporteId
                                    select servTransDet;
                var sevTransTicketList = await querySevTransTicket.ToListAsync()
                    ?? throw new Exception("Tickets no encontrados en Servicio Transporte");
                foreach (var detalle in sevTransTicketList)
                {
                    var dto = await _acopioContext.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == detalle.TicketId)
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
                        UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt,
                        UserModifiedName = servicioTransporteDeleteDto.UserModifiedName
                    };
                    _acopioContext.TicketHistorials.Add(historyTicket);

                    dto.TicketEstadoId = estadoTicket.TicketEstadoId;
                    dto.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                    dto.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;

                    var servTranDet = await _acopioContext.ServicioTransporteDetalles
                        .FirstOrDefaultAsync(d =>d.ServicioTransporteDetalleId == detalle.ServicioTransporteDetalleId)
                        ?? throw new Exception("Detalle Servicio Transporte no encontrado"); ;
                    servTranDet.ServicioTransporteDetalleStatus = false;
                    servTranDet.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                    servTranDet.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;
                }

                existing.ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId;
                existing.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;
                await _acopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();

                throw;
            }
        }
        private async Task<ServicioTransporteEstado?> GetServicioTransporteEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _acopioContext.ServicioTransporteEstados
                            where estado.ServicioTransporteEstadoDescripcion.Equals(estadoDescripcion)
                            select estado;
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<TicketEstado?> GetTicketEstado(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _acopioContext.TicketEstados
                            where estado.TicketEstadoDescripcion.Equals(estadoDescripcion)
                            select estado;
                return await query.FirstOrDefaultAsync();
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
        //private async Task<T?> GetEntityByDescriptionAsync<T>(string estadoDescripcion, Expression<Func<T, string>> descripcionSelector) where T : class
        //{
        //    try
        //    {
        //        var query = _acopioContext.Set<T>()
        //            .Where(e => 
        //            EF.Property<string>(e, descripcionSelector.Parameters[0].Name).Equals(estadoDescripcion));
        //        return await query.FirstOrDefaultAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}
