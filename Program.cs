using ASPNetCoreApp.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddRazorPages();

// Añadido manualmente. Registrar servicios necesarios para el mapeo de controladores. Agrega soporte para MVC.
builder.Services.AddControllersWithViews(); // Registra servicios que permiten manejar tanto controladores (API) como vistas (MVC)
// builder.Services.AddControllers(); // Registra servicios que permiten manejar solamente controladores que responden a solicitudes de API (RESTful)

// Añadido manualmente. Contexto de la conexión a la base de datos world.
builder.Services.AddDbContext<WorldContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("WorldConnection"), new MySqlServerVersion(new Version(8, 0, 0))));

// Añadido manualmente. Agregar servicios de Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger en modo desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1"));
}

// Añadido manualmente. Habilita el middleware para métodos HTTP sobrescritos (PUT, DELETE).
app.UseHttpMethodOverride();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

/* needs service builder.Services.AddRazorPages();
app.MapRazorPages()
   .WithStaticAssets();
*/

// Añadido manualmente.
app.MapControllers(); // Esto es importante para las API

app.Run();
