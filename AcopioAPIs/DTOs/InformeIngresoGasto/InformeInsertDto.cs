using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.InformeIngresoGasto
{
    public class InformeInsertDto: InsertDto
    {
        public int PersonaId { get; set; }
        public int TierraId { get; set; }
        public int ProveedorId { get; set; }
        public DateOnly InformeFecha { get; set; }
        public decimal InformeFacturaTotal { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public decimal InformeTotal { get; set; }

        public required List<InformeInsertFacturaDto> InformeFacturas { get; set; }
        public required List<InformeInsertCostoDto> InformeCostos { get; set; }
        public required List<InformeInsertRelacionesDto> InformeServiciosTransportes { get; set; }
        public required List<InformeInsertRelacionesDto> InformeServiciosPaleros { get; set; }
        public required List<InformeInsertRelacionesDto> InformeRecojos { get; set; }
        public required List<InformeInsertRelacionesDto> InformeLiquidaciones { get; set; }
    }
    public class InformeInsertFacturaDto
    {
        public DateOnly InformeFacturaFecha { get; set; }
        public required string InformeFacturaNumero { get; set; }
        public decimal InformeFacturaImporte { get; set; }

    }
    public class InformeInsertCostoDto
    {
        public decimal InformeCostoPrecio { get; set; }
        public decimal InformeCostoTonelada { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public int InformeCostoOrden { get; set; }

    }
    public class InformeInsertRelacionesDto
    {
        public int Id { get; set; }
    }

}
