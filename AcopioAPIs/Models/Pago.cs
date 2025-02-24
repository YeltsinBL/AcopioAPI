using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int ReferenciaId { get; set; }

    public DateOnly PagoFecha { get; set; }

    public bool? PagoEfectivo { get; set; }

    public string? PagoBanco { get; set; }

    public string? PagoCtaCte { get; set; }

    public decimal PagoPagado { get; set; }

    public string TipoReferencia { get; set; } = null!;

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public bool? PagoStatus { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }
}
