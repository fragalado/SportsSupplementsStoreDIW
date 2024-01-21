using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Rel_Orden_Carrito
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RelOrdenCarritoControlador : ControllerBase
    {
        public readonly Contexto context;

        public RelOrdenCarritoControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los datos de la tabla relacion de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los datos de la tabla relacion de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetRelOrdenCarrito()
        {
            // Devolvemos toda la lista de usuarios
            return context.Usuarios.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Rel_Orden_Carrito>> PostRelOrdenCarrito(Rel_Orden_Carrito relOrdenCarrito)
        {
            try
            {
                context.Rel_Orden_Carritos.Add(relOrdenCarrito);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetRelOrdenCarrito", new { id = relOrdenCarrito.id_rel_orden_carrito }, relOrdenCarrito);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpGet("{id_orden}")]
        public async Task<ActionResult<Rel_Orden_Carrito>> GetRelOrdenCarritoByIdOrden(long id_orden)
        {
            try
            {
                var relOrdenCarrito = await context.Rel_Orden_Carritos.FirstOrDefaultAsync(u => u.id_orden == id_orden);

                if (relOrdenCarrito == null)
                {
                    return NotFound();
                }

                return relOrdenCarrito;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}