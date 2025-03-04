using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.Reporte
{
    public class ReporteGastoResult
    {
        public int PersonId { get; set; }
        public required string PersonaNombre { get; set; }
        public required string ProveedorUT { get; set; }
        public decimal Utilidad { get; set; }

    }
}
