using System;
using System.Collections.Generic;

namespace MedWebApplication;

public partial class Ward
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();
}
