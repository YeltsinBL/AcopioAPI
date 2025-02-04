using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class VentaDetalle
{
    public int VentaDetalleId { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int VentaDetalleCantidad { get; set; }

    public decimal VentaDetallePrecio { get; set; }

    public bool VentaDetalleStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Ventum Venta { get; set; } = null!;
}
