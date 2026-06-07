using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.HospitalAppointmentSystem.Database.Models;

public partial class HospitalAppointmentDbContext : DbContext
{
    public HospitalAppointmentDbContext()
    {
    }

    public HospitalAppointmentDbContext(DbContextOptions<HospitalAppointmentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC07AFDDC0AC");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Docto__44FF419A");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Patie__45F365D3");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Doctors__3214EC07E000D9B3");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Patients__3214EC07B1A13C80");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
