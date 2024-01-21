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

        [HttpPost]
        public async Task<ActionResult<Token_Tabla>> PostToken(Token_Tabla token)
        {
            try
            {
                context.Tokens.Add(token);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetTokenByToken", new { token = token.cod_token }, token);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [HttpGet("{token}")]
        public async Task<ActionResult<Token_Tabla>> GetTokenByToken(string token)
        {
            var tokenDevolver = await context.Tokens.FirstOrDefaultAsync(t => t.cod_token == token);

            if (tokenDevolver == null)
            {
                return NotFound();
            }

            return tokenDevolver;
        }
    }
}
