﻿using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class ServicioTransporte
{
    public int ServicioTransporteId { get; set; }

    public DateOnly ServicioTransporteFecha { get; set; }

    public int CarguilloId { get; set; }

    public decimal ServicioTransportePrecio { get; set; }

    public int ServicioTransporteEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public decimal ServicioTransporteTotal { get; set; }

    public virtual Carguillo Carguillo { get; set; } = null!;

    public virtual ICollection<ServicioTransporteDetalle> ServicioTransporteDetalles { get; set; } = new List<ServicioTransporteDetalle>();

    public virtual RecojoEstado ServicioTransporteEstado { get; set; } = null!;
}