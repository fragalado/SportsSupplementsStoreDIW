using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Acceso
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoControlador : ControllerBase
    {
        public readonly Contexto context;

        public AccesoControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los accesos de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los accesos de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Acceso>>> GetAccesos()
        {
            // Devolvemos toda la lista de accesos
            return context.Accesos.ToList();
        }

        /// <summary>
        /// Método que obtiene un acceso por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id del acceso en la base de datos</param>
        /// <returns>Devuelve un acceso</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Acceso>> GetAcceso(long id)
        {
            // Obtenemos el acceso por el id
            var acceso = await context.Accesos.FindAsync(id);

            // Comprobamos si el acceso es nulo, si es nulo devolvemos not found
            if (acceso == null)
            {
                return NotFound();
            }

            // Si el acceso no es nulo se devolverá el acceso.
            return acceso;
        }

        [HttpPut]
        public async Task<IActionResult> PutAcceso(Acceso acceso)
        {
            context.Entry(acceso).State = EntityState.Modified;

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
        public async Task<ActionResult<Acceso>> PostAcceso(Acceso acceso)
        {
            try
            {
                context.Accesos.Add(acceso);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetAcceso", new { id = acceso.id_acceso }, acceso);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcceso(long id)
        {
            try
            {
                var acceso = await context.Accesos.FindAsync(id);

                if (acceso == null)
                {
                    return NotFound();
                }

                context.Accesos.Remove(acceso);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
