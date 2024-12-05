using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CorteEstado
{
    public int CorteEstadoId { get; set; }

    public string CorteEstadoDescripcion { get; set; } = null!;

    public bool CorteEstadoStatus { get; set; }

    public virtual ICollection<Corte> Cortes { get; set; } = new List<Corte>();
}
