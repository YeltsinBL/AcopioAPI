using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class FacturaVentaEstado
{
    public int FacturaVentaEstadoId { get; set; }

    public string FacturaVentaDescripcion { get; set; } = null!;

    public bool FacturaVentaStatus { get; set; }
}
