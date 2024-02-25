using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Suplemento
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SuplementoControlador : ControllerBase
    {
        public readonly Contexto context;

        public SuplementoControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los suplementos/productos de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los suplementos de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suplemento>>> GetSuplementos()
        {
            // Devolvemos toda la lista de suplementos
            return context.Suplementos.ToList();
        }

        /// <summary>
        /// Método que obtiene un suplemento por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id del suplemento a buscar</param>
        /// <returns>Devuelve un suplemento</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Suplemento>> GetSuplementoById(long id)
        {
            // Obtenemos el suplemento por el id
            var suplemento = await context.Suplementos.FindAsync(id);

            // Comprobamos si el suplemento es nulo, si es nulo devolvemos not found
            if (suplemento == null)
            {
                return NotFound();
            }

            // Si el suplemento no es nulo se devolverá el suplemento.
            return suplemento;
        }

        /// <summary>
        /// Método que actualiza un suplemento en la base de datos
        /// </summary>
        /// <param name="suplemento">Suplemento a actualizar</param>
        /// <returns>Devuelve el estado</returns>
        [HttpPut]
        public async Task<IActionResult> PutSuplemento(Suplemento suplemento)
        {
            // Marcamos el estado del suplemento como modificado para que EntityFramework realice la actualizacion
            context.Entry(suplemento).State = EntityState.Modified;

            try
            {
                // Guardamos los cambios
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }

            return NoContent();
        }

        /// <summary>
        /// Método que crea un nuevo suplemento en la base de datos
        /// </summary>
        /// <param name="suplemento">Suplemento a crear</param>
        /// <returns>Devuelve el suplemento creado</returns>
        [HttpPost]
        public async Task<ActionResult<Suplemento>> PostSuplemento(Suplemento suplemento)
        {
            try
            {
                // Agregamos el suplemento
                context.Suplementos.Add(suplemento);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el suplemento creado
                return CreatedAtAction("GetSuplementoById", new { id = suplemento.id_suplemento }, suplemento);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Método que elimina un suplemento en la base de datos por su id
        /// </summary>
        /// <param name="id">Id del suplemento a borrar</param>
        /// <returns>Devuelve el estado</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuplemento(long id)
        {
            try
            {
                // Buscamos el suplemento
                var suplemento = await context.Suplementos.FindAsync(id);

                // Comprobamos si el suplemento existe
                if (suplemento == null)
                {
                    return NotFound();
                }

                // Eliminamos el suplemento
                context.Suplementos.Remove(suplemento);

                // Guardamos los cambios
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
