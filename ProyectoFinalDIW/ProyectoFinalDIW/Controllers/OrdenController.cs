using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
	public class OrdenController : Controller
	{
		// Inicializamos la interfaz de Orden
		private OrdenInterfaz ordenInterfaz = new OrdenImplementacion();

		public ActionResult ComprarCarrito()
		{
			bool ok = ordenInterfaz.ComprarCarritoUsuario(HttpContext.Session.GetString("email"));

            if (ok)
            {
				TempData["carritoComprado"] = "Se ha realizado la compra correctamente!";
            }
			else
			{
				TempData["carritoCancel"] = "Se ha cancelado la compra!";
			}
            return RedirectToAction("VistaCarrito", "Carrito");
		}
	}
}
