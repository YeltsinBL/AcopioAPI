using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class LiquidacionFinanciamiento
{
    public int LiquidacionFinanciamientoId { get; set; }

    public int LiquidacionId { get; set; }

    public DateOnly LiquidacionFinanciamientoFecha { get; set; }

    public decimal LiquidacionFinanciamientoAcuenta { get; set; }

    public int LiquidacionFinanciamientoTiempo { get; set; }

    public decimal LiquidacionFinanciamientoInteresMes { get; set; }

    public decimal LiquidacionFinanciamientoInteres { get; set; }

    public decimal LiquidacionFinanciamientoTotal { get; set; }

    public bool? LiquidacionFinanciamientoStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public string? ImagenUrl { get; set; }

    public string? ImagenComentario { get; set; }

    public virtual Liquidacion Liquidacion { get; set; } = null!;
}
