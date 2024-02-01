using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;
using System.Diagnostics;

namespace ProyectoFinalDIW.Controllers
{
    public class HomeController : Controller
    {
        // Inicializamos la interfaz Admin para usar el método obtenerTodosLosSuplementos
        AdminInterfaz adminInterfaz = new AdminImplementacion();

        public IActionResult Index()
        {
            // Control de sesión
            if (!ControlaSesion())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos todos los suplementos
            List<SuplementoDTO> listaSuplementos = adminInterfaz.ObtieneTodosLosSuplementos().Result;

            // Ahora nos vamos a quedar con solo 6 suplementos y lo vamos a devolver con la vista
            listaSuplementos = listaSuplementos.Take(6).ToList();
            return View(listaSuplementos);
        }

        public IActionResult VistaSuplementos(int st)
        {
            // Control de sesión
            if (!ControlaSesion())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }
            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos todos los suplementos
            List<SuplementoDTO> listaSuplementos = adminInterfaz.ObtieneTodosLosSuplementos().Result;

            if (st == 1)
            {
                // Proteínas
                listaSuplementos = listaSuplementos.Where(suplemento => suplemento.Tipo_suplemento == "Proteína").ToList();
            }
            else if(st == 2)
            {
                // Creatinas
                listaSuplementos = listaSuplementos.Where(suplemento => suplemento.Tipo_suplemento == "Creatina").ToList();
            }

            // Devolvemos la vista
            return View(listaSuplementos);
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
