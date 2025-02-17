using AcopioAPIs.Models;

namespace AcopioAPIs.DTOs.InformeIngresoGasto
{
    public class InformeDto
    {
        public int InformeId { get; set; }
        public int PersonaId { get; set; }
        public required string PersonaNombre { get; set; }
        public int TierraId { get; set; }
        public required string TierraCampo { get; set; }
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public DateTime InformeFecha { get; set; }
        public decimal InformeFacturaTotal { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public decimal InformeTotal { get; set; }
        public bool InformeStatus { get; set; }
        public string? InformeResultado { get; set; }
        public required List<InformeFacturaDto> InformeFacturas { get; set; }
        public required List<InformeCostoDto> InformeCostos { get; set; }
        public required List<InformeServicioDto> InformeServiciosTransportes { get; set; }
        public required List<InformeServicioDto> InformeServiciosPaleros { get; set; }
        public required List<InformeCorteDto> InformeCortes { get; set; }
        public required List<InformeLiquidacionDto> InformeLiquidaciones { get; set; }
    }
    public class InformeFacturaDto
    {
        public int InformeFacturaId { get; set; }
        public DateTime InformeFacturaFecha { get; set; }
        public required string InformeFacturaNumero { get; set; }
        public decimal InformeFacturaImporte { get; set; }
        public bool InformeFacturaStatus { get; set; }

    }
    public class InformeCostoDto
    {
        public int InformeCostoId { get; set; }
        public decimal InformeCostoPrecio { get; set; }
        public decimal InformeCostoTonelada { get; set; }
        public decimal InformeCostoTotal { get; set; }
        public int InformeCostoOrden { get; set; }
        public bool InformeCostoStatus { get; set; }
    }
    public class InformeServicioDto
    {
        public int ServicioId { get; set; }
        public DateTime ServicioFecha { get; set; }
        public decimal ServicioPesoBruto { get; set; }
        public decimal ServicioPrecio { get; set; }
        public decimal ServicioTotal { get; set; }
        public required string CarguilloTitular { get; set; }
    }
    public class InformeCorteDto
    {
        public int CorteId { get; set; }
        public DateTime CorteFecha { get; set; }
        public required string TierraUC { get; set; }
        public required string TierraCampo { get; set; }
        public decimal CortePrecio { get; set; }
        public decimal CortePesoBrutoTotal { get; set; }
        public decimal CorteTotal { get; set; }

    }
    public class InformeLiquidacionDto
    {
        public int LiquidacionId { get; set; }
        public DateTime LiquidacionFechaInicio { get; set; }
        public DateTime LiquidacionFechaFin { get; set; }
        public decimal LiquidacionPesoNeto { get; set; }
        public decimal LiquidacionPesoBruto { get; set; }
        public decimal LiquidacionToneladaTotal { get; set; }
        public decimal LiquidacionPagar { get; set; }
        public decimal LiquidacionToneladaPrecioCompra { get; set; }
        public required string PersonaNombre { get; set; }
        public required string TierraCampo {get; set; }
        public required string ProveedorUT {get; set; }

    }
}
