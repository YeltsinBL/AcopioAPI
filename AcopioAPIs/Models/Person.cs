using System;
using System.Collections.Generic;

namespace AcopioAPIs.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string PersonDni { get; set; } = null!;

    public string PersonName { get; set; } = null!;

    public string PersonPaternalSurname { get; set; } = null!;

    public string PersonMaternalSurname { get; set; } = null!;

    public bool PersonStatus { get; set; }

    public int? PersonType { get; set; }

    public virtual TypePerson? PersonTypeNavigation { get; set; }

    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
