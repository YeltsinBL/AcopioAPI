using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Action
{
    public int ActionId { get; set; }

    public string ModuleName { get; set; } = null!;

    public int? ModuleId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Module? Module { get; set; }
}
