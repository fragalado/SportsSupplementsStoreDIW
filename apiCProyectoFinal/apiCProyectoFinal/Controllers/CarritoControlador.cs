using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Carrito
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoControlador : ControllerBase
    {
        public readonly Contexto context;

        public CarritoControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los carritos de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los carritos de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrito>>> GetCarritos()
        {
            // Devolvemos toda la lista de carritos
            return context.Carritos.ToList();
        }

        /// <summary>
        /// Método que obtiene un carrito por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id del carrito en la base de datos</param>
        /// <returns>Devuelve un carrito</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrito>> GetCarritoById(long id)
        {
            // Obtenemos el carrito por el id
            var carrito = await context.Carritos.FindAsync(id);

            // Comprobamos si el carrito es nulo, si es nulo devolvemos not found
            if (carrito == null)
            {
                return NotFound();
            }

            // Si el carrito no es nulo se devolverá el carrito.
            return carrito;
        }

        /// <summary>
        /// Método que actualiza un carrito en la base de datos
        /// </summary>
        /// <param name="carrito">Carrito a actualizar</param>
        /// <returns>Devuelve el resultado de la operacion</returns>
        [HttpPut]
        public async Task<IActionResult> PutCarrito(Carrito carrito)
        {
            // Cambiamos el estado del carrito a modificado para que EntityFramework realice la actualziacion
            context.Entry(carrito).State = EntityState.Modified;

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
        /// Método que crea un carrito en la base de datos
        /// </summary>
        /// <param name="carrito">Carrito a crear</param>
        /// <returns>Devuelve el carrito creado</returns>
        [HttpPost]
        public async Task<ActionResult<Carrito>> PostCarrito(Carrito carrito)
        {
            try
            {
                // Agregamos el carrito
                context.Carritos.Add(carrito);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el carrito creado
                return CreatedAtAction("GetCarritoById", new { id = carrito.id_carrito }, carrito);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Método que elimina un carrito en la base de datos por su id
        /// </summary>
        /// <param name="id">Id del carrito a eliminar</param>
        /// <returns>Devuelve el estado de la operacion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrito(long id)
        {
            try
            {
                // Buscamos el carrito por su id
                var carrito = await context.Carritos.FindAsync(id);

                // Comprobamos si existe el carrito
                if (carrito == null)
                {
                    return NotFound();
                }

                // Eliminamos el carrito
                context.Carritos.Remove(carrito);

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
