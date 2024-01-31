using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class AdminController : Controller
    {
        // Inicializamos la interfaz Admin
        AdminInterfaz adminInterfaz = new AdminImplementacion();

        public IActionResult VistaAdministracionUsuario()
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los usuarios
            List<UsuarioDTO> listaUsuarios = adminInterfaz.ObtieneTodosLosUsuarios().Result;

            // Ordenamos la lista de usuarios
            listaUsuarios = listaUsuarios.OrderBy(u => u.Id_acceso == 2 ? 0 : 1).ToList();

            return View(listaUsuarios);
        }

        public IActionResult VistaAdministracionProducto()
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los suplementos
            List<SuplementoDTO> listaSuplementos = adminInterfaz.ObtieneTodosLosSuplementos().Result;

            return View(listaSuplementos);
        }

        public IActionResult VistaEditarUsuario(int id)
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            // Obtenemos el usuario por el id
            UsuarioDTO usuario = adminInterfaz.BuscaUsuarioPorId(id).Result;

            if(usuario == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            return View(usuario);
        }

        public IActionResult VistaEditarSuplemento(int id)
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            // Obtenemos el suplemento por el id
            SuplementoDTO suplemento= adminInterfaz.BuscaSuplementoPorId(id).Result;

            if (suplemento == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            return View(suplemento);
        }

        // Métodos

        /// <summary>
        /// Método que obtiene el acceso del usuario y devuelve true si es admin o false si no.
        /// </summary>
        /// <returns>Devuelve un bool</returns>
        private bool ControlaSesionAdmin()
        {
            // Controla sesion
            try
            {
                string acceso = HttpContext.Session.GetString("acceso");

                if (acceso == "2")
                {
                    // El usuario es admin, luego devolvemos true
                    return true;
                }

                // Si el usuario no es admin devolvemos false
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ActionResult BorrarUsuario(int id)
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            string esBorrado = "noBorrado";
            try
            {
                Console.WriteLine("Ha entrado en borrar al usuario");
                Console.WriteLine(id);
                bool ok = adminInterfaz.BorraUsuarioPorId(id);

                if (ok)
                    esBorrado = "borrado";
            }
            catch (Exception)
            {
                TempData["error"] = true;
            }
            TempData["mensajeBorrado"] = esBorrado;
            return RedirectToAction("VistaAdministracionUsuario");
        }

        [HttpPost]
        public IActionResult EditarUsuario(UsuarioDTO usuario, IFormFile imagenFile)
        {
            if (imagenFile != null && imagenFile.Length > 0)
            {
                // Genera un nombre único para la imagen
                string nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagenFile.FileName);

                // Combina la ruta de la carpeta con el nombre de la imagen
                string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/usuarios", nombreImagen);

                // Guarda la imagen en el sistema de archivos
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    imagenFile.CopyTo(stream);
                }

                // Almacena la ruta de la imagen en la entidad Usuario
                usuario.RutaImagen_usuario = "/img/usuarios/" + nombreImagen;
            }

            // Hacemos un update del usuario a la base de datos
            bool ok = adminInterfaz.ActualizaUsuario(usuario);

            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            return RedirectToAction("VistaAdministracionUsuario", "Admin");
        }

        public ActionResult BorrarSuplemento(int id)
        {
            // Control de sesión
            if (!ControlaSesionAdmin())
            {
                return RedirectToAction("VistaLogin", "Acceso");
            }

            string esBorrado = "noBorrado";
            try
            {
                bool ok = adminInterfaz.BorraSuplementoPorId(id);

                if (ok)
                    esBorrado = "borrado";
            }
            catch (Exception)
            {
                TempData["error"] = true;
            }
            TempData["mensajeBorrado"] = esBorrado;
            return RedirectToAction("VistaAdministracionProducto");
        }

        [HttpPost]
        public IActionResult EditarSuplemento(SuplementoDTO suplemento, IFormFile imagenFile)
        {
            if (imagenFile != null && imagenFile.Length > 0)
            {
                // Genera un nombre único para la imagen
                string nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagenFile.FileName);

                // Combina la ruta de la carpeta con el nombre de la imagen
                string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/suplementos", nombreImagen);

                // Guarda la imagen en el sistema de archivos
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    imagenFile.CopyTo(stream);
                }

                // Almacena la ruta de la imagen en la entidad Usuario
                suplemento.RutaImagen_suplemento = "/img/suplementos/" + nombreImagen;
            }

            // Hacemos un update del suplemento a la base de datos
            bool ok = adminInterfaz.ActualizaSuplemento(suplemento);

            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            return RedirectToAction("VistaAdministracionProducto", "Admin");
        }
    }
}
