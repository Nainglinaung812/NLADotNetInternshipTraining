using Microsoft.EntityFrameworkCore; // Fixes DbContext, DbContextOptionsBuilder, and DbSet
using System.ComponentModel.DataAnnotations; // Fixes [Key]
using System.ComponentModel.DataAnnotations.Schema;
namespace NLADotNetInternshipTraining.EFCoreModelFirstSample;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=NLADotNetInternshipTraining;User ID=sa;Password=Linn@81220015228;TrustServerCertificate=True;");
        }
    }
    public DbSet<Student> Students { get; set; } = null!;
}
[Table("Tbl_Student")]
public class Student
{
    [Key]
    public int StudentId { get; set; }
    public string StudentNo { get; set; } = null!;
    public string StudentName { get; set; } = null!;
    public string FatherName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedDateTime { get; set; }
    public string? ModifiedBy { get; set; }

}
