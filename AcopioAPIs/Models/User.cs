using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int UserStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateOnly UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateOnly? UserModifiedAt { get; set; }

    public int? UserPersonId { get; set; }

    public virtual Person? UserPerson { get; set; }
}
