using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class AsignarTierraHistorial
{
    public int HistorialId { get; set; }

    public int TierraId { get; set; }

    public int ProveedorId { get; set; }

    public DateOnly AsignarTierraFecha { get; set; }

    public bool AsignarTierraStatus { get; set; }

    public string UserModifiedName { get; set; } = null!;

    public DateTime UserModifiedAt { get; set; }

    public virtual Proveedor Proveedor { get; set; } = null!;

    public virtual Tierra Tierra { get; set; } = null!;
}
