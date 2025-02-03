using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Distribuidor
{
    public int DistribuidorId { get; set; }

    public string DistribuidorNombre { get; set; } = null!;

    public string DistribuidorRuc { get; set; } = null!;

    public bool DistribuidorStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
