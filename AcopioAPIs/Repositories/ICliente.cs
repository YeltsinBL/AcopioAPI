using AcopioAPIs.DTOs.Cliente;
using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.Repositories
{
    public interface ICliente
    {
        Task<List<ClienteDto>> GetAll(string? nombre, bool? estado);
        Task<ResultDto<ClienteDto>> GetById(int id);
        Task<ResultDto<ClienteDto>> Insert(ClienteInsertDto clienteDto);
        Task<ResultDto<ClienteDto>> Update(ClienteUpdateDto clienteDto);
        Task<ResultDto<ClienteDto>> Delete(ClienteDeleteDto clienteDto);
    }
}
