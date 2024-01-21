using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de dependencias.
builder.Services.AddControllers(); // Agrega servicios relacionados con controladores MVC.
builder.Services.AddEndpointsApiExplorer(); // Agrega servicios para explorar API (usado con Swagger/OpenAPI).
builder.Services.AddSwaggerGen(); // Agrega servicios para generar la documentación Swagger/OpenAPI.

// Configura los parámetros de conexión a la base de datos
builder.Services.AddDbContext<Contexto>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"))
);

// Construir la aplicación web.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redireccionar las solicitudes HTTP a HTTPS.

app.UseAuthorization(); // Habilitar el middleware de autorización.

app.MapControllers(); // Mapear las rutas de controladores.

app.Run(); // Iniciar la aplicación web.
