using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class LoginController : Controller
    {
        // Inicializamos la intefaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        public IActionResult VistaLogin()
        {
            bool ok = Util.ControlaSesion(HttpContext);

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
            // Tambien podriamos poner:
            // return View("~/Views/Acceso/VistaLogin.cshtml");
        }

        [HttpPost]
        public ActionResult LoginUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Verificamos si el usuario está incompleto
                if (usuario.Psswd_usuario == null || usuario.Email_usuario == null)
                {
                    // ViewData nos permite enviar datos del controlador a la vista
                    ViewData["Mensaje"] = "Tienes que rellenar todos los datos!!";
                    return View("VistaLogin");
                }

                // Si todos los datos están completos hacemos el inicio de sesión
                UsuarioDTO usuarioEncontrado = usuarioInterfaz.LoginUsuario(usuario);

                if (usuarioEncontrado != null && usuarioEncontrado.EstaActivado_usuario)
                {
                    // Se ha realizado el login
                    HttpContext.Session.SetString("email", usuarioEncontrado.Email_usuario);
                    HttpContext.Session.SetString("nombre", usuarioEncontrado.Nombre_usuario);
                    HttpContext.Session.SetString("acceso", usuarioEncontrado.Id_acceso.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else if (usuarioEncontrado != null && !usuarioEncontrado.EstaActivado_usuario)
                {
                    // La cuenta no ha sido activada
                    ViewData["Mensaje"] = "La cuenta no ha sido activada!!";
                    return View("VistaLogin");
                }
                else
                {
                    // El email o la contraseña no coinciden
                    ViewData["Mensaje"] = "El email y/o contraseña no son correctos!!";
                    return View("VistaLogin");
                }
            }
            catch (Exception e)
            {
                // Si llega aquí es porque se ha producido un error al iniciar sesión
                // Luego mostramos mensaje de error y devolvemos la vista de login
                Console.WriteLine("[ERROR-LoginController-LoginUsuario] Error al iniciar sesión");

                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaLogin");
            }
        }
    }
}
