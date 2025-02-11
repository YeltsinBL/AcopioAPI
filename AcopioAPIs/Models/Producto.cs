using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string ProductoNombre { get; set; } = null!;

    public int ProductoCantidad { get; set; }

    public decimal ProductoPrecioVenta { get; set; }

    public bool ProductoStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public int? ProductoTipoId { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual ProductoTipo? ProductoTipo { get; set; }

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
