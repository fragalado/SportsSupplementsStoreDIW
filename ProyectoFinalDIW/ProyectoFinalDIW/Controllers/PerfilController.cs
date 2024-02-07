using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult VistaPerfil()
        {
            // Control de sesión
            bool ok = Util.ControlaSesion(HttpContext);

            if (!ok)
                return RedirectToAction("VistaLogin", "Acceso");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos el usuario por el email
            string emal = HttpContext.Session.GetString("email");



            return View();
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
