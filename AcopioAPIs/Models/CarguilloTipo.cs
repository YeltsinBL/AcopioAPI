using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class CarguilloTipo
{
    public int CarguilloTipoId { get; set; }

    public string CarguilloTipoDescripcion { get; set; } = null!;

    public bool CarguilloTipoEstado { get; set; }

    public bool IsCarguillo { get; set; }

    public virtual ICollection<CarguilloDetalle> CarguilloDetalles { get; set; } = new List<CarguilloDetalle>();

    public virtual ICollection<Carguillo> Carguillos { get; set; } = new List<Carguillo>();
}
