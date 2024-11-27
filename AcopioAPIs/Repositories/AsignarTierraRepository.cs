using AcopioAPIs.DTOs.AsignarTierra;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class AsignarTierraRepository : IAsignarTierra
    {
        private readonly DbacopioContext _context;

        public AsignarTierraRepository(DbacopioContext context)
        {
            _context = context;
        }

        public async Task<List<AsignarTierraResultDto>> GetAll()
        {
            var query = from asignarTierra in _context.AsignarTierras
                        select new AsignarTierraResultDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedor = asignarTierra.AsignarTierraProveedor,
                            AsignarTierraTierra = asignarTierra.AsignarTierraTierra,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus
                        };
            return await query.ToListAsync();
        }

        public async Task<AsignarTierraResultDto> GetById(int id)
        {
            var query = from asignarTierra in _context.AsignarTierras where asignarTierra.AsignarTierraId == id
                        select new AsignarTierraResultDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedor = asignarTierra.AsignarTierraProveedor,
                            AsignarTierraTierra = asignarTierra.AsignarTierraTierra,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
        }

        public async Task<AsignarTierraResultDto> Save(AsignarTierraInsertDto asignarTierraInsertDto)
        {
            var nuevaAsignaTierra = new AsignarTierra
            {
                AsignarTierraFecha = asignarTierraInsertDto.AsignarTierraFecha,
                AsignarTierraProveedor = asignarTierraInsertDto.AsignarTierraProveedor,
                AsignarTierraTierra = asignarTierraInsertDto.AsignarTierraTierra,
                AsignarTierraStatus = true,
                UserCreatedAt = asignarTierraInsertDto.UserCreatedAt,
                UserCreatedName = asignarTierraInsertDto.UserCreatedName
            };
            _context.Add(nuevaAsignaTierra);
            await _context.SaveChangesAsync();

            return new AsignarTierraResultDto 
            { 
                AsignarTierraId = nuevaAsignaTierra.AsignarTierraId,
                AsignarTierraFecha = nuevaAsignaTierra.AsignarTierraFecha,
                AsignarTierraProveedor = nuevaAsignaTierra.AsignarTierraProveedor,
                AsignarTierraTierra = nuevaAsignaTierra.AsignarTierraTierra,
                AsignarTierraStatus = true
            };
        }

        public async Task<AsignarTierraResultDto> Update(AsignarTierraUpdateDto asignarTierraUpdateDto)
        {
            var existingTierraAsignada = await _context.AsignarTierras
                .FirstOrDefaultAsync(at => at.AsignarTierraId == asignarTierraUpdateDto.AsignarTierraId);
            if (existingTierraAsignada == null)
            {
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
            }
            existingTierraAsignada.AsignarTierraFecha = asignarTierraUpdateDto.AsignarTierraFecha;
            existingTierraAsignada.AsignarTierraProveedor = asignarTierraUpdateDto.AsignarTierraProveedor;
            existingTierraAsignada.AsignarTierraTierra = asignarTierraUpdateDto.AsignarTierraTierra;
            existingTierraAsignada.UserModifiedAt = asignarTierraUpdateDto.UserModifiedAt;
            existingTierraAsignada.UserModifiedName = asignarTierraUpdateDto.UserModifiedName;
            await _context.SaveChangesAsync();
            return new AsignarTierraResultDto
            {
                AsignarTierraId = existingTierraAsignada.AsignarTierraId,
                AsignarTierraFecha = existingTierraAsignada.AsignarTierraFecha,
                AsignarTierraProveedor = existingTierraAsignada.AsignarTierraProveedor,
                AsignarTierraTierra = existingTierraAsignada.AsignarTierraTierra,
                AsignarTierraStatus = existingTierraAsignada.AsignarTierraStatus
            };
        }
        public async Task<bool> Delete(int id)
        {
            var existingTierraAsignada = await _context.AsignarTierras
                .FirstOrDefaultAsync(at => at.AsignarTierraId == id);
            if (existingTierraAsignada == null)
            {
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
            }
            existingTierraAsignada.AsignarTierraStatus = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
