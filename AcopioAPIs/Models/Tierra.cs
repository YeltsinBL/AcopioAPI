using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Tierra
{
    public int TierraId { get; set; }

    public string TierraUc { get; set; } = null!;

    public string TierraCampo { get; set; } = null!;

    public string TierraSector { get; set; } = null!;

    public string TierraValle { get; set; } = null!;

    public string TierraHa { get; set; } = null!;

    public bool TierraStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<AsignarTierraHistorial> AsignarTierraHistorials { get; set; } = new List<AsignarTierraHistorial>();

    public virtual ICollection<AsignarTierra> AsignarTierras { get; set; } = new List<AsignarTierra>();

    public virtual ICollection<Corte> Cortes { get; set; } = new List<Corte>();

    public virtual ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();
}
