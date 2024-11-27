using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Cosecha
{
    public int CosechaId { get; set; }

    public DateOnly CosechaFecha { get; set; }

    public string? CosechaSupervisor { get; set; }

    public double? CosechaHas { get; set; }

    public double? CosechaSac { get; set; }

    public double? CosechaRed { get; set; }

    public double? CosechaHumedad { get; set; }

    public int CosechaTierra { get; set; }

    public int CosechaProveedor { get; set; }

    public int CosechaCosechaTipo { get; set; }

    public virtual CosechaTipo CosechaCosechaTipoNavigation { get; set; } = null!;

    public virtual Proveedor CosechaProveedorNavigation { get; set; } = null!;

    public virtual Tierra CosechaTierraNavigation { get; set; } = null!;
}
