using AcopioAPIs.DTOs.Tesoreria;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class TesoreriaRepository : ITesoreria
    {

        private readonly DbacopioContext _dacopioContext;

        public TesoreriaRepository(DbacopioContext dacopioContext)
        {
            _dacopioContext = dacopioContext;
        }

        public async Task<List<TesoreriaResultDto>> GetAll(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId)
        {
            return await GetTesoreria(fechaDesde, fechaHasta, proveedorId, null)
                .ToListAsync(); 
        }


        public async Task<TesoreriaDto> GetById(int id)
        {
            try
            {
                var query = from tesoreria in _dacopioContext.Tesoreria
                            join liquidacion in _dacopioContext.Liquidacions
                                on tesoreria.LiquidacionId equals liquidacion.LiquidacionId
                            join proveedor in _dacopioContext.Proveedors
                                on liquidacion.ProveedorId equals proveedor.ProveedorId
                            join persona in _dacopioContext.Persons
                                on liquidacion.PersonaId equals persona.PersonId
                            join tierra in _dacopioContext.Tierras
                                on liquidacion.TierraId equals tierra.TierraId
                            where tesoreria.TesoreriaId == id
                            select new TesoreriaDto
                            {
                                TesoreriaId = tesoreria.TesoreriaId,
                                LiquidacionId = liquidacion.LiquidacionId,
                                TesoreriaBanco = tesoreria.TesoreriaBanco,
                                TesoreriaCtaCte = tesoreria.TesoreriaCtaCte,
                                TesoreriaFecha = tesoreria.TesoreriaFecha,
                                TesoreriaMonto = tesoreria.TesoreriaMonto,
                                ProveedorUT = proveedor.ProveedorUt,
                                PersonaNombre = persona.PersonName,
                                TierraCampo = tierra.TierraCampo
                            };
                return await query
                    .FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Tesoreria no encontrada");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TesoreriaResultDto> Save(TesoreriaInsertDto tesoreriaInsertDto)
        {
             using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (tesoreriaInsertDto == null) 
                    throw new Exception("No se enviaron datos para guardar la tesoreria");
                var liquidacion = await _dacopioContext.Liquidacions.FindAsync(tesoreriaInsertDto.LiquidacionId)
                    ?? throw new KeyNotFoundException("Liquidación no encontrada");
                var estados = from est in _dacopioContext.LiquidacionEstados
                              where est.LiquidacionEstadoDescripcion.Equals("Pagado")
                              select est;
                var estado = await estados.FirstOrDefaultAsync()
                    ?? throw new Exception("Estado de Liquidación no encontrado");
                liquidacion.LiquidacionEstadoId =  estado.LiquidacionEstadoId;
                liquidacion.UserModifiedAt = tesoreriaInsertDto.UserCreatedAt;
                liquidacion.UserModifiedName = tesoreriaInsertDto.UserCreatedName;

                var tesoreria = new Tesorerium
                {
                    LiquidacionId = tesoreriaInsertDto.LiquidacionId,
                    TesoreriaBanco = tesoreriaInsertDto.TesoreriaBanco,
                    TesoreriaCtaCte = tesoreriaInsertDto.TesoreriaCtaCte,
                    TesoreriaFecha = tesoreriaInsertDto.TesoreriaFecha,
                    TesoreriaMonto = tesoreriaInsertDto.TesoreriaMonto,
                    UserCreatedAt = tesoreriaInsertDto.UserCreatedAt,
                    UserCreatedName = tesoreriaInsertDto.UserCreatedName
                };
                _dacopioContext.Add(tesoreria);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return await GetTesoreria(null, null, null,tesoreria.TesoreriaId).FirstOrDefaultAsync()??
                    throw new Exception();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private IQueryable<TesoreriaResultDto> GetTesoreria(DateTime? fechaDesde, DateTime? fechaHasta, int? proveedorId, int? tesoreiaId)
        {
            return from tesoreria in _dacopioContext.Tesoreria
                   join liquidacion in _dacopioContext.Liquidacions
                       on tesoreria.LiquidacionId equals liquidacion.LiquidacionId
                   join proveedor in _dacopioContext.Proveedors
                       on liquidacion.ProveedorId equals proveedor.ProveedorId
                   join persona in _dacopioContext.Persons
                       on liquidacion.PersonaId equals persona.PersonId
                    join tierra in _dacopioContext.Tierras
                        on liquidacion.TierraId equals tierra.TierraId
                   where (fechaDesde == null || tesoreria.TesoreriaFecha >= fechaDesde)
                   && (fechaHasta == null || tesoreria.TesoreriaFecha <= fechaHasta)
                   && (proveedorId == null || liquidacion.ProveedorId == proveedorId)
                   && (tesoreiaId == null || tesoreria.TesoreriaId == tesoreiaId)
                   select new TesoreriaResultDto
                   {
                       TesoreriaId = tesoreria.TesoreriaId,
                       TesoreriaFecha = DateOnly.FromDateTime(tesoreria.TesoreriaFecha),
                       TesoreriaMonto = tesoreria.TesoreriaMonto,
                       ProveedorUT = proveedor.ProveedorUt,
                       PersonaNombre = persona.PersonName,
                       TierraCampo = tierra.TierraCampo
                   };
        }
    }
}
