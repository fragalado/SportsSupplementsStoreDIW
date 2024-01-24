using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using System.Diagnostics;

namespace ProyectoFinalDIW.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Control de sesión
            if (!ControlaSesion())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Métodos

        /// <summary>
        /// Método que obtiene el acceso del usuario y devuelve true si ha iniciado sesión o false si no.
        /// </summary>
        /// <returns>Devuelve un bool</returns>
        private bool ControlaSesion()
        {
            // Controla sesion
            try
            {
                string acceso = HttpContext.Session.GetString("acceso");

                if (acceso == "1" || acceso == "2")
                {
                    // El usuario ha iniciado sesión, luego devolvemos true
                    return true;
                }

                // Si el usuario no ha iniciado sesión devolvemos false
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
