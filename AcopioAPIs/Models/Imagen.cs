using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Imagen
{
    public int ImagenId { get; set; }

    public int ReferenciaId { get; set; }

    public string TipoReferencia { get; set; } = null!;

    public string ImagenUrl { get; set; } = null!;

    public string ImagenComentario { get; set; } = null!;

    public bool? ImagenStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }
}
