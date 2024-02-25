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

        /// <summary>
        /// Método que recibe una lista de objetos Rel_Orden_Carrito y crea cada objeto en la base de datos
        /// </summary>
        /// <param name="relOrdenCarrito">Lista con objetos de tipo Rel_Orden_Carrito</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostRelOrdenCarrito(List<Rel_Orden_Carrito> relOrdenCarrito)
        {
            try
            {
                // Recorremos la lista
                foreach (var aux in relOrdenCarrito)
                {
                    // Agregamos el objeto
					context.Rel_Orden_Carritos.Add(aux);
				}
                
                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el estado, ok, operacion exitosa
                return Ok();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }
    }
}