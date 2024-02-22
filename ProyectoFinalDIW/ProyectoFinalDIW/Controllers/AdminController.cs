using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDIW.Models;
using ProyectoFinalDIW.Servicios;
using System.Globalization;

namespace ProyectoFinalDIW.Controllers
{
    /// <summary>
    /// Controlador para gestionar la administracion de usuarios y de suplementos
    /// </summary>
    /// autor: Fran Gallego
    public class AdminController : Controller
    {
        // Inicializamos la interfaz Usuario y Suplemento para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        /// <summary>
        /// Método que devuelve la vista de administracion usuarios
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaAdministracionUsuario()
        {
            // Controlamos si el usuario iniciado es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a la vista login
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los usuarios y ordenamos por el acceso del usuario
            List<UsuarioDTO> listaUsuarios = usuarioInterfaz.ObtieneTodosLosUsuarios()
                                                            .Result
                                                            .OrderBy(u => u.Id_acceso == 2 ? 0 : 1)
                                                            .ToList();

            // Devolvemos la vista con la lista de los usuarios
            return View(listaUsuarios);
        }

        /// <summary>
        /// Método que devuelve la vista administracion de suplementos
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaAdministracionProducto()
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a login
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Obtenemos una lista con todos los suplementos
            List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

            // Devolvemos la vista con la lista de suplementos
            return View(listaSuplementos);
        }

        /// <summary>
        /// Método que devuelve la vista de editar usuario
        /// </summary>
        /// <param name="id">Id del usuario a mostrar</param>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaEditarUsuario(int id)
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si el usuario no es admin redirigimos a login
                return RedirectToAction("VistaLogin", "Login");
            }

            // Obtenemos el usuario por el id
            UsuarioDTO usuario = usuarioInterfaz.BuscaUsuarioPorId(id).Result;

            // Si el usuario es null redirigimos a home
            if(usuario == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Devolvemos la vista con el usuario
            return View(usuario);
        }

        /// <summary>
        /// Método que devuelve la vista editar suplemento
        /// </summary>
        /// <param name="id">Id del suplemento a mostrar</param>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaEditarSuplemento(int id)
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a login
                return RedirectToAction("VistaLogin", "Login");
            }

            // Obtenemos el suplemento por el id
            SuplementoDTO suplemento= suplementoInterfaz.BuscaSuplementoPorId(id).Result;

            // Si el suplemento es null redirigimos a home
            if (suplemento == null)
                return RedirectToAction("Index", "Home");

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Devolvemos la vista con el suplemento
            return View(suplemento);
        }

        /// <summary>
        /// Método que devuelve la vista para agregar un suplemento
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaAgregarSuplemento()
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a login
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Creamos un opbjeto suplemento
            SuplementoDTO suplemento = new SuplementoDTO();

            // Devolvemos la vista con el suplemento creado
            return View(suplemento);
        }

        /// <summary>
        /// Método que devuelve la vista para agregar usuario
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        public IActionResult VistaAgregarUsuario()
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a la vista login
                return RedirectToAction("VistaLogin", "Login");
            }

            ViewData["acceso"] = HttpContext.Session.GetString("acceso");

            // Creamos un objeto usuario
            UsuarioDTO usuarioDTO = new UsuarioDTO();

            // Devolvemos la vista con el usuario creado
            return View(usuarioDTO);
        }

        // Métodos

        /// <summary>
        /// Método que realiza el borrado de un usuario
        /// </summary>
        /// <param name="id">Id del usuario a borrar</param>
        /// <returns>Devuelve una redireccion</returns>
        public ActionResult BorrarUsuario(int id)
        {
            // Controlamos si el usuario es admin
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a la vista login
                return RedirectToAction("VistaLogin", "Login");
            }

            // Variable donde guardaremos si se ha borrado el usuario o no
            string esBorrado = "noBorrado"; // Por defecto lo ponemos en noBorrado
            try
            {
                // Borramos el usuario por el id
                bool okBorrado = usuarioInterfaz.BorraUsuarioPorId(id);

                // Controlamos si se ha borrado correctamente
                if (okBorrado)
                    esBorrado = "borrado"; // Si se ha borrado cambiamos el valor de esBorrado
            }
            catch (Exception)
            {
                TempData["error"] = true;
            }
            TempData["mensajeBorrado"] = esBorrado;
            // Redirigimos a la vista administracion usuarios
            return RedirectToAction("VistaAdministracionUsuario");
        }

        /// <summary>
        /// Método que realiza el update de un usuario
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO con los datos del usuario a actualizar</param>
        /// <param name="imagenFile">Objeto IFormFile con los datos del archivo</param>
        /// <returns>Devuelve una redireccion</returns>
        [HttpPost]
        public IActionResult EditarUsuario(UsuarioDTO usuario, IFormFile imagenFile)
        {
            // Controlamos si imagenFile es distinto de null
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

            // Controlamos si se ha actualizado corretamente o no
            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            // Redirigimos a la vista administracion usuario
            return RedirectToAction("VistaAdministracionUsuario", "Admin");
        }

        /// <summary>
        /// Método que borra un suplemento de la base de datos
        /// </summary>
        /// <param name="id">Id del suplemento a borrar</param>
        /// <returns>Devuelve una redireccion</returns>
        public ActionResult BorrarSuplemento(int id)
        {
            // Controlamos si el usuario es admin o no
            if (!Util.ControlaSesionAdmin(HttpContext))
            {
                // Si no es admin redirigimos a la vista login
                return RedirectToAction("VistaLogin", "Login");
            }

            // Variable donde guardaremos si el suplemento es borrado o no
            string esBorrado = "noBorrado";
            try
            {
                // Borramos el suplemento por el id
                bool okBorrado = suplementoInterfaz.BorraSuplementoPorId(id);

                // Controlamos si se ha borrado correctamente
                if (okBorrado)
                    esBorrado = "borrado";
            }
            catch (Exception)
            {
                TempData["error"] = true;
            }
            TempData["mensajeBorrado"] = esBorrado;

            // Redirigimos a la vista administracion productos
            return RedirectToAction("VistaAdministracionProducto");
        }

        /// <summary>
        /// Método para editar un suplemento
        /// </summary>
        /// <param name="suplemento">Objeto SuplementoDTO con los datos del suplemento</param>
        /// <param name="imagenFile">Objeto IFormFile con los datos del archivo</param>
        /// <param name="precioS">String con el precio del suplemento</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditarSuplemento(SuplementoDTO suplemento, IFormFile imagenFile, string precioS)
        {
            // Comprobamos si el precio esta con punto
            // Si esta con punto lo remplazamos con una ','
            if (precioS.Contains("."))
                precioS = precioS.Replace(".", ",");

            // Le asignamos el valor de precioS al precio del suplemento
            suplemento.Precio_suplemento = float.Parse(precioS);

            // Si imagenFile es distinto de null haremos la logica para la imagen del suplemento
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

            // Controlamos si se ha actualizado correctamente o no
            if (ok)
                TempData["mensajeActualizado"] = "true";
            else
                TempData["mensajeActualizado"] = "false";

            // Redirigimos a la vista de administracion de productos.
            return RedirectToAction("VistaAdministracionProducto", "Admin");
        }

        /// <summary>
        /// Método para agregar un suplemento a la base de datos
        /// </summary>
        /// <param name="suplemento">Objeto SuplementoDTO con los datos del suplemento a agregar</param>
        /// <param name="imagenFile">Objeto IFormFile con los datos del archivo</param>
        /// <param name="precioS">String con el precio del suplemento</param>
        /// <returns>Devuelve una redireccion</returns>
        [HttpPost]
        public IActionResult AgregarSuplemento(SuplementoDTO suplemento, IFormFile imagenFile, string precioS)
        {
            // Comprobamos si el precio esta con punto
            // Si esta con punto lo remplazamos con una ','
            if (precioS.Contains("."))
                precioS = precioS.Replace(".", ",");

            // Le asignamos el valor de precioS al precio del suplemento
            suplemento.Precio_suplemento = float.Parse(precioS);

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

            // Controlamos si se ha agregado correctamente o no
            if (ok)
                TempData["mensajeAgregado"] = "true";
            else
                TempData["mensajeAgregado"] = "false";

            // Redireccionamos a la vista administracion productos
            return RedirectToAction("VistaAdministracionProducto", "Admin");
        }

        /// <summary>
        /// Método para agregar un usuario a la base de datos
        /// </summary>
        /// <param name="usuarioDTO">Objeto UsuarioDTO con los datos del usuario a agregar</param>
        /// <param name="imagenFile">Objeto IFormFile con los datos del archivo</param>
        /// <returns>Devuelve una redireccion</returns>
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
            
            // Si el usuario devuelto es distinto de null quiere decir que el email introducido ya existe
            if(usuarioEncontrado != null)
            {
                // Se ha encontrado luego devolvemos mensaje de error 
                TempData["usuarioExiste"] = "true";
                return RedirectToAction("VistaAdministracionUsuario", "Admin");
            }

            // Si no se ha encontrado ningun usuario con el email seguimos:
            // Primero activamos el usuario
            usuarioDTO.EstaActivado_usuario = true;

            // Agregamos el usuario a la base de datos
            bool ok = usuarioInterfaz.AgregaUsuario(usuarioDTO);

            // Controlamos si se ha agregado correctamente o no
            if (ok)
                TempData["mensajeAgregado"] = "true";
            else
                TempData["mensajeAgregado"] = "false";

            // Redirigimos a la vista de administracion de usuarios
            return RedirectToAction("VistaAdministracionUsuario", "Admin");
        }
    }
}
