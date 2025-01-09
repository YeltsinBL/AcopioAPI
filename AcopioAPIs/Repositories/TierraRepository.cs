using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class TierraRepository : ITierra
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public TierraRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<TierraResultDto>> GetTierras(
            string? tierraUC, string? tierraCampo, string? tierraSector, string? tierraValle)
        {
            var query = from tierra in _context.Tierras
                        where (tierraUC.IsNullOrEmpty() || tierra.TierraUc.Contains(tierraUC!))
                        && (tierraCampo.IsNullOrEmpty() || tierra.TierraCampo.Contains(tierraCampo!))
                        && (tierraSector.IsNullOrEmpty() || tierra.TierraSector.Contains(tierraSector!))
                        && (tierraValle.IsNullOrEmpty() || tierra.TierraValle.Contains(tierraValle!))
                        select new TierraResultDto
                        {
                            TierraId = tierra.TierraId,
                            TierraUc = tierra.TierraUc,
                            TierraCampo = tierra.TierraCampo,
                            TierraHa = tierra.TierraHa,
                            TierraSector = tierra.TierraSector,
                            TierraStatus = tierra.TierraStatus,
                            TierraValle = tierra.TierraValle
                        };                
            return await query.ToListAsync();
        }
        public async Task<TierraResultDto> GetTierraById(int id)
        {
            try
            {
                var query = from tierra in _context.Tierras where tierra.TierraId == id
                            select new TierraResultDto
                            {
                                TierraId = tierra.TierraId,
                                TierraUc = tierra.TierraUc,
                                TierraCampo = tierra.TierraCampo,
                                TierraHa = tierra.TierraHa,
                                TierraSector = tierra.TierraSector,
                                TierraStatus = tierra.TierraStatus,
                                TierraValle = tierra.TierraValle
                            };
                return await query.FirstOrDefaultAsync() ?? 
                    throw new KeyNotFoundException("Tierra no encontrada.");
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<TierraResultDto> Save(TierraInsertDto tierraInsertDto)
        {
            try
            {
                var nuevaTierra = new Tierra
                {
                    TierraUc = tierraInsertDto.TierraUc,
                    TierraCampo = tierraInsertDto.TierraCampo,
                    TierraHa = tierraInsertDto.TierraHa,
                    TierraSector = tierraInsertDto.TierraSector,
                    TierraStatus = true,
                    TierraValle = tierraInsertDto.TierraValle,
                    UserCreatedName = tierraInsertDto.UserCreatedName,
                    UserCreatedAt = tierraInsertDto.UserCreatedAt
                };

                _context.Tierras.Add(nuevaTierra);
                await _context.SaveChangesAsync();
                return new TierraResultDto
                {
                    TierraId = nuevaTierra.TierraId, // ID generado por la base de datos
                    TierraUc = nuevaTierra.TierraUc,
                    TierraCampo = nuevaTierra.TierraCampo,
                    TierraHa = nuevaTierra.TierraHa,
                    TierraSector = nuevaTierra.TierraSector,
                    TierraStatus = nuevaTierra.TierraStatus,
                    TierraValle = nuevaTierra.TierraValle
                };
            }
            catch (Exception)
            {

                throw;
            }            
        }

        public async Task<TierraResultDto> Update(TierraUpdateDto tierraUpdateDto)
        {
            try
            {
                var existingTierra = await _context.Tierras.FindAsync(tierraUpdateDto.TierraId)
                    ?? throw new KeyNotFoundException("Tierra no encontrada.");

                // Actualizar los campos necesarios
                existingTierra.TierraUc = tierraUpdateDto.TierraUc ?? existingTierra.TierraUc;
                existingTierra.TierraCampo = tierraUpdateDto.TierraCampo ?? existingTierra.TierraCampo;
                existingTierra.TierraHa = tierraUpdateDto.TierraHa ?? existingTierra.TierraHa;
                existingTierra.TierraSector = tierraUpdateDto.TierraSector ?? existingTierra.TierraSector;
                existingTierra.TierraValle = tierraUpdateDto.TierraValle ?? existingTierra.TierraValle;
                existingTierra.UserModifiedName = tierraUpdateDto.UserModifiedName;
                existingTierra.UserModifiedAt = tierraUpdateDto.UserModifiedAt;

                // Guardar los cambios
                await _context.SaveChangesAsync();
                return new TierraResultDto
                {
                    TierraId = existingTierra.TierraId,
                    TierraUc = existingTierra.TierraUc,
                    TierraCampo = existingTierra.TierraCampo,
                    TierraHa = existingTierra.TierraHa,
                    TierraSector = existingTierra.TierraSector,
                    TierraStatus = existingTierra.TierraStatus,
                    TierraValle = existingTierra.TierraValle
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(TierraDeleteDto tierraDeleteDto)
        {
            try
            {
                var tierra = await _context.Tierras.FirstOrDefaultAsync(t => t.TierraId == tierraDeleteDto.TierraId)
                    ?? throw new KeyNotFoundException("Tierra no encontrada.");

                tierra.TierraStatus = false;
                tierra.UserModifiedName = tierraDeleteDto.UserModifiedName;
                tierra.UserModifiedAt = tierraDeleteDto.UserModifiedAt;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TierraResultDto>> GetAvailableTierras()
        {
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion.QueryAsync<TierraResultDto>(
                        "usp_TierraGetAvailable", commandType: CommandType.StoredProcedure
                    );

                return proveedores.ToList();
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
