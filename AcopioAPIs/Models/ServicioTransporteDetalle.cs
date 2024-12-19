using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ServicioTransporteDetalle
{
    public int ServicioTransporteDetalleId { get; set; }

    public int ServicioTransporteId { get; set; }

    public int TicketId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public bool ServicioTransporteDetalleStatus { get; set; }

    public virtual ServicioTransporte ServicioTransporte { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
