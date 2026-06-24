using NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace NLADotNetInternshipTraining.MvcDynamicRbacWebapi.Features.Auth;

public class AuthService
{
    private readonly RbacdbContext _context;

    public AuthService(RbacdbContext context)
    {
        _context = context;
    }

    public async Task<(AppUser? User, string? RoleName)> LoginAsync(AuthRequest request)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x =>
            x.Username == request.Username &&
            x.Password == request.Password);

        if (user == null) return (null, null);

        var role = await _context.TblRoles.FirstOrDefaultAsync(r => r.Id == user.RoleId);

        return (user, role?.RoleName);
    }

    public RbacdbContext GetContext() => _context;
}
