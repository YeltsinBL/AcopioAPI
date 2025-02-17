using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ServicioPaleroDetalle
{
    public int ServicioPaleroDetalleId { get; set; }

    public int ServicioPaleroId { get; set; }

    public int? TicketId { get; set; }

    public bool ServicioPaleroDetalleStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ServicioPalero ServicioPalero { get; set; } = null!;

    public virtual Ticket? Ticket { get; set; }
}
