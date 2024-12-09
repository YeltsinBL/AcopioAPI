using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CarguilloDetalle
{
    public int CarguilloDetalleId { get; set; }

    public int CarguilloId { get; set; }

    public int CarguilloTipoId { get; set; }

    public string? CarguilloDetallePlaca { get; set; }

    public bool CarguilloDetalleEstado { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Carguillo Carguillo { get; set; } = null!;

    public virtual CarguilloTipo CarguilloTipo { get; set; } = null!;

    public virtual ICollection<Ticket> TicketCarguilloDetalleCamions { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketCarguilloDetalleVehiculos { get; set; } = new List<Ticket>();
}
