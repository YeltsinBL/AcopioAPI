using AcopioAPIs.DTOs.Cosecha;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CosechaRepository : ICosecha
    {
        private readonly DbacopioContext _context;

        public CosechaRepository(DbacopioContext context)
        {
            _context = context;
        }

        public async Task<List<CosechaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta,
            string? tierraUC, string? proveedotUT, int? tipoCosechaId)
        {
            return await GetCosechaResults(
                    fechaDesde, fechaHasta, tierraUC, proveedotUT, tipoCosechaId, null
                ).ToListAsync();
        }

        public async Task<CosechaDto> GetById(int id)
        {
            try
            {
                var query = from cosecha in _context.Cosechas
                            join tierra in _context.Tierras
                                on cosecha.CosechaTierra equals tierra.TierraId
                            join proveedor in _context.Proveedors
                                on cosecha.CosechaProveedor equals proveedor.ProveedorId
                            join tipo in _context.CosechaTipos
                                on cosecha.CosechaCosechaTipo equals tipo.CosechaTipoId
                            where cosecha.CosechaId == id
                            select new CosechaDto
                            {
                                CosechaId = cosecha.CosechaId,
                                CosechaFecha = cosecha.CosechaFecha,
                                CosechaTierraUC = tierra.TierraUc,
                                CosechaTierraValle = tierra.TierraValle,
                                CosechaTierraSector = tierra.TierraSector,
                                CosechaProveedorUT = proveedor.ProveedorUt,
                                CosechaTierraCampo = tierra.TierraCampo,
                                CosechaCosechaTipo = tipo.CosechaTipoDescripcion,
                                CosechaCosechaId = cosecha.CosechaCosechaTipo,
                                CosechaHAS = cosecha.CosechaHas,
                                CosechaHumedad = cosecha.CosechaHumedad,
                                CosechaProveedorId = cosecha.CosechaProveedor,
                                CosechaRed = cosecha.CosechaRed,
                                CosechaSac = cosecha.CosechaSac,
                                CosechaSupervisor = cosecha.CosechaSupervisor,
                                CosechaTierraId = cosecha.CosechaTierra
                            };
                return await query.FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Cosecha no encontrada");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CosechaResultDto> Save(CosechaInsertDto insert)
        {
            try
            {
                var nuevaCosecha = new Cosecha
                {
                    CosechaFecha = insert.CosechaFecha,
                    CosechaTierra = insert.CosechaTierraId,
                    CosechaProveedor = insert.CosechaProveedorId,
                    CosechaSupervisor = insert.CosechaSupervisor,
                    CosechaHas = insert.CosechaHas,
                    CosechaSac = insert.CosechaSac,
                    CosechaRed = insert.CosechaRed,
                    CosechaHumedad = insert.CosechaHumedad,
                    CosechaCosechaTipo = insert.CosechaCosechaTipoId,
                    UserCreatedAt = insert.UserCreatedAt,
                    UserCreatedName = insert.UserCreatedName
                };
                _context.Cosechas.Add( nuevaCosecha );
                await _context.SaveChangesAsync();
                return await GetCosechaResults(null, null, "","",null, nuevaCosecha.CosechaId)
                    .FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CosechaResultDto> Update(CosechaUpdateDto update)
        {
            var existing = await _context.Cosechas
                .FirstOrDefaultAsync(c => c.CosechaId == update.CosechaId) 
                ?? throw new Exception("Cosecha no encontrada");

            existing.CosechaHas = update.CosechaHas;
            existing.CosechaSac = update.CosechaSac;
            existing.CosechaRed = update.CosechaRed;
            existing.CosechaHumedad = update.CosechaHumedad;
            existing.CosechaCosechaTipo = update.CosechaCosechaTipoId;
            existing.UserModifiedAt = update.UserModifiedAt;
            existing.UserModifiedName = update.UserModifiedName;

            await _context.SaveChangesAsync();
            return await GetCosechaResults(null, null, "", "", null, update.CosechaId)
                    .FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("");
        }
        public async Task<List<CosechaTipoDto>> GetTipo()
        {
            var query = from cosechaTipo in _context.CosechaTipos
                     select new CosechaTipoDto
                     {
                         CosechaTipoId = cosechaTipo.CosechaTipoId,
                         Descripcion = cosechaTipo.CosechaTipoDescripcion
                     };
            return await query.ToListAsync();
        }

        private IQueryable<CosechaResultDto> GetCosechaResults(DateOnly? fechaDesde, DateOnly? fechaHasta,
            string? tierraUC, string? proveedotUT, int? tipoCosechaId, int? cosechaId)
        {
            return from cosecha in _context.Cosechas
                               join tierra in _context.Tierras
                                   on cosecha.CosechaTierra equals tierra.TierraId
                               join proveedor in _context.Proveedors
                                   on cosecha.CosechaProveedor equals proveedor.ProveedorId
                               join tipo in _context.CosechaTipos
                                   on cosecha.CosechaCosechaTipo equals tipo.CosechaTipoId
                                where (fechaDesde == null || cosecha.CosechaFecha>=fechaDesde)
                                && (fechaHasta == null || cosecha.CosechaFecha <=fechaHasta)
                                && (tierraUC.IsNullOrEmpty() || tierra.TierraUc.Contains(tierraUC!))
                                && (proveedotUT.IsNullOrEmpty() || proveedor.ProveedorUt.Contains(proveedotUT!))
                                && (tipoCosechaId == null || tipo.CosechaTipoId == tipoCosechaId)
                                && (cosechaId == null || cosecha.CosechaId == cosechaId)
                               select new CosechaResultDto
                               {
                                   CosechaId = cosecha.CosechaId,
                                   CosechaFecha = cosecha.CosechaFecha,
                                   CosechaTierraUC = tierra.TierraUc,
                                   CosechaTierraValle = tierra.TierraValle,
                                   CosechaTierraSector = tierra.TierraSector,
                                   CosechaProveedorUT = proveedor.ProveedorUt,
                                   CosechaTierraCampo = tierra.TierraCampo,
                                   CosechaCosechaTipo = tipo.CosechaTipoDescripcion
                               };
        }
    }
}
