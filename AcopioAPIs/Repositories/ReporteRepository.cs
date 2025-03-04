using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.InformeIngresoGasto;
using AcopioAPIs.DTOs.Reporte;
using AcopioAPIs.Models;
using AcopioAPIs.Utils;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ReporteRepository : IReporte
    {
        private readonly IConfiguration _configuration;

        public ReporteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResultDto<List<ReporteGastoResult>>> GetResultGasto(int? personaId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                using var conexion = GetConnection();
                using var informe = await conexion.QueryMultipleAsync(
                    "usp_ReporteGasto",
                    new { PersonaId = personaId, FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                    commandType: CommandType.StoredProcedure);
                var master = (await informe.ReadAsync<ReporteGastoResult>()).ToList();
                return ResponseHelper.ReturnData(master, "Informe recuperado");

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
