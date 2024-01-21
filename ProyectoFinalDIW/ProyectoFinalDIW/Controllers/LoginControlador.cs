using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDIW.Controllers
{
    [Controller]
    public class LoginControlador : Controller
    {
        
        public IActionResult Index()
        {
            return View();
            // Tambien podriamos poner:
            // return View("~/Views/Shared/Index.cshtml");
        }
        public IActionResult VistaBienvenida()
        {
            return View();
        }
        
        public IActionResult VistaRecuperarContrasenya()
        {
            return View();
        }
        
        public IActionResult VistaRegister()
        {
            return View();
        }
    }
}
