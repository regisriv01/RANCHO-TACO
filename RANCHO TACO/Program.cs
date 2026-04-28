using Microsoft.EntityFrameworkCore;
using RanchoTaco.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Servicios MVC
builder.Services.AddControllersWithViews();

// 🔥 Base de datos PostgreSQL (Render)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 🔹 Sesión (para login y carrito)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 🔹 Manejo de errores
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

// 🔥 Crear DB automáticamente (compatible con PostgreSQL)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // 👉 Usa Migrate en PostgreSQL (no EnsureCreated)
    db.Database.Migrate();
}

app.Run();