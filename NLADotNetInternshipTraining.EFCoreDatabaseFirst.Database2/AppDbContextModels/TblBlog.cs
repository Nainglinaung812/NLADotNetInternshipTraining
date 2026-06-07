using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database2.AppDbContextModels;

[Table("Tbl_Blog")]
public partial class TblBlog
{
    [Key]
    public int BlogId { get; set; }

    [StringLength(250)]
    public string BlogTitle { get; set; } = null!;

    [StringLength(100)]
    public string BlogAuthor { get; set; } = null!;

    public string BlogContent { get; set; } = null!;
}
