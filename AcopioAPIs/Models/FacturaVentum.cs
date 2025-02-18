using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class FacturaVentum
{
    public int FacturaVentaId { get; set; }

    public DateOnly FacturaVentaFecha { get; set; }

    public int FacturaVentaEstadoId { get; set; }

    public string FacturaVentaNumero { get; set; } = null!;

    public decimal FacturaVentaCantidad { get; set; }

    public string FacturaVentaUnidadMedida { get; set; } = null!;

    public decimal FacturaVentaImporte { get; set; }

    public decimal FacturaVentaDetraccion { get; set; }

    public decimal FacturaVentaPendientePago { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<FacturaVentaPersona> FacturaVentaPersonas { get; set; } = new List<FacturaVentaPersona>();
}
