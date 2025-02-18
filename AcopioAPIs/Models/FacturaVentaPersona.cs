using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class FacturaVentaPersona
{
    public int FacturaVentaPersonaId { get; set; }

    public int FacturaVentaId { get; set; }

    public int PersonaId { get; set; }

    public bool FacturaVentaPersonStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual FacturaVentum FacturaVenta { get; set; } = null!;

    public virtual Person Persona { get; set; } = null!;
}
