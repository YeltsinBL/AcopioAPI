using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.FacturaVenta;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class FacturaVentaRepository : IFacturaVenta
    {

        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;

        public FacturaVentaRepository(DbacopioContext dbacopioContext, IConfiguration configuration)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
        }

        public async Task<List<FacturaVentaResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, string? numero, int? estadoId)
        {
            try
            {
                var query = from factura in _dbacopioContext.FacturaVenta
                            join estado in _dbacopioContext.FacturaVentaEstados
                                on factura.FacturaVentaEstadoId equals estado.FacturaVentaEstadoId
                            where (fechaDesde == null || factura.FacturaVentaFecha >= fechaDesde)
                            && (fechaHasta == null || factura.FacturaVentaFecha <= fechaHasta)
                            && (numero == null || factura.FacturaVentaNumero.Contains(numero))
                            && (estadoId == null || factura.FacturaVentaEstadoId == estadoId)
                            select new FacturaVentaResultDto
                            {
                                FacturaVentaId = factura.FacturaVentaId,
                                FacturaVentaFecha = factura.FacturaVentaFecha,
                                FacturaNumero = factura.FacturaVentaNumero,
                                FacturaCantidad = factura.FacturaVentaCantidad,
                                FacturaUnidadMedida = factura.FacturaVentaUnidadMedida,
                                FacturaImporteTotal = factura.FacturaVentaImporte,
                                FacturaDetraccion = factura.FacturaVentaDetraccion,
                                FacturaPendientePago = factura.FacturaVentaPendientePago,
                                FacturaVentaEstado =estado.FacturaVentaDescripcion,
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TipoResultDto>> GetAllEstados()
        {
            try
            {
                var query = from estados in _dbacopioContext.FacturaVentaEstados
                            select new TipoResultDto 
                            { 
                                Id= estados.FacturaVentaEstadoId,
                                Nombre= estados.FacturaVentaDescripcion 
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<FacturaVentaDto>> GetById(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_FacturaVentaGetById", new { FacturaVentaId = id },
                    commandType: CommandType.StoredProcedure);

                var master = multi.Read<FacturaVentaDto>().FirstOrDefault();
                var detail = multi.Read<FacturaVentaPersonaDto>().AsList();
                if (master == null) throw new Exception("Factura Venta no encontrada");
                master.FacturaVentaPersonas = detail;
                return new ResultDto<FacturaVentaDto>
                {
                    Result= true,
                    ErrorMessage="Factura Venta recuperado",
                    Data = master
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<FacturaVentaResultDto>> Save(FacturaVentaInsertDto insertDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (insertDto == null) throw new Exception("No se enviaron datos para guardar la Factura Venta");
                if(insertDto.FacturaVentaPersonas == null) 
                    throw new Exception("No se enviaron datos del sambrador para guardar la Factura Venta");
                var estado = await _dbacopioContext.FacturaVentaEstados.FindAsync(insertDto.FacturaVentaEstadoId)
                    ?? throw new Exception("No se encontró el estado de la Factura Venta");
                var factura = new FacturaVentum
                {
                    FacturaVentaFecha= insertDto.FacturaVentaFecha,
                    FacturaVentaNumero = insertDto.FacturaNumero,
                    FacturaVentaEstadoId = estado.FacturaVentaEstadoId,
                    FacturaVentaCantidad = insertDto.FacturaCantidad,
                    FacturaVentaUnidadMedida = insertDto.FacturaUnidadMedida,
                    FacturaVentaImporte = insertDto.FacturaImporteTotal,
                    FacturaVentaDetraccion = insertDto.FacturaDetraccion,
                    FacturaVentaPendientePago = insertDto.FacturaPendientePago,
                    UserCreatedAt = insertDto.UserCreatedAt,
                    UserCreatedName = insertDto.UserCreatedName,
                };
                foreach (var item in insertDto.FacturaVentaPersonas)
                {
                    var persona = await _dbacopioContext.Persons.FindAsync(item.PersonaId)
                        ?? throw new Exception("Persona no encontrada");
                    var facturaPerson = new FacturaVentaPersona
                    {
                        PersonaId = item.PersonaId,
                        FacturaVentaPersonStatus = true,
                        UserCreatedAt = insertDto.UserCreatedAt,
                        UserCreatedName = insertDto.UserCreatedName,
                    };
                    factura.FacturaVentaPersonas.Add(facturaPerson);
                }
                _dbacopioContext.Add(factura);
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<FacturaVentaResultDto>
                {
                    Result = true,
                    ErrorMessage = "Factura Venta guardada",
                    Data = new FacturaVentaResultDto
                    {
                        FacturaVentaId = factura.FacturaVentaId,
                        FacturaVentaFecha = factura.FacturaVentaFecha,
                        FacturaNumero = factura.FacturaVentaNumero,
                        FacturaCantidad = factura.FacturaVentaCantidad,
                        FacturaUnidadMedida = factura.FacturaVentaUnidadMedida,
                        FacturaImporteTotal = factura.FacturaVentaImporte,
                        FacturaDetraccion = factura.FacturaVentaDetraccion,
                        FacturaPendientePago = factura.FacturaVentaPendientePago,
                        FacturaVentaEstado = estado.FacturaVentaDescripcion,
                    }
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<ResultDto<FacturaVentaResultDto>> Update(FacturaVentaUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
