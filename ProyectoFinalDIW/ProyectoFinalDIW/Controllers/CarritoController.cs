using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para gestionar el carrito del usuario
    /// </summary>
    /// autor: Fran Gallego
    public class CarritoController : Controller
    {
        // Inicializamos la interfaz Carrito para usar sus métodos
        private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();

        // Inicializamos la interfaz Suplemento para usar sus métodos
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        /// <summary>
        /// Método que devuelve la vista de carritos
        /// </summary>
        /// <returns>Devuelve la vista</returns>
        public IActionResult VistaCarrito()
        {
            // Controlamos si el usuario ha iniciado sesion o no
            // Si no ha iniciado sesion redirigimos a la vista de login
            if (!Util.ControlaSesion(HttpContext))
                return RedirectToAction("VistaLogin", "Login");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos el email del usuario iniciado
            string email = HttpContext.Session.GetString("email")!;

            // Obtenemos el carrito del usuario
            List<CarritoDTO> listaCarrito = carritoInterfaz.ObtieneCarritoUsuario(email).Result;

            // Obtenemos todos los suplementos para pasarlos a la vista
            List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

            // Obtenemos el precio total del carrito
            float totalCarrito = carritoInterfaz.ObtienePrecioTotalCarrito(listaCarrito, listaSuplementos);

            // Pasamos las listas a la vista a través de ViewBag
            ViewBag.carritos = listaCarrito;
			ViewBag.suplementos = listaSuplementos;

            // Pasamos el precio a la vista
            ViewBag.precioTotal = totalCarrito;

			// Devolvemos la vista
			return View();
        }

        /// <summary>
        /// Método para borrar un carrito
        /// </summary>
        /// <param name="id">Id del carrito a borrar</param>
        /// <returns>Devuelve una redireccion</returns>
        public ActionResult BorrarCarrito(int id)
        {
            // Borramos el carrito
            bool ok = carritoInterfaz.BorraCarrito(id);

            // Controlamos si se ha borrado correctamente o no
            if (ok)
                TempData["carritoBorrado"] = "true";
            else
                TempData["carritoBorrado"] = "false";

            // Redirigimos a la vista carrito
            return RedirectToAction("VistaCarrito", "Carrito");
        }
    }
}
