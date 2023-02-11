using System;
using System.Collections.Generic;

namespace MedWebApplication;

public partial class BloodGroup
{
    public byte Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Patient> Patients { get; } = new List<Patient>();
}
