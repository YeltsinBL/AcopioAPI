using AcopioAPIs.DTOs.Pago;
using AcopioAPIs.Models;

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
                UserCreatedAt = fecha,
                UserCreatedName = user,
            };

            _dbacopioContext.Add(pago);
            await _dbacopioContext.SaveChangesAsync();
            return pago;
        }
    }
}
