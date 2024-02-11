using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class NutricionController : Controller
    {
		// Inicializamos la interfaz Carrito para usar sus métodos
		private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();

        public IActionResult Index()
        {
            return View();
        }

		public ActionResult AgregaSuplementoCarrito(long id_suplemento)
		{
			try
			{
				// Control de sesión
				bool ok = Util.ControlaSesion(HttpContext);

				if (!ok)
				{
					return RedirectToAction("VistaLogin", "Login");
				}

				ViewData["acceso"] = HttpContext.Session.GetString("acceso");

				// Obtenemos el email del usuario
				string email = HttpContext.Session.GetString("email")!;

				// Agregamos el suplemento
				bool agregado = carritoInterfaz.AgregaSuplemento(id_suplemento, email);

				if(agregado)
					TempData["suplementoAgregado"] = "El suplemento se ha agregado al carrito con éxito!";
				else
					TempData["suplementoError"] = "El suplemento no se ha podido agregar al carrito!";

				// Devolvemos la vista
				return RedirectToAction("VistaSuplementos", "Home");
			}
			catch (Exception)
			{
				TempData["suplementoError"] = "El suplemento no se ha podido agregar al carrito!";
				return RedirectToAction("VistaSuplementos", "Home");
			}
		}
	}
}
