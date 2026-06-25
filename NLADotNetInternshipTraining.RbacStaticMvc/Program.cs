using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
// 💡 ကိုကို့ရဲ့ Database Project က Namespace တွေကို ချိတ်ဆက်ခြင်း
using NLADotNetInternshipTraining.Database;
using NLADotNetInternshipTraining.Database.AppDbContextModels;
using NLADotNetInternshipTraining.RbacStaticMvc.Features.Auth;
using NLADotNetInternshipTraining.RbacStaticMvc.Features.Product;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register AppDbContext with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MvcCookieAuthDb"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProductView", policy =>
        policy.RequireClaim("permission", "Product.View"));

    options.AddPolicy("ProductCreate", policy =>
        policy.RequireClaim("permission", "Product.Create"));

    options.AddPolicy("ProductUpdate", policy =>
        policy.RequireClaim("permission", "Product.Update"));

    options.AddPolicy("ProductDelete", policy =>
        policy.RequireClaim("permission", "Product.Delete"));
});

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.TblUsers.Any())
    {
        context.TblUsers.AddRange(
            new AppUser
            {
                Id = 1,
                Username = "admin",
                Password = "123",
                Role = "Admin",
                Permissions = new List<string> { "Product.View", "Product.Create", "Product.Update", "Product.Delete" }
            },
            new AppUser
            {
                Id = 2,
                Username = "staff",
                Password = "123",
                Role = "Staff",
                Permissions = new List<string> { "Product.View" }
            },
                new AppUser
                {
                    Id = 3,
                    Username = "tutor",
                    Password = "123",
                    Role = "tutor",
                    Permissions = new List<string> { "Product.View", "Product.Create" }
                }
        );
        context.SaveChanges();
    }
    var users = context.TblUsers.ToList();
    foreach (var u in users)
    {
        string userPermissions = u.Permissions != null ? string.Join(", ", u.Permissions) : "No Permissions";
        Console.WriteLine($"[InMemory DB] User: {u.Username} | Role: {u.Role} | Permissions: [{userPermissions}]");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();