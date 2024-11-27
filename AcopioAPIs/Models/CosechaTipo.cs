using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CosechaTipo
{
    public int CosechaTipoId { get; set; }

    public string CosechaTipoDescripcion { get; set; } = null!;

    public bool CosechaTipoStatus { get; set; }

    public virtual ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();
}
