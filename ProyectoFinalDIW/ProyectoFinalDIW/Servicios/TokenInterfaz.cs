using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio a Token
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public interface TokenInterfaz
    {
        /// <summary>
        /// Método que obtiene un token de la base de datos y lo devuelve.
        /// </summary>
        /// <param name="token">Código token</param>
        /// <returns>Devuelve el token si existe o null si no existe</returns>
        public Task<TokenDTO> ObtenerToken(string token);
    }
}
