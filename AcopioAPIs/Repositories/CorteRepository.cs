using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class CorteRepository : ICorte
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public CorteRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<CorteEstadoDto>> GetCorteEstados()
        {
            var query = from tipo in _context.CorteEstados
                        select new CorteEstadoDto
                        {
                            CorteTipoId = tipo.CorteEstadoId,
                            CorteDescripcion = tipo.CorteEstadoDescripcion
                        };
            return await query.ToListAsync();
        }
        public async Task<List<CorteResultDto>> GetAll(int? tierraId, DateTime? fechaDesde,
            DateTime? fechaHasta, int? estadoId)
        {
            try
            {
                using var conexion = GetConnection();
                var cortes = await conexion.QueryAsync<CorteResultDto>(
                    "usp_CorteGetAll",
                    new
                    {
                        TierraId = tierraId,
                        CorteFechaDesde = fechaDesde,
                        CorteFechaHasta = fechaHasta,
                        EstadoId = estadoId
                    }, 
                    commandType: CommandType.StoredProcedure);
                return cortes.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CorteDto> GetById(int id)
        {
            try
            {
                using (var conexion = GetConnection())
                {
                    using (var multi = await conexion.QueryMultipleAsync(
                        "usp_CorteGetById", 
                        new { CorteId = id }, 
                        commandType: CommandType.StoredProcedure))
                    {
                        var master = multi.Read<CorteDto>().FirstOrDefault();
                        var details = multi.Read<CorteDetailDto>().AsList();
                        if (master == null)
                        {
                            throw new Exception("No se encontró el Corte");
                        }
                        master.CorteDetail = details;
                        return master;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CorteResultDto> Save(CorteInsertDto corteInsertDto)
        {
            try
            {
                if (corteInsertDto == null)
                    throw new Exception("No se enviaron datos para guardar el corte");
                if(corteInsertDto.CorteDetail.Count == 0)
                    throw new Exception("No se enviaron tickets para guardar el corte");

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    foreach (var ticket in corteInsertDto.CorteDetail)
                    {
                        var dto = await _context.Tickets
                            .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId)
                            ?? throw new Exception("Cosecha no encontrada");
                        var historyTicket = new TicketHistorial
                        {
                            TicketId = dto.TicketId,
                            TicketIngenio = dto.TicketIngenio,
                            TicketViaje = dto.TicketViaje,
                            CarguilloId = dto.CarguilloId,
                            TicketChofer = dto.TicketChofer,
                            TicketFecha = dto.TicketFecha,
                            CarguilloDetalleCamionId = dto.CarguilloDetalleCamionId,
                            TicketCamionPeso = dto.TicketCamionPeso,
                            CarguilloDetalleVehiculoId = dto.CarguilloDetalleVehiculoId,
                            TicketVehiculoPeso = dto.TicketVehiculoPeso,
                            TicketUnidadPeso = dto.TicketUnidadPeso,
                            TicketPesoBruto = dto.TicketPesoBruto,
                            TicketEstadoId = dto.TicketEstadoId,                            
                            UserModifiedAt = corteInsertDto.UserCreatedAt,
                            UserModifiedName = corteInsertDto.UserCreatedName
                        };
                        _context.TicketHistorials.Add(historyTicket);

                        dto.TicketEstadoId = 2; // Archivado
                        dto.UserModifiedAt = corteInsertDto.UserCreatedAt;
                        dto.UserModifiedName = corteInsertDto.UserCreatedName;
                    }

                    var corte = new Corte
                    {
                        CorteFecha = corteInsertDto.CorteFecha,
                        TierraId = corteInsertDto.TierraId,
                        CortePrecio = corteInsertDto.CortePrecio,
                        CorteEstadoId = 1, // Activo
                        CortePesoBrutoTotal = corteInsertDto.CortePesoBrutoTotal,
                        CorteTotal = corteInsertDto.CorteTotal,
                        CarguilloId = corteInsertDto.CarguilloId,
                        CarguilloPrecio = corteInsertDto.CarguilloPrecio,
                        UserCreatedAt = corteInsertDto.UserCreatedAt,
                        UserCreatedName = corteInsertDto.UserCreatedName
                    };
                    foreach (var item in corteInsertDto.CorteDetail)
                    {
                        var detail = new CorteDetalle
                        {
                            TicketId = item.TicketId,
                            UserCreatedAt = corteInsertDto.UserCreatedAt,
                            UserCreatedName = corteInsertDto.UserCreatedName,
                        };
                        corte.CorteDetalles.Add(detail);
                    }
                    _context.Cortes.Add(corte);
                    await _context.SaveChangesAsync();


                    await transaction.CommitAsync();

                    var query = from cortes in _context.Cortes
                                join tierra in _context.Tierras 
                                    on cortes.TierraId equals tierra.TierraId
                                where cortes.CorteId == corte.CorteId
                                select new CorteResultDto
                                {
                                    CorteId = corte.CorteId,
                                    CorteFecha = cortes.CorteFecha.ToDateTime(TimeOnly.Parse("0:00 PM")),
                                    TierraUC = tierra.TierraUc,
                                    CortePrecio = (double)cortes.CortePrecio,
                                    CorteCantidadTicket = corte.CorteDetalles.Count,
                                    CorteEstadoDescripcion = "Activo",
                                    CortePesoBrutoTotal = (double)corte.CortePesoBrutoTotal,
                                    CorteTotal = (double)corte.CorteTotal
                                };
                    return await query.FirstOrDefaultAsync() ??
                        throw new KeyNotFoundException("Corte guardado pero no encontrado.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Task<bool> DeleteById(int id)
        {
            throw new NotImplementedException();
        }
        private SqlConnection GetConnection() 
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
