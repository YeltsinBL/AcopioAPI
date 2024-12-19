using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class LiquidacionEstado
{
    public int LiquidacionEstadoId { get; set; }

    public string LiquidacionEstadoDescripcion { get; set; } = null!;

    public bool LiquidacionEstadoStatus { get; set; }

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();
}
