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

		/// <summary>
		/// Método que obtiene una orden por el id del usuario pasado por parámetros y la fecha de la orden pasada por parametros
		/// </summary>
		/// <param name="id_usuario">Id del usuario de la orden en la base de datos</param>
		/// <param name="fch_orden">Fecha de la orden a buscar</param>
		/// <returns>Devuelve un objeto Orden</returns>
		[HttpGet("{id_usuario}/{fch_orden}")]
        public async Task<ActionResult<Orden>> GetOrden(long id_usuario, DateTime fch_orden)
        {
            // Obtenemos la orden por el id del usuario y la fecha de la orden
            var orden = await context.Ordenes.Where(orden => orden.id_usuario == id_usuario && orden.fch_orden == fch_orden).FirstOrDefaultAsync();

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
