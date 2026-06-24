using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLADotNetInternshipTraining.MvcDynamicRbacWebapi.Features.Auth;
using NLADotNetInternshipTraining.MvcDynamicRbacWebapi.Features.Product;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.RbacDyanmicDatabase.AppDbContextModels;

var builder = WebApplication.CreateBuilder(args);

// ✅ ၁။ MVC အတွက် Controllers With Views ကို ပြောင်းလဲပြင်ဆင်ခြင်း
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

// ✅ ၂။ Register RbacdbContext 
builder.Services.AddDbContext<RbacdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ ၃။ Authentication စနစ်ကို JWT မှ MVC အတွက် သင့်တော်သော Cookie Authentication သို့ ပြောင်းလဲခြင်း
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";           // Login စာမျက်နှာ လမ်းကြောင်း
        options.AccessDeniedPath = "/Auth/AccessDenied"; // ခွင့်ပြုချက်မရှိရင် သွားမည့် လမ်းကြောင်း
        options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
    });

var app = builder.Build();

// ✅ ၄။ Seed initial data (RbacdbContext ပုံစံအသစ်ဖြင့်)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RbacdbContext>();

    try
    {
        // 1. Roles များကို စစ်ဆေးပြီး ထည့်ခြင်း
        var adminRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
        if (adminRole == null) { adminRole = new TblRole { RoleName = "Admin" }; context.TblRoles.Add(adminRole); }

        var staffRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Staff");
        if (staffRole == null) { staffRole = new TblRole { RoleName = "Staff" }; context.TblRoles.Add(staffRole); }

        var tutorRole = await context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == "Tutor");
        if (tutorRole == null) { tutorRole = new TblRole { RoleName = "Tutor" }; context.TblRoles.Add(tutorRole); }

        await context.SaveChangesAsync();

        // 2. Permissions များကို စစ်ဆေးပြီး ထည့်ခြင်း
        var p1 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.View");
        if (p1 == null) { p1 = new TblPermission { PermissionName = "Product.View" }; context.TblPermissions.Add(p1); }

        var p2 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Create");
        if (p2 == null) { p2 = new TblPermission { PermissionName = "Product.Create" }; context.TblPermissions.Add(p2); }

        var p3 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Update");
        if (p3 == null) { p3 = new TblPermission { PermissionName = "Product.Update" }; context.TblPermissions.Add(p3); }

        var p4 = await context.TblPermissions.FirstOrDefaultAsync(p => p.PermissionName == "Product.Delete");
        if (p4 == null) { p4 = new TblPermission { PermissionName = "Product.Delete" }; context.TblPermissions.Add(p4); }

        await context.SaveChangesAsync();

        // 3. Role Permissions များကို စစ်ဆေးပြီး တွဲပေးခြင်း
        void AddRolePermissionIfNotExist(int roleId, int permissionId)
        {
            var exists = context.TblRolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (!exists)
            {
                context.TblRolePermissions.Add(new TblRolePermission { RoleId = roleId, PermissionId = permissionId });
            }
        }

        AddRolePermissionIfNotExist(adminRole.Id, p1.Id);
        AddRolePermissionIfNotExist(adminRole.Id, p2.Id);
        AddRolePermissionIfNotExist(adminRole.Id, p3.Id);
        AddRolePermissionIfNotExist(adminRole.Id, p4.Id);

        AddRolePermissionIfNotExist(staffRole.Id, p1.Id);
        AddRolePermissionIfNotExist(staffRole.Id, p2.Id);
        AddRolePermissionIfNotExist(staffRole.Id, p3.Id);
        AddRolePermissionIfNotExist(staffRole.Id, p4.Id);

        AddRolePermissionIfNotExist(tutorRole.Id, p1.Id);

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
    catch (Exception ex)
    {
        Console.WriteLine($"Database Seed Warning: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // CSS, JS ဖိုင်တွေ ပွင့်ဖို့အတွက်

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ✅ ၅။ API Routes မဟုတ်ဘဲ MVC ရဲ့ Default Page View Pattern သို့ ပြောင်းလဲခြင်း
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();