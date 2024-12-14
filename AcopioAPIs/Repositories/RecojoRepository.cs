using AcopioAPIs.DTOs.Recojo;
using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class RecojoRepository : IRecojo
    {
        private readonly DbacopioContext _dbContext;

        public RecojoRepository(DbacopioContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RecojoEstadoResultDto>> ListRecojoEstado()
        {
            try
            {
                var query = from estado in _dbContext.RecojoEstados
                            where estado.RecojoEstadoStatus == true
                            select new RecojoEstadoResultDto
                            {
                                RecojoEstadoId = estado.RecojoEstadoId,
                                RecojoEstadoDescripcion = estado.RecojoEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<RecojoResultDto>> ListRecojo(DateOnly? fechaDesde, DateOnly? fechaHasta, int? recojoEstadoId)
        {
            try
            {
                var query = from recojo in _dbContext.Recojos
                            join estado in _dbContext.RecojoEstados on recojo.RecojoEstadoId equals estado.RecojoEstadoId
                            where (fechaDesde == null || recojo.RecojoFechaInicio >= fechaDesde)
                            && (fechaHasta == null || recojo.RecojoFechaFin <= fechaHasta)
                            && (recojoEstadoId == null || recojo.RecojoEstadoId == recojoEstadoId)
                            select new RecojoResultDto
                            {
                                RecojoId = recojo.RecojoId,
                                RecojoFechaInicio = recojo.RecojoFechaInicio,
                                RecojoFechaFin = recojo.RecojoFechaFin,
                                RecojoTicketCantidad = recojo.RecojoTicketCantidad,
                                RecojoCamionesPrecio = recojo.RecojoCamionesPrecio,
                                RecojoDiasPrecio = recojo.RecojoDiasPrecio,
                                RecojoTotalPrecio = recojo.RecojoTotalPrecio,
                                RecojoEstadoDescripcion = estado.RecojoEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RecojoDto> GetRecojoById(int recojoId)
        {
            try
            {
                var query = from recojo in _dbContext.Recojos
                            join estado in _dbContext.RecojoEstados on recojo.RecojoEstadoId equals estado.RecojoEstadoId
                            where recojo.RecojoId == recojoId
                            select new RecojoDto
                            {
                                RecojoId = recojo.RecojoId,
                                RecojoFechaInicio = recojo.RecojoFechaInicio,
                                RecojoFechaFin = recojo.RecojoFechaFin,
                                RecojoTicketCantidad = recojo.RecojoTicketCantidad,
                                RecojoCamionesPrecio = recojo.RecojoCamionesPrecio,
                                RecojoDiasPrecio = recojo.RecojoDiasPrecio,
                                RecojoTotalPrecio = recojo.RecojoTotalPrecio,
                                RecojoEstadoDescripcion = estado.RecojoEstadoDescripcion,
                                RecojoCamionesCantidad = recojo.RecojoCamionesCantidad,
                                RecojoDiasCantidad = recojo.RecojoDiasCantidad
                            };
                return await query.FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Recojo no encontrado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RecojoResultDto> SaveRecojo(RecojoInsertDto insertDto)
        {
            try
            {
                var estadoActivo = await RecojoEstadoGet("activo") ?? throw new Exception("No se encontró un estado activo");
                var newRecojo = new Recojo
                {
                    RecojoFechaInicio = insertDto.RecojoFechaInicio,
                    RecojoFechaFin = insertDto.RecojoFechaFin,
                    RecojoTicketCantidad = insertDto.RecojoTicketCantidad,
                    RecojoCamionesCantidad = insertDto.RecojoCamionesCantidad,
                    RecojoCamionesPrecio = insertDto.RecojoCamionesPrecio,
                    RecojoDiasCantidad = insertDto.RecojoDiasCantidad,
                    RecojoDiasPrecio = insertDto.RecojoDiasPrecio,
                    RecojoTotalPrecio = insertDto.RecojoTotalPrecio,
                    RecojoEstadoId = estadoActivo.RecojoEstadoId,
                    UserCreatedAt = insertDto.UserCreatedAt,
                    UserCreatedName = insertDto.UserCreatedName,
                };
                _dbContext.Recojos.Add(newRecojo);
                await _dbContext.SaveChangesAsync();
                var response = new RecojoResultDto
                {
                    RecojoFechaInicio = insertDto.RecojoFechaInicio,
                    RecojoFechaFin = insertDto.RecojoFechaFin,
                    RecojoTicketCantidad = insertDto.RecojoTicketCantidad,
                    RecojoCamionesPrecio = insertDto.RecojoCamionesPrecio,
                    RecojoDiasPrecio = insertDto.RecojoDiasPrecio,
                    RecojoTotalPrecio = insertDto.RecojoTotalPrecio,
                    RecojoEstadoDescripcion = estadoActivo.RecojoEstadoDescripcion,
                    RecojoId = newRecojo.RecojoId
                };
                return response;
            }
            catch (Exception)
            {
                Console.WriteLine("Error REpository");
                throw;
            }
        }

        public async Task<RecojoResultDto> UpdateRecojo(RecojoUpdateDto updateDto)
        {
            try
            {
                var existing = await _dbContext.Recojos.FindAsync(updateDto.RecojoId)
                    ?? throw new KeyNotFoundException("Recojo no encontrada.");
                existing.RecojoFechaInicio = updateDto.RecojoFechaInicio;
                existing.RecojoFechaFin = updateDto.RecojoFechaFin;
                existing.RecojoTicketCantidad = updateDto.RecojoTicketCantidad;
                existing.RecojoCamionesCantidad = updateDto.RecojoCamionesCantidad;
                existing.RecojoCamionesPrecio = updateDto.RecojoCamionesPrecio;
                existing.RecojoDiasCantidad = updateDto.RecojoDiasCantidad;
                existing.RecojoDiasPrecio = updateDto.RecojoDiasPrecio;
                existing.RecojoTotalPrecio = updateDto.RecojoTotalPrecio;
                existing.UserModifiedAt = updateDto.UserModifiedAt;
                existing.UserModifiedName = updateDto.UserModifiedName;

                await _dbContext.SaveChangesAsync();

                
                var response = new RecojoResultDto
                {
                    RecojoId = updateDto.RecojoId,
                    RecojoFechaInicio = updateDto.RecojoFechaInicio,
                    RecojoFechaFin = updateDto.RecojoFechaFin,
                    RecojoTicketCantidad = updateDto.RecojoTicketCantidad,
                    RecojoCamionesPrecio = updateDto.RecojoCamionesPrecio,
                    RecojoDiasPrecio = updateDto.RecojoDiasPrecio,
                    RecojoTotalPrecio = updateDto.RecojoTotalPrecio,
                    RecojoEstadoDescripcion = updateDto.RecojoEstadoDescripcion                    
                };
                return response;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteRecojo(RecojoDeleteDto deleteDto)
        {
            try
            {
                var recojo = await _dbContext.Recojos.FirstOrDefaultAsync(t => t.RecojoId == deleteDto.RecojoId);
                if (recojo == null) return false;

                var estado = await RecojoEstadoGet("anulado");
                if (estado == null) return false;

                recojo.RecojoEstadoId = estado.RecojoEstadoId!;
                recojo.UserModifiedAt = deleteDto.UserModifiedAt;
                recojo.UserModifiedName = deleteDto.UserModifiedName;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<RecojoEstado?> RecojoEstadoGet(string estadoDescripcion)
        {
            try
            {
                var query = from estado in _dbContext.RecojoEstados
                            where estado.RecojoEstadoDescripcion.ToLower() == estadoDescripcion
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
