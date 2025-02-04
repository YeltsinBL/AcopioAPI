using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Ventum
{
    public int VentaId { get; set; }

    public DateOnly VentaFecha { get; set; }

    public int TipoComprobanteId { get; set; }

    public int VentaNumeroDocumento { get; set; }

    public int PersonaId { get; set; }

    public int VentaTipoId { get; set; }

    public int? VentaDia { get; set; }

    public DateOnly? VentaFechaVence { get; set; }

    public decimal VentaTotal { get; set; }

    public int VentaEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Person Persona { get; set; } = null!;

    public virtual TipoComprobante TipoComprobante { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();

    public virtual VentaEstado VentaEstado { get; set; } = null!;

    public virtual VentaTipo VentaTipo { get; set; } = null!;
}
