using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class VentaTipo
{
    public int VentaTipoId { get; set; }

    public string VentaTipoNombre { get; set; } = null!;

    public bool? VentaTipoStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
