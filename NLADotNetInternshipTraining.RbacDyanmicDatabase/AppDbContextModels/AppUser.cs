using System;
using System.Collections.Generic;

namespace NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

public partial class AppUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual TblRole Role { get; set; } = null!;
}
