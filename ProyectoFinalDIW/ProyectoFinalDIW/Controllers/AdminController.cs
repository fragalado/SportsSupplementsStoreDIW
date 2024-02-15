using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;

namespace ProyectoFinalDIW.Controllers
{
    public class AdminController : Controller
    {
        // Inicializamos la interfaz Usuario y Suplemento para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        public IActionResult VistaAdministracionUsuario()
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los usuarios
            List<UsuarioDTO> listaUsuarios = usuarioInterfaz.ObtieneTodosLosUsuarios().Result;

            // Ordenamos la lista de usuarios
            listaUsuarios = listaUsuarios.OrderBy(u => u.Id_acceso == 2 ? 0 : 1).ToList();

            return View(listaUsuarios);
        }

        public IActionResult VistaAdministracionProducto()
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los suplementos
            List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

            return View(listaSuplementos);
        }

        public IActionResult VistaEditarUsuario(int id)
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            // Obtenemos el usuario por el id
            UsuarioDTO usuario = usuarioInterfaz.BuscaUsuarioPorId(id).Result;

            if(usuario == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            return View(usuario);
        }

        public IActionResult VistaEditarSuplemento(int id)
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            // Obtenemos el suplemento por el id
            SuplementoDTO suplemento= suplementoInterfaz.BuscaSuplementoPorId(id).Result;

            if (suplemento == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            return View(suplemento);
        }

        public IActionResult VistaAgregarSuplemento()
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Creamos un opbjeto suplemento
            SuplementoDTO suplemento = new SuplementoDTO();

            return View(suplemento);
        }

        public IActionResult VistaAgregarUsuario()
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Creamos un objeto usuario
            UsuarioDTO usuarioDTO = new UsuarioDTO();

            return View(usuarioDTO);
        }

        // Métodos
        public ActionResult BorrarUsuario(int id)
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            string esBorrado = "noBorrado";
            try
            {
                Console.WriteLine("Ha entrado en borrar al usuario");
                Console.WriteLine(id);
                bool ok2 = usuarioInterfaz.BorraUsuarioPorId(id);

                if (ok2)
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
            bool ok = usuarioInterfaz.ActualizaUsuario(usuario);

            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            return RedirectToAction("VistaAdministracionUsuario", "Admin");
        }

        public ActionResult BorrarSuplemento(int id)
        {
            // Control de sesión
            bool ok = Util.ControlaSesionAdmin(HttpContext);

            if (!ok)
            {
                return RedirectToAction("VistaLogin", "Login");
            }

            string esBorrado = "noBorrado";
            try
            {
                bool ok2 = suplementoInterfaz.BorraSuplementoPorId(id);

                if (ok2)
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
            bool ok = suplementoInterfaz.ActualizaSuplemento(suplemento);

            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            return RedirectToAction("VistaAdministracionProducto", "Admin");
        }

        [HttpPost]
        public IActionResult AgregarSuplemento(SuplementoDTO suplemento, IFormFile imagenFile)
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

                // Almacena la ruta de la imagen en la entidad Suplementos
                suplemento.RutaImagen_suplemento = "/img/suplementos/" + nombreImagen;
            }

            // Agregamos el suplemento a la base de datos
            bool ok = suplementoInterfaz.AgregaSuplemento(suplemento);

            if (ok)
                TempData["mensajeAgregado"] = "true";
            else
                TempData["mensajeAgregado"] = "false";

            return RedirectToAction("VistaAdministracionProducto", "Admin");
        }
        [HttpPost]
        public IActionResult AgregarUsuario(UsuarioDTO usuarioDTO, IFormFile imagenFile)
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
                usuarioDTO.RutaImagen_usuario = "/img/usuarios/" + nombreImagen;
            }

            // Comprobamos si el usuario existe
            UsuarioDTO usuarioEncontrado = usuarioInterfaz.BuscaUsuarioPorEmail(usuarioDTO.Email_usuario).Result;
            
            if(usuarioEncontrado != null)
            {
                // Se ha encontrado luego devolvemos mensaje de error 
                TempData["usuarioExiste"] = "true";
                return RedirectToAction("VistaAdministracionUsuario", "Admin");
            }

            // Agregamos el usuario a la base de datos
            // Primero activamos el usuario
            usuarioDTO.EstaActivado_usuario = true;
            bool ok = usuarioInterfaz.AgregaUsuario(usuarioDTO);

            if (ok)
                TempData["mensajeAgregado"] = "true";
            else
                TempData["mensajeAgregado"] = "false";

            return RedirectToAction("VistaAdministracionUsuario", "Admin");
        }
    }
}
