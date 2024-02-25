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
		public async Task<ActionResult<Orden>> GetOrdenById(long id)
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

		///// <summary>
		///// Método que obtiene una orden por el id del usuario pasado por parámetros y la fecha de la orden pasada por parametros
		///// </summary>
		///// <param name="id_usuario">Id del usuario de la orden en la base de datos</param>
		///// <param name="fch_orden">Fecha de la orden a buscar</param>
		///// <returns>Devuelve un objeto Orden</returns>
		//[HttpGet("{id_usuario}/{fch_orden}")]
  //      public async Task<ActionResult<Orden>> GetOrdenByIdUsuarioAndFecha(long id_usuario, DateTime fch_orden)
  //      {
  //          // Obtenemos la orden por el id del usuario y la fecha de la orden
  //          var orden = await context.Ordenes.Where(orden => orden.id_usuario == id_usuario && orden.fch_orden == fch_orden).FirstOrDefaultAsync();

  //          // Comprobamos si la orden es nula, si es nula devolvemos not found
  //          if (orden == null)
  //          {
  //              return NotFound();
  //          }

  //          // Si la orden no es nula se devolverá la orden.
  //          return orden;
  //      }

        /// <summary>
        /// Método que actualiza una orden en la base de datos
        /// </summary>
        /// <param name="orden">Orden a actualizar</param>
        /// <returns>Devuelve el estado de la operacion</returns>
        [HttpPut]
        public async Task<IActionResult> PutOrden(Orden orden)
        {
            // Cambiamos el estado de la orden a modificado para que EntityFramework realice la actualiacion
            context.Entry(orden).State = EntityState.Modified;

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
        /// Método que crea un nuevo orden en la base de datos
        /// </summary>
        /// <param name="orden">Orden a crear</param>
        /// <returns>Devuelve el orden creado</returns>
        [HttpPost]
        public async Task<ActionResult<Orden>> PostOrden(Orden orden)
        {
            try
            {
                // Agrega el orden
                context.Ordenes.Add(orden);

                // Guarda los cambios
                await context.SaveChangesAsync();

                // Devolvemos el orden creado
                return CreatedAtAction("GetOrdenById", new { id = orden.id_orden }, orden);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Método que elimina una orden de la base de datos
        /// </summary>
        /// <param name="id">Id del orden a borrar</param>
        /// <returns>Devuelve el estado de la operacion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            try
            {
                // Buscamos el orden por el id
                var orden = await context.Ordenes.FindAsync(id);

                // Comprobamos si el orden existe
                if (orden == null)
                {
                    return NotFound();
                }

                // Eliminamos el orden
                context.Ordenes.Remove(orden);

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
