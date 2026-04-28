using Microsoft.EntityFrameworkCore;
using RanchoTaco.Data;


var builder = WebApplication.CreateBuilder(args);

// 🔹 Servicios MVC
builder.Services.AddControllersWithViews();

// 🔹 Base de datos SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Sesión (para login y carrito)
builder.Services.AddSession();

var app = builder.Build();

// 🔹 Manejo de errores (esto ya viene por defecto)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔹 Activar sesión
app.UseSession();

// 🔹 Rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();