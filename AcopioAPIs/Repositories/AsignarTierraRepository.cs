using AcopioAPIs.DTOs.AsignarTierra;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<List<AsignarTierraDto>> GetAll(string? tierraUC, string? proveedorUT, DateOnly? fechaDesde, DateOnly? fechaHasta)
        {
            var query = from asignarTierra in _context.AsignarTierras
                        join proveedor in _context.Proveedors
                            on asignarTierra.AsignarTierraProveedor equals proveedor.ProveedorId
                        join tierra in _context.Tierras
                            on asignarTierra.AsignarTierraTierra equals tierra.TierraId
                        join pp in _context.ProveedorPeople on proveedor.ProveedorId equals pp.ProveedorId
                        join per in _context.Persons on pp.PersonId equals per.PersonId
                        where (tierraUC.IsNullOrEmpty() || tierra.TierraUc.Contains(tierraUC!))
                        && (proveedorUT.IsNullOrEmpty() || proveedor.ProveedorUt.Contains(proveedorUT!))
                        && (fechaDesde == null || asignarTierra.AsignarTierraFecha >= fechaDesde)
                        && (fechaHasta == null || asignarTierra.AsignarTierraFecha <= fechaHasta)
                        orderby asignarTierra.AsignarTierraFecha
                        group new { asignarTierra, tierra, proveedor, per } by new
                        {
                            asignarTierra.AsignarTierraId,
                            asignarTierra.AsignarTierraFecha,
                            asignarTierra.AsignarTierraProveedor,
                            asignarTierra.AsignarTierraTierra,
                            proveedor.ProveedorUt,
                            tierra.TierraUc,
                            asignarTierra.AsignarTierraStatus,
                            tierra.TierraCampo,
                        } into grouped
                        select new AsignarTierraDto
                        {
                            AsignarTierraId = grouped.Key.AsignarTierraId,
                            AsignarTierraFecha = grouped.Key.AsignarTierraFecha,
                            AsignarTierraProveedorUT = grouped.Key.ProveedorUt,
                            AsignarTierraTierraUC = grouped.Key.TierraUc,
                            AsignarTierraStatus = grouped.Key.AsignarTierraStatus,
                            TierraCampo = grouped.Key.TierraCampo,
                            AsignarTierraProveedorId = grouped.Key.AsignarTierraProveedor,
                            AsignarTierraTierraId = grouped.Key.AsignarTierraTierra,
                            ProveedoresNombres = string.Join(", ", grouped.Select(g =>
                    g.per.PersonName + " " + g.per.PersonPaternalSurname + " " + g.per.PersonMaternalSurname))
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
                            TierraCampo = tierra.TierraCampo
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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (asignarTierraUpdateDto == null) throw new Exception("No se enviaron datos para guardar la asignación de tierra");

                var asignado = await _context.AsignarTierras.FindAsync(asignarTierraUpdateDto.AsignarTierraId)
                    ?? throw new Exception("Asignación de Tierra no encontrada");
                var historial = new AsignarTierraHistorial
                {
                    ProveedorId = asignado.AsignarTierraProveedor,
                    TierraId = asignado.AsignarTierraTierra,
                    AsignarTierraFecha = asignado.AsignarTierraFecha,
                    AsignarTierraStatus = asignado.AsignarTierraStatus,
                    UserModifiedAt = asignarTierraUpdateDto.UserModifiedAt,
                    UserModifiedName = asignarTierraUpdateDto.UserModifiedName
                };
                _context.Add(historial);
                await _context.SaveChangesAsync();

                asignado.AsignarTierraProveedor = asignarTierraUpdateDto.AsignarTierraProveedorId;
                asignado.AsignarTierraTierra = asignarTierraUpdateDto.AsignarTierraTierraId;
                asignado.AsignarTierraFecha = asignarTierraUpdateDto.AsignarTierraFecha;
                asignado.UserModifiedName = asignarTierraUpdateDto.UserModifiedName;
                asignado.UserModifiedAt = asignarTierraUpdateDto.UserModifiedAt;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetAsignaTierra(asignarTierraUpdateDto.AsignarTierraId);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
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
                            AsignarTierraFecha = asignarTierra.AsignarTierraFecha,
                            AsignarTierraProveedorUT = proveedor.ProveedorUt,
                            AsignarTierraTierraUC = tierra.TierraUc,
                            AsignarTierraStatus = asignarTierra.AsignarTierraStatus,
                            TierraCampo = tierra.TierraCampo
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
