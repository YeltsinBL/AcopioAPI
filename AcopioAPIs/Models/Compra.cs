using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Compra
{
    public int CompraId { get; set; }

    public DateOnly CompraFecha { get; set; }

    public int TipoComprobanteId { get; set; }

    public string CompraNumeroComprobante { get; set; } = null!;

    public int DistribuidorId { get; set; }

    public decimal CompraTotal { get; set; }

    public bool CompraStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual Distribuidor Distribuidor { get; set; } = null!;

    public virtual TipoComprobante TipoComprobante { get; set; } = null!;
}
