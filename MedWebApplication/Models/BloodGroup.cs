using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedWebApplication;

public partial class BloodGroup
{
    public byte Id { get; set; }

	[Display(Name = "Група крові")]
	public string? Name { get; set; }

    public virtual ICollection<Patient> Patients { get; } = new List<Patient>();
}
