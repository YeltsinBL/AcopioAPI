using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public byte[] UserPassword { get; set; } = null!;

    public bool UserStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public int? UserPersonId { get; set; }

    public bool UserResetPassword { get; set; }

    public byte[]? UserKeySalt { get; set; }

    public string? VerificarToken { get; set; }

    public virtual ICollection<HistorialRefreshToken> HistorialRefreshTokens { get; set; } = new List<HistorialRefreshToken>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    public virtual Person? UserPerson { get; set; }
}
