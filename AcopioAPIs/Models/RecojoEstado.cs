using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class RecojoEstado
{
    public int RecojoEstadoId { get; set; }

    public string RecojoEstadoDescripcion { get; set; } = null!;

    public bool RecojoEstadoStatus { get; set; }

    public virtual ICollection<Recojo> Recojos { get; set; } = new List<Recojo>();
}
