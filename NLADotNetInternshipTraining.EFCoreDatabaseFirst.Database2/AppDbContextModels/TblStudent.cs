using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database2.AppDbContextModels;

[Table("Tbl_Student")]
public partial class TblStudent
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(50)]
    public string StudentNo { get; set; } = null!;

    [StringLength(50)]
    public string StudentName { get; set; } = null!;

    [StringLength(50)]
    public string FatherName { get; set; } = null!;

    public string Address { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    public bool IsDelete { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDateTime { get; set; }

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDateTime { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }
}
