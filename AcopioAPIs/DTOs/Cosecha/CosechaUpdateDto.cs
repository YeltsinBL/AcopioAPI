using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Cosecha
{
    public class CosechaUpdateDto: UpdateDto
    {
        public int CosechaId { get; set; }

        public decimal? CosechaHas { get; set; }

        public decimal? CosechaSac { get; set; }

        public decimal? CosechaRed { get; set; }

        public decimal? CosechaHumedad { get; set; }

        public int CosechaCosechaTipoId { get; set; }

    }
}
