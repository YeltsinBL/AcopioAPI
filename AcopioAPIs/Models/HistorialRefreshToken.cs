using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class HistorialRefreshToken
{
    public int HistorialTokenId { get; set; }

    public int UserId { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    public bool? EsActivo { get; set; }

    public virtual User User { get; set; } = null!;
}
