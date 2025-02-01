using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Distribuidor;
using AcopioAPIs.DTOs.Producto;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class DistribuidorRepository : IDistribuidor
    {
        private readonly DbacopioContext _dbacopioContext;

        public DistribuidorRepository(DbacopioContext dbacopioContext)
        {
            _dbacopioContext = dbacopioContext;
        }

        public async Task<List<DistribuidorDto>> GetAll(string? ruc, string? nombre, bool? estado)
        {
            var query = from distribuidor in _dbacopioContext.Distribuidors
                        where (ruc == null || distribuidor.DistribuidorRuc.Contains(ruc))
                        && (nombre == null || distribuidor.DistribuidorNombre.Contains(nombre))
                        && (estado == null || distribuidor.DistribuidorStatus == estado)
                        select new DistribuidorDto
                        {
                            DistribuidorId = distribuidor.DistribuidorId,
                            DistribuidorRuc = distribuidor.DistribuidorRuc,
                            DistribuidorNombre = distribuidor.DistribuidorNombre,
                            DistribuidorStatus = distribuidor.DistribuidorStatus
                        };
            return await query.ToListAsync();
        }

        public async Task<ResultDto<DistribuidorDto>> GetById(int id)
        {
            try
            {
                var distribuidor = await _dbacopioContext.Distribuidors.FindAsync(id)
                    ?? throw new KeyNotFoundException("Distribuidor no encontrado");
                return new ResultDto<DistribuidorDto>
                {
                    Result = true,
                    ErrorMessage = "Distribuidor recuperado",
                    Data = new DistribuidorDto
                    {
                        DistribuidorId = distribuidor.DistribuidorId,
                        DistribuidorRuc = distribuidor.DistribuidorRuc,
                        DistribuidorNombre = distribuidor.DistribuidorNombre,
                        DistribuidorStatus = distribuidor.DistribuidorStatus
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<DistribuidorDto>> Insert(DistribuidorInsertDto distribuidorDto)
        {
            try
            {
                if (distribuidorDto == null)
                    throw new Exception("No se enviaron datos para guardar el distribuidor");
                var exist = await _dbacopioContext.Distribuidors.AnyAsync(
                     p => p.DistribuidorNombre.Equals(distribuidorDto.DistribuidorNombre)
                     || p.DistribuidorRuc.Equals(distribuidorDto.DistribuidorRuc));
                if (exist) throw new Exception("El nombre y/o ruc del distribuidor ya existe");
                var distribuidor = new Distribuidor
                {
                    DistribuidorNombre = distribuidorDto.DistribuidorNombre,
                    DistribuidorRuc = distribuidorDto.DistribuidorRuc,
                    DistribuidorStatus = true,
                    UserCreatedAt = distribuidorDto.UserCreatedAt,
                    UserCreatedName = distribuidorDto.UserCreatedName
                };
                _dbacopioContext.Distribuidors.Add(distribuidor);
                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<DistribuidorDto>
                {
                    Result = true,
                    ErrorMessage = "Distribuidor guardado",
                    Data = new DistribuidorDto
                    {
                        DistribuidorId = distribuidor.DistribuidorId,
                        DistribuidorRuc  = distribuidor.DistribuidorRuc,
                        DistribuidorNombre = distribuidor.DistribuidorNombre,
                        DistribuidorStatus = distribuidor.DistribuidorStatus
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<DistribuidorDto>> Update(DistribuidorUpdateDto distribuidorDto)
        {
            try
            {
                var distribuidor = await _dbacopioContext.Distribuidors.FindAsync(distribuidorDto.DistribuidorId)
                    ?? throw new KeyNotFoundException("Distribuidor no encontrado");
                var exist = await _dbacopioContext.Distribuidors.AnyAsync(
                    p => (p.DistribuidorNombre.Equals(distribuidorDto.DistribuidorNombre)
                    || p.DistribuidorRuc.Equals(distribuidorDto.DistribuidorRuc))
                    && p.DistribuidorId != distribuidorDto.DistribuidorId);
                if (exist) throw new Exception("El nombre y/o ruc del distribuidor ya existe");
                distribuidor.DistribuidorRuc = distribuidorDto.DistribuidorRuc;
                distribuidor.DistribuidorNombre = distribuidorDto.DistribuidorNombre;
                distribuidor.DistribuidorStatus = true;
                distribuidor.UserModifiedAt = distribuidorDto.UserModifiedAt;
                distribuidor.UserModifiedName = distribuidorDto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();

                return new ResultDto<DistribuidorDto>
                {
                    Result = true,
                    ErrorMessage = "Distribuidor actualizado",
                    Data = new DistribuidorDto
                    {
                        DistribuidorId = distribuidorDto.DistribuidorId,
                        DistribuidorRuc = distribuidorDto.DistribuidorRuc,
                        DistribuidorNombre = distribuidorDto.DistribuidorNombre,
                        DistribuidorStatus = true
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResultDto<DistribuidorDto>> Delete(DistribuidorDeleteDto distribuidorDto)
        {
            try
            {
                var distribuidor = await _dbacopioContext.Distribuidors.FindAsync(distribuidorDto.DistribuidorId)
                    ?? throw new KeyNotFoundException("Distribuidor no encontrado");
                if (!distribuidor.DistribuidorStatus) throw new Exception("El distribuidor ya está desactivado");
                distribuidor.DistribuidorStatus = false;
                distribuidor.UserModifiedAt = distribuidorDto.UserModifiedAt;
                distribuidor.UserModifiedName = distribuidorDto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();
                return new ResultDto<DistribuidorDto>
                {
                    Result = true,
                    ErrorMessage = "Distribuidor desactivado",
                    Data = new DistribuidorDto
                    {
                        DistribuidorId = distribuidorDto.DistribuidorId,
                        DistribuidorRuc = distribuidor.DistribuidorRuc,
                        DistribuidorNombre = distribuidor.DistribuidorNombre,
                        DistribuidorStatus = distribuidor.DistribuidorStatus
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
