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
        /// <param name="id">Id del suplemento en la base de datos</param>
        /// <returns>Devuelve un suplemento</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Suplemento>> GetSuplemento(long id)
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

        [HttpPut]
        public async Task<IActionResult> PutSuplemento(Suplemento suplemento)
        {
            context.Entry(suplemento).State = EntityState.Modified;

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
        public async Task<ActionResult<Suplemento>> PostSuplemento(Suplemento suplemento)
        {
            try
            {
                context.Suplementos.Add(suplemento);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetSuplemento", new { id = suplemento.id_suplemento }, suplemento);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuplemento(long id)
        {
            try
            {
                var suplemento = await context.Suplementos.FindAsync(id);

                if (suplemento == null)
                {
                    return NotFound();
                }

                context.Suplementos.Remove(suplemento);
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
