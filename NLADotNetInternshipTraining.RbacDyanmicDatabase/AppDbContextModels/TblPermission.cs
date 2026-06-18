using System;
using System.Collections.Generic;

namespace NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

public partial class TblPermission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
