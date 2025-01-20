using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioPaleroInsertDto: InsertDto
    {
        public DateOnly ServicioFecha { get; set; }
        public int CarguilloId { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal? ServicioPesoBruto { get; set; }
        public decimal ServicioTotal { get; set; }
        public required List<ServicioPaleroDetailDto> ServicioDetail { get; set; }
    }
}
