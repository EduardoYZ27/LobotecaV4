using Microsoft.EntityFrameworkCore;
using Loboteca.Models;
using Loboteca.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LobotecaContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"))

);

builder.Services.AddHttpClient<MediaWikiService>();
builder.Services.AddHttpClient<GoogleBooksService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Inicio}/{id?}");
app.Run();