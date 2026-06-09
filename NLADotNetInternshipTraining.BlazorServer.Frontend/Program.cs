using NLADotNetInternshipTraining.BlazorServer.Frontend.Components;
using NLADotNetInternshipTraining.BlazorServer.Frontend.Services;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// builder.Services.AddHttpClient();

// builder.Services.AddScoped<HttpClientService>();

builder.Services.AddHttpClient("BlogApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7196/api/");
});

builder.Services.AddScoped<HttpClientService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
