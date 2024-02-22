using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio a Usuario
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public interface UsuarioInterfaz
    {
        /// <summary>
        /// Método que hace el registro de un usuario a la base de datos y envia un correo para confirmar cuenta. Si el email introducido ya existe no hará el registro.
        /// </summary>
        /// <param name="usuario">Objeto usuario con los datos.</param>
        /// <returns>Devuelve true si se ha producido el registro, false si ya existe el email o null si se ha producido un error.</returns>
        public bool? RegistrarUsuario(UsuarioDTO usuario);

        /// <summary>
        /// Método que realiza el inicio de sesión. Comprueba si el usuario pasado por parámetros existe en la base de datos.
        /// Si existe devuelve el usuario encontrado en la base de datos.
        /// Si no existe devuelve null.
        /// </summary>
        /// <param name="usuario">Objeto usuario</param>
        /// <returns>Devuelve el usuario si existe en la base de datos o null si no existe</returns>
        public UsuarioDTO LoginUsuario(UsuarioDTO usuario);

        /// <summary>
        /// Método que obtiene un usuario de la base de datos, crea un token, lo guarda en la base de datos y envia un correo al email que tiene el objeto usuario.
        /// </summary>
        /// <param name="usuario">Objeto usuario.</param>
        /// <returns>Devuelve true si se ha enviado un correo de recuperación o false si no se ha enviado.</returns>
        public bool RecuperaPassword(UsuarioDTO usuario);

        /// <summary>
        /// Método que modifica la contraseña de un usuario en la base de datos
        /// </summary>
        /// <param name="token">Objeto token</param>
        /// <param name="password">Contraseña nueva</param>
        /// <returns>Devuelve true si se ha modificado o false si ha ocurrido un error</returns>
        public bool ModificaPassword(TokenDTO token, string password);

        /// <summary>
        /// Método que activa la cuenta de un usuario
        /// </summary>
        /// <param name="token">Objeto token</param>
        /// <returns>Devuelve true si se ha activado la cuenta o false si se ha producido algún error</returns>
        public bool ActivaCuenta(TokenDTO token);

        /// <summary>
        /// Método que obtiene un usuario por el email y lo devuelve. Si no encuentra ningún usuario con el email introducido devolverá null.
        /// </summary>
        /// <param name="emailUsuario">Email del usuario a buscar</param>
        /// <returns>Devuelve el usuario encontrado o null si no lo encuentra</returns>
        public Task<UsuarioDTO> BuscaUsuarioPorEmail(string emailUsuario);

        /// <summary>
        /// Método que obtiene un usuario de la base de datos según su id.
        /// </summary>
        /// <param name="id">Id del usuario a devolver</param>
        /// <returns>Devuelve el usuario encontrado o null en caso de no encontrarlo</returns>
        public Task<UsuarioDTO> BuscaUsuarioPorId(long id);

        /// <summary>
        /// Método que obtiene todos los usuarios de la base de datos y los devuelve.
        /// </summary>
        /// <returns>Devuelve una lista con objetos de tipo Usuario</returns>
        public Task<List<UsuarioDTO>> ObtieneTodosLosUsuarios();

        /// <summary>
        /// Método que borra un usuario de la base de datos.
        /// </summary>
        /// <param name="id">Id del usuario a borrar</param>
        /// <returns>Devuelve true si se ha borrado correctamente o false si no se ha podido borrar</returns>
        public bool BorraUsuarioPorId(int id);

        /// <summary>
        /// Método que actualiza un usuario pasado por parámetros en la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto UsuarioDTO</param>
        /// <returns>Devuelve true si se ha actualizado correctamente o false si se ha producido un error</returns>
        public bool ActualizaUsuario(UsuarioDTO usuario);

        /// <summary>
        /// Método que agrega un usuario en la base de datos
        /// </summary>
        /// <param name="usuarioDTO">Objeto usuarioDTO a añadir</param>
        /// <returns>Devuelve true si se ha agregado con éxito o false si no</returns>
        public bool AgregaUsuario(UsuarioDTO usuarioDTO);
    }
}
