﻿using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Venta;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class VentaRepository : IVenta
    {

        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;

        public VentaRepository(DbacopioContext dbacopioContext, IConfiguration configuration, IStorageService storageService)
        {
            _dbacopioContext = dbacopioContext;
            _configuration = configuration;
            _storageService = storageService;
        }


        public async Task<List<TipoResultDto>> GetTipoVenta()
        {
            var query = from tipo in _dbacopioContext.VentaTipos
                        select new TipoResultDto
                        {
                            Nombre = tipo.VentaTipoNombre,
                            Id = tipo.VentaTipoId,
                        };
            return await query.ToListAsync();
        }

        public async Task<List<TipoResultDto>> GetVentaEstado()
        {
            var query = from tipo in _dbacopioContext.VentaEstados
                        select new TipoResultDto
                        {
                            Nombre = tipo.VentaEstadoNombre,
                            Id = tipo.VentaEstadoId,
                        };
            return await query.ToListAsync();
        }
        public async Task<List<TipoResultDto>> GetVentaCliente()
        {
            var query = from tipo in _dbacopioContext.Persons
                        where tipo.PersonStatus == true
                        orderby tipo.PersonName
                        select new TipoResultDto
                        {
                            Nombre = tipo.PersonName + " " + tipo.PersonPaternalSurname+ " " + tipo.PersonMaternalSurname,
                            Id = tipo.PersonId,
                        };
            return await query.ToListAsync();
        }

        public async Task<List<VentaResultDto>> GetVentaResults(DateOnly? fechaDesde, DateOnly? fechaHasta, int? tipoVentaId, int? numeroComprobante, int? estadoId)
        {
            var query = from venta in _dbacopioContext.Venta
                        join tipoComprobante in _dbacopioContext.TipoComprobantes
                            on venta.TipoComprobanteId equals tipoComprobante.TipoComprobanteId
                        join persona in _dbacopioContext.Persons   
                            on venta.PersonaId equals persona.PersonId
                        join estado in _dbacopioContext.VentaEstados
                            on venta.VentaEstadoId equals estado.VentaEstadoId
                        join tipoVentas in _dbacopioContext.VentaTipos
                            on venta.VentaTipoId equals tipoVentas.VentaTipoId
                        where (fechaDesde == null || venta.VentaFecha >= fechaDesde) &&
                            (fechaHasta == null || venta.VentaFecha <= fechaHasta) &&
                            (tipoVentaId == null || tipoVentas.VentaTipoId == tipoVentaId) &&
                            (numeroComprobante == null || venta.VentaNumeroDocumento == numeroComprobante) &&
                            (estadoId == null || venta.VentaEstadoId == estadoId)
                        orderby venta.VentaFecha
                        select new VentaResultDto
                        {
                            VentaId = venta.VentaId,
                            VentaFecha = venta.VentaFecha,
                            TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                            VentaNumeroDocumento = venta.VentaNumeroDocumento.ToString("D6"),
                            PersonaNombre = persona.PersonName + " " + persona.PersonPaternalSurname + " " + persona.PersonMaternalSurname,
                            VentaTotal = venta.VentaTotal,
                            VentaEstadoNombre = estado.VentaEstadoNombre,
                            VentaTipoNombre = tipoVentas.VentaTipoNombre,
                            VentaFechaVence = venta.VentaFechaVence
                        };
            return await query.ToListAsync();
        }

        public async Task<ResultDto<VentaDto>> GetVenta(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_VentaGetById",
                    new { VentaId = id },
                    commandType: CommandType.StoredProcedure);
                var venta = await multi.ReadFirstOrDefaultAsync<VentaDto>()
                    ?? throw new Exception("No se encontró la venta");
                venta.VentaDetalles = (await multi.ReadAsync<VentaDetalleDto>()).ToList();
                venta.DetallePagos = (await multi.ReadAsync<DetallePagoResultDto>()).ToList();
                return new ResultDto<VentaDto>
                {
                    Result = true,
                    ErrorMessage = "Venta encontrada",
                    Data = venta
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<VentaResultDto>> InsertVenta(VentaInsertDto ventaDto, List<IFormFile>? imagenes)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (ventaDto == null) throw new Exception("No se enviaron datos para guardar la venta");
                if (ventaDto.VentaDetalles == null || ventaDto.VentaDetalles.Count == 0)
                    throw new Exception("La venta no tiene detalles");
                var tipoComprobante = await GetDataById(_dbacopioContext.TipoComprobantes,ventaDto.TipoComprobanteId)
                    ?? throw new Exception("No se encontró el Tipo de Comprobante.");
                var ventaTipo = await GetDataById(_dbacopioContext.VentaTipos, ventaDto.VentaTipoId)
                    ?? throw new Exception("No se encontró el Tipo de Venta.");
                var persona = await GetDataById(_dbacopioContext.Persons,ventaDto.PersonaId)
                    ?? throw new Exception("No se encontró a la persona.");
                var ventaEstado = await GetDataById(_dbacopioContext.VentaEstados, ventaDto.VentaEstadoId)
                    ?? throw new Exception("No se encontró el Estado de la Venta.");
                using var conexion = GetConnection();
                int nuevoNumero = await conexion.ExecuteScalarAsync<int>(
                    "SELECT ISNULL(MAX(VentaNumeroDocumento), 0) + 1 FROM Venta"
                );
                var venta = new Ventum
                {
                    VentaFecha = ventaDto.VentaFecha,
                    TipoComprobanteId = ventaDto.TipoComprobanteId,
                    VentaNumeroDocumento = nuevoNumero,
                    VentaTipoId = ventaDto.VentaTipoId,
                    PersonaId = ventaDto.PersonaId,
                    VentaDia = ventaDto.VentaDia,
                    VentaFechaVence = ventaDto.VentaDia > 0 ? ventaDto.VentaFecha.AddDays(ventaDto.VentaDia): null,
                    VentaTotal = ventaDto.VentaTotal,
                    VentaPagado = ventaDto.VentaPagado,
                    VentaPendientePagar = ventaDto.VentaPendientePagar,
                    VentaEstadoId = ventaDto.VentaEstadoId,
                    UserCreatedAt = ventaDto.UserCreatedAt,
                    UserCreatedName = ventaDto.UserCreatedName
                };
                foreach (var detalle in ventaDto.VentaDetalles)
                {
                    var producto = await GetDataById(_dbacopioContext.Productos, detalle.ProductoId)
                        ?? throw new Exception("No se encontró el producto.");
                    venta.VentaDetalles.Add(new VentaDetalle
                    {
                        ProductoId = detalle.ProductoId,
                        VentaDetalleCantidad = detalle.Cantidad,
                        VentaDetallePrecio = detalle.Precio,
                        VentaDetalleStatus = true,
                        UserCreatedName = ventaDto.UserCreatedName,
                        UserCreatedAt = ventaDto.UserCreatedAt
                    });

                    producto.ProductoCantidad -= detalle.Cantidad;
                    producto.UserModifiedAt = ventaDto.UserCreatedAt;
                    producto.UserModifiedName = ventaDto.UserCreatedName;
                }
                int cantidad = 0;
                if(ventaDto.DetallePagos != null)
                {
                    foreach (var item in ventaDto.DetallePagos)
                    {
                        var imagen = imagenes.IsNullOrEmpty() ? null : cantidad >= imagenes!.Count ? null : imagenes![cantidad];
                        var imagenURL = "";
                        if (!item.DetallePagoImagen.IsNullOrEmpty() && imagen != null)
                        {
                            imagenURL = await _storageService.UploadImageAsync("DetallePago", imagen);
                            if (imagenURL.IsNullOrEmpty()) throw new Exception("Error al subir imagen a Cloudinary");
                            cantidad++;
                        }
                        var detalle = new VentaDetallePago
                        {
                            VentaDetallePagoFecha = item.DetallePagoFecha,
                            VentaDetallePagoEfectivo = item.DetallePagoEfectivo,
                            VentaDetallePagoBanco = item.DetallePagoBanco,
                            VentaDetallePagoCtaCte = item.DetallePagoCtaCte,
                            VentaDetallePagoPagado = item.DetallePagoPagado,
                            ImagenUrl = imagenURL,
                            ImagenComentario = item.DetallePagoComentario,
                            VentaDetallePagoStatus = true,
                            UserCreatedAt = ventaDto.UserCreatedAt,
                            UserCreatedName = ventaDto.UserCreatedName
                        };
                        venta.VentaDetallePagos.Add(detalle);
                    }
                }
                _dbacopioContext.Add(venta);
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<VentaResultDto>
                {
                    Result = true,
                    ErrorMessage = "Venta guardada",
                    Data = new VentaResultDto
                    {
                        VentaId = venta.VentaId,
                        VentaFecha = venta.VentaFecha,
                        TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                        VentaNumeroDocumento = venta.VentaNumeroDocumento.ToString("D6"),
                        PersonaNombre = persona.PersonName + " " + persona.PersonPaternalSurname + " " + persona.PersonMaternalSurname,
                        VentaTotal = venta.VentaTotal,
                        VentaEstadoNombre=ventaEstado.VentaEstadoNombre,
                        VentaTipoNombre =ventaTipo.VentaTipoNombre,
                        VentaFechaVence=venta.VentaFechaVence
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<ResultDto<VentaResultDto>> UpdateVenta(VentaUpdateDto ventaDto, List<IFormFile>? imagenes)
        {
            try
            {
                if (ventaDto == null) throw new Exception("No se enviaron datos para guardar la venta");
                var ventaEstado = await _dbacopioContext.VentaEstados
                    .FirstOrDefaultAsync(c => c.VentaEstadoNombre.Equals("anulado"))
                    ?? throw new Exception("No se encontró el Estado Anulado de la Venta.");
                if (ventaEstado.VentaEstadoId == ventaDto.VentaEstadoId)
                    throw new Exception("La venta se encuentra anulada");
                var ventaPagado = await _dbacopioContext.VentaEstados
                    .FirstOrDefaultAsync(c => c.VentaEstadoNombre.Equals("pagado"))
                    ?? throw new Exception("No se encontró el Estado Pagado de la Venta.");
                var tipoComprobante = await GetDataById(_dbacopioContext.TipoComprobantes, ventaDto.TipoComprobanteId)
                    ?? throw new Exception("No se encontró el Tipo de Comprobante.");
                var persona = await GetDataById(_dbacopioContext.Persons, ventaDto.PersonaId)
                    ?? throw new Exception("No se encontró a la persona.");
                var ventaTipo = await GetDataById(_dbacopioContext.VentaTipos, ventaDto.VentaTipoId)
                    ?? throw new Exception("No se encontró el Tipo de Venta.");
                var venta = await _dbacopioContext.Venta
                    .Include(c => c.VentaDetalles)
                    .FirstOrDefaultAsync(c => c.VentaId == ventaDto.VentaId)
                    ?? throw new Exception("No se encontró la venta");

                venta.VentaFecha = ventaDto.VentaFecha;
                venta.TipoComprobanteId = ventaDto.TipoComprobanteId;
                venta.PersonaId = ventaDto.PersonaId;
                venta.VentaTipoId = ventaDto.VentaTipoId;
                venta.VentaDia = ventaDto.VentaDia;
                venta.VentaFechaVence = ventaDto.VentaDia > 0 ? ventaDto.VentaFecha.AddDays(ventaDto.VentaDia) : null;
                venta.VentaEstadoId = (venta.VentaTotal == ventaDto.VentaPagado 
                    && ventaDto.VentaPendientePagar == 0) ? 
                    ventaPagado.VentaEstadoId : ventaDto.VentaEstadoId;
                venta.VentaPagado = ventaDto.VentaPagado;
                venta.VentaPendientePagar = ventaDto.VentaPendientePagar;
                venta.UserModifiedAt = ventaDto.UserModifiedAt;
                venta.UserModifiedName = ventaDto.UserModifiedName;

                int cantidad = 0;
                foreach (var item in ventaDto.DetallePagos)
                {
                    var imagen = imagenes.IsNullOrEmpty() ? null : cantidad >= imagenes!.Count ? null : imagenes![cantidad];
                    var imagenURL = "";
                    if (!item.DetallePagoImagen.IsNullOrEmpty() && imagen!=null)
                    {
                        imagenURL = await _storageService.UploadImageAsync("DetallePago", imagen);
                        if (imagenURL.IsNullOrEmpty()) throw new Exception("Error al subir imagen a Cloudinary");
                        cantidad++;
                    }
                    var detalle = new VentaDetallePago
                    {
                        VentaDetallePagoFecha = item.DetallePagoFecha,
                        VentaDetallePagoEfectivo = item.DetallePagoEfectivo,
                        VentaDetallePagoBanco = item.DetallePagoBanco,
                        VentaDetallePagoCtaCte = item.DetallePagoCtaCte,
                        VentaDetallePagoPagado = item.DetallePagoPagado,
                        ImagenUrl = imagenURL,
                        ImagenComentario = item.DetallePagoComentario,
                        VentaDetallePagoStatus = true,
                        UserCreatedAt = ventaDto.UserModifiedAt,
                        UserCreatedName = ventaDto.UserModifiedName
                    };
                    venta.VentaDetallePagos.Add(detalle);
                }
                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<VentaResultDto>
                {
                    Result = true,
                    ErrorMessage = "Venta modificada",
                    Data = new VentaResultDto
                    {
                        VentaId = venta.VentaId,
                        VentaFecha = venta.VentaFecha,
                        TipoComprobanteDescripcion = tipoComprobante.TipoComprobanteNombre,
                        VentaNumeroDocumento = venta.VentaNumeroDocumento.ToString("D6"),
                        PersonaNombre = persona.PersonName + " " + persona.PersonPaternalSurname + " " + persona.PersonMaternalSurname,
                        VentaTotal = venta.VentaTotal,
                        VentaEstadoNombre = ventaEstado.VentaEstadoNombre,
                        VentaTipoNombre = ventaTipo.VentaTipoNombre,
                        VentaFechaVence = venta.VentaFechaVence
                    }
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<bool>> DeleteVenta(DeleteDto ventaDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (ventaDto == null) throw new Exception("No se enviaron datos para anular la venta");
                var estado = await _dbacopioContext.VentaEstados.FirstOrDefaultAsync(c => c.VentaEstadoNombre.Equals("anulado"))
                    ?? throw new Exception("No se encontró el Estado Anulado de la Venta.");
                var venta = await _dbacopioContext.Venta
                    .Include(c => c.VentaDetalles)
                    .Include(c => c.VentaDetallePagos)
                    .FirstOrDefaultAsync(c => c.VentaId == ventaDto.Id)
                    ?? throw new Exception("No se encontró la venta");
                venta.VentaEstadoId = estado.VentaEstadoId;
                venta.UserModifiedAt = ventaDto.UserModifiedAt;
                venta.UserModifiedName = ventaDto.UserModifiedName;
                foreach (var detalle in venta.VentaDetalles)
                {
                    var producto = await GetDataById(_dbacopioContext.Productos, detalle.ProductoId)
                        ?? throw new Exception("No se encontró el producto.");
                    producto.ProductoCantidad += detalle.VentaDetalleCantidad;
                    producto.UserModifiedAt = ventaDto.UserModifiedAt;
                    producto.UserModifiedName = ventaDto.UserModifiedName;

                    detalle.VentaDetalleStatus = false;
                    detalle.UserModifiedAt = ventaDto.UserModifiedAt;
                    detalle.UserModifiedName = ventaDto.UserModifiedName;
                }
                foreach (var item in venta.VentaDetallePagos)
                {
                    item.VentaDetallePagoStatus = false;
                    item.UserModifiedAt = ventaDto.UserModifiedAt;
                    item.UserModifiedName = ventaDto.UserModifiedName;
                }
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ResultDto<bool>
                {
                    Result = true,
                    ErrorMessage = "Venta anulada",
                    Data = true
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static async Task<T?> GetDataById<T>(DbSet<T> tipo, int Id) where T : class
        {
            return await tipo.FindAsync(Id);
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }
    }
}
