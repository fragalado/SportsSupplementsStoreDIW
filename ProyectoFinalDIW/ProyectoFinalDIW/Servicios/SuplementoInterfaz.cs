using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio a Suplemento
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public interface SuplementoInterfaz
    {
        /// <summary>
        /// Método que obtiene todos los suplementos de la base de datos y los devuelve.
        /// </summary>
        /// <returns>Devuelve una lista con objetos de tipo Suplemento</returns>
        public Task<List<SuplementoDTO>> ObtieneTodosLosSuplementos();

        /// <summary>
        /// Método que obtiene un suplemento de la base de datos según su id.
        /// </summary>
        /// <param name="id">Id del suplemento a devolver</param>
        /// <returns>Devuelve el suplemento encontrado o null en caso de no encontrarlo</returns>
        public Task<SuplementoDTO> BuscaSuplementoPorId(long id);

        /// <summary>
        /// Método que borra un suplemento de la base de datos.
        /// </summary>
        /// <param name="id">Id del suplemento a borrar</param>
        /// <returns>Devuelve true si se ha borrado correctamente o false si no se ha podido borrar</returns>
        public bool BorraSuplementoPorId(int id);

        /// <summary>
        /// Método que actualiza un suplemento pasado por parámetros en la base de datos.
        /// </summary>
        /// <param name="suplemento">Objeto SuplementoDTO</param>
        /// <returns>Devuelve true si se ha actualizado correctamente o false si se ha producido un error</returns>
        public bool ActualizaSuplemento(SuplementoDTO suplemento);

        /// <summary>
        /// Método que agrega un suplemento a la base de datos
        /// </summary>
        /// <param name="suplemento">Objeto suplemento a agregar a la base de datos</param>
        /// <returns>Devuelve true si se ha agregado correctamente o false si se ha producido un error</returns>
        public bool AgregaSuplemento(SuplementoDTO suplemento);
    }
}
