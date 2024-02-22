using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para gestionar el modificar password
    /// </summary>
    /// autor: Fran Gallego
    public class RestablecerPasswordController : Controller
    {
        // Inicializamos la interfaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        // Inicializamos la interfaz Token para usar sus métodos
        private TokenInterfaz tokenInterfaz = new TokenImplementacion();

        /// <summary>
        /// Método que devuelve la vista para recuperar(modificar) contraseña
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaRecuperarContrasenya()
        {
            // Controlamos la sesion
            if (Util.ControlaSesion(HttpContext))
            {
                // Si el usuario ya ha iniciado sesion redirigimos a la vista home
                return RedirectToAction("Index", "Home");
            }

            // Devolvemos la vista
            return View();
        }

        /// <summary>
        /// Método que devuelve la vista para cambiar contraseña
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaCambiarContrasenya()
        {
            // Controlamos la sesion
            if (Util.ControlaSesion(HttpContext))
            {
                // Si el usuario ya ha iniciado sesion redirigimos a la vista home
                return RedirectToAction("Index", "Home");
            }

            // Devolvemos la vista
            return View();
        }

        /// <summary>
        /// Método que realiza el envio de un correo para cambiar las contraseñas
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO con el email del usuario</param>
        /// <returns>Devuelve una redireccion</returns>
        [HttpPost]
        public ActionResult RecuperarPassword(UsuarioDTO usuario)
        {
            try
            {
                // Controlamos que el email del usuario sea distinto de null
                if (usuario.Email_usuario == null)
                {
                    ViewData["Mensaje"] = "Tienes que introducir un email!!";
                    return View("VistaRecuperarContrasenya");
                }

                // Si llega aqui es porque se ha introducido un email
                // Llamamos al metodo RecuperaPassword que realiza el envio del correo
                bool ok = usuarioInterfaz.RecuperaPassword(usuario);

                // Controlamos si se ha enviado el correo o no
                if (ok)
                {
                    ViewData["MensajeInfo"] = "Se ha enviado un correo para modificar la contraseña.";
                    return View("VistaRecuperarContrasenya");
                }
                else
                {
                    ViewData["Mensaje"] = "El email introducido no existe!!";
                    return View("VistaRecuperarContrasenya");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-RestablecerPasswordController-RecuperarPassword] Error al recuperar password");

                // Si llega aquí es porque se ha producido un error al recuperar password
                // Luego mostramos mensaje de error y devolvemos la vista de recuperar contrasenya
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaRecuperarContrasenya");
            }
        }

        /// <summary>
        /// Método que realiza la modificacion de la contraseña de un usuario
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO con el token y la contraseña</param>
        /// <returns>Devuelve una redireccion o la vista en caso de error</returns>
        [HttpPost]
        public ActionResult ModificarPassword(UsuarioDTO usuario)
        {
            // Guardamos los valores del objeto UsuarioDTO en variables
            string token = usuario.Nombre_usuario;
            string password1 = usuario.Psswd_usuario;
            string password2 = usuario.Email_usuario;

            try
            {
                // Controlamos que los valores sean distintos de null y que las contraseñas coincidan
                if (token == null || password1 == null || password2 == null)
                {
                    ViewData["Mensaje"] = "Tienes que rellenar todos los datos!!";
                    return View("VistaCambiarContrasenya", new { tk = token });
                }
                else if (password1 != password2)
                {
                    TempData["Mensaje"] = "Las contraseñas no coinciden!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }

                // Si llega aqui es porque es las contraseñas son iguales
                // Obtenemos el token de la base de datos
                TokenDTO tokenDto = tokenInterfaz.ObtenerToken(token).Result;

                // Controlamos si el token es válido o no
                // Será válido si es distinto de null y la fecha límite del token no se ha pasado
                DateTime fecha = DateTime.Now;
                if (tokenDto == null)
                {
                    TempData["Mensaje"] = "El token no existe!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }
                else if (tokenDto.Fch_fin_token < fecha)
                {
                    TempData["Mensaje"] = "El token ha caducado!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }

                // Si llega aqui es porque el token es valido
                // Luego llamamos al método modificar password y le pasamos los parámetros
                bool ok = usuarioInterfaz.ModificaPassword(tokenDto, password1);

                if (ok)
                {
                    // Se ha modificado la contraseña correctamente
                    TempData["MensajeCorrecto"] = "La contraseña ha sido actualizada!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }
                else
                {
                    TempData["Mensaje"] = "Ha ocurrido un error, vuelve a intentarlo más tarde!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-RestablecerPasswordController-ModificarPassword] Error al modificar password");

                // Si llega aquí es porque se ha producido un error al modificar password
                // Luego mostramos mensaje de error y devolvemos la vista de recuperar contrasenya
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaCambiarContrasenya", new { tk = token });
            }
        }
    }
}
