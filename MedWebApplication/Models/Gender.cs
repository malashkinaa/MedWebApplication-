using System;
using System.Collections.Generic;

namespace MedWebApplication;

public partial class Gender
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Patient> Patients { get; } = new List<Patient>();
}
