using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Proveedor
{
    public int ProveedorId { get; set; }

    public string ProveedorUt { get; set; } = null!;

    public bool ProveedorStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateOnly UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateOnly? UserModifiedAt { get; set; }

    public int? ProveedorPerson { get; set; }

    public virtual ICollection<AsignarTierraHistorial> AsignarTierraHistorials { get; set; } = new List<AsignarTierraHistorial>();

    public virtual ICollection<AsignarTierra> AsignarTierras { get; set; } = new List<AsignarTierra>();

    public virtual ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual Person? ProveedorPersonNavigation { get; set; }
}
