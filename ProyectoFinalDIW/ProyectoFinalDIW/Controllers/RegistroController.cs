using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para la gestion de registro de sesion
    /// </summary>
    /// autor: Fran Gallego
    public class RegistroController : Controller
    {
        // Inicializamos la interfaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        /// <summary>
        /// Método que devuelve la vista de registro de sesion
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaRegister()
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
        /// Método para realizar el registro de un usuario
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO con los datos del usuario a registrar</param>
        /// <returns>Devuelve una redireccion o una vista en caso de error</returns>
        [HttpPost]
        public ActionResult RegistrarUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Verificamos si el usuario está incompleto
                if (usuario.Nombre_usuario == null || usuario.Psswd_usuario == null || usuario.Email_usuario == null)
                {
                    // ViewData nos permite enviar datos del controlador a la vista
                    ViewData["Mensaje"] = "Tienes que rellenar todos los datos!!";
                    return View("VistaRegister");
                }

                // Intentamos registrar al usuario
                bool? usuarioRegisrado = usuarioInterfaz.RegistrarUsuario(usuario);

                // Controlamos si se ha realizado correctamente o no
                if (usuarioRegisrado == true)
                {
                    // Usuario registrado con éxito, redirigimos a la vista para mostrar mensaje de correo
                    ViewData["MensajeCorrecto"] = "Se ha enviado un correo para que confirmes la cuenta!!";
                    return View("VistaRegister");
                }
                else if (usuarioRegisrado == null)
                {
                    // Si llega aquí es porque se ha producido un error al registrar al usuario
                    // Luego mostramos mensaje de error y devolvemos la vista de registro
                    ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                    return View("VistaRegister");
                }
                else
                {
                    // Si llega aquí es porque el email introducido ya existe
                    // Luego mostramos mensaje de error y devolvemos la vista de registro
                    ViewData["Mensaje"] = "El email introducido ya existe!!";
                    return View("VistaRegister");
                }
            }
            catch (Exception e)
            {
                // Si llega aquí es porque se ha producido un error al registrar al usuario
                // Luego mostramos mensaje de error y devolvemos la vista de registro
                Console.WriteLine("[ERROR-RegistroController-RegistrarUsuario] Error al registrar usuario");

                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaRegister");
            }
        }
    }
}
