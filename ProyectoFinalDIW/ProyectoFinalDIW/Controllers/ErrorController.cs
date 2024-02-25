using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para la vista de errores
    /// </summary>
    /// autor: Fran Gallego
    public class ErrorController : Controller
    {
        /// <summary>
        /// Método que devuelve la vista de errores
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaError()
        {
            ViewData["acceso"] = HttpContext.Session.GetString("acceso");
            return View();
        }
    }
}
