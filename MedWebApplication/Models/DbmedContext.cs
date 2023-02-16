using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedWebApplication;

public partial class DbmedContext : DbContext
{
    public DbmedContext()
    {
    }

    public DbmedContext(DbContextOptions<DbmedContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<BloodGroup> BloodGroups { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    public virtual DbSet<WardType> WardTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=EDM; Database=DBMed; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PatientDetail");

            entity.ToTable("Appointment");

            entity.Property(e => e.Diagnosis)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.Medicines)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.OnDate).HasColumnType("date");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Symptoms)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.WardId).HasColumnName("WardID");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatientDetail_Patient");

            entity.HasOne(d => d.Ward).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_Appointment_Ward");
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.ToTable("BloodGroup");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Gender");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsFixedLength();
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Client");

            entity.ToTable("Patient");

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.AnyMajorDiseaseSufferedEarlier).HasMaxLength(400);
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.BloodGroupId).HasColumnName("BloodGroupID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.Patients)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Patient");

            //entity.HasOne(d => d.Gender).WithMany(p => p.Patients)
            //    .HasForeignKey(d => d.GenderId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Patient_Gender");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.ToTable("Ward");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<WardType>(entity =>
        {
            entity.ToTable("WardType");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
