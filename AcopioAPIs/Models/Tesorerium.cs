using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Tesorerium
{
    public int TesoreriaId { get; set; }

    public int LiquidacionId { get; set; }

    public DateOnly TesoreriaFecha { get; set; }

    public decimal TesoreriaMonto { get; set; }

    public decimal? TesoreriaPendientePagar { get; set; }

    public decimal? TesoreriaPagado { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Liquidacion Liquidacion { get; set; } = null!;

    public virtual ICollection<TesoreriaDetallePago> TesoreriaDetallePagos { get; set; } = new List<TesoreriaDetallePago>();
}
