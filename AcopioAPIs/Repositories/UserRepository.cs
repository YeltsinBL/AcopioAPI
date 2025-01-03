using AcopioAPIs.DTOs.User;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

namespace AcopioAPIs.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DbacopioContext _dacopioContext;
        private readonly IConfiguration _configuration;

        public UserRepository(DbacopioContext dacopioContext, IConfiguration configuration)
        {
            _dacopioContext = dacopioContext;
            _configuration = configuration;
        }


        public async Task<List<UserResultDto>> GetAll(int? typeUserId, string? name, string? userName, bool? estado)
        {
            try
            {
                var query = GetUserBy(typeUserId, name, userName, estado, null);
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDto> GetById(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_UserGetById", new { UserId = id }, commandType: CommandType.StoredProcedure);
                var master = await multi.ReadFirstOrDefaultAsync<UserDto>()
                    ?? throw new KeyNotFoundException("No se encontró el Usuario");
                master.UserModules = (await multi.ReadAsync<UserResultModuleDto>()).ToList();
                return master;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserResultDto> Insert(UserInsertDto insertDto)
        {
            using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (insertDto == null) throw new Exception("No se envió información para guardar al usuario");
                bool isDniValid = !insertDto.PersonDNI.IsNullOrEmpty() &&
                    await _dacopioContext.Persons.AnyAsync(p => p.PersonDni == insertDto.PersonDNI);

                if (isDniValid) throw new Exception($"Ya existe el DNI {insertDto.PersonDNI}.");
                if(await _dacopioContext.Users.AnyAsync(p => p.UserName == insertDto.UserName))
                    throw new Exception($"Ya existe el usuario {insertDto.UserName}.");
                var person = new Person
                {
                    PersonDni = insertDto.PersonDNI,
                    PersonName = insertDto.PersonName,
                    PersonPaternalSurname = insertDto.PersonPaternalSurname,
                    PersonMaternalSurname = insertDto.PersonMaternalSurname,
                    PersonStatus = true,
                    PersonType = insertDto.TypePersonId,
                    UserCreatedAt = insertDto.UserCreatedAt,
                    UserCreatedName = insertDto.UserCreatedName
                };

                var user = new User
                {
                    UserName = insertDto.UserName,
                    UserPassword = Encoding.UTF8.GetBytes(insertDto.UserPassword), // Convert string to byte[]
                    UserStatus = true,
                    UserResetPassword = true,
                    UserCreatedAt = insertDto.UserCreatedAt,
                    UserCreatedName = insertDto.UserCreatedName,
                };
                if(insertDto.UserModules != null)
                {
                    foreach (var modulo in insertDto.UserModules)
                    {
                        var permiso = new UserPermission
                        {
                            ModuleId = modulo.ModuleId,
                            UserPermissionStatus = true,
                            UserCreatedAt = insertDto.UserCreatedAt,
                            UserCreatedName = insertDto.UserCreatedName,
                        };
                        user.UserPermissions.Add(permiso);
                    }
                }
                person.Users.Add(user);
                _dacopioContext.Persons.Add(person);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var query = GetUserBy(null, null, null, null, user.UserId);
                return await query.FirstOrDefaultAsync()
                    ?? throw new Exception("");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<UserResultDto> Update(UserUpdateDto updateDto)
        {
            using var transaction = await _dacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (updateDto == null) throw new Exception("No se enviaron datos para guarda el usuario");
                if (!updateDto.PersonDNI.IsNullOrEmpty() &&
                    await _dacopioContext.Persons.AnyAsync(p => p.PersonDni == updateDto.PersonDNI
                    && p.PersonId != updateDto.PersonId))
                    throw new Exception($"Ya existe el DNI {updateDto.PersonDNI}.");
                var person = await _dacopioContext.Persons.FindAsync(updateDto.PersonId)
                    ?? throw new KeyNotFoundException("Usuario no encontrada.");

                person.PersonType = updateDto.TypePersonId;
                person.PersonDni = updateDto.PersonDNI;
                person.PersonName = updateDto.PersonName;
                person.PersonPaternalSurname = updateDto.PersonPaternalSurname;
                person.PersonMaternalSurname = updateDto.PersonMaternalSurname;
                person.UserModifiedAt = updateDto.UserModifiedAt;
                person.UserModifiedName = updateDto.UserModifiedName;
                if (updateDto.UserModules != null)
                {
                    foreach (var modulo in updateDto.UserModules)
                    {
                        var permiso = await _dacopioContext.UserPermissions
                            .FirstOrDefaultAsync(p => 
                            p.UserId == updateDto.UserId && p.ModuleId == modulo.ModuleId);
                        if (permiso != null)
                        {
                            permiso.UserPermissionStatus = modulo.ModuleStatus;
                            permiso.UserModifiedAt = updateDto.UserModifiedAt;
                            permiso.UserModifiedName = updateDto.UserModifiedName;
                            _dacopioContext.UserPermissions.Update(permiso);
                        }
                        else
                        {
                            var newPermission = new UserPermission
                            {
                                UserId = updateDto.UserId,
                                ModuleId = modulo.ModuleId,
                                UserPermissionStatus = true,
                                UserCreatedAt = updateDto.UserModifiedAt,
                                UserCreatedName = updateDto.UserModifiedName,
                            };
                            _dacopioContext.UserPermissions.Add(newPermission);
                        }
                    }
                }
                _dacopioContext.Persons.Update(person);
                await _dacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var query = GetUserBy(null, null, null, null, updateDto.UserId);
                return await query.FirstOrDefaultAsync()
                    ?? throw new Exception("Usuario no encontrado");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> Delete(UserDeleteDto deleteDto)
        {
            try
            {
                if (deleteDto == null) throw new Exception("No se enviaron datos del usuario.");
                var user = await _dacopioContext.Users.FindAsync(deleteDto.UserId)
                    ?? throw new KeyNotFoundException("Usuario no encontrada.");

                user.UserStatus = false;
                user.UserModifiedAt = deleteDto.UserModifiedAt;
                user.UserModifiedName = deleteDto.UserModifiedName;

                var listPermisos = _dacopioContext.UserPermissions
                    .Where(p => p.UserId == deleteDto.UserId);
                foreach (var permiso in listPermisos)
                {
                    permiso.UserPermissionStatus = false;
                    permiso.UserModifiedAt = deleteDto.UserModifiedAt;
                    permiso.UserModifiedName = deleteDto.UserModifiedName;
                    _dacopioContext.UserPermissions.Update(permiso);
                }

                _dacopioContext.Users.Update(user);
                await _dacopioContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<UserModulesResultDto>> GetAssignedModules(string userName)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty("El usuario es obligatorio");
                var user = await _dacopioContext.Users.Where(u
                    => u.UserName.Equals(userName)).FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Usuario no encontrado");

                var modules = await _dacopioContext.UserPermissions
                    .Where(p => p.UserId == user.UserId && p.UserPermissionStatus)
                    .Select(p => p.Module)
                    .ToListAsync();

                var modulos = modules
                    .Where(m => m.ModulePrimaryId == null)
                    .Select(m => new UserModulesResultDto
                    {
                        ModuleId = m.ModuleId,
                        ModuleName = m.ModuleName,
                        ModuleIcon = m.ModuleIcon,
                        ModuleColor = m.ModuleColor,
                        ModuleRoute = m.ModuleRoute,
                        SubModules = modules
                            .Where(s => s.ModulePrimaryId == m.ModuleId)
                            .Select(s => new UserSubModulesResultDto
                            {
                                ModuleId = s.ModuleId,
                                ModuleName = s.ModuleName,
                                ModuleIcon = s.ModuleIcon,
                                ModuleColor = s.ModuleColor,
                                ModuleRoute = s.ModuleRoute
                            })
                            .ToList()
                    })
                    .ToList();

                return modulos;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<UserResultModuleDto>> GetAllModules()
        {
            try
            {
                return await _dacopioContext.Modules
                    .Where(m => m.ModuleStatus)
                    .OrderBy(m => m.ModulePrimaryId == null ? m.ModuleId:m.ModulePrimaryId)
                    .ThenBy(m => m.ModulePrimaryId)
                    .Select(m => new UserResultModuleDto
                    {
                        ModuleId = m.ModuleId,
                        ModuleName = m.ModuleName,
                        ModuleAgregado = false
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IQueryable<UserResultDto> GetUserBy(int? typeUserId, string? name, string? userName, bool? estado, int? userId)
        {
            try
            {
                return from user in _dacopioContext.Users
                       join person in _dacopioContext.Persons
                           on user.UserPersonId equals person.PersonId
                       join type in _dacopioContext.TypePeople
                           on person.PersonType equals type.TypePesonId
                       where (typeUserId == null || type.TypePesonId == typeUserId)
                       && (name == null ||
                           (person.PersonDni+ " " + person.PersonName + " " + 
                           person.PersonPaternalSurname + " " +
                           person.PersonMaternalSurname).Contains(name))
                       && (userName == null || user.UserName.Contains(userName))
                       && (estado == null || user.UserStatus == estado)
                       && (userId == null || user.UserId == userId)
                       select new UserResultDto
                       {
                           PersonDNI = person.PersonDni,
                           PersonName = person.PersonName + " " +person.PersonPaternalSurname + " " +
                                        person.PersonMaternalSurname,
                           UserName = user.UserName,
                           UserId = user.UserId,
                           UserStatus = user.UserStatus,
                           TypePersonName = type.TypePesonName,
                           UserResetPassword = user.UserResetPassword
                       };
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("default"));
        }

    }
}
