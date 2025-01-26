using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class TesoreriaDetallePago
{
    public int TesoreriaDetallePagoId { get; set; }

    public int TesoreriaId { get; set; }

    public DateOnly TesoreriaDetallePagoFecha { get; set; }

    public bool? TesoreriaDetallePagoEfectivo { get; set; }

    public string? TesoreriaDetallePagoBanco { get; set; }

    public string? TesoreriaDetallePagoCtaCte { get; set; }

    public decimal TesoreriaDetallePagoPagado { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public virtual Tesorerium Tesoreria { get; set; } = null!;
}
