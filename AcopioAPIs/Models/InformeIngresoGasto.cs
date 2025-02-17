using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class InformeIngresoGasto
{
    public int InformeId { get; set; }

    public int PersonaId { get; set; }

    public int TierraId { get; set; }

    public int ProveedorId { get; set; }

    public DateOnly InformeFecha { get; set; }

    public decimal InformeFacturaTotal { get; set; }

    public decimal InformeCostoTotal { get; set; }

    public decimal InformeTotal { get; set; }

    public bool InformeStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public string? InformeResultado { get; set; }

    public virtual ICollection<Corte> Cortes { get; set; } = new List<Corte>();

    public virtual ICollection<InformeIngresoGastoCosto> InformeIngresoGastoCostos { get; set; } = new List<InformeIngresoGastoCosto>();

    public virtual ICollection<InformeIngresoGastoFactura> InformeIngresoGastoFacturas { get; set; } = new List<InformeIngresoGastoFactura>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual Person Persona { get; set; } = null!;

    public virtual Proveedor Proveedor { get; set; } = null!;

    public virtual ICollection<ServicioPalero> ServicioPaleros { get; set; } = new List<ServicioPalero>();

    public virtual ICollection<ServicioTransporte> ServicioTransportes { get; set; } = new List<ServicioTransporte>();

    public virtual Tierra Tierra { get; set; } = null!;
}
