using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public string? ModuleIcon { get; set; }

    public string? ModuleColor { get; set; }

    public string? ModuleRoute { get; set; }

    public int? ModulePrimaryId { get; set; }

    public bool ModuleStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<Module> InverseModulePrimary { get; set; } = new List<Module>();

    public virtual Module? ModulePrimary { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
