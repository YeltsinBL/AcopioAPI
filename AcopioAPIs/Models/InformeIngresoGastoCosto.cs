using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class InformeIngresoGastoCosto
{
    public int InformeCostoId { get; set; }

    public int InformeId { get; set; }

    public decimal InformeCostoPrecio { get; set; }

    public decimal InformeCostoTonelada { get; set; }

    public decimal InformeCostoTotal { get; set; }

    public bool InformeCostoStatus { get; set; }

    public int? InformeCostoOrden { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual InformeIngresoGasto Informe { get; set; } = null!;
}
