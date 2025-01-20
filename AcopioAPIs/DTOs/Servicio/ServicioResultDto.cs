namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioResultDto
    {
        public int ServicioId { get; set; }
        public DateOnly ServicioFecha { get; set; }
        public required string ServicioCarguilloTitular { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal? ServicioPesoBruto { get; set; }
        public decimal ServicioTotal { get; set; }
        public required string ServicioEstadoDescripcion { get; set; }
    }
}
