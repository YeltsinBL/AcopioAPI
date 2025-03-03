﻿using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class VentaDetallePago
{
    public int VentaDetallePagoId { get; set; }

    public int VentaId { get; set; }

    public DateOnly VentaDetallePagoFecha { get; set; }

    public bool? VentaDetallePagoEfectivo { get; set; }

    public string? VentaDetallePagoBanco { get; set; }

    public string? VentaDetallePagoCtaCte { get; set; }

    public decimal VentaDetallePagoPagado { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? ImagenUrl { get; set; }

    public string? ImagenComentario { get; set; }

    public bool? VentaDetallePagoStatus { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Ventum Venta { get; set; } = null!;
}
