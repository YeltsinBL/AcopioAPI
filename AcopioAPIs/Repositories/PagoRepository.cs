using AcopioAPIs.DTOs.Pago;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class PagoRepository
    {
        private readonly DbacopioContext _dbacopioContext;

        public PagoRepository(DbacopioContext dbacopioContext)
        {
            _dbacopioContext = dbacopioContext;
        }
        public async Task<Pago> CrearPagoAsync(int referenciaId, string tipoReferencia, DateTime fecha, string user, PagoInsertDto item)
        {
            var pago = new Pago
            {
                ReferenciaId = referenciaId,
                TipoReferencia = tipoReferencia,
                PagoFecha = item.PagoFecha,
                PagoEfectivo = item.PagoEfectivo,
                PagoBanco = item.PagoBanco,
                PagoCtaCte = item.PagoCtaCte,
                PagoPagado = item.PagoPagado,
                PagoStatus = true,
                UserCreatedAt = fecha,
                UserCreatedName = user,
            };

            _dbacopioContext.Add(pago);
            await _dbacopioContext.SaveChangesAsync();
            return pago;
        }
        public async Task AnularPago(int referenciaId, string tipoReferencia, DateTime fecha, string user)
        {
            var pagos = await _dbacopioContext.Pagos
                .Where(p => p.ReferenciaId == referenciaId
                         && p.TipoReferencia == tipoReferencia
                         && p.PagoStatus == true)
                .ToListAsync();

            if (pagos.Count == 0) return;

            foreach (var pago in pagos)
            {
                pago.PagoStatus = false;
                pago.UserModifiedAt = fecha;
                pago.UserModifiedName = user;
            }

            await _dbacopioContext.SaveChangesAsync();
            return;
        }
    }
}
