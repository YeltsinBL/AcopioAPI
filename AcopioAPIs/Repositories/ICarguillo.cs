﻿using AcopioAPIs.DTOs.Carguillo;

namespace AcopioAPIs.Repositories
{
    public interface ICarguillo
    {
        Task<List<CarguilloTipoResultDto>> GetCarguilloTipos(bool isCarguillo);
        Task<List<CarguilloResultDto>> GetCarguillos(int? tipoCarguilloId, string? titular, bool? estado);
        Task<CarguilloDto> GetCarguilloById(int carguilloId);
        Task<CarguilloResultDto> SaveCarguillo(CarguilloInsertDto insertDto);
        Task<CarguilloResultDto> UpdateCarguillo(CarguilloUpdateDto updateDto);
        Task<CarguilloPlacasResultDto> GetCarguilloDetalles(int carguilloId);
        Task<List<CarguilloResultDto>> GetCarguillosTicket();
    }
}
