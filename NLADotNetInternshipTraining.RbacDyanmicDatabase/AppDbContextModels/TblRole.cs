using System;
using System.Collections.Generic;

namespace NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

public partial class TblRole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
