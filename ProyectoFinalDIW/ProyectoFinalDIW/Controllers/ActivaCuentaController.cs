using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador que gestiona el activar cuenta
    /// </summary>
    /// autor: Fran Gallego
    public class ActivaCuentaController : Controller
    {
        // Inicializamos la interfaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        // Inicializamos la interfaz Token para usar sus métodos
        private TokenInterfaz tokenInterfaz = new TokenImplementacion();

        /// <summary>
        /// Método para mostrar la vista de confirmacion de email
        /// </summary>
        /// <returns>Devuelve la vista</returns>
        public IActionResult VistaConfirmaEmail()
        {
            // Controlamos la sesion, si el usuario ha iniciado sesión redirigimos a Home
            if (Util.ControlaSesion(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            // Si el usuario no ha iniciado sesión devolvemos la vista
            return View();
        }

        /// <summary>
        /// Método que realiza la activacion de una cuenta
        /// </summary>
        /// <param name="tokenForm">Token</param>
        /// <returns>Devuelve una redireccion</returns>
        [HttpPost]
        public ActionResult ConfirmaEmail(String tokenForm)
        {
            try
            {
                // Obtenemos el token de la base de datos
                TokenDTO tokenDto = tokenInterfaz.ObtenerToken(tokenForm).Result;

                // Controlamos si el token es válido o no
                // Será válido si es distinto de null y la fecha límite del token no se ha pasado
                // Obtenemos la fecha actual
                DateTime fecha = DateTime.Now;
                if (tokenDto == null)
                {
                    TempData["Mensaje"] = "El token no existe!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = tokenForm });
                }
                else if (tokenDto.Fch_fin_token < fecha)
                {
                    TempData["Mensaje"] = "El token ha caducado!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = tokenForm });
                }

                // Si llega aqui es porque el token es valido
                // Luego llamamos al método activar cuenta y le pasamos los parámetros
                bool ok = usuarioInterfaz.ActivaCuenta(tokenDto);

                // Controlamos si se ha activado la cuenta correctamente
                if (ok)
                {
                    // Se ha activado la cuenta correctamente
                    TempData["MensajeCorrecto"] = "Se ha activado la cuenta correctamente!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = tokenForm });
                }
                else
                {
                    TempData["Mensaje"] = "Ha ocurrido un error, vuelve a intentarlo más tarde!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = tokenForm });
                }
            }
            catch (Exception)
            {
                return View("VistaConfirmaEmail");
            }
        }
    }
}
