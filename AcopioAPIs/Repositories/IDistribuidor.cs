using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Distribuidor;

namespace AcopioAPIs.Repositories
{
    public interface IDistribuidor
    {
        Task<List<DistribuidorDto>> GetAll(string? ruc, string? nombre, bool? estado);
        Task<ResultDto<DistribuidorDto>> GetById(int id);
        Task<ResultDto<DistribuidorDto>> Insert(DistribuidorInsertDto distribuidorDto);
        Task<ResultDto<DistribuidorDto>> Update(DistribuidorUpdateDto distribuidorDto);
        Task<ResultDto<DistribuidorDto>> Delete(DistribuidorDeleteDto distribuidorDto);
    }
}
