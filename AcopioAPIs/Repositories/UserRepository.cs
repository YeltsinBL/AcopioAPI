using AcopioAPIs.DTOs.User;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AcopioAPIs.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DbacopioContext _dacopioContext;

        public UserRepository(DbacopioContext dacopioContext)
        {
            _dacopioContext = dacopioContext;
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
                var query = from user in _dacopioContext.Users
                            join person in _dacopioContext.Persons
                                on user.UserPersonId equals person.PersonId
                            join type in _dacopioContext.TypePeople
                                on person.PersonType equals type.TypePesonId
                            where user.UserId == id
                            select new UserDto
                            {
                                UserId = user.UserId,
                                UserName = user.UserName,
                                UserStatus = user.UserStatus,
                                PersonId = person.PersonId,
                                PersonDNI = person.PersonDni,
                                PersonName = person.PersonName,
                                PersonPaternalSurname = person.PersonPaternalSurname,
                                PersonMaternalSurname = person.PersonMaternalSurname,
                                PersonStatus = person.PersonStatus,
                                TypePersonId = type.TypePesonId,
                                TypePersonName = type.TypePesonName,
                            };
                return await query.FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Usuario no encontrado");
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
                    ?? throw new KeyNotFoundException("Usuario no encontrada."); ;
                user.UserStatus = false;
                user.UserModifiedAt = deleteDto.UserModifiedAt;
                user.UserModifiedName = deleteDto.UserModifiedName;

                _dacopioContext.Users.Update(user);
                await _dacopioContext.SaveChangesAsync();
                return true;
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

    }
}
