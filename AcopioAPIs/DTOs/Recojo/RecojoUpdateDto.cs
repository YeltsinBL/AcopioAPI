﻿using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Recojo
{
    public class RecojoUpdateDto:UpdateDto
    {
        public int RecojoId { get; set; }
        public DateOnly RecojoFechaInicio { get; set; }
        public DateOnly RecojoFechaFin { get; set; }
        public decimal RecojoCamionesCantidad { get; set; }
        public decimal RecojoCamionesPrecio { get; set; }
        public int RecojoDiasCantidad { get; set; }
        public decimal RecojoDiasPrecio { get; set; }
        public decimal RecojoTotalPrecio { get; set; }
        public required string RecojoEstadoDescripcion { get; set; }
        public string? RecojoCampo { get; set; }
    }
}
