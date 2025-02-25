using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string TicketIngenio { get; set; } = null!;

    public string? TicketCampo { get; set; }

    public string TicketViaje { get; set; } = null!;

    public int CarguilloId { get; set; }

    public string? TicketChofer { get; set; }

    public DateOnly TicketFecha { get; set; }

    public int? CarguilloDetalleCamionId { get; set; }

    public decimal TicketCamionPeso { get; set; }

    public int? CarguilloDetalleVehiculoId { get; set; }

    public decimal TicketVehiculoPeso { get; set; }

    public string TicketUnidadPeso { get; set; } = null!;

    public decimal TicketPesoBruto { get; set; }

    public int TicketEstadoId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public bool? EnServicioPalero { get; set; }

    public int? CarguilloPaleroId { get; set; }

    public virtual Carguillo Carguillo { get; set; } = null!;

    public virtual CarguilloDetalle? CarguilloDetalleCamion { get; set; }

    public virtual CarguilloDetalle? CarguilloDetalleVehiculo { get; set; }

    public virtual Carguillo? CarguilloPalero { get; set; }

    public virtual ICollection<CorteDetalle> CorteDetalles { get; set; } = new List<CorteDetalle>();

    public virtual ICollection<LiquidacionTicket> LiquidacionTickets { get; set; } = new List<LiquidacionTicket>();

    public virtual ICollection<ServicioPaleroDetalle> ServicioPaleroDetalles { get; set; } = new List<ServicioPaleroDetalle>();

    public virtual ICollection<ServicioTransporteDetalle> ServicioTransporteDetalles { get; set; } = new List<ServicioTransporteDetalle>();

    public virtual TicketEstado TicketEstado { get; set; } = null!;
}
