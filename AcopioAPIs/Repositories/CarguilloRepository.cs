using AcopioAPIs.DTOs.Carguillo;
using AcopioAPIs.DTOs.Ticket;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CarguilloRepository : ICarguillo
    {
        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;

        public CarguilloRepository(DbacopioContext dbacopioContext, IConfiguration configuration)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
        }

        public async Task<List<CarguilloTipoResultDto>> GetCarguilloTipos(bool isCarguillo)
        {
            try
            {
                var query = from tipo in _dbacopioContext.CarguilloTipos
                            where tipo.IsCarguillo == isCarguillo
                            select new CarguilloTipoResultDto
                            {
                                CarguilloTipoId = tipo.CarguilloTipoId,
                                CarguilloTipoDescripcion = tipo.CarguilloTipoDescripcion,
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CarguilloResultDto>> GetCarguillos(int? tipoCarguilloId, string? titular, bool? estado)
        {
            try
            {
                var query = from carguillo in _dbacopioContext.Carguillos
                            join tipo in _dbacopioContext.CarguilloTipos
                            on carguillo.CarguilloTipoId equals tipo.CarguilloTipoId
                            where (
                                ((tipoCarguilloId == null || tipoCarguilloId == 0) && tipo.CarguilloTipoEstado == true)
                                || carguillo.CarguilloTipoId == tipoCarguilloId
                            )
                            && (titular == null || titular == "" || carguillo.CarguilloTitular.Contains(titular!))
                            && (estado == null || estado == carguillo.CarguilloEstado)
                            select new CarguilloResultDto
                            {
                                CarguilloId = carguillo.CarguilloId,
                                CarguilloTipoDescripcion = tipo.CarguilloTipoDescripcion,
                                CarguilloTitular = carguillo.CarguilloTitular,
                                CarguilloEstado = carguillo.CarguilloEstado
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CarguilloDto> GetCarguilloById(int carguilloId)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_CarguilloGetById", new { CarguilloId = carguilloId }, commandType: CommandType.StoredProcedure);
                var master = await multi.ReadFirstOrDefaultAsync<CarguilloDto>() 
                    ?? throw new Exception("No se encontró el Carguillo");
                master.CarguilloDetalle  = (await multi.ReadAsync<CarguilloDetalleDto>()).ToList();
                return master;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CarguilloResultDto> SaveCarguillo(CarguilloInsertDto insertDto)
        {
            try
            {
                if (insertDto == null) throw new Exception("No se enviaron datos para guardar el carguillo");
                if(insertDto.CarguilloTipoId == 2 && 
                    (insertDto.CarguilloDetalle == null || insertDto.CarguilloDetalle.Count==0))
                    throw new Exception("No se enviaron datos para guardar la(s) placa(s) del carguillo");
                
                using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
                var exists = await _dbacopioContext.Carguillos.FirstOrDefaultAsync(c =>
                c.CarguilloTitular == insertDto.CarguilloTitular && c.CarguilloTipoId == insertDto.CarguilloTipoId);
                if (exists != null) throw new Exception("Ya existe un carguillo con este titular.");
                var carguillo = new Carguillo
                {
                    CarguilloTitular = insertDto.CarguilloTitular,
                    CarguilloTipoId = insertDto.CarguilloTipoId,
                    CarguilloEstado = true,
                    UserCreatedAt = insertDto.UserCreatedAt,
                    UserCreatedName = insertDto.UserCreatedName,
                };
                
                if(insertDto.CarguilloTipoId == 2 && insertDto.CarguilloDetalle != null)
                {
                    foreach (var placa in insertDto.CarguilloDetalle)
                    {
                        var existsPlaca = await _dbacopioContext.CarguilloDetalles.FirstOrDefaultAsync(d =>
                        d.CarguilloTipoId == placa.CarguilloTipoId && d.CarguilloDetallePlaca == placa.CarguilloDetallePlaca);

                        if (existsPlaca != null) throw new Exception($"Ya existe la placa {placa.CarguilloDetallePlaca.ToUpper()}.");
                        
                        var cam = new CarguilloDetalle
                        {
                            CarguilloDetalleEstado = true,
                            CarguilloDetallePlaca = placa.CarguilloDetallePlaca,
                            CarguilloTipoId = placa.CarguilloTipoId,
                            UserCreatedAt = insertDto.UserCreatedAt,
                            UserCreatedName = insertDto.UserCreatedName,
                        };
                        carguillo.CarguilloDetalles.Add(cam);
                    }
                }
                
                _dbacopioContext.Carguillos.Add(carguillo);
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return await CarguilloResult(carguillo.CarguilloId) ??
                    throw new KeyNotFoundException("Verifique si se guardó el carguillo");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CarguilloResultDto> UpdateCarguillo(CarguilloUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null) throw new Exception("No se enviaron datos para guardar el carguito");
                if (updateDto.CarguilloTipoId == 2 &&
                    (updateDto.CarguilloDetalle == null || updateDto.CarguilloDetalle.Count == 0))
                    throw new Exception("No se enviaron datos para guardar la(s) placa(s) del camión/carreta");
                
                using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
                
                var exists = await _dbacopioContext.Carguillos.FirstOrDefaultAsync(c =>
                c.CarguilloTitular == updateDto.CarguilloTitular && c.CarguilloTipoId == updateDto.CarguilloTipoId
                && c.CarguilloId != updateDto.CarguilloId);
                if (exists != null) throw new Exception("Ya existe un carguillo con este titular.");

                var existCarguillo = await _dbacopioContext.Carguillos
                    .Include(c => c.CarguilloDetalles)
                    .FirstOrDefaultAsync(c =>
                c.CarguilloId == updateDto.CarguilloId) ?? throw new Exception("No se encontró al carguillo");
                
                existCarguillo.CarguilloTitular = updateDto.CarguilloTitular;
                existCarguillo.CarguilloTipoId = updateDto.CarguilloTipoId;
                existCarguillo.UserModifiedAt = updateDto.UserModifiedAt;
                existCarguillo.UserModifiedName = updateDto.UserModifiedName;

                if(updateDto.CarguilloTipoId == 2)
                {
                    foreach (var camion in updateDto.CarguilloDetalle!)
                    {
                        var existCamion = existCarguillo.CarguilloDetalles
                            .FirstOrDefault(cd =>
                            cd.CarguilloDetalleId == camion.CarguilloDetalleId);
                        if(existCamion != null && existCamion.CarguilloDetalleId > 0)
                        {
                            existCamion.CarguilloTipoId = camion.CarguilloTipoId;
                            existCamion.CarguilloDetallePlaca = camion.CarguilloDetallePlaca;
                            existCamion.CarguilloDetalleEstado = camion.CarguilloDetalleEstado;
                            existCamion.UserModifiedAt = updateDto.UserModifiedAt;
                            existCamion.UserModifiedName = updateDto.UserModifiedName;
                        }
                        else
                        {
                            existCarguillo.CarguilloDetalles.Add(new CarguilloDetalle
                            {
                                CarguilloId = updateDto.CarguilloId,
                                CarguilloTipoId = camion.CarguilloTipoId,
                                CarguilloDetallePlaca = camion.CarguilloDetallePlaca,
                                CarguilloDetalleEstado = camion.CarguilloDetalleEstado,
                                UserCreatedAt = updateDto.UserModifiedAt,
                                UserCreatedName =updateDto.UserModifiedName
                            }); 
                        }
                    }
                }

                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return await CarguilloResult(updateDto.CarguilloId) ??
                    throw new KeyNotFoundException("Verifique si se actualizó el carguillo");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<CarguilloResultDto> CarguilloResult(int carguilloId)
        {
            try
            {
                var query = from car in _dbacopioContext.Carguillos
                            join tipo in _dbacopioContext.CarguilloTipos
                            on car.CarguilloTipoId equals tipo.CarguilloTipoId
                            where car.CarguilloId == carguilloId
                            select new CarguilloResultDto
                            {
                                CarguilloId = car.CarguilloId,
                                CarguilloTipoDescripcion = tipo.CarguilloTipoDescripcion,
                                CarguilloTitular = car.CarguilloTitular,
                                CarguilloEstado = car.CarguilloEstado
                            };
                return await query.FirstOrDefaultAsync() ??
                    throw new KeyNotFoundException("Carguillo guardado pero no encontrado");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CarguilloPlacasResultDto> GetCarguilloDetalles(int carguilloId)
        {
            try
            {
                var tipos = await GetCarguilloTipos(false);

                using var connection = GetConnection();
                // Define los IDs de los tipos que vas a usar
                var tipoVehiculoId = tipos.FirstOrDefault(t => t.CarguilloTipoDescripcion.Contains("Vehiculo"))?.CarguilloTipoId;
                var tipoCamionId = tipos.FirstOrDefault(t => t.CarguilloTipoDescripcion.Contains("Camión/Carreta"))?.CarguilloTipoId;

                // Consulta SQL para obtener los detalles
                var sql = @"
                            SELECT 
                                cd.CarguilloDetalleId,
                                cd.CarguilloId,
                                cd.CarguilloTipoId,
                                cd.CarguilloDetallePlaca,
                                ct.CarguilloTipoDescripcion,
                                cd.CarguilloDetalleEstado
                            FROM CarguilloDetalle cd
                            INNER JOIN CarguilloTipo ct ON cd.CarguilloTipoId = ct.CarguilloTipoId
                            WHERE cd.CarguilloId = @CarguilloId and cd.CarguilloDetalleEstado=1";

                // Ejecuta la consulta y obtén los detalles
                var detalles = await connection.QueryAsync<CarguilloDetalleDto>(sql, new { CarguilloId = carguilloId });

                // Clasifica los resultados según el tipo
                var result = new CarguilloPlacasResultDto
                {
                    CarguilloTipoCamion = detalles
                        .Where(d => d.CarguilloTipoId == tipoCamionId)
                        .ToList(),

                    CarguilloTipoVehiculo = detalles
                        .Where(d => d.CarguilloTipoId == tipoVehiculoId)
                        .ToList()
                };

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<CarguilloResultDto>> GetCarguillosTicket()
        {
            try
            {

                var estadoTicket = from estados in _dbacopioContext.TicketEstados
                             where estados.TicketEstadoDescripcion.Equals("archivado")
                             select estados;
                var estado = await estadoTicket.FirstOrDefaultAsync()
                    ?? throw new Exception("Estado de Ticket no encontrado");

                var query = (from c in _dbacopioContext.Carguillos
                             join ct in _dbacopioContext.CarguilloTipos
                                 on c.CarguilloTipoId equals ct.CarguilloTipoId
                             join t in _dbacopioContext.Tickets
                                 on c.CarguilloId equals t.CarguilloId
                             where c.CarguilloEstado == true
                                   && t.TicketEstadoId == estado.TicketEstadoId
                             select new CarguilloResultDto
                             {
                                 
                                 CarguilloId = c.CarguilloId,
                                 CarguilloTitular = c.CarguilloTitular,
                                 CarguilloTipoDescripcion = ct.CarguilloTipoDescripcion,
                                 CarguilloEstado = c.CarguilloEstado                                 
                             })
                             .Distinct()
                             .ToListAsync();
                return await query;
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
