using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;
using System.Diagnostics;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para la vista home
    /// </summary>
    /// autor: Fran Gallego
    public class HomeController : Controller
    {
        // Inicializamos la interfaz Suplemento para usar sus métodos
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        /// <summary>
        /// Método que devuelve la vista index
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult Index()
        {
            try
            {
                // Log
                Util.LogInfo("HomeController", "Index", "Ha entrado en Index");

                // Controlamos si el usuario ha iniciado sesion o no
                if (!Util.ControlaSesion(HttpContext))
                {
                    // Si no ha iniciado sesion redirigimos a la vista login
                    return RedirectToAction("VistaLogin", "Login");
                }

                ViewData["acceso"] = HttpContext.Session.GetString("acceso");

                // Obtenemos todos los suplementos
                List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

                // Ahora nos vamos a quedar con solo 6 suplementos y lo vamos a devolver con la vista
                if (listaSuplementos.Count > 6)
                    listaSuplementos = listaSuplementos.Take(6).ToList();

                // Devolvemos la vista con los suplementos
                return View(listaSuplementos);
            }
            catch (Exception)
            {
                // Log
                Util.LogError("HomeController", "Index", "Se ha producido un error");

                // Redirigimos a la vista de error
                return RedirectToAction("VistaError", "Error");
            }
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
    }
}
