using System;
using System.Collections.Generic;

namespace NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

public partial class TblRolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual TblPermission Permission { get; set; } = null!;

    public virtual TblRole Role { get; set; } = null!;
}
