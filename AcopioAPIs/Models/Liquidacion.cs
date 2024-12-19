using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Liquidacion
{
    public int LiquidacionId { get; set; }

    public int CorteId { get; set; }

    public int TierraId { get; set; }

    public int ProveedorId { get; set; }

    public DateOnly LiquidacionFechaInicio { get; set; }

    public DateOnly LiquidacionFechaFin { get; set; }

    public decimal LiquidacionPesoBruto { get; set; }

    public decimal LiquidacionPesoNeto { get; set; }

    public decimal LiquidacionTotalPesoBruto { get; set; }

    public decimal LiquidacionTotalPesoNeto { get; set; }

    public decimal LiquidacionToneladaPrecioCompra { get; set; }

    public decimal LiquidacionToneladaTotal { get; set; }

    public decimal LiquidacionFinanciamientoAcuenta { get; set; }

    public decimal LiquidacionPagar { get; set; }

    public int LiquidacionEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Corte Corte { get; set; } = null!;

    public virtual LiquidacionEstado LiquidacionEstado { get; set; } = null!;

    public virtual ICollection<LiquidacionFinanciamiento> LiquidacionFinanciamientos { get; set; } = new List<LiquidacionFinanciamiento>();

    public virtual Proveedor Proveedor { get; set; } = null!;

    public virtual Tierra Tierra { get; set; } = null!;
}
