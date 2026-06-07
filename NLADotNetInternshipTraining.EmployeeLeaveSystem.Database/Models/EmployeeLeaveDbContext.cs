using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models;

public partial class EmployeeLeaveDbContext : DbContext
{
    public EmployeeLeaveDbContext()
    {
    }

    public EmployeeLeaveDbContext(DbContextOptions<EmployeeLeaveDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveApproval> LeaveApprovals { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC075EBE1808");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LeaveApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveApp__3214EC070C2497FD");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.LeaveRequest).WithMany(p => p.LeaveApprovals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveAppr__Leave__4BAC3F29");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveReq__3214EC0727D9907F");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveRequ__Emplo__44FF419A");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveRequ__Leave__45F365D3");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveTyp__3214EC07F546952F");

            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
