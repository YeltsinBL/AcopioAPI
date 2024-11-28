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
                        join proveedor in _context.Proveedors
                            on asignarTierra.AsignarTierraProveedor equals proveedor.ProveedorId
                        join tierra in _context.Tierras
                            on asignarTierra.AsignarTierraTierra equals tierra.TierraId
                        select new AsignarTierraResultDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedorUT = proveedor.ProveedorUt,
                            AsignarTierraTierraUC = tierra.TierraUc,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus
                        };
            return await query.ToListAsync();
        }

        public async Task<AsignarTierraDto> GetById(int id)
        {
            var query = from asignarTierra in _context.AsignarTierras
                        join proveedor in _context.Proveedors
                            on asignarTierra.AsignarTierraProveedor equals proveedor.ProveedorId
                        join tierra in _context.Tierras
                            on asignarTierra.AsignarTierraTierra equals tierra.TierraId
                        where asignarTierra.AsignarTierraId == id
                        select new AsignarTierraDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedorId = asignarTierra.AsignarTierraProveedor,
                            AsignarTierraTierraId = asignarTierra.AsignarTierraTierra,
                            AsignarTierraProveedorUT = proveedor.ProveedorUt,
                            AsignarTierraTierraUC = tierra.TierraUc,
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
        }

        public async Task<AsignarTierraResultDto> Save(AsignarTierraInsertDto asignarTierraInsertDto)
        {
            var nuevaAsignaTierra = new AsignarTierra
            {
                AsignarTierraFecha = asignarTierraInsertDto.AsignarTierraFecha,
                AsignarTierraProveedor = asignarTierraInsertDto.AsignarTierraProveedorId,
                AsignarTierraTierra = asignarTierraInsertDto.AsignarTierraTierraId,
                AsignarTierraStatus = true,
                UserCreatedAt = asignarTierraInsertDto.UserCreatedAt,
                UserCreatedName = asignarTierraInsertDto.UserCreatedName
            };
            _context.Add(nuevaAsignaTierra);
            await _context.SaveChangesAsync();

            return await GetAsignaTierra(nuevaAsignaTierra.AsignarTierraId);           
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
            existingTierraAsignada.AsignarTierraProveedor = asignarTierraUpdateDto.AsignarTierraProveedorId;
            existingTierraAsignada.AsignarTierraTierra = asignarTierraUpdateDto.AsignarTierraTierraId;
            existingTierraAsignada.UserModifiedAt = asignarTierraUpdateDto.UserModifiedAt;
            existingTierraAsignada.UserModifiedName = asignarTierraUpdateDto.UserModifiedName;
            await _context.SaveChangesAsync();
            return await GetAsignaTierra(asignarTierraUpdateDto.AsignarTierraId);
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

        public async Task<AsignarTierraResultDto> GetAsignaTierra(int id)
        {
            var query = from asignarTierra in _context.AsignarTierras
                        join proveedor in _context.Proveedors
                            on asignarTierra.AsignarTierraProveedor equals proveedor.ProveedorId
                        join tierra in _context.Tierras
                            on asignarTierra.AsignarTierraTierra equals tierra.TierraId
                        where asignarTierra.AsignarTierraId == id
                        select new AsignarTierraResultDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedorUT = proveedor.ProveedorUt,
                            AsignarTierraTierraUC = tierra.TierraUc,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
        }
    }
}
