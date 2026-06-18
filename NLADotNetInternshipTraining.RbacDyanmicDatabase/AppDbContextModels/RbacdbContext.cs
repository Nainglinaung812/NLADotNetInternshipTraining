using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

public partial class RbacdbContext : DbContext
{
    public RbacdbContext()
    {
    }

    public RbacdbContext(DbContextOptions<RbacdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__3214EC07A05C086C");

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Username, "UQ__AppUser__536C85E4CAF1E2EF").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUser__RoleId__3E52440B");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Perm__3214EC07875E6CD1");

            entity.ToTable("Tbl_Permission");

            entity.HasIndex(e => e.PermissionName, "UQ__Tbl_Perm__0FFDA3575EDF5DD5").IsUnique();

            entity.Property(e => e.PermissionName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Role__3214EC07196391C9");

            entity.ToTable("Tbl_Role");

            entity.HasIndex(e => e.RoleName, "UQ__Tbl_Role__8A2B6160EA40E972").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Role__3214EC07CDC280E0");

            entity.ToTable("Tbl_RolePermission");

            entity.HasOne(d => d.Permission).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__Tbl_RoleP__Permi__4222D4EF");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Tbl_RoleP__RoleI__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
