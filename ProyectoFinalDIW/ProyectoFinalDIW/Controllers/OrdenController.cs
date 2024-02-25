using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
	/// <summary>
	/// Controlador para gestionar la compra de un carrito
	/// </summary>
	/// autor: Fran gallego
	public class OrdenController : Controller
	{
		// Inicializamos la interfaz de Orden
		private OrdenInterfaz ordenInterfaz = new OrdenImplementacion();

		/// <summary>
		/// Método para comprar los carritos del usuario
		/// </summary>
		/// <returns>Devuelve una redireccion</returns>
		public ActionResult ComprarCarrito()
		{
			try
			{
                // Log
                Util.LogInfo("OrdenController", "ComprarCarrito", "Ha entrado en ComprarCarrito");

                // Control de sesion
                if (!Util.ControlaSesion(HttpContext))
                    return RedirectToAction("VistaLogin", "Login");

                // Realizamos la compra de los carritos del usuario
                bool ok = ordenInterfaz.ComprarCarritoUsuario(HttpContext.Session.GetString("email"));

                // Controlamos si se ha realizado correctamente o no
                if (ok)
                    TempData["carritoComprado"] = "Se ha realizado la compra correctamente!";
                else
                    TempData["carritoCancel"] = "Se ha cancelado la compra!";

                // Redirigimos a la vista carrito
                return RedirectToAction("VistaCarrito", "Carrito");
            }
			catch (Exception)
			{
                // Log
                Util.LogError("OrdenController", "ComprarCarrito", "Se ha producido un error");

                // Redirigimos a la vista de error
                return RedirectToAction("VistaError", "Error");
            }
		}
	}
}
