using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SIPAME.Data;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});

var app = builder.Build();

// Configurar la cultura para que acepte formato dd/MM/yyyy
var supportedCultures = new[] { new CultureInfo("es-ES"), new CultureInfo("es-MX") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-MX"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pagos}/{action=Create}/{id?}");

app.Run();
