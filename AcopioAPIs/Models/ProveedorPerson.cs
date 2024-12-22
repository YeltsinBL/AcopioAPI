using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ProveedorPerson
{
    public int ProveedorPersonId { get; set; }

    public int PersonId { get; set; }

    public int ProveedorId { get; set; }

    public bool ProveedorPersonStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Proveedor Proveedor { get; set; } = null!;
}
