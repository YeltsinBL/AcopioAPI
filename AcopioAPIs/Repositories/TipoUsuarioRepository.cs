using AcopioAPIs.DTOs.TipoUsuario;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AcopioAPIs.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuario
    {
        private readonly DbacopioContext _context;

        public TipoUsuarioRepository(DbacopioContext context)
        {
            _context = context;
        }

        public async Task<List<TipoUsuarioDto>> ListTipos(string? nombre, bool? estado)
        {
            try
            {
                return await GetTipoUsuarioDtos(nombre, estado, null)
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TipoUsuarioDto> GetTipo(int id)
        {
            try
            {
                return await GetTipoUsuarioDtos(null, null, id)
                    .FirstOrDefaultAsync() ??
                    throw new KeyNotFoundException("Tipo de usuario no encontrado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TipoUsuarioDto> InsertTipo(TipoUsuarioInsertDto tipoUsuario)
        {
            try
            {
                if (await GetTipoUsuario(tipoUsuario.TipoUsuarioNombre) != null) 
                    throw new ArgumentException("El tipo de usuario ya existe");
                var tipo = new TypePerson
                {
                    TypePesonName = tipoUsuario.TipoUsuarioNombre,
                    TypePesonStatus = true,
                    UserCreatedAt = tipoUsuario.UserCreatedAt,
                    UserCreatedName = tipoUsuario.UserCreatedName
                };
                _context.TypePeople.Add(tipo);
                await _context.SaveChangesAsync();
                return new TipoUsuarioDto
                {
                    TipoUsuarioId = tipo.TypePesonId,
                    TipoUsuarioNombre = tipo.TypePesonName,
                    TipoUsuarioEstado = tipo.TypePesonStatus
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TipoUsuarioDto> UpdateTipo(TipoUsuarioUpdateDto tipoUsuario)
        {
            try
            {
                var existe = await GetTipoUsuario(tipoUsuario.TipoUsuarioNombre);
                if (existe != null && existe.TypePesonId != tipoUsuario.TipoUsuarioId)
                    throw new ArgumentException("El tipo de usuario ya existe");
                var tipo = await GetTypePersonById(tipoUsuario.TipoUsuarioId)
                    ?? throw new KeyNotFoundException("Tipo de usuario no encontrado");
                tipo.TypePesonName = tipoUsuario.TipoUsuarioNombre;
                tipo.TypePesonStatus = true;
                tipo.UserModifiedAt = tipoUsuario.UserModifiedAt;
                tipo.UserModifiedName = tipoUsuario.UserModifiedName;
                await _context.SaveChangesAsync();
                return new TipoUsuarioDto
                {
                    TipoUsuarioId = tipo.TypePesonId,
                    TipoUsuarioNombre = tipo.TypePesonName,
                    TipoUsuarioEstado = tipo.TypePesonStatus
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteTipo(TipoUsuarioDeleteDto tipoUsuario)
        {
            try
            {
                var tipo = await GetTypePersonById(tipoUsuario.TipoUsuarioId)
                    ?? throw new KeyNotFoundException("Tipo de usuario no encontrado");
                tipo.TypePesonStatus = false;
                tipo.UserModifiedAt = tipoUsuario.UserModifiedAt;
                tipo.UserModifiedName = tipoUsuario.UserModifiedName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IQueryable<TipoUsuarioDto> GetTipoUsuarioDtos(string? nombre, bool? estado, int? id)
        {
            try
            {
                return from tipo in _context.TypePeople
                       where (nombre.IsNullOrEmpty() || tipo.TypePesonName.Contains(nombre!))
                                && (estado == null || tipo.TypePesonStatus == estado)
                                && (id == null || tipo.TypePesonId == id)
                       select new TipoUsuarioDto
                       {
                           TipoUsuarioId = tipo.TypePesonId,
                           TipoUsuarioNombre = tipo.TypePesonName,
                           TipoUsuarioEstado = tipo.TypePesonStatus
                       };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<TypePerson?> GetTypePersonById(int id)
        {
            try
            {
                return await _context.TypePeople.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<TypePerson?> GetTipoUsuario(string nombre)
        {
            try
            {
                return await _context.TypePeople
                    .FirstOrDefaultAsync(t => t.TypePesonName == nombre );
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
