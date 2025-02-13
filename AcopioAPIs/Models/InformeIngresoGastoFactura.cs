using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class InformeIngresoGastoFactura
{
    public int InformeFacturaId { get; set; }

    public int InformeId { get; set; }

    public DateOnly InformeFacturaFecha { get; set; }

    public string InformeFacturaNumero { get; set; } = null!;

    public decimal InformeFacturaImporte { get; set; }

    public bool InformeFacturaStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual InformeIngresoGasto Informe { get; set; } = null!;
}
