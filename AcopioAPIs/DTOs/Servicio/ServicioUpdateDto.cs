﻿using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Servicio
{
    public class ServicioUpdateDto:UpdateDto
    {
        public int ServicioId { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal ServicioTotal { get; set; }
        public required string ServicioEstadoDescripcion { get; set; }
    }
}
