using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class AdminController : Controller
    {
        // Inicializamos la interfaz Admin
        AdminInterfaz adminInterfaz = new AdminImplementacion();

        public IActionResult VistaAdministracionUsuario()
        {
            // Control de sesión
            if (!ControlaSesion())
            {
                return RedirectToAction("VistaLogin", "AccesoControlador");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los usuarios
            List<UsuarioDTO> listaUsuarios = adminInterfaz.ObtieneTodosLosUsuarios().Result;

            return View(listaUsuarios);
        }

        public IActionResult VistaAdministracionProducto()
        {
            // Control de sesión
            if (!ControlaSesion())
            {
                return RedirectToAction("VistaLogin", "AccesoControlador");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los suplementos
            List<SuplementoDTO> listaSuplementos = adminInterfaz.ObtieneTodosLosSuplementos().Result;

            return View(listaSuplementos);
        }

        // Métodos

        /// <summary>
        /// Método que obtiene el acceso del usuario y devuelve false si ha iniciado sesión o true si no.
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
