using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace NLADotNetInternshipTraining.DynamicRbacWebApi.Features.Auth;

public class AuthService
{
    private readonly RbacdbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(RbacdbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest request)
    {
        // 💡 အဆင့် ၁။ Username တူညီသော User ကို ပထမဦးစွာ ဆွဲထုတ်ခြင်း (Case-insensitive)
        var user = await _context.AppUsers.FirstOrDefaultAsync(x =>
            x.Username.ToLower() == request.Username.ToLower());

        if (user is null) return null;

        // 💡 အဆင့် ၂။ ပြင်ဆင်လိုက်သည့်နေရာ: user.Password က string ဖြစ်ပြီး request.Password ကလည်း string ဖြစ်၍ တိုက်ရိုက် စစ်ဆေးလိုက်ပါသည်
        if (user.Password != request.Password)
        {
            return null; // Password မကိုက်ညီပါက ဝင်ခွင့်မပြုပါ
        }

        // 💡 အဆင့် ၃။ ရာထူး (Role Name) ဆွဲထုတ်ခြင်း
        var role = await _context.TblRoles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
        var roleName = role?.RoleName ?? "";

        // 💡 အဆင့် ၄။ Dynamic Permissions များကို ကြားခံ Table များမှတစ်ဆင့် LINQ Join ဆွဲထုတ်ခြင်း
        var permissions = await (from rp in _context.TblRolePermissions
                                 join p in _context.TblPermissions on rp.PermissionId equals p.Id
                                 where rp.RoleId == user.RoleId
                                 select p.PermissionName).ToListAsync();

        // 💡 အဆင့် ၅။ JWT Claims သတ်မှတ်ခြင်း
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
        };

        // Permission တစ်ခုချင်းစီကို Claim ထဲသို့ Dynamic ထည့်သွင်းခြင်း
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        // 💡 အဆင့် ၆။ JWT Token ထုတ်ပေးခြင်း
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")
            ),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            AccessToken = jwt,
            Role = roleName,
            Permissions = permissions
        };
    }
}