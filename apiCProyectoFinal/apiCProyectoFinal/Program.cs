using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de dependencias.
builder.Services.AddControllers(); // Agrega servicios relacionados con controladores MVC.
builder.Services.AddEndpointsApiExplorer(); // Agrega servicios para explorar API (usado con Swagger/OpenAPI).
builder.Services.AddSwaggerGen(); // Agrega servicios para generar la documentaci�n Swagger/OpenAPI.

// Configura los par�metros de conexi�n a la base de datos
builder.Services.AddDbContext<Contexto>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"))
);

// Construir la aplicaci�n web.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redireccionar las solicitudes HTTP a HTTPS.

app.UseAuthorization(); // Habilitar el middleware de autorizaci�n.

app.MapControllers(); // Mapear las rutas de controladores.

app.Run(); // Iniciar la aplicaci�n web.
