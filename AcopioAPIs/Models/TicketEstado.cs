using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class TicketEstado
{
    public int TicketEstadoId { get; set; }

    public string TicketEstadoDescripcion { get; set; } = null!;

    public bool TicketEstadoStatus { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
