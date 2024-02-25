using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Email
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public class EmailImplementacion : EmailInterfaz
    {
        public bool EnviaCorreo(UsuarioDTO usuario, string urlCorreo, bool esActivado)
        {
            try
            {
                // Log
                Util.LogInfo("EmailImplementacion", "EnviaCorreo", "Ha entrado en EnviaCorreo");

                // Creamos un token
                Guid guid = Guid.NewGuid();

                // Convertimos el token creado a string
                string token = guid.ToString();

                // Creamos ahora la fecha limite
                DateTime fechaLimite = DateTime.Now.AddMinutes(5); // 5 minutos para realizar la operación

                // Creamos un objeto TokenDTO con el token, la fecha limite y el id del usuario
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

                    // Verificamos la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        // Log
                        Util.LogInfo("EmailImplementacion", "EnviaCorreo", "Token creado correctamente");

                        // Llamamos a los métodos para enviar el correo
                        String mensaje = MensajeCorreo(token, urlCorreo, esActivado);
                        return EnviarMensaje(mensaje, usuario.Email_usuario, "Activar cuenta", "suplementostore@frangallegodorado.es", esActivado);
                    }
                    else
                    {
                        // Log
                        Util.LogInfo("EmailImplementacion", "EnviaCorreo", "No se ha creado el token correctamente");
                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviaCorreo", "No se ha podido enviar el correo debido a la cancelacion de una tarea");
                return false;
            }
        }

        public string MensajeCorreo(string token, string direccion, bool esActivado)
        {
            // Log
            Util.LogInfo("EmailImplementacion", "MensajeCorreo", "Ha entrado en MensajeCorreo");

            if (esActivado)
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

        public bool EnviarMensaje(string body, string to, string subject, string frommail, bool esActivado)
        {
            bool resultado = true;
            SmtpClient smtpClient = null;
            try
            {
                // Log
                Util.LogInfo("EmailImplementacion", "EnviarMensaje", "Ha entrado en EnviarMensaje");

                // Parámetros de conexión
                string host = "smtp.ionos.es";
                string miLogin = "suplementoStore@frangallegodorado.es";
                string miPassword = "LentosJavaC23;24/Java&C";

                // Configuramos cliente SMTP
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
                        msg.From = new MailAddress($"'InfoWeb' <suplementoStore@frangallegodorado.es>");

                        // A quien envía el correo
                        msg.ReplyToList.Add(new MailAddress(frommail));
                        msg.To.Add(new MailAddress(to));

                        // Establecer el asunto del mensaje
                        msg.Subject = subject;

                        // Construir el cuerpo
                        if (esActivado)
                            body = " Activar cuenta: " + body;
                        else
                            body = " Restablecer contraseña: " + body;
                        msg.Body = body;
                        msg.IsBodyHtml = true;

                        // Enviar el mensaje
                        smtpClient.Send(msg);
                    }
                }
            }
            catch (Exception e)
            {
                // Log
                Util.LogError("EmailImplementacion", "EnviarMensaje", "no se ha podido enviar el correo electronico");
                resultado = false;
            }
            finally
            {
                smtpClient?.Dispose();
            }
            return resultado;
        }
    }
}
