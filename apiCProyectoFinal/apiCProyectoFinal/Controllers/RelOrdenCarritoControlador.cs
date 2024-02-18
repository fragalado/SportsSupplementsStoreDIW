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
        public async Task<ActionResult<IEnumerable<Rel_Orden_Carrito>>> GetRelOrdenCarrito()
        {
            // Devolvemos toda la lista de rel_orden_carrito
            return context.Rel_Orden_Carritos.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> PostRelOrdenCarrito(List<Rel_Orden_Carrito> relOrdenCarrito)
        {
            try
            {
                foreach (var aux in relOrdenCarrito)
                {
					context.Rel_Orden_Carritos.Add(aux);
				}
                
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }
    }
}