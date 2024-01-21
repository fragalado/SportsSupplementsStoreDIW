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
        public async Task<ActionResult<Usuario>> GetUsuario(long id)
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

        [HttpPut]
        public async Task<IActionResult> PutUsuario(Usuario usuario)
        {
            context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetUsuario", new { id = usuario.id_usuario }, usuario);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            try
            {
                var usuario = await context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound();
                }

                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{correo}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByCorreo(string correo)
        {
            try
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.email_usuario == correo);

                if(usuario == null)
                {
                    return NotFound();
                }

                return usuario;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
