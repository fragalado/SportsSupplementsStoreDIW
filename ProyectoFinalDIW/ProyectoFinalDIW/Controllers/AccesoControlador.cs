using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    [Controller]
    public class AccesoControlador : Controller
    {
        // Inicializamos la interfaz acceso
        private AccesoInterfaz accesoInferfaz = new AccesoImplementacion();

        public IActionResult VistaLogin()
        {
            bool ok = ControlaSesion();

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
            // Tambien podriamos poner:
            // return View("~/Views/Shared/VistaLogin.cshtml");
        }
        
        public IActionResult VistaRecuperarContrasenya()
        {
            bool ok = ControlaSesion();

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult VistaCambiarContrasenya()
        {
            bool ok = ControlaSesion();

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult VistaRegister()
        {
            bool ok = ControlaSesion();

            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Métodos

        /// <summary>
        /// Método que obtiene el acceso del usuario y devuelve false si ha iniciado sesión o true si no.
        /// </summary>
        /// <returns>Devuelve un bool</returns>
        private bool ControlaSesion()
        {
            // Controla sesion
            try
            {
                string acceso = HttpContext.Session.GetString("acceso");

                if(acceso == "1" || acceso == "2")
                {
                    // El usuario ha iniciado sesión, luego devolvemos true
                    return true;                    
                }

                // Si el usuario no ha iniciado sesión devolvemos false
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

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
                bool? usuarioRegisrado = accesoInferfaz.RegistrarUsuario(usuario);
                if (usuarioRegisrado == true)
                {
                    // Usuario registrado con éxito, redirigimos a la vista de inicio de sesión
                    return View("VistaLogin");
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
                Console.WriteLine("[ERROR-AccesoControlador-RegistrarUsuario] Error al registrar usuario");

                // Si llega aquí es porque se ha producido un error al registrar al usuario
                // Luego mostramos mensaje de error y devolvemos la vista de registro
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaRegister");
            }
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
                UsuarioDTO usuarioEncontrado = accesoInferfaz.LoginUsuario(usuario);

                if(usuarioEncontrado != null)
                {
                    // Se ha realizado el login
                    HttpContext.Session.SetString("email", usuarioEncontrado.Email_usuario);
                    HttpContext.Session.SetString("nombre", usuarioEncontrado.Nombre_usuario);
                    HttpContext.Session.SetString("acceso", usuarioEncontrado.Id_acceso.ToString());

                    return RedirectToAction("Index", "Home");
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
                Console.WriteLine("[ERROR-AccesoControlador-LoginUsuario] Error al iniciar sesión");

                // Si llega aquí es porque se ha producido un error al iniciar sesión
                // Luego mostramos mensaje de error y devolvemos la vista de login
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaLogin");
            }
        }

        [HttpPost]
        public ActionResult RecuperarPassword(UsuarioDTO usuario)
        {
            try
            {
                if (usuario.Email_usuario == null)
                {
                    ViewData["Mensaje"] = "Tienes que introducir un email!!";
                    return View("VistaRecuperarContrasenya");
                }

                // Si llega aqui es porque se ha introducido un email
                // Llamamos al método RecuperaPassword
                bool ok = accesoInferfaz.RecuperaPassword(usuario);

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
                Console.WriteLine("[ERROR-AccesoControlador-RecuperarPassword] Error al recuperar password");

                // Si llega aquí es porque se ha producido un error al recuperar password
                // Luego mostramos mensaje de error y devolvemos la vista de recuperar contrasenya
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaRecuperarContrasenya");
            }
        }
    }
}
