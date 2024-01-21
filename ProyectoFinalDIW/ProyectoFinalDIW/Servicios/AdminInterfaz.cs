using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio al controlador AdminController
    /// </summary>
    public interface AdminInterfaz
    {
        /// <summary>
        /// Método que obtiene todos los usuarios de la base de datos y los devuelve.
        /// </summary>
        /// <returns>Devuelve una lista con objetos de tipo Usuario</returns>
        public Task<List<UsuarioDTO>> ObtieneTodosLosUsuarios();

        /// <summary>
        /// Método que obtiene todos los suplementos de la base de datos y los devuelve.
        /// </summary>
        /// <returns>Devuelve una lista con objetos de tipo Suplemento</returns>
        public Task<List<SuplementoDTO>> ObtieneTodosLosSuplementos();
    }
}
