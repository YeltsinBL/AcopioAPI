using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class TicketHistorial
{
    public int HistorialId { get; set; }

    public int TicketId { get; set; }

    public string TicketIngenio { get; set; } = null!;

    public string? TicketCampo { get; set; }

    public string TicketViaje { get; set; } = null!;

    public int CarguilloId { get; set; }

    public string? TicketChofer { get; set; }

    public DateOnly TicketFecha { get; set; }

    public int CarguilloDetalleCamionId { get; set; }

    public decimal TicketCamionPeso { get; set; }

    public int CarguilloDetalleVehiculoId { get; set; }

    public decimal TicketVehiculoPeso { get; set; }

    public string TicketUnidadPeso { get; set; } = null!;

    public decimal TicketPesoBruto { get; set; }

    public int TicketEstadoId { get; set; }

    public string UserModifiedName { get; set; } = null!;

    public DateTime UserModifiedAt { get; set; }
}
