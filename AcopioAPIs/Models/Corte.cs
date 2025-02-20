﻿using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Corte
{
    public int CorteId { get; set; }

    public DateOnly CorteFecha { get; set; }

    public int TierraId { get; set; }

    public decimal CortePrecio { get; set; }

    public int CorteEstadoId { get; set; }

    public decimal CortePesoBrutoTotal { get; set; }

    public decimal CorteTotal { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public int? InformeIngresoGastoId { get; set; }

    public virtual ICollection<CorteDetalle> CorteDetalles { get; set; } = new List<CorteDetalle>();

    public virtual CorteEstado CorteEstado { get; set; } = null!;

    public virtual InformeIngresoGasto? InformeIngresoGasto { get; set; }

    public virtual Tierra Tierra { get; set; } = null!;
}
