using AcopioAPIs.DTOs.Tierra;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class TierraRepository : ITierra
    {
        private readonly DbacopioContext _context;

        public TierraRepository(DbacopioContext context)
        {
            _context = context;
        }

        public async Task<List<TierraResultDto>> GetTierras()
        {
            var query = from tierra in _context.Tierras
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

        public async Task<TierraResultDto> Save(TierraInsertDto tierraInsertDto)
        {
            var nuevaTierra = new Tierra
            {
                TierraUc = tierraInsertDto.TierraUc,
                TierraCampo = tierraInsertDto.TierraCampo,
                TierraHa = tierraInsertDto.TierraHa,
                TierraSector = tierraInsertDto.TierraSector,
                TierraStatus = tierraInsertDto.TierraStatus,
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

        public async Task<TierraResultDto> Update(TierraUpdateDto tierraUpdateDto)
        {
            var existingTierra = await _context.Tierras.FindAsync(tierraUpdateDto.TierraId);

            if (existingTierra == null)
            {
                throw new KeyNotFoundException("Tierra no encontrada.");
            }

            // Actualizar los campos necesarios
            existingTierra.TierraUc = tierraUpdateDto.TierraUc ?? existingTierra.TierraUc;
            existingTierra.TierraCampo = tierraUpdateDto.TierraCampo ?? existingTierra.TierraCampo;
            existingTierra.TierraHa = tierraUpdateDto.TierraHa ?? existingTierra.TierraHa;
            existingTierra.TierraSector = tierraUpdateDto.TierraSector ?? existingTierra.TierraSector;
            existingTierra.TierraStatus = tierraUpdateDto.TierraStatus;
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

        public async Task<bool> Delete(int id)
        {
            var tierra = await _context.Tierras.FirstOrDefaultAsync(t => t.TierraId == id);
            if (tierra == null)
            {
                return false;
            }

            tierra.TierraStatus = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
