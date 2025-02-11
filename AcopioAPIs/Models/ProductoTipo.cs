using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ProductoTipo
{
    public int ProductoTipoId { get; set; }

    public string ProductoTipoDetalle { get; set; } = null!;

    public bool ProductoTipoStatus { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
