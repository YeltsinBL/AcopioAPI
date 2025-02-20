using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CompraDetalleRecojo
{
    public int CompraDetalleRecojoId { get; set; }

    public int CompraId { get; set; }

    public int ProductoId { get; set; }

    public int CompraDetallePorRecoger { get; set; }

    public int CompraDetalleRecogidos { get; set; }

    public int CompraDetallePendientes { get; set; }

    public bool? CompraDetalleStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public DateOnly? CompraDetalleRecojoFecha { get; set; }

    public string? CompraDetalleRecojoGuia { get; set; }

    public virtual Compra Compra { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
