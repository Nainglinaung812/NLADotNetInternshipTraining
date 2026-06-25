using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NLADotNetInternshipTraining.DynamicRbacWebApi.Features.Auth;
using NLADotNetInternshipTraining.DynamicRbacWebApi.Features.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtWebApi", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

// ✅ Register RbacdbContext စနစ်တကျ ပြင်ဆင်ခြင်း
builder.Services.AddDbContext<RbacdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        )
    };
});

var app = builder.Build();

// Seed initial data (Database မဖျက်ဘဲ အလုပ်လုပ်မည့် ပုံစံအသစ်)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RbacdbContext>();
    context.Database.EnsureCreated();

    // 1. Roles များကို တစ်ခုချင်းစီ စစ်ဆေးပြီး မရှိမှ ထည့်ခြင်း
    var adminRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
    if (adminRole == null) { adminRole = new TblRole { RoleName = "Admin" }; context.TblRoles.Add(adminRole); }

    var staffRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Staff");
    if (staffRole == null) { staffRole = new TblRole { RoleName = "Staff" }; context.TblRoles.Add(staffRole); }

    var tutorRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Tutor");
    if (tutorRole == null) { tutorRole = new TblRole { RoleName = "Tutor" }; context.TblRoles.Add(tutorRole); }

    await context.SaveChangesAsync(); // ID တွေ ရအောင် အရင် Save ပါတယ်

    // 2. Permissions များကို တစ်ခုချင်းစီ စစ်ဆေးပြီး မရှိမှ ထည့်ခြင်း
    var p1 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.View");
    if (p1 == null) { p1 = new TblPermission { PermissionName = "Product.View" }; context.TblPermissions.Add(p1); }

    var p2 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Create");
    if (p2 == null) { p2 = new TblPermission { PermissionName = "Product.Create" }; context.TblPermissions.Add(p2); }

    var p3 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Update");
    if (p3 == null) { p3 = new TblPermission { PermissionName = "Product.Update" }; context.TblPermissions.Add(p3); }

    var p4 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Delete");
    if (p4 == null) { p4 = new TblPermission { PermissionName = "Product.Delete" }; context.TblPermissions.Add(p4); }

    await context.SaveChangesAsync();

    // 3. Role Permissions များကို တွဲပေးခြင်း (မရှိသေးမှ ထည့်ရန် Helper Method သဘောမျိုး စစ်ပြီး ထည့်ပါမည်)
    void AddRolePermissionIfNotExist(int roleId, int permissionId)
    {
        var exists = context.TblRolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        if (!exists)
        {
            context.TblRolePermissions.Add(new TblRolePermission { RoleId = roleId, PermissionId = permissionId });
        }
    }

    // Admin Permissions
    AddRolePermissionIfNotExist(adminRole.Id, p1.Id);
    AddRolePermissionIfNotExist(adminRole.Id, p2.Id);
    AddRolePermissionIfNotExist(adminRole.Id, p3.Id);
    AddRolePermissionIfNotExist(adminRole.Id, p4.Id);

    // Staff Permissions
    AddRolePermissionIfNotExist(staffRole.Id, p1.Id);
    AddRolePermissionIfNotExist(staffRole.Id, p2.Id);
    AddRolePermissionIfNotExist(staffRole.Id, p3.Id);
    AddRolePermissionIfNotExist(staffRole.Id, p4.Id);

    // Tutor Permissions
        AddRolePermissionIfNotExist(tutorRole.Id, p3.Id);

    AddRolePermissionIfNotExist(tutorRole.Id, p4.Id);

    await context.SaveChangesAsync();

    // 4. AppUsers ကို စစ်ဆေးပြီး ထည့်ခြင်း
    if (!context.AppUsers.Any(u => u.Username == "admin"))
    {
        context.AppUsers.Add(new AppUser { Username = "admin", Password = "123", RoleId = adminRole.Id });
    }
    if (!context.AppUsers.Any(u => u.Username == "staff"))
    {
        context.AppUsers.Add(new AppUser { Username = "staff", Password = "123", RoleId = staffRole.Id });
    }
    if (!context.AppUsers.Any(u => u.Username == "tutor"))
    {
        context.AppUsers.Add(new AppUser { Username = "tutor", Password = "123", RoleId = tutorRole.Id });
    }

    await context.SaveChangesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();