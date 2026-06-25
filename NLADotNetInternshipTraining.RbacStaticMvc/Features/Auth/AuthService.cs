// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using NLADotNetInternshipTraining.Database;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.Database.AppDbContextModels; 
using NLADotNetInternshipTraining.Database;
// using Microsoft.IdentityModel.Tokens;
// using NLADotNetInternshipTraining.Database.AppDbContextModels;
namespace NLADotNetInternshipTraining.RbacStaticMvc.Features.Auth;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> LoginAsync(AuthRequest request)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x =>
            x.Username == request.Username &&
            x.Password == request.Password);

        return user;
    }
}