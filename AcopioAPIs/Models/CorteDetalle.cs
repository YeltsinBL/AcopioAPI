using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CorteDetalle
{
    public int CorteDetalleId { get; set; }

    public int CorteId { get; set; }

    public int TicketId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public virtual Corte Corte { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
