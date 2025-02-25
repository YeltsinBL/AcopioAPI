using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;
using AcopioAPIs.DTOs.InformeIngresoGasto;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Models;
using AcopioAPIs.Utils;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class InformeIngresoGastoRepository : IInformeIngresoGasto
    {
        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;

        public InformeIngresoGastoRepository(DbacopioContext dbacopioContext, IConfiguration configuration)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
        }

        public async Task<List<InformeResultDto>> GetAll(DateOnly? fechaDesde, DateOnly? fechaHasta, int? proveedorId, bool? estadoId)
        {
            try
            {
                var query = from informe in _dbacopioContext.InformeIngresoGastos
                            join persona in _dbacopioContext.Persons
                                on informe.PersonaId equals persona.PersonId
                            join proveedor in _dbacopioContext.Proveedors
                                on informe.ProveedorId equals proveedor.ProveedorId
                            join tierra in _dbacopioContext.Tierras
                                on informe.TierraId equals tierra.TierraId
                            where (fechaDesde == null || informe.InformeFecha >= fechaDesde)
                            && (fechaHasta == null || informe.InformeFecha <= fechaHasta)
                            && (proveedorId == null || informe.ProveedorId == proveedorId)
                            && (estadoId == null || informe.InformeStatus == estadoId)
                            select new InformeResultDto 
                            { 
                                InformeId = informe.InformeId,
                                InformeFecha = informe.InformeFecha,
                                PersonaNombre = persona.PersonName + ' ' + persona.PersonPaternalSurname + ' ' + persona.PersonMaternalSurname,
                                TierraCampo = tierra.TierraCampo,
                                InformeCostoTotal = informe.InformeCostoTotal,
                                InformeFacturaTotal = informe.InformeFacturaTotal,
                                InformeTotal = informe.InformeTotal,
                                InformeStatus = informe.InformeStatus,
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LiquidacionResultDto>> GetLiquidacions(int personaId)
        {
            try
            {
                var query = from liquid in _dbacopioContext.Liquidacions
                            join estado in _dbacopioContext.LiquidacionEstados
                                on liquid.LiquidacionEstadoId equals estado.LiquidacionEstadoId
                            join tierra in _dbacopioContext.Tierras
                                on liquid.TierraId equals tierra.TierraId
                            join proveedor in _dbacopioContext.Proveedors
                                on liquid.ProveedorId equals proveedor.ProveedorId
                            join persona in _dbacopioContext.Persons
                                on liquid.PersonaId equals persona.PersonId
                            where liquid.PersonaId == personaId && liquid.InformeIngresoGastoId == null
                            && liquid.LiquidacionEstadoId !=3
                            select new LiquidacionResultDto
                            {
                                LiquidacionId = liquid.LiquidacionId,
                                PersonaNombre = persona.PersonName + ' ' + persona.PersonPaternalSurname + ' ' + persona.PersonMaternalSurname,
                                TierraCampo = tierra.TierraCampo,
                                ProveedorUT = proveedor.ProveedorUt,
                                LiquidacionFechaInicio = liquid.LiquidacionFechaInicio,
                                LiquidacionFechaFin = liquid.LiquidacionFechaFin,
                                LiquidacionPesoNeto = liquid.LiquidacionPesoNeto,
                                LiquidacionPesoBruto = liquid.LiquidacionPesoBruto,
                                LiquidacionToneladaTotal = liquid.LiquidacionToneladaTotal,
                                LiquidacionFinanciamientoACuenta = liquid.LiquidacionFinanciamientoAcuenta,
                                LiquidacionPagar = liquid.LiquidacionPagar,
                                LiquidacionAdicionalTotal = liquid.LiquidacionAdicionalTotal,
                                LiquidacionEstadoDescripcion = estado.LiquidacionEstadoDescripcion,
                                LiquidacionToneladaPrecioCompra = liquid.LiquidacionToneladaPrecioCompra
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CorteResultDto>> GetCorteResults(int tierraId)
        {
            try
            {
                var query = from cortes in _dbacopioContext.Cortes
                            join tierra in _dbacopioContext.Tierras
                                on cortes.TierraId equals tierra.TierraId
                            where cortes.InformeIngresoGastoId == null 
                            && tierra.TierraId == tierraId
                            && cortes.CorteEstadoId != 3
                            select new CorteResultDto
                            {
                                CorteId = cortes.CorteId,
                                CorteFecha = cortes.CorteFecha,
                                TierraUC = tierra.TierraUc,
                                CortePrecio = cortes.CortePrecio,
                                CorteCantidadTicket = cortes.CorteDetalles.Count,
                                CorteEstadoDescripcion = "",
                                CortePesoBrutoTotal = cortes.CortePesoBrutoTotal,
                                CorteTotal = cortes.CorteTotal,
                                TierraCampo = tierra.TierraCampo
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ServicioResultDto>> GetServiciosPalero()
        {
            try
            {
                var query = from servicio in _dbacopioContext.ServicioPaleros
                            join estado in _dbacopioContext.ServicioTransporteEstados
                                on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                            join carguillo in _dbacopioContext.Carguillos
                                on servicio.CarguilloId equals carguillo.CarguilloId
                            where servicio.InformeIngresoGastoId == null && servicio.ServicioTransporteEstadoId!=3
                            select new ServicioResultDto
                            {
                                ServicioId = servicio.ServicioPaleroId,
                                ServicioFecha = servicio.ServicioPaleroFecha,
                                ServicioCarguilloTitular = carguillo.CarguilloTitular,
                                ServicioPrecio = servicio.ServicioPaleroPrecio,
                                ServicioPesoBruto = servicio.ServicioPaleroPesoBruto,
                                ServicioTotal = servicio.ServicioPaleroTotal,
                                ServicioEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ServicioResultDto>> GetServiciosTransporte()
        {
            try
            {
                var query = from servicio in _dbacopioContext.ServicioTransportes
                            join estado in _dbacopioContext.ServicioTransporteEstados
                                on servicio.ServicioTransporteEstadoId equals estado.ServicioTransporteEstadoId
                            join carguillo in _dbacopioContext.Carguillos
                                on servicio.CarguilloId equals carguillo.CarguilloId
                            where servicio.InformeIngresoGastoId == null && servicio.ServicioTransporteEstadoId != 3
                            select new ServicioResultDto
                            {
                                ServicioId = servicio.ServicioTransporteId,
                                ServicioFecha = servicio.ServicioTransporteFecha,
                                ServicioCarguilloTitular = carguillo.CarguilloTitular,
                                ServicioPrecio = servicio.ServicioTransportePrecio,
                                ServicioPesoBruto = servicio.ServicioTransportePesoBruto,
                                ServicioTotal = servicio.ServicioTransporteTotal,
                                ServicioEstadoDescripcion = estado.ServicioTransporteEstadoDescripcion
                            };
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<InformeDto>> GetInformeById(int informeId)
        {
            try
            {
                using var conexion = GetConnection();
                using var informe = await conexion.QueryMultipleAsync(
                    "usp_InformeIngresoGastoGetById", 
                    new { InformeId = informeId }, 
                    commandType: CommandType.StoredProcedure);
                var master = (await informe.ReadAsync<InformeDto>()).FirstOrDefault()
                    ?? throw new Exception("Informe Ingresos y Gastos no encontrado");
                var facturas = (await informe.ReadAsync<InformeFacturaDto>()).AsList();
                var costos = (await informe.ReadAsync<InformeCostoDto>()).AsList();

                var transportes = (await informe.ReadAsync<InformeServicioDto>()).AsList();
                var paleros = (await informe.ReadAsync<InformeServicioDto>()).AsList();
                var Cortes = (await informe.ReadAsync<InformeCorteDto>()).AsList();
                var liquidaciones = (await informe.ReadAsync<InformeLiquidacionDto>()).AsList();
                
                master.InformeFacturas = facturas;
                master.InformeCostos = costos;
                master.InformeServiciosTransportes = transportes;
                master.InformeServiciosPaleros = paleros;
                master.InformeCortes = Cortes;
                master.InformeLiquidaciones = liquidaciones;

                return ResponseHelper.ReturnData(master, true, "Informe recuperado");

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ResultDto<InformeResultDto>> Save(InformeInsertDto informeInsertDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (informeInsertDto == null) throw new Exception("No se enviaron datos para guardar el Informe.");
                if (informeInsertDto.InformeFacturas == null) throw new Exception("No se enviaron las Facturas para guardar el informe");
                if (informeInsertDto.InformeCostos == null) throw new Exception("No se enviaron los Costos para guardar el informe");
                var persona = await _dbacopioContext.Persons.FindAsync(informeInsertDto.PersonaId)
                    ?? throw new Exception("Persona no encontrada");
                var tierra = await _dbacopioContext.Tierras.FindAsync(informeInsertDto.TierraId)
                    ?? throw new Exception("Tierra no encontrada");

                var informe = new InformeIngresoGasto
                {
                    PersonaId = informeInsertDto.PersonaId,
                    TierraId = informeInsertDto.TierraId,
                    ProveedorId = informeInsertDto.ProveedorId,
                    InformeFecha = informeInsertDto.InformeFecha,
                    InformeFacturaTotal = informeInsertDto.InformeFacturaTotal,
                    InformeCostoTotal = informeInsertDto.InformeCostoTotal,
                    InformeTotal = informeInsertDto.InformeTotal,
                    InformeStatus = true,
                    UserCreatedName = informeInsertDto.UserCreatedName,
                    UserCreatedAt = informeInsertDto.UserCreatedAt,
                    InformeResultado= informeInsertDto.InformeResultado,
                };
                foreach (var item in informeInsertDto.InformeFacturas)
                {
                    var factura = new InformeIngresoGastoFactura
                    {
                        InformeFacturaFecha = item.InformeFacturaFecha,
                        InformeFacturaNumero = item.InformeFacturaNumero,
                        InformeFacturaImporte = item.InformeFacturaImporte,
                        InformeFacturaStatus = true,
                        UserCreatedName = informeInsertDto.UserCreatedName,
                        UserCreatedAt = informeInsertDto.UserCreatedAt

                    };
                    informe.InformeIngresoGastoFacturas.Add(factura);
                }
                foreach (var item in informeInsertDto.InformeCostos)
                {
                    var costo = new InformeIngresoGastoCosto
                    {
                        InformeCostoPrecio = item.InformeCostoPrecio,
                        InformeCostoTonelada = item.InformeCostoTonelada,
                        InformeCostoTotal = item.InformeCostoTotal,
                        InformeCostoStatus = true,
                        InformeCostoOrden = item.InformeCostoOrden,
                        UserCreatedName = informeInsertDto.UserCreatedName,
                        UserCreatedAt = informeInsertDto.UserCreatedAt
                    };
                    informe.InformeIngresoGastoCostos.Add(costo);
                }
                _dbacopioContext.Add(informe);
                await _dbacopioContext.SaveChangesAsync();

                foreach (var item in informeInsertDto.InformeServiciosTransportes)
                {
                    var transporte = await _dbacopioContext.ServicioTransportes.FindAsync(item.Id)
                        ?? throw new Exception("Servicio Transporte no encontrado");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                    transporte.UserModifiedAt = informeInsertDto.UserCreatedAt;
                    transporte.UserModifiedName = informeInsertDto.UserCreatedName;
                }
                foreach (var item in informeInsertDto.InformeServiciosPaleros)
                {
                    var transporte = await _dbacopioContext.ServicioPaleros.FindAsync(item.Id)
                        ?? throw new Exception("Servicio Palero no encontrado");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                    transporte.UserModifiedAt = informeInsertDto.UserCreatedAt;
                    transporte.UserModifiedName = informeInsertDto.UserCreatedName;
                }
                foreach (var item in informeInsertDto.InformeCortes)
                {
                    var transporte = await _dbacopioContext.Cortes.FindAsync(item.Id)
                        ?? throw new Exception("Corte no encontrado");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                    transporte.UserModifiedAt = informeInsertDto.UserCreatedAt;
                    transporte.UserModifiedName = informeInsertDto.UserCreatedName;
                }
                foreach (var item in informeInsertDto.InformeLiquidaciones)
                {
                    var transporte = await _dbacopioContext.Liquidacions.FindAsync(item.Id)
                        ?? throw new Exception("Liquidación no encontrada");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                    transporte.UserModifiedAt = informeInsertDto.UserCreatedAt;
                    transporte.UserModifiedName = informeInsertDto.UserCreatedName;
                }

                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseHelper.ReturnData(new InformeResultDto
                {
                    PersonaNombre = persona.PersonName,
                    TierraCampo = tierra.TierraCampo,
                    InformeCostoTotal = informeInsertDto.InformeCostoTotal,
                    InformeFacturaTotal = informeInsertDto.InformeFacturaTotal,
                    InformeFecha = informeInsertDto.InformeFecha,
                    InformeId = informe.InformeId,
                    InformeStatus = true,
                    InformeTotal = informeInsertDto.InformeTotal
                }, true, "Informe guardado");

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<ResultDto<InformeResultDto>> Update(InformeUpdateDto informeUpdateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<bool>> Delete(InformeDeleteDto informeDeleteDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (informeDeleteDto == null) throw new Exception("No se enviaron datos para anular el Informe.");
                var informe = await _dbacopioContext.InformeIngresoGastos
                    .Include(c=>c.InformeIngresoGastoFacturas)
                    .Include(c=>c.InformeIngresoGastoCostos)
                    .FirstOrDefaultAsync(c=>c.InformeId==informeDeleteDto.InformeId)
                    ?? throw new Exception("Informe  no encontrado");
                informe.InformeStatus = false;
                informe.UserModifiedAt= informeDeleteDto.UserModifiedAt;
                informe.UserModifiedName = informeDeleteDto.UserModifiedName;

                foreach (var item in informe.InformeIngresoGastoFacturas)
                {
                    item.InformeFacturaStatus = false;
                    item.UserModifiedAt = informeDeleteDto.UserModifiedAt;
                    item.UserModifiedName = informeDeleteDto.UserModifiedName;
                }
                foreach (var item in informe.InformeIngresoGastoCostos)
                {
                    item.InformeCostoStatus = false;
                    item.UserModifiedAt = informeDeleteDto.UserModifiedAt;
                    item.UserModifiedName = informeDeleteDto.UserModifiedName;
                }

                await UpdateRelatedEntities<ServicioTransporte>(informeDeleteDto.InformeId, informeDeleteDto);
                await UpdateRelatedEntities<ServicioPalero>(informeDeleteDto.InformeId, informeDeleteDto);
                await UpdateRelatedEntities<Corte>(informeDeleteDto.InformeId, informeDeleteDto);
                await UpdateRelatedEntities<Liquidacion>(informeDeleteDto.InformeId, informeDeleteDto);


                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseHelper.ReturnData(true, true, "Informe anulado");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task UpdateRelatedEntities<T>(int informeId, InformeDeleteDto informeDeleteDto) where T : class
        {
            await _dbacopioContext.Set<T>()
                .Where(e => EF.Property<int>(e, "InformeIngresoGastoId") == informeId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => EF.Property<int?>(e, "InformeIngresoGastoId"), (int?)null)
                    .SetProperty(e => EF.Property<DateTime>(e, "UserModifiedAt"), informeDeleteDto.UserModifiedAt)
                    .SetProperty(e => EF.Property<string>(e, "UserModifiedName"), informeDeleteDto.UserModifiedName)
                );
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }

    }
}
