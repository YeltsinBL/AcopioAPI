﻿using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Recojo
{
    public int RecojoId { get; set; }

    public DateOnly RecojoFechaInicio { get; set; }

    public DateOnly RecojoFechaFin { get; set; }

    public int RecojoTicketCantidad { get; set; }

    public int RecojoCamionesCantidad { get; set; }

    public decimal RecojoCamionesPrecio { get; set; }

    public int RecojoDiasCantidad { get; set; }

    public decimal RecojoDiasPrecio { get; set; }

    public decimal RecojoTotalPrecio { get; set; }

    public int RecojoEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual RecojoEstado RecojoEstado { get; set; } = null!;
}