using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.ServicioTransporte;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class ServicioTransporteRepository : IServicioTransporte
    {
        private readonly DbacopioContext _acopioContext;

        public ServicioTransporteRepository(DbacopioContext acopioContext)
        {
            _acopioContext = acopioContext;
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
                                ServicioTransporteTicketCantidad    = servicio.ServicioTransporteTicketCantidad,
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
                var query = from servicio in _acopioContext.ServicioTransportes
                            join estado in _acopioContext.ServicioTransporteEstados
                                on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                            join carguillo in _acopioContext.Carguillos
                                on servicio.CarguilloId equals carguillo.CarguilloId
                            where servicio.ServicioTransporteId == servicioTransporteId
                            select new ServicioTransporteDto
                            {
                                ServicioTransporteId                = servicio.ServicioTransporteId,
                                ServicioTransporteFecha             = servicio.ServicioTransporteFecha,
                                CarguilloId                         = servicio.CarguilloId,
                                ServicioTransporteCarguilloTitular  = carguillo.CarguilloTitular,
                                ServicioTransportePrecio            = servicio.ServicioTransportePrecio,
                                ServicioTransporteTicketCantidad    = servicio.ServicioTransporteTicketCantidad,
                                ServicioTransporteEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                            };
                return await query.FirstOrDefaultAsync() ??
                    throw new Exception("Servicio Transporte no encontrado");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ServicioTransporteResultDto> SaveServicioTransporte(ServicioTransporteInsertDto servicioTransporteInsertDto)
        {
            try
            {
                var estado = await GetServicioTransporteEstado("activo") 
                    ?? throw new Exception("Estado del Servicio Transporte no encontrado");
                var carguillo = await _acopioContext.Carguillos.FindAsync(servicioTransporteInsertDto.CarguilloId)
                    ?? throw new Exception("Carguillo no encontrado");

                var servicio = new ServicioTransporte
                {
                    ServicioTransporteFecha = servicioTransporteInsertDto.ServicioTransporteFecha,
                    CarguilloId = servicioTransporteInsertDto.CarguilloId,
                    ServicioTransportePrecio = servicioTransporteInsertDto.ServicioTransportePrecio,
                    ServicioTransporteTicketCantidad = servicioTransporteInsertDto.ServicioTransporteTicketCantidad,
                    ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId,
                    UserCreatedAt = servicioTransporteInsertDto.UserCreatedAt,
                    UserCreatedName = servicioTransporteInsertDto.UserCreatedName
                };

                _acopioContext.ServicioTransportes.Add(servicio);
                await _acopioContext.SaveChangesAsync();

                var response = new ServicioTransporteResultDto
                {
                    ServicioTransporteId = servicio.ServicioTransporteId,
                    ServicioTransporteFecha = servicioTransporteInsertDto.ServicioTransporteFecha,
                    ServicioTransporteCarguilloTitular = carguillo.CarguilloTitular,
                    ServicioTransportePrecio = servicioTransporteInsertDto.ServicioTransportePrecio,
                    ServicioTransporteTicketCantidad = servicioTransporteInsertDto.ServicioTransporteTicketCantidad,
                    ServicioTransporteEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                };
                return response;
            }
            catch (Exception)
            {

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
                existing.ServicioTransporteTicketCantidad = servicioTransporteUpdateDto.ServicioTransporteTicketCantidad;
                existing.UserModifiedAt = servicioTransporteUpdateDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteUpdateDto.UserModifiedName;
                await _acopioContext.SaveChangesAsync();

                var response = new ServicioTransporteResultDto
                {
                    ServicioTransporteId = servicioTransporteUpdateDto.ServicioTransporteId,
                    ServicioTransporteFecha = servicioTransporteUpdateDto.ServicioTransporteFecha,
                    ServicioTransporteCarguilloTitular = carguillo.CarguilloTitular,
                    ServicioTransportePrecio = servicioTransporteUpdateDto.ServicioTransportePrecio,
                    ServicioTransporteTicketCantidad = servicioTransporteUpdateDto.ServicioTransporteTicketCantidad,
                    ServicioTransporteEstadoDescripcion = servicioTransporteUpdateDto.ServicioTransporteEstadoDescripcion

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
            try
            {
                var estado = await GetServicioTransporteEstado("anulado")
                    ?? throw new Exception("Estado de Servicio Transporte no encontrado");
                var existing = await _acopioContext.ServicioTransportes.FindAsync(servicioTransporteDeleteDto.ServicioTransporteId)
                    ?? throw new Exception("Servicio Transporte no encontrado");
                existing.ServicioTransporteEstadoId = estado.ServicioTransporteEstadoId;
                existing.UserModifiedAt = servicioTransporteDeleteDto.UserModifiedAt;
                existing.UserModifiedName = servicioTransporteDeleteDto.UserModifiedName;
                await _acopioContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

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
    }
}
