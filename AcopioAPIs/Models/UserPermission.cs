using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class UserPermission
{
    public int UserPermissionId { get; set; }

    public int? UserId { get; set; }

    public int? ModuleId { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual Module? Module { get; set; }

    public virtual User? User { get; set; }
}
