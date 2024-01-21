using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Orden
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenControlador : ControllerBase
    {
        public readonly Contexto context;

        public OrdenControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los ordenes de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los ordenes de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrdenes()
        {
            // Devolvemos toda la lista de ordenes
            return context.Ordenes.ToList();
        }

        /// <summary>
        /// Método que obtiene una orden por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id de la orden en la base de datos</param>
        /// <returns>Devuelve una orden</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Orden>> GetOrden(long id)
        {
            // Obtenemos la orden por el id
            var orden = await context.Ordenes.FindAsync(id);

            // Comprobamos si la orden es nula, si es nula devolvemos not found
            if (orden == null)
            {
                return NotFound();
            }

            // Si la orden no es nula se devolverá la orden.
            return orden;
        }

        [HttpPut]
        public async Task<IActionResult> PutOrden(Orden orden)
        {
            context.Entry(orden).State = EntityState.Modified;

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
        public async Task<ActionResult<Orden>> PostOrden(Orden orden)
        {
            try
            {
                context.Ordenes.Add(orden);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetOrden", new { id = orden.id_orden }, orden);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            try
            {
                var orden = await context.Ordenes.FindAsync(id);

                if (orden == null)
                {
                    return NotFound();
                }

                context.Ordenes.Remove(orden);
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
