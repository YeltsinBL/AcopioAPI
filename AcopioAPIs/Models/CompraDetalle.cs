using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CompraDetalle
{
    public int CompraDetalleId { get; set; }

    public int CompraId { get; set; }

    public int ProductoId { get; set; }

    public int CompraDetalleCantidad { get; set; }

    public decimal CompraDetallePrecio { get; set; }

    public bool? CompraDetalleStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Compra Compra { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
