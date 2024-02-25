using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Token_Tabla
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TokenControlador : ControllerBase
    {
        public readonly Contexto context;

        public TokenControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los tokens de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los tokens de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token_Tabla>>> GetTokens()
        {
            // Devolvemos toda la lista de tokens
            return context.Tokens.ToList();
        }

        /// <summary>
        /// Método que crea un nuevo token en la base de datos
        /// </summary>
        /// <param name="token">Token a crear</param>
        /// <returns>Devuelve el token creado</returns>
        [HttpPost]
        public async Task<ActionResult<Token_Tabla>> PostToken(Token_Tabla token)
        {
            try
            {
                // Agregamos el token
                context.Tokens.Add(token);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el token creado
                return CreatedAtAction("GetTokenByToken", new { token = token.cod_token }, token);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Método que obtiene un token por su codigo
        /// </summary>
        /// <param name="token">Codigo del token a buscar</param>
        /// <returns>Devuelve el token encontrado</returns>
        [HttpGet("{token}")]
        public async Task<ActionResult<Token_Tabla>> GetTokenByToken(string token)
        {
            // Buscamos el token por su codigo de token
            var tokenDevolver = await context.Tokens.FirstOrDefaultAsync(t => t.cod_token == token);

            // Comprobamos si el token existe, es distinto de null
            if (tokenDevolver == null)
            {
                return NotFound();
            }

            // Devolvemos el token
            return tokenDevolver;
        }
    }
}
