using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Cosecha
{
    public int CosechaId { get; set; }

    public DateOnly CosechaFecha { get; set; }

    public string? CosechaSupervisor { get; set; }

    public decimal? CosechaHas { get; set; }

    public decimal? CosechaSac { get; set; }

    public decimal? CosechaRed { get; set; }

    public decimal? CosechaHumedad { get; set; }

    public int CosechaTierra { get; set; }

    public int CosechaProveedor { get; set; }

    public int CosechaCosechaTipo { get; set; }
    public string UserCreatedName { get; set; } = null!;

    public DateOnly UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateOnly? UserModifiedAt { get; set; }

    public virtual CosechaTipo CosechaCosechaTipoNavigation { get; set; } = null!;

    public virtual Proveedor CosechaProveedorNavigation { get; set; } = null!;

    public virtual Tierra CosechaTierraNavigation { get; set; } = null!;
}
