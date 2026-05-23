using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;

namespace NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblStudent> TblStudents { get; set; }
    public virtual DbSet<TblBlog> TblBlogs { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=NLADotNetInternshipTraining;User ID=sa;Password=Linn@81220015228;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblStudent>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.ToTable("Tbl_Student");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FatherName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.StudentName).HasMaxLength(50);
            entity.Property(e => e.StudentNo).HasMaxLength(50);
        });
        modelBuilder.Entity<TblBlog>(entity =>
{
    // Primary Key သတ်မှတ်ခြင်း
    entity.HasKey(e => e.BlogId);

    // Database ထဲက Table နာမည်အမှန်နဲ့ ချိတ်ခြင်း 
    // (အကယ်၍ SQL ထဲမှာ TblBlog လို့ပဲ ပေးခဲ့ရင် "TblBlog" လို့ ပြောင်းရေးပါ)
    entity.ToTable("Tbl_Blog");

    entity.Property(e => e.BlogAuthor).HasMaxLength(100);
    entity.Property(e => e.BlogTitle).HasMaxLength(250);
});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
