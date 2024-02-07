using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos que darán servicio a Email
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public interface EmailInterfaz
    {
        /// <summary>
        /// Método que crea un token y hace el POST a la base de datos del token creado. Además envia un correo al gmail del usuario pasado por parámetros
        /// </summary>
        /// <param name="usuario">Objeto usuario al que se le envia el correo</param>
        /// <param name="urlCorreo">Url a la que te llevará al pulsar el botón del mensaje de correo</param>
        /// <param name="esActivado">Booleano que será true si es para email de confirmación o false si es para recuperar contraseña</param>
        /// <returns>Devuelve true si ha enviado el correo o false si no</returns>
        public bool EnviaCorreo(UsuarioDTO usuario, string urlCorreo, bool esActivado);

        /// <summary>
        /// Método que crea el cuerpo del correo que se enviará al email del usuario.
        /// </summary>
        /// <param name="token">Token creado</param>
        /// <param name="direccion">Dirección URL</param>
        /// <param name="esActivado">Booleano que será true si es para email de confirmación o false si es para recuperar contraseña</param>
        /// <returns>Devuelve el cuerpo del correo</returns>
        public string MensajeCorreo(string token, string direccion, bool esActivado);

        /// <summary>
        /// Método que envia un correo al email introducido
        /// </summary>
        /// <param name="body">Cuerpo del correo</param>
        /// <param name="to">Email del usuario al que se enviará el correo</param>
        /// <param name="html">Indica si es html</param>
        /// <param name="subject">Asunto del mensaje</param>
        /// <param name="frommail">Email desde el cuál se enviará el correo</param>
        /// <param name="cco">Indica si se utiliza copia oculta o no</param>
        /// <returns></returns>
        public bool EnviarMensaje(string body, string to, bool html, string subject, string frommail, bool cco, bool esActivado);
    }
}
