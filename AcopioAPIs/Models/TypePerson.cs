using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class TypePerson
{
    public int TypePesonId { get; set; }

    public string TypePesonName { get; set; } = null!;

    public bool TypePesonStatus { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
