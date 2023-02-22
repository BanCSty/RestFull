using Microsoft.EntityFrameworkCore;
using RestFull.Migrations;

var builder = WebApplication.CreateBuilder(args);

//Логирование в консоли
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SetDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //SSL
    app.UseHsts();
}

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ServiceObjects}/{action=Index}/{id?}");

app.Run();
