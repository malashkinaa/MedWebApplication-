using System;
using System.Collections.Generic;

namespace MedWebApplication;

public partial class Appointment
{
    public int Id { get; set; }

    public string Symptoms { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string Medicines { get; set; } = null!;

    public int? WardId { get; set; }

    public int PatientId { get; set; }

    public DateTime OnDate { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Ward? Ward { get; set; }
}
