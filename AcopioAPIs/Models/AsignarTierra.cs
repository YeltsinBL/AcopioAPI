using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class AsignarTierra
{
    public int AsignarTierraId { get; set; }

    public DateOnly AsignarTierraFecha { get; set; }

    public bool AsignarTierraStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateOnly UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateOnly? UserModifiedAt { get; set; }

    public int AsignarTierraProveedor { get; set; }

    public int AsignarTierraTierra { get; set; }

    public virtual Proveedor AsignarTierraProveedorNavigation { get; set; } = null!;

    public virtual Tierra AsignarTierraTierraNavigation { get; set; } = null!;
}
