using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models;

public partial class LeaveApproval
{
    [Key]
    public int Id { get; set; }

    public int LeaveRequestId { get; set; }

    [StringLength(100)]
    public string ApprovedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ActionDate { get; set; }

    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDateTime { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDateTime { get; set; }

    [StringLength(100)]
    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("LeaveRequestId")]
    [InverseProperty("LeaveApprovals")]
    public virtual LeaveRequest LeaveRequest { get; set; } = null!;
}
