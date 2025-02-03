using AcopioAPIs.DTOs.Tipos;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class TiposRepository : ITipos
    {
        private readonly DbacopioContext _dbacopioContext;

        public TiposRepository(DbacopioContext dbacopioContext)
        {
            _dbacopioContext = dbacopioContext;
        }

        public async Task<List<TipoCompronteResultDto>> GetTipoComprontes()
        {
            var query = from tipo in _dbacopioContext.TipoComprobantes
                        select new TipoCompronteResultDto { 
                            TipoComprobanteId = tipo.TipoComprobanteId,
                            TipoComprobanteNombre = tipo.TipoComprobanteNombre
                        };
            return await query.ToListAsync();
        }
    }
}
