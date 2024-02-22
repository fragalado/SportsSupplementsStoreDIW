using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para gestionar el inicio de sesión
    /// </summary>
    /// autor: Fran Gallego
    public class LoginController : Controller
    {
        // Inicializamos la intefaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        /// <summary>
        /// Método para mostrar la vista de login
        /// </summary>
        /// <returns>Devuelve la vista</returns>
        public IActionResult VistaLogin()
        {
            // Verificamos si el usuario ya ha iniciado sesión
            if (Util.ControlaSesion(HttpContext))
            {
                // Si ha iniciado sesión redirigimos a Home
                return RedirectToAction("Index", "Home");
            }

            // Devolvemos la vista
            return View();
            // Tambien podriamos poner:
            // return View("~/Views/Login/VistaLogin.cshtml");
        }

        /// <summary>
        /// Método para realizar el inicio de sesión
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO con los datos del usuario</param>
        /// <returns>Devuelve la vista de login en caso de error o una redirección a Home</returns>
        [HttpPost]
        public ActionResult LoginUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Verificamos si el usuario está incompleto
                if (usuario.Psswd_usuario == null || usuario.Email_usuario == null)
                {
                    // ViewData nos permite enviar datos del controlador a la vista
                    ViewData["Mensaje"] = "Tienes que rellenar todos los datos!!";
                    return View("VistaLogin");
                }

                // Si todos los datos están completos hacemos el inicio de sesión
                UsuarioDTO usuarioEncontrado = usuarioInterfaz.LoginUsuario(usuario);

                // Si el usuario devuelto es distinto de null y su propiedad estaActivado_usuario es true
                // se ha realizado el inicio de sesión correctamente
                if (usuarioEncontrado != null && usuarioEncontrado.EstaActivado_usuario)
                {
                    // Se ha realizado el login
                    HttpContext.Session.SetString("email", usuarioEncontrado.Email_usuario);
                    HttpContext.Session.SetString("acceso", usuarioEncontrado.Id_acceso.ToString());

                    return RedirectToAction("Index", "Home");
                }
                // Si la propiedad estaActivado_usuario es false quiere decir que la cuenta no esta activada
                else if (usuarioEncontrado != null && !usuarioEncontrado.EstaActivado_usuario)
                {
                    // La cuenta no ha sido activada
                    ViewData["Mensaje"] = "La cuenta no ha sido activada!!";
                    return View("VistaLogin");
                }
                // Si no es ninguna de las anteriores quiere decir que el email o la contraseña son incorrectas
                else
                {
                    // El email o la contraseña no coinciden
                    ViewData["Mensaje"] = "El email y/o contraseña no son correctos!!";
                    return View("VistaLogin");
                }
            }
            catch (Exception e)
            {
                // Si llega aquí es porque se ha producido un error al iniciar sesión
                // Luego mostramos mensaje de error y devolvemos la vista de login
                Console.WriteLine("[ERROR-LoginController-LoginUsuario] Error al iniciar sesión");

                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaLogin");
            }
        }
    }
}
