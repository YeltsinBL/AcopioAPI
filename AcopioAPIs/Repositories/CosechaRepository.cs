using AcopioAPIs.DTOs.AsignarTierra;
using AcopioAPIs.DTOs.Cosecha;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CosechaRepository : ICosecha
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public CosechaRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<CosechaResultDto>> GetAll()
        {
            try
            {
                using var conexion = GetConnection();
                var cosecha = await conexion.QueryAsync<CosechaResultDto>(
                    "usp_CosechaGetAll", commandType: CommandType.StoredProcedure);
                return cosecha.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CosechaResultDto> GetById(int id)
        {
            try
            {
                using var conexion = GetConnection();
                var cosecha = await conexion.QueryFirstOrDefaultAsync<CosechaResultDto>(
                    "usp_CosechaGetById", new { Id = id }, commandType: CommandType.StoredProcedure
                    );
                return cosecha ?? throw new Exception("Cosecha no encontrada");
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
                return await GetById(nuevaCosecha.CosechaId);
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
            return await GetById(update.CosechaId); ;
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

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
