using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ServicioPalero
{
    public int ServicioPaleroId { get; set; }

    public DateOnly ServicioPaleroFecha { get; set; }

    public int CarguilloId { get; set; }

    public decimal ServicioPaleroPrecio { get; set; }

    public decimal ServicioPaleroTotal { get; set; }

    public int ServicioTransporteEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Carguillo Carguillo { get; set; } = null!;

    public virtual ICollection<ServicioPaleroDetalle> ServicioPaleroDetalles { get; set; } = new List<ServicioPaleroDetalle>();

    public virtual ServicioTransporteEstado ServicioTransporteEstado { get; set; } = null!;
}
