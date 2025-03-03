﻿using AcopioAPIs.DTOs.Common;
using AcopioAPIs.DTOs.Corte;

namespace AcopioAPIs.Repositories
{
    public interface ICorte
    {
        Task<List<CorteEstadoDto>> GetCorteEstados();
        Task<List<CorteResultDto>> GetAll(DateOnly? fechaDesde,
            DateOnly? fechaHasta, int? tierraId, int? estadoId);
        Task<CorteDto> GetById(int id);
        Task<ResultDto<CorteResultDto>> Save(
            CorteInsertDto corteInsertDto,
            List<IFormFile> imagenes,
            List<string> descripciones
            );
        Task<ResultDto<CorteResultDto>> Update(
            CorteUpdateDto corteUpdateDto,
            List<IFormFile> imagenes,
            List<string> descripciones);
        Task<ResultDto<int>> Delete(CorteDeleteDto corteDelete);
    }
}
