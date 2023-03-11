using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedWebApplication;

public partial class Patient
{
	public int Id { get; set; }

	[Display(Name = "Ім'я")]
	public string Name { get; set; } = null!;


	[Display(Name = "Стать")]
	public int GenderId { get; set; }

	[DataType(DataType.Date)]
	[Display(Name = "Дата народження")]
	public DateTime BirthDate { get; set; }

	[Display(Name = "Група крові")]
	public byte BloodGroupId { get; set; }

	[Display(Name = "Адреса")]
	public string Address { get; set; } = null!;

	[Display(Name = "Номер телефону")]
	public int PhoneNumber { get; set; }

	[Display(Name = "Пошта")]
	public string Email { get; set; } = null!;

	[Display(Name = "Попередні хвороби")]
	public string? AnyMajorDiseaseSufferedEarlier { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

	[Display(Name = "Група крові")]
	public virtual BloodGroup BloodGroup { get; set; } = null!;

	[Display(Name = "Стать")]
	public virtual Gender Gender { get; set; } = null!;
}
