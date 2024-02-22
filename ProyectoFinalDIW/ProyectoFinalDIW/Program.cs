// WebApplicationBuilder es el responsable de construir la aplicacion ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para permitir controladores y vistas en la aplicacion
builder.Services.AddControllersWithViews();

// Cache de memoria distribuida que sirve para almacenar informacion de sesion en la memoria
builder.Services.AddDistributedMemoryCache();
// Agrega servicios de sesion para permitir el uso de la sesion en la aplicacion
builder.Services.AddSession();

// Construye la aplicacion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Habilita la redireccion HTTPS, se encarga de redirigir todas las solicitudes HTTP a HTTPS
app.UseHttpsRedirection();
// Habilita el manejo de archivos estaticos, como CSS, JS y imagenes
app.UseStaticFiles();

// Habilita el middleware sesion
app.UseSession();

// Habilita el middleware de enrutamiento
app.UseRouting();

// Habilita el middleware de autoriacion
app.UseAuthorization();

// Configura la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=VistaLogin}/{id?}"); // Hacemos que al iniciar se inicie en Login/VistaLogin

// Inicia la aplicacion
app.Run();
