using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class PerfilController : Controller
    {
        // Inicializamos la interfaz de Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        public IActionResult VistaPerfil()
        {
            // Control de sesión
            bool ok = Util.ControlaSesion(HttpContext);

            if (!ok)
                return RedirectToAction("VistaLogin", "Acceso");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos el email
            string email = HttpContext.Session.GetString("email")!;

            // Obtenemos el usuario por el email
            UsuarioDTO usuarioEncontrado = usuarioInterfaz.BuscaUsuarioPorEmail(email).Result;

            // Devolvemos la vista con el usuario
            return View(usuarioEncontrado);
        }

        public IActionResult CerrarSesion()
        {
            // Control de sesión
            bool ok = Util.ControlaSesion(HttpContext);

            if (!ok)
                return RedirectToAction("VistaLogin", "Login");

            HttpContext.Session.SetString("acceso", "0");

            TempData["sesionCerrada"] = "Se ha cerrado sesión con éxito!!";
            return RedirectToAction("VistaLogin", "Login");
        }
    }
}
