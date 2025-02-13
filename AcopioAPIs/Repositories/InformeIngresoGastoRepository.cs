using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.InformeIngresoGasto;
using AcopioAPIs.DTOs.Liquidacion;
using AcopioAPIs.DTOs.Recojo;
using AcopioAPIs.DTOs.Servicio;
using AcopioAPIs.Models;
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

        public async Task<List<LiquidacionResultDto>> GetLiquidacions(int proveedorId)
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
                            where liquid.ProveedorId == proveedorId && liquid.InformeIngresoGastoId == null
                            && liquid.LiquidacionEstadoId !=3
                            select new LiquidacionResultDto
                            {
                                LiquidacionId = liquid.LiquidacionId,
                                PersonaNombre = (persona.PersonName + ' ' + persona.PersonPaternalSurname + ' ' + persona.PersonMaternalSurname),
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

        public async Task<List<RecojoResultDto>> GetRecojoResults()
        {
            try
            {
                var query = from recojo in _dbacopioContext.Recojos
                            join estado in _dbacopioContext.RecojoEstados on recojo.RecojoEstadoId equals estado.RecojoEstadoId
                            where recojo.InformeIngresoGastoId == null && recojo.RecojoEstadoId !=3
                            select new RecojoResultDto
                            {
                                RecojoId = recojo.RecojoId,
                                RecojoFechaInicio = recojo.RecojoFechaInicio,
                                RecojoFechaFin = recojo.RecojoFechaFin,
                                RecojoCamionesPrecio = recojo.RecojoCamionesPrecio,
                                RecojoDiasPrecio = recojo.RecojoDiasPrecio,
                                RecojoTotalPrecio = recojo.RecojoTotalPrecio,
                                RecojoEstadoDescripcion = estado.RecojoEstadoDescripcion,
                                RecojoCampo = recojo.RecojoCampo
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
                var recojos = (await informe.ReadAsync<InformeRecojoDto>()).AsList();
                var liquidaciones = (await informe.ReadAsync<InformeLiquidacionDto>()).AsList();
                
                master.InformeFacturas = facturas;
                master.InformeCostos = costos;
                master.InformeServiciosTransportes = transportes;
                master.InformeServiciosPaleros = paleros;
                master.InformeRecojos = recojos;
                master.InformeLiquidaciones = liquidaciones;

                return new ResultDto<InformeDto>
                {
                    Result = true,
                    ErrorMessage ="Informe recuperado",
                    Data = master,
                };

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

                var transporteIds = informeInsertDto.InformeServiciosTransportes.Select(s => s.Id).ToList();

                // Consultar todos los transportes existentes
                var transportes = await _dbacopioContext.ServicioTransportes
                    .Where(t => transporteIds.Contains(t.ServicioTransporteId))
                    .ToListAsync();

                // Verificar si faltan transportes en la base de datos
                if (transporteIds.Except(transportes.Select(t => t.ServicioTransporteId)).ToList().Count > 0)
                    throw new Exception("Servicio Transporte no encontrado");
                // Asignar InformeIngresoGastoId a los transportes encontrados
                transportes.ForEach(t => t.InformeIngresoGastoId = informe.InformeId);

                //foreach (var item in informeInsertDto.InformeServiciosTransportes)
                //{
                //    var transporte = await _dbacopioContext.ServicioTransportes.FindAsync(item.Id)
                //        ?? throw new Exception("Servicio Transporte no encontrado");
                //    transporte.InformeIngresoGastoId = informe.InformeId;
                //}
                foreach (var item in informeInsertDto.InformeServiciosPaleros)
                {
                    var transporte = await _dbacopioContext.ServicioPaleros.FindAsync(item.Id)
                        ?? throw new Exception("Servicio Palero no encontrado");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                }
                foreach (var item in informeInsertDto.InformeRecojos)
                {
                    var transporte = await _dbacopioContext.Recojos.FindAsync(item.Id)
                        ?? throw new Exception("Recojo no encontrado");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                }
                foreach (var item in informeInsertDto.InformeLiquidaciones)
                {
                    var transporte = await _dbacopioContext.Liquidacions.FindAsync(item.Id)
                        ?? throw new Exception("Liquidación no encontrada");
                    transporte.InformeIngresoGastoId = informe.InformeId;
                }

                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<InformeResultDto>
                {
                    Result = true,
                    ErrorMessage = "Informe guardado",
                    Data = new InformeResultDto
                    {
                        PersonaNombre=persona.PersonName,
                        TierraCampo=tierra.TierraCampo,
                        InformeCostoTotal= informeInsertDto.InformeCostoTotal,
                        InformeFacturaTotal= informeInsertDto.InformeFacturaTotal,
                        InformeFecha = informeInsertDto.InformeFecha,
                        InformeId=informe.InformeId,
                        InformeStatus = true,
                        InformeTotal = informeInsertDto.InformeTotal
                    }
                };

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

        public Task<ResultDto<bool>> Delete(InformeDeleteDto informeDeleteDto)
        {
            throw new NotImplementedException();
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }

    }
}
