using Microsoft.EntityFrameworkCore;
using Npgsql;
using RanchoTaco.Data;
using RANCHO_TACO.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EmailService>();
builder.Services.AddControllersWithViews();

// =============================
// 🔥 CONEXIÓN AUTO (Render + Local)
// =============================
string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string? appsettingsConn = builder.Configuration.GetConnectionString("DefaultConnection");

string connectionString;

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    // 👉 Render (o local si defines DATABASE_URL)
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    connectionString =
        $"Host={uri.Host};" +
        $"Port={uri.Port};" +
        $"Database={uri.AbsolutePath.TrimStart('/')};" +
        $"Username={userInfo[0]};" +
        $"Password={userInfo[1]};" +
        $"SSL Mode=Require;" +
        $"Trust Server Certificate=true";
}
else if (!string.IsNullOrWhiteSpace(appsettingsConn))
{
    // 👉 Local con appsettings.json
    connectionString = appsettingsConn;
}
else
{
    throw new Exception("No hay cadena de conexión. Define DATABASE_URL o DefaultConnection.");
}

// (Opcional) Log para verificar
Console.WriteLine("DB Host: " + new NpgsqlConnectionStringBuilder(connectionString).Host);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// =============================
// 🔹 SESIÓN
// =============================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// =============================
// 🔹 MIDDLEWARE
// =============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// =============================
// 🔹 RUTAS
// =============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// =============================
// 🔥 MIGRACIONES AUTOMÁTICAS
// =============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();