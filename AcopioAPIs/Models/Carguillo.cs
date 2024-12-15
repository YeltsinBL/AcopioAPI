using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Carguillo
{
    public int CarguilloId { get; set; }

    public string CarguilloTitular { get; set; } = null!;

    public int CarguilloTipoId { get; set; }

    public bool CarguilloEstado { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<CarguilloDetalle> CarguilloDetalles { get; set; } = new List<CarguilloDetalle>();

    public virtual CarguilloTipo CarguilloTipo { get; set; } = null!;

    public virtual ICollection<Corte> Cortes { get; set; } = new List<Corte>();

    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
