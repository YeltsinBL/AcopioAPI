using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class LiquidacionAdicional
{
    public int LiquidacionAdicionalId { get; set; }

    public int LiquidacionId { get; set; }

    public string LiquidacionAdicionalMotivo { get; set; } = null!;

    public decimal LiquidacionAdicionalTotal { get; set; }

    public bool LiquidacionAdicionalStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Liquidacion Liquidacion { get; set; } = null!;
}
