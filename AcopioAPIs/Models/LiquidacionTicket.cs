using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class LiquidacionTicket
{
    public int LiquidacionTicketId { get; set; }

    public int LiquidacionId { get; set; }

    public int TicketId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Liquidacion Liquidacion { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
