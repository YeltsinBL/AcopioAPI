using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ProveedorRepository : IProveedor
    {
        private readonly IConfiguration _configuration;

        public ProveedorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ProveedorResultDto>> List()
        {
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion.QueryAsync<ProveedorResultDto>("SELECT * FROM Proveedores");
                
                return proveedores.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Proveedor> Get(int id)
        {
            try
            {
                using (var conexion = GetConnection())
                {
                    conexion.Open();
                    var proveedores = await conexion
                        .QueryFirstOrDefaultAsync<Proveedor>("SELECT * FROM Proveedores WHERE ProveedorId = @Id", new {Id = id});
                    return proveedores ?? throw new Exception("No se encontró la información.");
                };                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProveedorResultDto> Save(ProveedorInsertDto proveedor)
        {
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion
                    .QueryFirstOrDefaultAsync<ProveedorResultDto>(
                        "usp_InsertProveedor", proveedor, commandType: CommandType.StoredProcedure
                    );
                return proveedores ?? throw new Exception("No se guardó la información.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProveedorResultDto> Update(ProveedorUpdateDto proveedor)
        {
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion
                    .QueryFirstOrDefaultAsync<ProveedorResultDto>(
                        "usp_UpdateProveedor", proveedor, commandType: CommandType.StoredProcedure
                    );
                return proveedores ?? throw new Exception("No se modificó la información.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using var conexion = GetConnection();
                await conexion.ExecuteAsync(
                    "usp_DeleteProveedor",
                    id,
                    commandType: CommandType.StoredProcedure
                );
                return true;
                
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
