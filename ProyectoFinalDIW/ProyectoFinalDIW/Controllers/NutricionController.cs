using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
	/// <summary>
	/// Controlador para la vista de nutricon
	/// </summary>
	/// autor: Fran Gallego
    public class NutricionController : Controller
    {
		// Inicializamos la interfaz Carrito para usar sus métodos
		private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();

        // Inicializamos la interfaz Suplemento para usar sus métodos
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

		/// <summary>
		/// Método que devuelve la vista de suplementos
		/// </summary>
		/// <param name="st">Tipo de suplemento a mostrar (1: Proteína; 2: Creatina; 3: Todo)</param>
		/// <returns>Devuelve una vista</returns>
        public IActionResult VistaSuplementos(int st)
        {
			try
			{
                // Log
                Util.LogInfo("NutricionController", "VistaSuplementos", "Ha entrado en VistaSuplementos");

                // Controlamos si el usuario ha iniciado sesion o no
                if (!Util.ControlaSesion(HttpContext))
                {
                    // Redireccionamos a la vista de login
                    return RedirectToAction("VistaLogin", "Login");
                }

                ViewData["acceso"] = HttpContext.Session.GetString("acceso");

                // Obtenemos todos los suplementos
                List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

                // Ahora filtramos la lista por el tipo de suplemento
                if (st == 1)
                {
                    // Proteínas
                    listaSuplementos = listaSuplementos.Where(suplemento => suplemento.Tipo_suplemento == "Proteína").ToList();
                }
                else if (st == 2)
                {
                    // Creatinas
                    listaSuplementos = listaSuplementos.Where(suplemento => suplemento.Tipo_suplemento == "Creatina").ToList();
                }

                // Devolvemos la vista con los suplementos
                return View(listaSuplementos);
            }
			catch (Exception)
			{
                // Log
                Util.LogError("NutricionController", "VistaSuplementos", "Se ha producido un error");

                // Redirigimos a la vista de error
                return RedirectToAction("VistaError", "Error");
            }
        }

		/// <summary>
		/// Método para agregar un suplemento al carrito
		/// </summary>
		/// <param name="id_suplemento">Id del suplemento a agregar</param>
		/// <returns>Devuelve una redireccion</returns>
        public ActionResult AgregaSuplementoCarrito(long id_suplemento)
		{
			try
			{
                // Log
                Util.LogInfo("NutricionController", "AgregaSuplementoCarrito", "Ha entrado en AgregaSuplementoCarrito");

                // Control de sesión
                if (!Util.ControlaSesion(HttpContext))
				{
					return RedirectToAction("VistaLogin", "Login");
				}

				ViewData["acceso"] = HttpContext.Session.GetString("acceso");

				// Obtenemos el email del usuario
				string email = HttpContext.Session.GetString("email")!;

				// Agregamos el suplemento al carrito
				bool agregado = carritoInterfaz.AgregaSuplemento(id_suplemento, email);

				// Controlamos si se ha agregado correctamente o no
				if(agregado)
					TempData["suplementoAgregado"] = "El suplemento se ha agregado al carrito con éxito!";
				else
					TempData["suplementoError"] = "El suplemento no se ha podido agregar al carrito!";

				// Redirigimos a la vista suplementos
				return RedirectToAction("VistaSuplementos");
			}
			catch (Exception)
			{
                // Log
                Util.LogError("NutricionController", "AgregaSuplementoCarrito", "Se ha producido un error");

                TempData["suplementoError"] = "El suplemento no se ha podido agregar al carrito!";
				return RedirectToAction("VistaSuplementos");
			}
		}
	}
}
