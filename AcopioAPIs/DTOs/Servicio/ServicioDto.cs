using AcopioAPIs.DTOs.Pago;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioDto
    {
        public int ServicioId { get; set; }
        public DateTime ServicioFecha { get; set; }
        public int CarguilloId { get; set; }
        public required string CarguilloTitular { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal? ServicioPesoBruto { get; set; }
        public decimal ServicioTotal { get; set; }
        public required string ServicioEstadoDescripcion { get; set; }
        public decimal ServicioPendientePagar { get; set; }
        public decimal ServicioPagado { get; set; }
        public required List<ServicioDetailDto> ServicioDetails { get; set; }
        public required List<PagoResultDto> DetallePagos { get; set; }

    }
}
