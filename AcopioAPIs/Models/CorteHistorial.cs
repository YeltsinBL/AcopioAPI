using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CorteHistorial
{
    public int CorteHistorialId { get; set; }

    public int CorteId { get; set; }

    public DateOnly CorteFecha { get; set; }

    public int TierraId { get; set; }

    public decimal CortePrecio { get; set; }

    public int CorteEstadoId { get; set; }

    public decimal CortePesoBrutoTotal { get; set; }

    public decimal CorteTotal { get; set; }

    public int CarguilloId { get; set; }

    public decimal CarguilloPrecio { get; set; }

    public string UserModifiedName { get; set; } = null!;

    public DateTime UserModifiedAt { get; set; }
}
