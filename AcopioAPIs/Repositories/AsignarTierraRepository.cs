using AcopioAPIs.DTOs.AsignarTierra;
using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class AsignarTierraRepository : IAsignarTierra
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public AsignarTierraRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<AsignarTierraDto>> GetAll()
        {
            var query = from asignarTierra in _context.AsignarTierras
                        join proveedor in _context.Proveedors
                            on asignarTierra.AsignarTierraProveedor equals proveedor.ProveedorId
                        join tierra in _context.Tierras
                            on asignarTierra.AsignarTierraTierra equals tierra.TierraId
                        select new AsignarTierraDto
                        {
                            AsignarTierraId = asignarTierra.AsignarTierraId,
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedorId = asignarTierra.AsignarTierraProveedor,
                            AsignarTierraTierraId = asignarTierra.AsignarTierraTierra,
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
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion
                    .QueryFirstOrDefaultAsync<AsignarTierraResultDto>(
                        "usp_AsignarTierraUpdate", asignarTierraUpdateDto, commandType: CommandType.StoredProcedure
                    );
                return proveedores ?? throw new Exception("No se modificó la información.");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Delete(AsignarTierraDeleteDto asignarTierraDeleteDto)
        {
            try
            {
                using var conexion = GetConnection();
                await conexion
                    .ExecuteAsync(
                        "usp_AsignarTierraDelete", asignarTierraDeleteDto, commandType: CommandType.StoredProcedure
                    );
                return true;
            }
            catch (Exception)
            {

                throw;
            }
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
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                            AsignarTierraProveedorUT = proveedor.ProveedorUt,
                            AsignarTierraTierraUC = tierra.TierraUc,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus
                        };
            return await query.FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("Tierra Asignada no encontrada.");
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
