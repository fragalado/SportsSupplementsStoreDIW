using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Acceso
    /// </summary>
    public class AccesoImplementacion : AccesoInterfaz
    {
        public UsuarioDTO LoginUsuario(UsuarioDTO usuario)
        {
            // Buscamos si existe un usuario con el email introducido.
            UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario).Result;

            // Si existe comprobaremos que la password coincide
            if (usuarioEncontrado != null)
            {
                // Encriptamos la contraseña
                usuario.Psswd_usuario = Util.EncriptarContra(usuario.Psswd_usuario);

                if (usuarioEncontrado.Psswd_usuario == usuario.Psswd_usuario)
                {
                    // Coinciden luego devolvemos el usuarioEncontrado
                    return usuarioEncontrado;
                }
            }

            // Si llega aquí es porque no se ha encontrado un usuario o no coinciden las contraseñas
            return null;
        }

        public bool? RegistrarUsuario(UsuarioDTO usuario)
        {
			try
			{
                // Buscamos si existe un usuario con el email introducido.
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario).Result;

                if(usuarioEncontrado != null)
                {
                    // Se ha encontrado un usuario con el email introducido
                    // Luego devolveremos false
                    return false;
                }

                // Si no existe ningún usuario con el email introducido haremos el registro del usuario
                // Encriptamos la contraseña
                usuario.Psswd_usuario = Util.EncriptarContra(usuario.Psswd_usuario);

				// Convertimos el usuario a json
                string usuarioJson = JsonConvert.SerializeObject(usuario, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el POST
                    Uri url = new Uri("https://localhost:7029/api/UsuarioControlador");

                    // Configurar la solicitud HTTP POST
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(usuarioJson, Encoding.UTF8, "application/json")).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Usuario creado exitosamente");

                        // Enviamos correo
                        // Obtenemos el usuario de la base de datos para poder obtener el id
                        UsuarioDTO usuarioBD = BuscaUsuarioPorEmail(usuario).Result;

                        // Enviamos el correo
                        bool ok = EnviaCorreo(usuarioBD, "https://localhost:7194/Acceso/VistaConfirmaEmail", true);

                        if (ok)
                            return true;
                        else
                            return null;
                    }
                    else
                    {
                        Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-RegistrarUsuario] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-RegistrarUsuario] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-RegistrarUsuario] Error la tarea fue cancelada");
                return null;
            }
        }

		/// <summary>
		/// Método que obtiene un usuario por el email y lo devuelve. Si no encuentra ningún usuario con el email introducido devolverá null.
		/// </summary>
		/// <param name="usuario">Objeto usuario que contendrá el email a buscar</param>
		/// <returns>Devuelve el usuario encontrado o null si no lo encuentra</returns>
		private async Task<UsuarioDTO> BuscaUsuarioPorEmail(UsuarioDTO usuario)
		{
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador/correo/" + usuario.Email_usuario;

            try 
			{
                // Realiza la consulta GET
                string responseData;
                using (HttpClient client = new HttpClient())
                {
                    // Realiza la solicitud GET a la API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Lee y devuelve el contenido de la respuesta como cadena
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // En caso de error, lanza una excepción o maneja el error según tus necesidades
                        return null;
                    }
                }

                // Deserializa la respuesta JSON a un objeto C#
                UsuarioDTO usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(responseData);

                // Ahora puedes trabajar con 'usuarios', que es una lista con los datos de la API
                if(usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return usuario = null;
            }
			catch (InvalidOperationException e) 
			{
                Console.WriteLine("[ERROR-AccesoImplementacion-BuscaUsuarioPorEmail] Error operación no válida");
                return usuario = null;
			}
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-BuscaUsuarioPorEmail] Error en la solicitud HTTP");
                return usuario = null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-BuscaUsuarioPorEmail] Error la tarea fue cancelada");
                return usuario = null;
            }
        }

        public bool RecuperaPassword(UsuarioDTO usuario)
        {
            try
            {
                // Obtenemos el usuario de la base de datos
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario).Result;

                // Si usuarioEncontrado es null devolvemos false
                if(usuarioEncontrado == null)
                {
                    return false;
                }

                bool ok = EnviaCorreo(usuarioEncontrado, "https://localhost:7194/Acceso/VistaCambiarContrasenya", false);

                if (ok)
                    return true;
                return false; // Caso contrario
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-RecuperaPassword] Error la tarea fue cancelada");
                return false;
            }
        }

        /// <summary>
        /// Método que crea un token y hace el POST a la base de datos del token creado. Además envia un correo al gmail del usuario pasado por parámetros
        /// </summary>
        /// <param name="usuario">Objeto usuario al que se le envia el correo</param>
        /// <param name="urlCorreo">Url a la que te llevará al pulsar el botón del mensaje de correo</param>
        /// <param name="esActivado">Booleano que será true si es para email de confirmación o false si es para recuperar contraseña</param>
        /// <returns>Devuelve true si ha enviado el correo o false si no</returns>
        private bool EnviaCorreo(UsuarioDTO usuario, string urlCorreo, bool esActivado)
        {
            try
            {
                // Si no es null crearemos un token
                Guid guid = Guid.NewGuid();

                // Convertir el GUID a una cadena (string)
                string token = guid.ToString();

                // Creamos ahora la fecha limite
                DateTime fechaLimite = DateTime.Now.AddMinutes(5); // 5 minutos para realizar la operación de cambio de contrasenya

                // Creamos un objeto TokenDTO
                TokenDTO tokenDTO = new TokenDTO(token, fechaLimite, usuario.Id_usuario);

                // Ahora realizamos el POST del token a la base de datos
                // Convertimos el token a json
                string tokenJson = JsonConvert.SerializeObject(tokenDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el POST
                    Uri url = new Uri("https://localhost:7029/api/TokenControlador");

                    // Configurar la solicitud HTTP POST
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(tokenJson, Encoding.UTF8, "application/json")).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Token creado exitosamente");

                        // Llamamos a los métodos para enviar el correo
                        String mensaje = MensajeCorreo(token, urlCorreo, esActivado);
                        bool ok = EnviarMensaje(mensaje, usuario.Email_usuario, true, "Activar cuenta", "suplementostore@frangallegodorado.es", true, esActivado);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Método que crea el cuerpo del correo que se enviará al email del usuario.
        /// </summary>
        /// <param name="token">Token creado</param>
        /// <param name="direccion">Dirección URL</param>
        /// <param name="esActivado">Booleano que será true si es para email de confirmación o false si es para recuperar contraseña</param>
        /// <returns>Devuelve el cuerpo del correo</returns>
        private string MensajeCorreo(string token, string direccion, bool esActivado)
        {
            if(esActivado)
                return $@"
        <div style=""font-family: 'Optima', sans-serif; max-width: 600px; margin: 0 auto; color: #192255; line-height: 1.6;"">
            <h2 style=""color: #192255; font-size: 24px; font-weight: bold; text-transform: uppercase; margin-bottom: 20px; text-align: left;"">Confirmar cuenta</h2>

            <p style=""font-size: 16px; text-align: left; margin-bottom: 30px;"">
                Se ha enviado una petición para activar la cuenta. Si has sido tú, haz clic en el siguiente botón para poder activar la cuenta:
            </p>

            <a href=""{direccion}?tk={token}"" style=""text-decoration: none;"" target=""_blank"">
                <button style=""background-color: #285845; color: white; padding: 15px 25px; border: none; border-radius: 5px; font-size: 18px; cursor: pointer; text-transform: uppercase;"">
                    Activar Cuenta
                </button>
            </a>
        </div>
    ";
            else
                return $@"
        <div style=""font-family: 'Optima', sans-serif; max-width: 600px; margin: 0 auto; color: #192255; line-height: 1.6;"">
            <h2 style=""color: #192255; font-size: 24px; font-weight: bold; text-transform: uppercase; margin-bottom: 20px; text-align: left;"">Restablecer Contraseña</h2>

            <p style=""font-size: 16px; text-align: left; margin-bottom: 30px;"">
                Se ha enviado una petición para restablecer la contraseña. Si no has sido tú, por favor cambia la contraseña inmediatamente.
                Si has sido tú, haz clic en el siguiente botón para restablecer tu contraseña:
            </p>

            <a href=""{direccion}?tk={token}"" style=""text-decoration: none;"" target=""_blank"">
                <button style=""background-color: #285845; color: white; padding: 15px 25px; border: none; border-radius: 5px; font-size: 18px; cursor: pointer; text-transform: uppercase;"">
                    Restablecer Contraseña
                </button>
            </a>
        </div>
    ";
        }

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
        private bool EnviarMensaje(string body, string to, bool html, string subject, string frommail, bool cco, bool esActivado)
        {
            bool resultado = true;
            SmtpClient smtpClient = null;
            try
            {

                // Parámetros de conexión con un correo de ionos
                string host = "smtp.ionos.es";
                string miLogin = "suplementostore@frangallegodorado.es";
                string miPassword = "LentosJavaC23;24/Java&C";

                // Configurar cliente SMTP
                using (smtpClient = new SmtpClient(host))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(miLogin, miPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Port = 587;

                    // Crear mensaje
                    using (MailMessage msg = new MailMessage())
                    {
                        // Dirección de quien lo envía
                        msg.From = new MailAddress($"'InfoWeb' <suplementostore@frangallegodorado.es>");

                        // A quien envía el correo
                        msg.ReplyToList.Add(new MailAddress(frommail));
                        msg.To.Add(new MailAddress(to));

                        // Si se utiliza copia oculta
                        if (cco)
                            msg.Bcc.Add(new MailAddress(frommail));

                        // Establecer el asunto del mensaje
                        msg.Subject = subject;

                        // Construir el cuerpo
                        if (html)
                        {
                            if(esActivado)
                                body = " Activar cuenta: " + body;
                            else
                                body = " Restablecer contraseña: " + body;
                            msg.Body = body;
                            msg.IsBodyHtml = true;
                        }
                        else
                        {
                            msg.Body = body;
                            msg.IsBodyHtml = false;
                        }

                        // Enviar el mensaje
                        smtpClient.Send(msg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR-AccesoImplementacion-EnviarMensaje] Error: {e.Message}");
                resultado = false;
            }
            finally
            {
                smtpClient?.Dispose();
            }
            return resultado;
        }

        public async Task<bool> ModificaPassword(TokenDTO token, string password)
        {
            // Obtenemos el usuario por el id
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador/" + token.Id_usuario;

            try
            {
                // Realiza la consulta GET
                string responseData;
                using (HttpClient client = new HttpClient())
                {
                    // Realiza la solicitud GET a la API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Lee y devuelve el contenido de la respuesta como cadena
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // En caso de error, lanza una excepción o maneja el error según tus necesidades
                        return false;
                    }
                }

                // Deserializa la respuesta JSON a un objeto C#
                UsuarioDTO usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(responseData);

                // Ahora puedes trabajar con el tokenEncontrado
                if (usuarioEncontrado != null)
                {
                    // Si el usuario es distinto de null vamos a modificar la contraseña y hacer el PUT
                    usuarioEncontrado.Psswd_usuario = Util.EncriptarContra(password);

                    // Convertimos el usuario a json
                    string usuarioJson = JsonConvert.SerializeObject(usuarioEncontrado, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    // Configurar la solicitud HTTP
                    using (HttpClient client = new HttpClient())
                    {
                        // Url a la que haremos el PUT
                        Uri url = new Uri("https://localhost:7029/api/UsuarioControlador");

                        // Configurar la solicitud HTTP PUT
                        HttpResponseMessage response = client.PutAsync(url, new StringContent(usuarioJson, Encoding.UTF8, "application/json")).Result;

                        // Verificar la respuesta del servidor
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Usuario actualizado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                            return false;
                        }
                    }
                }
                Console.WriteLine("No hay coincidencia");
                return false;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error la tarea fue cancelada");
                return false;
            }
        }

        public async Task<TokenDTO> ObtenerToken(string token)
        {
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/TokenControlador/" + token;

            try
            {
                // Realiza la consulta GET
                string responseData;
                using (HttpClient client = new HttpClient())
                {
                    // Realiza la solicitud GET a la API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Lee y devuelve el contenido de la respuesta como cadena
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // En caso de error, lanza una excepción o maneja el error según tus necesidades
                        return null;
                    }
                }

                // Deserializa la respuesta JSON a un objeto C#
                TokenDTO tokenEncontrado = JsonConvert.DeserializeObject<TokenDTO>(responseData);

                // Ahora puedes trabajar con el tokenEncontrado
                if (tokenEncontrado != null)
                {
                    return tokenEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ObtenerToken] Error la tarea fue cancelada");
                return null;
            }
        }

        public async Task<bool> ActivaCuenta(TokenDTO token)
        {
            // Activamos la cuenta del usuario
            // Para ello obtenemos el usuario de la base de datos y después hacemos un PUT a la base de datos con el usuario cambiado
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador/" + token.Id_usuario;

            try
            {
                // Realiza la consulta GET
                string responseData;
                using (HttpClient client = new HttpClient())
                {
                    // Realiza la solicitud GET a la API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Lee y devuelve el contenido de la respuesta como cadena
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // En caso de error, lanza una excepción o maneja el error según tus necesidades
                        return false;
                    }
                }

                // Deserializa la respuesta JSON a un objeto C#
                UsuarioDTO usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(responseData);

                // Ahora puedes trabajar con el tokenEncontrado
                if (usuarioEncontrado != null)
                {
                    // Si el usuario es distinto de null vamos a modificar la propiedad estaActivado_usuario y hacer el PUT
                    usuarioEncontrado.EstaActivado_usuario = true;

                    // Convertimos el usuario a json
                    string usuarioJson = JsonConvert.SerializeObject(usuarioEncontrado, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    // Configurar la solicitud HTTP
                    using (HttpClient client = new HttpClient())
                    {
                        // Url a la que haremos el PUT
                        Uri url = new Uri("https://localhost:7029/api/UsuarioControlador");

                        // Configurar la solicitud HTTP PUT
                        HttpResponseMessage response = client.PutAsync(url, new StringContent(usuarioJson, Encoding.UTF8, "application/json")).Result;

                        // Verificar la respuesta del servidor
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Usuario actualizado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                            return false;
                        }
                    }
                }
                Console.WriteLine("No hay coincidencia");
                return false;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ActivaCuenta] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ActivaCuenta] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AccesoImplementacion-ActivaCuenta] Error la tarea fue cancelada");
                return false;
            }
        }
    }
}
