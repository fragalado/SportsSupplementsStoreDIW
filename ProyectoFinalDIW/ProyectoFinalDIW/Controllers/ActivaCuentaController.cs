using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class ActivaCuentaController : Controller
    {
        // Inicializamos la interfaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        // Inicializamos la interfaz Token para usar sus métodos
        private TokenInterfaz tokenInterfaz = new TokenImplementacion();

        public IActionResult VistaConfirmaEmail()
        {
            bool ok = Util.ControlaSesion(HttpContext);

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ConfirmaEmail(UsuarioDTO usuario)
        {
            try
            {
                string token = usuario.Nombre_usuario;
                // Obtenemos el token de la base de datos
                TokenDTO tokenDto = tokenInterfaz.ObtenerToken(token).Result;

                // Controlamos si el token es válido o no
                // Será válido si es distinto de null y la fecha límite del token no se ha pasado
                DateTime fecha = DateTime.Now;
                if (tokenDto == null)
                {
                    TempData["Mensaje"] = "El token no existe!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = token });
                }
                else if (tokenDto.Fch_fin_token < fecha)
                {
                    TempData["Mensaje"] = "El token ha caducado!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = token });
                }

                // Si llega aqui es porque el token es valido
                // Luego llamamos al método activar cuenta y le pasamos los parámetros
                bool ok = usuarioInterfaz.ActivaCuenta(tokenDto).Result;

                if (ok)
                {
                    // Se ha activado la cuenta correctamente
                    TempData["MensajeCorrecto"] = "Se ha activado la cuenta correctamente!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = token });
                }
                else
                {
                    TempData["Mensaje"] = "Ha ocurrido un error, vuelve a intentarlo más tarde!!";
                    return RedirectToAction("VistaConfirmaEmail", new { tk = token });
                }
            }
            catch (Exception)
            {
                return View("VistaConfirmaEmail");
            }
        }
    }
}
