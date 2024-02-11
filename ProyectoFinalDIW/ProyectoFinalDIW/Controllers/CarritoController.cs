using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class CarritoController : Controller
    {
        // Inicializamos la interfaz Carrito para usar sus métodos
        private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();

        // Inicializamos la interfaz Suplemento para usar sus métodos
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        public IActionResult VistaCarrito()
        {
            // Control de sesión
            bool ok = Util.ControlaSesion(HttpContext);

            if (!ok)
                return RedirectToAction("VistaLogin", "Acceso");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos el email
            string email = HttpContext.Session.GetString("email")!;

            // Obtenemos el carrito del usuario
            List<CarritoDTO> listaCarrito = carritoInterfaz.ObtieneCarritoUsuario(email).Result;

            // Obtenemos todos los suplementos para pasarlos a la vista
            List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

            // Pasamos las listas a la vista a través de ViewBag
            ViewBag.carritos = listaCarrito;
			ViewBag.suplementos = listaSuplementos;

			// Devolvemos la vista
			return View();
        }
    }
}
