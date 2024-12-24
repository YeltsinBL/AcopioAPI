using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Proveedor;
using AcopioAPIs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace AcopioAPIs.Repositories
{
    public class ProveedorRepository : IProveedor
    {
        private readonly DbacopioContext _dbacopioContext;
        private readonly IConfiguration _configuration;

        public ProveedorRepository(IConfiguration configuration, DbacopioContext dbacopioContext)
        {
            _configuration = configuration;
            _dbacopioContext = dbacopioContext;
        }

        public async Task<List<ProveedorResultDto>> List(string? ut, string? nombre, bool? estado)
        {
            try
            {
                var query = from p in _dbacopioContext.Persons
                            join pp in _dbacopioContext.ProveedorPeople
                                on p.PersonId equals pp.PersonId
                            join pr in _dbacopioContext.Proveedors
                                on pp.ProveedorId equals pr.ProveedorId
                            where (ut == null || pr.ProveedorUt == ut)
                            && (nombre == null || (p.PersonName + " " + p.PersonPaternalSurname + " " + p.PersonMaternalSurname).Contains(nombre))
                            && (estado == null || pr.ProveedorStatus == estado)
                            select new ProveedorResultDto
                            {
                                ProveedorId = pr.ProveedorId,
                                ProveedorUT = pr.ProveedorUt,
                                PersonDNI = p.PersonDni,
                                ProveedorNombre = p.PersonName + " " + p.PersonPaternalSurname + " " + p.PersonMaternalSurname,
                                ProveedorStatus = pr.ProveedorStatus
                            };

                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProveedorDTO> Get(int id)
        {
            try
            {
                using var conexion = GetConnection();
                using var multi = await conexion.QueryMultipleAsync(
                    "usp_ProveedorGetById",
                    new { Id = id }, 
                    commandType: CommandType.StoredProcedure);
                var proveedores = await multi.ReadFirstOrDefaultAsync<ProveedorDTO>()
                    ?? throw new Exception("No se encontró el Proveedor");
                proveedores.ProveedorPerson = (await multi.ReadAsync<ProveedorPersonDto>()).ToList();
                return proveedores;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProveedorResultDto> Save(ProveedorInsertDto proveedor)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (proveedor == null) throw new Exception("No se enviaron datos para guardar el proveedor.");
                if (proveedor.ProveedorPerson == null) throw new Exception("No se enviaron datos del titular.");
                var typePerson = await GetTypePerson("proveedor") 
                    ?? throw new Exception("No se encontró el tipo de persona Proveedor.");
                if (await _dbacopioContext.Proveedors.AnyAsync(c => c.ProveedorUt == proveedor.ProveedorUT))
                    throw new Exception($"Ya existe la UT {proveedor.ProveedorUT}");
                var provee = new Proveedor
                {
                     ProveedorStatus = true,
                     ProveedorUt = proveedor.ProveedorUT,
                     UserCreatedAt = proveedor.UserCreatedAt,
                     UserCreatedName = proveedor.UserCreatedName
                };
                _dbacopioContext.Proveedors.Add(provee);
                await _dbacopioContext.SaveChangesAsync();

                foreach (var personProv in proveedor.ProveedorPerson)
                {
                    bool isDniValid = personProv.PersonDNI != null &&
                    await _dbacopioContext.Persons.AnyAsync(p => p.PersonDni == personProv.PersonDNI);

                    if (isDniValid) throw new Exception($"Ya existe el DNI {personProv.PersonDNI}.");
                    var person = new Person
                    {
                        PersonDni = personProv.PersonDNI,
                        PersonName = personProv.PersonName,
                        PersonPaternalSurname = personProv.PersonPaternalSurname,
                        PersonMaternalSurname = personProv.PersonMaternalSurname,
                        PersonStatus = true,
                        PersonType = typePerson.TypePesonId                        
                    };
                    _dbacopioContext.Persons.Add(person);
                    await _dbacopioContext.SaveChangesAsync();

                    var proveedorPerson = new ProveedorPerson
                    {
                        PersonId = person.PersonId,
                        ProveedorId = provee.ProveedorId,
                        ProveedorPersonStatus = true,
                        UserCreatedName = proveedor.UserCreatedName,
                        UserCreatedAt = proveedor.UserCreatedAt
                    };
                    _dbacopioContext.ProveedorPeople.Add(proveedorPerson);

                }
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return await GetProveedor(provee.ProveedorId) ??
                   throw new Exception("Proveedor guardado pero no encontrado");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProveedorResultDto> Update(ProveedorUpdateDto proveedor)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (proveedor == null) throw new Exception("No se enviaron datos para guardar al proveedor.");
                if (proveedor.ProveedorPersons == null) throw new Exception("No se enviaron datos del titular.");
                var typePerson = await GetTypePerson("proveedor")
                    ?? throw new Exception("No se encontró el tipo de persona Proveedor.");
                if (await _dbacopioContext.Proveedors.AnyAsync(p => p.ProveedorUt == proveedor.ProveedorUT && p.ProveedorId != proveedor.ProveedorId))
                    throw new Exception($"Ya existe la UT {proveedor.ProveedorUT}.");

                var provee = await _dbacopioContext.Proveedors
                    .Include(p => p.ProveedorPeople)
                    .FirstOrDefaultAsync(p => p.ProveedorId == proveedor.ProveedorId) 
                    ?? throw new Exception("Proveedor no encontrado");

                provee.ProveedorUt = proveedor.ProveedorUT;
                provee.UserModifiedAt = proveedor.UserModifiedAt;
                provee.UserModifiedName = proveedor.UserModifiedName;

                _dbacopioContext.Proveedors.Update(provee);
                await _dbacopioContext.SaveChangesAsync();

                foreach (var personas in proveedor.ProveedorPersons)
                {
                    if (await _dbacopioContext.Persons
                        .AnyAsync(p => p.PersonDni == personas.PersonDNI 
                            && p.PersonId != personas.PersonId))
                        throw new Exception($"Ya existe el DNI {personas.PersonDNI}.");

                    var persona = await _dbacopioContext.Persons
                        .FirstOrDefaultAsync(c => c.PersonId == personas.PersonId);

                    var persProv = provee.ProveedorPeople
                        .FirstOrDefault(pp => pp.ProveedorPersonId == personas.ProveedorPersonId);

                    if(persProv != null && persona != null)
                    {
                        persProv.ProveedorPersonStatus = personas.PersonStatus;
                        persProv.UserModifiedAt = proveedor.UserModifiedAt;
                        persProv.UserModifiedName= proveedor.UserModifiedName;

                        persona.PersonDni = personas.PersonDNI;
                        persona.PersonName = personas.PersonName;
                        persona.PersonPaternalSurname = personas.PersonPaternalSurname;
                        persona.PersonMaternalSurname = personas.PersonMaternalSurname;
                        persona.PersonStatus = personas.PersonStatus;

                        _dbacopioContext.Persons.Update(persona);
                        await _dbacopioContext.SaveChangesAsync();

                    }
                    else
                    {
                        var person = new Person
                        {
                            PersonDni = personas.PersonDNI,
                            PersonName = personas.PersonName,
                            PersonPaternalSurname = personas.PersonPaternalSurname,
                            PersonMaternalSurname = personas.PersonMaternalSurname,
                            PersonStatus = true,
                            PersonType = typePerson.TypePesonId
                        };
                        _dbacopioContext.Persons.Add(person);
                        await _dbacopioContext.SaveChangesAsync();

                        var proveedorPerson = new ProveedorPerson
                        {
                            PersonId = person.PersonId,
                            ProveedorId = provee.ProveedorId,
                            ProveedorPersonStatus = true,
                            UserCreatedName = proveedor.UserModifiedName,
                            UserCreatedAt = proveedor.UserModifiedAt
                        };
                        _dbacopioContext.ProveedorPeople.Add(proveedorPerson);
                        await _dbacopioContext.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                return await GetProveedor(proveedor.ProveedorId)
                    ?? throw new Exception("Proveedor guardado pero no encontrado");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(ProveedorDeleteDto proveedorDeleteDto)
        {
            using var transaction = await _dbacopioContext.Database.BeginTransactionAsync();
            try
            {
                if (proveedorDeleteDto == null) throw new Exception("No se enviaron datos para guardar al proveedor.");
                var provee = await _dbacopioContext.Proveedors
                    .Include(p => p.ProveedorPeople)
                    .FirstOrDefaultAsync(p => p.ProveedorId == proveedorDeleteDto.ProveedorId)
                    ?? throw new Exception("Proveedor no encontrado");

                provee.ProveedorStatus = false;
                provee.UserModifiedAt = proveedorDeleteDto.UserModifiedAt;
                provee.UserModifiedName = proveedorDeleteDto.UserModifiedName;

                _dbacopioContext.Proveedors.Update(provee);

                foreach (var personas in provee.ProveedorPeople)
                {
                    var persona = await _dbacopioContext.Persons
                       .FirstOrDefaultAsync(c => c.PersonId == personas.PersonId)
                       ?? throw new Exception("Persona no encontrada");

                    persona.PersonStatus = false;
                    _dbacopioContext.Persons.Update(persona);

                    var persProv = provee.ProveedorPeople
                        .FirstOrDefault(pp => pp.ProveedorPersonId == personas.ProveedorPersonId)
                        ?? throw new Exception("Relación Proveedor-Persona no encontrada");

                    persProv.ProveedorPersonStatus = false;
                    persProv.UserModifiedAt = proveedorDeleteDto.UserModifiedAt;
                    persProv.UserModifiedName = proveedorDeleteDto.UserModifiedName;

                    _dbacopioContext.ProveedorPeople.Update(persProv);
                }
                await _dbacopioContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<ProveedorResultDto>> GetAvailableProveedor()
        {
            try
            {
                using var conexion = GetConnection();
                var proveedores = await conexion.QueryAsync<ProveedorResultDto>(
                        "usp_ProveedorGetAvailable", commandType: CommandType.StoredProcedure
                    );

                return proveedores.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<TypePerson?> GetTypePerson(string description)
        {
            var query = from type in _dbacopioContext.TypePeople
                        where type.TypePesonName.Equals(description)
                        select type;
            return await query.FirstOrDefaultAsync();
        }
        private async Task<ProveedorResultDto?> GetProveedor(int proveedorId)
        {
            var query = from p in _dbacopioContext.Persons
                        join pp in _dbacopioContext.ProveedorPeople
                            on p.PersonId equals pp.PersonId
                        join pr in _dbacopioContext.Proveedors
                            on pp.ProveedorId equals pr.ProveedorId
                        where pp.ProveedorId == proveedorId
                        select new ProveedorResultDto
                        {
                            ProveedorId = pr.ProveedorId,
                            ProveedorUT = pr.ProveedorUt,
                            PersonDNI = p.PersonDni,
                            ProveedorNombre = p.PersonName + " " + p.PersonPaternalSurname + " " + p.PersonMaternalSurname,
                            ProveedorStatus = pr.ProveedorStatus
                        };
            return await query.FirstOrDefaultAsync();
        }
        private SqlConnection GetConnection() 
        {
            return new SqlConnection(_configuration.GetConnectionString("default")); 
        }

        public async Task<List<ProveedorGroupedDto>> ListNew(string? ut, string? nombre, bool? estado)
        {
            try
            {
                var query = from p in _dbacopioContext.Persons
                            join pp in _dbacopioContext.ProveedorPeople
                                on p.PersonId equals pp.PersonId
                            join pr in _dbacopioContext.Proveedors
                                on pp.ProveedorId equals pr.ProveedorId
                            where (ut == null || pr.ProveedorUt == ut)
                                && (nombre == null || (p.PersonName + " " + p.PersonPaternalSurname + " " + p.PersonMaternalSurname).Contains(nombre))
                                && (estado == null || pr.ProveedorStatus == estado)
                            select new
                            {
                                pr.ProveedorId,
                                pr.ProveedorUt,
                                p.PersonDni,
                                ProveedorNombre = p.PersonName + " " + p.PersonPaternalSurname + " " + p.PersonMaternalSurname,
                                pr.ProveedorStatus,
                                pp.ProveedorPersonStatus
                            };

                // Agrupar por ProveedorId y ProveedorUT
                var grouped = await query
                    .GroupBy(item => new { item.ProveedorId, item.ProveedorUt, item.ProveedorStatus })
                    .Select(group => new ProveedorGroupedDto
                    {
                        ProveedorId = group.Key.ProveedorId,
                        ProveedorUT = group.Key.ProveedorUt,
                        ProveedorStatus = group.Key.ProveedorStatus,
                        Personas = group
                            .Where(g => !string.IsNullOrEmpty(g.PersonDni)) // Filtrar si el DNI no es null o vacío
                            .Select(g => new PersonaDto
                            {
                                PersonDNI = g.PersonDni,
                                ProveedorNombre = g.ProveedorNombre,
                                ProveedorPersonStatus = g.ProveedorPersonStatus
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return grouped;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
