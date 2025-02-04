using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class TipoComprobante
{
    public int TipoComprobanteId { get; set; }

    public string TipoComprobanteNombre { get; set; } = null!;

    public bool? TipoComprobanteStatus { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
