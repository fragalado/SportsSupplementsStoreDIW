using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métofos que darán servicio para hacer operaciones registro, login, olvidar contraseña, etc.
    /// </summary>
    public interface AccesoInterfaz
    {
        /// <summary>
        /// Método que hace el registro de un usuario a la base de datos. Si el email introducido ya existe no hará el registro.
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
    }
}
