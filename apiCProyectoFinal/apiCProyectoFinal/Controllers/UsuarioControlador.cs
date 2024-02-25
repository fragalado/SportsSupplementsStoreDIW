using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Usuario
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioControlador : ControllerBase
    {
        public readonly Contexto context;

        public UsuarioControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los usuarios de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los usuarios de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Devolvemos toda la lista de usuarios
            return context.Usuarios.ToList();
        }

        /// <summary>
        /// Método que obtiene un usuario por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id del usuario en la base de datos</param>
        /// <returns>Devuelve un usuario</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(long id)
        {
            // Obtenemos el usuario por el id
            var usuario = await context.Usuarios.FindAsync(id);

            // Comprobamos si el usuario es nulo, si es nulo devolvemos not found
            if(usuario == null)
            {
                return NotFound();
            }

            // Si el usuario no es nulo se devolverá el usuario.
            return usuario;
        }

        /// <summary>
        /// Método que actualiza un usuario pasado por parámetros en la base de datos
        /// </summary>
        /// <param name="usuario">Usuario a actualizar</param>
        /// <returns>Devuelve el usuario actualizado</returns>
        [HttpPut]
        public async Task<IActionResult> PutUsuario(Usuario usuario)
        {
            // Marcamos el estado del usuario como modificado para que EntityFramework realice la actualizacion
            context.Entry(usuario).State = EntityState.Modified;

            try
            {
                // Guardamos los cambios en la base de datos
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Devolvemos no content
                return NoContent();
            }

            // Devolvemos operacion exitosa
            return Ok();
        }

        /// <summary>
        /// Método que crea un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="usuario">Usuario a crear</param>
        /// <returns>Devuelve el usuario creado</returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                // Agregamos el nuevo usuario
                context.Usuarios.Add(usuario);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el usuario creado
                return CreatedAtAction("GetUsuarioById", new { id = usuario.id_usuario }, usuario);
            }
            catch (Exception)
            {
                // Devolvemos no content en caso de error
                return NoContent();
            }
        }

        /// <summary>
        /// Método que elimina un usuario de la base de datos por su id
        /// </summary>
        /// <param name="id">Id del usuario a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            try
            {
                // Buscamos el usuario por su id
                var usuario = await context.Usuarios.FindAsync(id);

                // Verificamos si el usuario existe, es distinto de null
                if (usuario == null)
                {
                    return NotFound();
                }

                // Eliminamos el usuario
                context.Usuarios.Remove(usuario);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos no content
                return NoContent();
            }
            catch (Exception)
            {
                // En caso de error
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que obtiene un usuario por su email
        /// </summary>
        /// <param name="correo">Email del usuario a buscar</param>
        /// <returns>Devuelve el usuario encontrado</returns>
        [HttpGet("correo/{correo}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByCorreo(string correo)
        {
            try
            {
                // Buscamos el usuario por su email
                var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.email_usuario == correo);

                // Comprobamos si el usuario existe
                if(usuario == null)
                {
                    return NotFound();
                }

                // Devolvemos el usuario
                return usuario;
            }
            catch (Exception)
            {
                // En caso de error
                return BadRequest();
            }
        }

    }
}
