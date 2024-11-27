using Microsoft.EntityFrameworkCore;
using Loboteca.Models;
using Loboteca.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura la conexión a la base de datos
builder.Services.AddDbContext<LobotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"))
);

// Agrega servicios HTTP para APIs externas
builder.Services.AddHttpClient<MediaWikiService>();
builder.Services.AddHttpClient<GoogleBooksService>();

// Habilitar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión (30 minutos)
    options.Cookie.HttpOnly = true; // Seguridad
    options.Cookie.IsEssential = true; // Requerido para cumplimiento de GDPR
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Habilitar middleware de sesiones
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Inicio}/{id?}");

app.Run();
