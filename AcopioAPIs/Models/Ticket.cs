﻿using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string TicketIngenio { get; set; } = null!;

    public string TicketViaje { get; set; } = null!;

    public int CarguilloId { get; set; }

    public string TicketChofer { get; set; } = null!;

    public DateOnly TicketFecha { get; set; }

    public int CarguilloDetalleCamionId { get; set; }

    public decimal TicketCamionPeso { get; set; }

    public int CarguilloDetalleVehiculoId { get; set; }

    public decimal TicketVehiculoPeso { get; set; }

    public string TicketUnidadPeso { get; set; } = null!;

    public decimal TicketPesoBruto { get; set; }

    public int TicketEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Carguillo Carguillo { get; set; } = null!;

    public virtual CarguilloDetalle CarguilloDetalleCamion { get; set; } = null!;

    public virtual CarguilloDetalle CarguilloDetalleVehiculo { get; set; } = null!;

    public virtual ICollection<CorteDetalle> CorteDetalles { get; set; } = new List<CorteDetalle>();

    public virtual TicketEstado TicketEstado { get; set; } = null!;
}