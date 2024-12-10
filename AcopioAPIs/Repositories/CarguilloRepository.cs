using AcopioAPIs.DTOs.Carguillo;
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

        public async Task<List<CarguilloResultDto>> GetCarguillos(int tipoCarguilloId, string? titular, bool? estado)
        {
            try
            {
                var query = from carguillo in _dbacopioContext.Carguillos
                            join tipo in _dbacopioContext.CarguilloTipos
                            on carguillo.CarguilloTipoId equals tipo.CarguilloTipoId
                            where (
                                (tipoCarguilloId == 0 && (carguillo.CarguilloTipoId == 1 || carguillo.CarguilloTipoId == 2))
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
                    "CarguilloGetById", new { CarguilloId = carguilloId }, commandType: CommandType.StoredProcedure);
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
                    throw new KeyNotFoundException("Carguillo guardado pero no encontrado");

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
                    throw new KeyNotFoundException("Carguillo guardado pero no encontrado");
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
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
