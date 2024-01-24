using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    [Controller]
    public class AccesoController : Controller
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
            // return View("~/Views/Acceso/VistaLogin.cshtml");
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

        public IActionResult VistaConfirmaEmail()
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
        /// Método que obtiene el acceso del usuario y devuelve true si ha iniciado sesión o false si no.
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

                if(usuarioEncontrado != null && usuarioEncontrado.EstaActivado_usuario)
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

        [HttpPost]
        public ActionResult ModificarPassword(UsuarioDTO usuario)
        {
            string token = usuario.Nombre_usuario;
            string password1 = usuario.Psswd_usuario;
            string password2 = usuario.Email_usuario;
            try
            {
                if (token == null || password1 == null || password2 == null)
                {
                    ViewData["Mensaje"] = "Tienes que rellenar todos los datos!!";
                    return View("VistaCambiarContrasenya", new { tk = token });
                } 
                else if(password1 != password2)
                {
                    TempData["Mensaje"] = "Las contraseñas no coinciden!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }

                // Si llega aqui es porque es las contraseñas son iguales
                // Obtenemos el token de la base de datos
                TokenDTO tokenDto = accesoInferfaz.ObtenerToken(token).Result;

                // Controlamos si el token es válido o no
                // Será válido si es distinto de null y la fecha límite del token no se ha pasado
                DateTime fecha = DateTime.Now;
                if (tokenDto == null)
                {
                    TempData["Mensaje"] = "El token no existe!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                } else if (tokenDto.Fch_fin_token < fecha) 
                {
                    TempData["Mensaje"] = "El token ha caducado!!";
                    return RedirectToAction("VistaCambiarContrasenya", new { tk = token });
                }

                // Si llega aqui es porque el token es valido
                // Luego llamamos al método modificar password y le pasamos los parámetros
                bool ok = accesoInferfaz.ModificaPassword(tokenDto, password1).Result;

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
                Console.WriteLine("[ERROR-AccesoControlador-RecuperarPassword] Error al recuperar password");

                // Si llega aquí es porque se ha producido un error al recuperar password
                // Luego mostramos mensaje de error y devolvemos la vista de recuperar contrasenya
                ViewData["Mensaje"] = "Se ha producido un error. Vuelve a intentarlo más tarde!!";
                return View("VistaRecuperarContrasenya");
            }
        }

        [HttpPost]
        public ActionResult ConfirmaEmail(UsuarioDTO usuario)
        {
            try
            {
                string token = usuario.Nombre_usuario;
                // Obtenemos el token de la base de datos
                TokenDTO tokenDto = accesoInferfaz.ObtenerToken(token).Result;

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
                bool ok = accesoInferfaz.ActivaCuenta(tokenDto).Result;

                if (ok)
                {
                    // Se ha modificado la contraseña correctamente
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
