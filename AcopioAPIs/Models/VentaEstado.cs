using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class VentaEstado
{
    public int VentaEstadoId { get; set; }

    public string VentaEstadoNombre { get; set; } = null!;

    public bool? VentaEstadoStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
