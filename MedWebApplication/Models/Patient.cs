using System;
using System.Collections.Generic;

namespace MedWebApplication;

public partial class Patient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GenderId { get; set; }

    public DateTime BirthDate { get; set; }

    public byte BloodGroupId { get; set; }

    public string Address { get; set; } = null!;

    public int PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string? AnyMajorDiseaseSufferedEarlier { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual BloodGroup BloodGroup { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;
}
