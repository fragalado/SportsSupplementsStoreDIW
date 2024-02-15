using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio a Carrito
    /// </summary>
    /// autor: Fran Gallego
    /// Fecha: 11/02/2024
    public interface CarritoInterfaz
    {
        /// <summary>
        /// Método que obtiene el carrito de un usuario pasado por parámetros
        /// </summary>
        /// <param name="emailUsuario">Email del usuario</param>
        /// <returns>Devuelve una lista de tipo CarritoDTO, List<CarritoDTO></returns>
        public Task<List<CarritoDTO>> ObtieneCarritoUsuario(string emailUsuario);

        /// <summary>
        /// Método que borra un carrito por su id
        /// </summary>
        /// <param name="id_carrito">Id del carrito a borrar</param>
        /// <returns>Devuelve true si se ha borrado o false si no</returns>
        public bool BorraCarrito(long id_carrito);

        /// <summary>
        /// Método que agrega un suplemento al carrito
        /// </summary>
        /// <param name="id_suplemento">Id del suplemento que se agrega</param>
        /// <param name="emailUsuario">Email del usuario que lo agrega</param>
        /// <returns>Devuelve true si se ha agregado al carrito o false si no</returns>
        public bool AgregaSuplemento(long id_suplemento, String emailUsuario);

        /// <summary>
        /// Método que obtiene el precio total del carrito
        /// </summary>
        /// <param name="listaCarrito">Lista con objetos de tipo CarritoDTO</param>
        /// <param name="listaSuplementos">Lista con objetos de tipo SuplementoDTO</param>
        /// <returns>Devuelve un tipo float; Precio total del carrito</returns>
        public float ObtienePrecioTotalCarrito(List<CarritoDTO> listaCarrito, List<SuplementoDTO> listaSuplementos);
    }
}
