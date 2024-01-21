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
        public async Task<ActionResult<Carrito>> GetCarrito(long id)
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

        [HttpPut]
        public async Task<IActionResult> PutCarrito(Carrito carrito)
        {
            context.Entry(carrito).State = EntityState.Modified;

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
        public async Task<ActionResult<Carrito>> PostCarrito(Carrito carrito)
        {
            try
            {
                context.Carritos.Add(carrito);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetCarrito", new { id = carrito.id_carrito }, carrito);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrito(long id)
        {
            try
            {
                var carrito = await context.Carritos.FindAsync(id);

                if (carrito == null)
                {
                    return NotFound();
                }

                context.Carritos.Remove(carrito);
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
