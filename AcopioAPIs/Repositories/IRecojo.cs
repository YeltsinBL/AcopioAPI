using AcopioAPIs.DTOs.Recojo;
using AcopioAPIs.Models;

namespace AcopioAPIs.Repositories
{
    public interface IRecojo
    {
        Task<List<RecojoEstadoResultDto>> ListRecojoEstado();
        Task<List<RecojoResultDto>> ListRecojo(DateOnly? fechaDesde, DateOnly? fechaHasta, int? recojoEstadoId);
        Task<RecojoDto> GetRecojoById(int recojoId);
        Task<RecojoResultDto> SaveRecojo(RecojoInsertDto insertDto);
        Task<RecojoResultDto> UpdateRecojo(RecojoUpdateDto updateDto);
        Task<bool> DeleteRecojo(RecojoDeleteDto deleteDto);
    }
}
