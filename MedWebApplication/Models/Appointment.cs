using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedWebApplication;

public partial class Appointment
{
    public int Id { get; set; }

	[Display(Name = "Симптоми")]
	public string Symptoms { get; set; } = null!;

	[Display(Name = "Діагноз")]
	public string Diagnosis { get; set; } = null!;

	[Display(Name = "Прописані ліки")]
	public string Medicines { get; set; } = null!;

    public int? WardId { get; set; }

    public int PatientId { get; set; }

	[Display(Name = "Дата огляду")]
	public DateTime OnDate { get; set; }

	[Display(Name = "Пацієнт")]
	public virtual Patient Patient { get; set; } = null!;

	[Display(Name = "Відділення")]
	public virtual Ward? Ward { get; set; }
}
