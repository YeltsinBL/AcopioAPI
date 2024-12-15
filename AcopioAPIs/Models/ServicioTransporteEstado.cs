using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ServicioTransporteEstado
{
    public int ServicioTransporteEstadoId { get; set; }

    public string ServicioTransporteEstadoDescripcion { get; set; } = null!;

    public bool ServicioTransporteEstadoStatus { get; set; }
}
