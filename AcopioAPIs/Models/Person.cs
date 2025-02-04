using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? PersonDni { get; set; }

    public string PersonName { get; set; } = null!;

    public string PersonPaternalSurname { get; set; } = null!;

    public string PersonMaternalSurname { get; set; } = null!;

    public bool PersonStatus { get; set; }

    public int? PersonType { get; set; }

    public string UserCreatedName { get; set; } = null!;

    public DateTime UserCreatedAt { get; set; }

    public string? UserModifiedName { get; set; }

    public DateTime? UserModifiedAt { get; set; }

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual TypePerson? PersonTypeNavigation { get; set; }

    public virtual ICollection<ProveedorPerson> ProveedorPeople { get; set; } = new List<ProveedorPerson>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
