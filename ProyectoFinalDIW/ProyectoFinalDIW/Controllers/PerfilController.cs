using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para la vista perfil
    /// </summary>
    /// autor: Fran Gallego
    public class PerfilController : Controller
    {
        // Inicializamos la interfaz de Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        /// <summary>
        /// Método que devuelve la vista perfil
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaPerfil()
        {
            try
            {
                // Log
                Util.LogInfo("PerfilController", "VistaPerfil", "Ha entrado en VistaPerfil");

                // Control de sesión
                if (!Util.ControlaSesion(HttpContext))
                    return RedirectToAction("VistaLogin", "Login");

                ViewData["acceso"] = HttpContext.Session.GetString("acceso");

                // Obtenemos el email
                string email = HttpContext.Session.GetString("email")!;

                // Obtenemos el usuario por el email
                UsuarioDTO usuarioEncontrado = usuarioInterfaz.BuscaUsuarioPorEmail(email).Result;

                // Devolvemos la vista con el usuario
                return View(usuarioEncontrado);
            }
            catch (Exception)
            {
                // Log
                Util.LogError("PerfilController", "VistaPerfil", "Se ha producido un error");

                // Redirigimos a la vista de error
                return RedirectToAction("VistaError", "Error");
            }
        }

        /// <summary>
        /// Método para realizar el cierre de sesion
        /// </summary>
        /// <returns>Devuelve una redireccion</returns>
        public IActionResult CerrarSesion()
        {
            try
            {
                // Log
                Util.LogInfo("PerfilController", "CerrarSesion", "Ha entrado en CerrarSesion");

                // Control de sesión
                if (!Util.ControlaSesion(HttpContext))
                    return RedirectToAction("VistaLogin", "Login");

                // Cambiamos el acceso a 0
                HttpContext.Session.SetString("acceso", "0");

                // Mensaje de cierre de sesion con exito
                TempData["sesionCerrada"] = "Se ha cerrado sesión con éxito!!";

                // Redirigimos a la vista login
                return RedirectToAction("VistaLogin", "Login");
            }
            catch (Exception)
            {
                // Log
                Util.LogError("PerfilController", "CerrarSesion", "Se ha producido un error");

                // Redirigimos a la vista de error
                return RedirectToAction("VistaError", "Error");
            }
        }
    }
}
