using AcopioAPIs.DTOs.Cliente;
using AcopioAPIs.DTOs.Common;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Repositories
{
    public class ClienteRepository : ICliente
    {
        private readonly DbacopioContext _dbacopioContext;

        public ClienteRepository(DbacopioContext dbacopioContext)
        {
            _dbacopioContext = dbacopioContext;
        }
        public async Task<List<ClienteDto>> GetAll(string? nombre, bool? estado)
        {
            var tipo = await _dbacopioContext.TypePeople.FirstOrDefaultAsync(
                    p => p.TypePesonName.Contains("cliente"));
            var query = from persona in _dbacopioContext.Persons
                        where  
                        (nombre == null || 
                            (persona.PersonDni + " " + persona.PersonName + " " + persona.PersonPaternalSurname + " " + persona.PersonMaternalSurname).Contains(nombre)
                        )
                        && (estado == null || persona.PersonStatus == estado)
                        && (tipo == null || tipo.TypePesonId == persona.PersonType)
                        select new ClienteDto
                        {
                            ClienteId = persona.PersonId,
                            ClienteDni = persona.PersonDni,
                            ClienteNombre = persona.PersonName,
                            ClienteApellidoPaterno = persona.PersonPaternalSurname,
                            ClienteApellidoMaterno = persona.PersonMaternalSurname,
                            ClienteStatus = persona.PersonStatus,
                        };
            return await query.ToListAsync();
        }

        public async Task<ResultDto<ClienteDto>> GetById(int id)
        {
            var persona = await _dbacopioContext.Persons.FindAsync(id)
                    ?? throw new KeyNotFoundException("Cliente no encontrado");
            return ReturnResponse(persona, "recuperado");
        }

        public async Task<ResultDto<ClienteDto>> Insert(ClienteInsertDto clienteDto)
        {
            try
            {
                if (clienteDto == null)
                    throw new Exception("No se enviaron datos para guardar el cliente");
                var exist = await _dbacopioContext.Persons.AnyAsync(
                     p => !string.IsNullOrWhiteSpace(p.PersonDni) 
                     && !string.IsNullOrEmpty(clienteDto.ClienteDni)
                     && p.PersonDni.Equals(clienteDto.ClienteDni));                
                if (exist) throw new Exception("El DNI ya existe.");

                var tipo = await _dbacopioContext.TypePeople.FirstOrDefaultAsync(
                    p => p.TypePesonName.Equals("cliente"))
                    ?? throw new Exception("No se encontró el Tipo Cliente");
                var persona = new Person
                {
                    PersonDni = clienteDto.ClienteDni,
                    PersonName = clienteDto.ClienteNombre,
                    PersonPaternalSurname = clienteDto.ClienteApellidoPaterno,
                    PersonMaternalSurname = clienteDto.ClienteApellidoMaterno,
                    PersonType= tipo.TypePesonId,
                    PersonStatus = true,
                    UserCreatedAt = clienteDto.UserCreatedAt,
                    UserCreatedName = clienteDto.UserCreatedName
                };
                _dbacopioContext.Persons.Add(persona);
                await _dbacopioContext.SaveChangesAsync();
                return ReturnResponse(persona, "guardado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResultDto<ClienteDto>> Update(ClienteUpdateDto clienteDto)
        {
            try
            {
                var persona = await _dbacopioContext.Persons.FindAsync(clienteDto.ClienteId)
                    ?? throw new KeyNotFoundException("Cliente no encontrado");
                var exist = await _dbacopioContext.Persons.AnyAsync(
                    p => !string.IsNullOrWhiteSpace(p.PersonDni)
                     && !string.IsNullOrEmpty(clienteDto.ClienteDni)
                     && p.PersonDni.Equals(clienteDto.ClienteDni)
                    && p.PersonId != clienteDto.ClienteId);
                if (exist) throw new Exception("El DNI ya existe");
                persona.PersonDni = clienteDto.ClienteDni;
                persona.PersonName = clienteDto.ClienteNombre;
                persona.PersonPaternalSurname = clienteDto.ClienteApellidoPaterno;
                persona.PersonMaternalSurname = clienteDto.ClienteApellidoMaterno;
                persona.PersonStatus = true;
                persona.UserModifiedAt = clienteDto.UserModifiedAt;
                persona.UserModifiedName = clienteDto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();

                return ReturnResponse(persona, "actualizado");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResultDto<ClienteDto>> Delete(ClienteDeleteDto clienteDto)
        {
            try
            {
                var persona = await _dbacopioContext.Persons.FindAsync(clienteDto.ClienteId)
                    ?? throw new KeyNotFoundException("Cliente no encontrado");
                if (!persona.PersonStatus) throw new Exception("El cliente ya está desactivado");
                persona.PersonStatus = false;
                persona.UserModifiedAt = clienteDto.UserModifiedAt;
                persona.UserModifiedName = clienteDto.UserModifiedName;

                await _dbacopioContext.SaveChangesAsync();
                return ReturnResponse(persona, "desactivado");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static ResultDto<ClienteDto> ReturnResponse(Person persona, string mensaje)
        {
            return new ResultDto<ClienteDto>
            {
                Result = true,
                ErrorMessage = "Cliente " + mensaje,
                Data = new ClienteDto
                {
                    ClienteId = persona.PersonId,
                    ClienteDni = persona.PersonDni,
                    ClienteNombre = persona.PersonName,
                    ClienteApellidoPaterno = persona.PersonPaternalSurname,
                    ClienteApellidoMaterno = persona.PersonMaternalSurname,
                    ClienteStatus = persona.PersonStatus,
                }
            };
        }

    }
}
