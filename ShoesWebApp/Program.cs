using ShoeClassLibrary.Contexts;
using System.Globalization;
using ShoesClassLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ShoeContext>();
builder.Services.AddScoped<AuthenticationService>();

CultureInfo.DefaultThreadCurrentCulture =
    new("ru-RU") { NumberFormat = { NumberDecimalSeparator = "." } };

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.UseStaticFiles();
app.Run();
