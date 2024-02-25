using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Usuario
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public class UsuarioImplementacion : UsuarioInterfaz
    {
        // Inicializamos la implementación de email para poder usar sus métodos
        private EmailInterfaz emailIntefaz = new EmailImplementacion();

        public UsuarioDTO LoginUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "LoginUsuario", "Ha entrado en LoginUsuario");

                // Buscamos si existe un usuario con el email introducido.
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

                // Si existe comprobaremos que la password coincide
                if (usuarioEncontrado != null && Util.EncriptarContra(usuario.Psswd_usuario) == usuarioEncontrado.Psswd_usuario)
                {
                    return usuarioEncontrado;
                }

                // Si llega aquí es porque no se ha encontrado un usuario o no coinciden las contraseñas
                return null;
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "LoginUsuario", "No se ha podido hacer el login debido a un excepcion agregada");

                return null;
            }
        }

        public bool? RegistrarUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "RegistrarUsuario", "Ha entrado en RegistrarUsuario");

                // Buscamos si existe un usuario con el email introducido.
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

                if (usuarioEncontrado != null)
                {
                    // Se ha encontrado un usuario con el email introducido
                    // Luego devolveremos false
                    return false;
                }

                // Si no existe ningún usuario con el email introducido haremos el registro del usuario
                bool ok = AgregaUsuario(usuario);

                if (ok)
                {
                    // Enviamos correo
                    // Obtenemos el usuario de la base de datos para poder obtener el id
                    UsuarioDTO usuarioBD = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

                    // Enviamos el correo
                    bool okCorreo = emailIntefaz.EnviaCorreo(usuarioBD, "https://localhost:7194/ActivaCuenta/VistaConfirmaEmail", true);

                    if (okCorreo)
                        return true;
                    else
                        return null;
                }

                return null;
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "RegistrarUsuario", "No se ha podido hacer el registro debido a un excepcion agregada");

                return null;
            }
        }

        public async Task<UsuarioDTO> BuscaUsuarioPorEmail(string emailUsuario)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "BuscaUsuarioPorEmail", "Ha entrado en BuscaUsuarioPorEmail");

                // URL de la API que deseas consultar
                string apiUrl = "https://localhost:7029/api/UsuarioControlador/correo/" + emailUsuario;

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

                // Deserializa la respuesta JSON a un objeto UsuarioDTO
                UsuarioDTO usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(responseData);

                // Comprobamos si el usuario es distinto de null, si lo es lo devolvemos
                if (usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorEmail", "No se ha podido buscar el usuario por el email debido a una operacion invalida");

                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorEmail", "No se ha podido buscar el usuario por el email debido a un error en la solicitud HTTP");

                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorEmail", "No se ha podido buscar el usuario por el email debido a la cancelacion de una tarea");

                return null;
            }
        }

        public bool RecuperaPassword(UsuarioDTO usuario)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "RecuperaPassword", "Ha entrado en RecuperaPassword");

                // Obtenemos el usuario de la base de datos
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

                // Si usuarioEncontrado es null devolvemos false
                if (usuarioEncontrado == null)
                {
                    return false;
                }

                // Si el usuarioEncontrado es distinto de null enviamos el correo
                bool ok = emailIntefaz.EnviaCorreo(usuarioEncontrado, "https://localhost:7194/RestablecerPassword/VistaCambiarContrasenya", false);

                // Comprobamos si se ha enviado el correo o no
                if (ok)
                    return true;
                return false; // Caso contrario
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "RecuperaPassword", "No se ha podido enviar el correo de recupera password debido a un excepcion agregada");

                return false;
            }
        }

        public bool ModificaPassword(TokenDTO token, string password)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "ModificaPassword", "Ha entrado en ModificaPassword");

                // Obtenemos el usuario por el id
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(token.Id_usuario).Result;

                // Comprobaremos que el usuarioEncontrado sea distinto de null
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
                            // Log
                            Util.LogInfo("UsuarioImplementacion", "ModificaPassword", "Usuario password actualizada correctamente");

                            return true;
                        }
                        else
                        {
                            // Log
                            Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido actualizar la password del usuario");

                            return false;
                        }
                    }
                }
                Console.WriteLine("No hay coincidencia");
                return false;
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ModificaPassword", "No se ha podido modificar la password debido a la cancelacion de una tarea");
                return false;
            }
        }

        public bool ActivaCuenta(TokenDTO token)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "ActivaCuenta", "Ha entrado en ActivaCuenta");

                // Activamos la cuenta del usuario
                // Para ello obtenemos el usuario de la base de datos y después hacemos un PUT a la base de datos con el usuario cambiado
                // Obtenemos el usuario por el id
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(token.Id_usuario).Result;

                // Comprobamos que el usuarioEncontrado sea distinto de null
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
                            // Log
                            Util.LogInfo("UsuarioImplementacion", "ActivaCuenta", "Usuario activado correctamente");

                            return true;
                        }
                        else
                        {
                            // Log
                            Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar el usuario");

                            return false;
                        }
                    }
                }
                Console.WriteLine("No hay coincidencia");
                return false;
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActivaCuenta", "No se ha podido activar la cuenta debido a la cancelacion de una tarea");
                return false;
            }
        }

        public async Task<List<UsuarioDTO>> ObtieneTodosLosUsuarios()
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "ObtieneTodosLosUsuarios", "Ha entrado en ObtieneTodosLosUsuarios");

                // URL que se desea consultar
                string apiUrl = "https://localhost:7029/api/UsuarioControlador";

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
                        // En caso de error devolvemos null
                        return null;
                    }
                }

                // Deserializa la respuesta JSON a una Lista de objetos UsuarioDTO
                List<UsuarioDTO> listaUsuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(responseData);

                // Devolvemos la lista
                return listaUsuarios;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ObtieneTodosLosUsuarios", "No se ha podido obtener todos los usuarios debido a una operacion invalida");

                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ObtieneTodosLosUsuarios", "No se ha podido obtener todos los usuarios debido a un error en la solicitud HTTP");

                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ObtieneTodosLosUsuarios", "No se ha podido obtener todos los usuarios debido a la cancelacion de una tarea");

                return null;
            }
        }

        public bool BorraUsuarioPorId(int id)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "BorraUsuarioPorId", "Ha entrado en BorraUsuarioPorId");

                // Buscamos el usuario por el id
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(id).Result;

                // Si el usuarioEncontrado es igual a null o es admin devolvemos false
                if (usuarioEncontrado == null || usuarioEncontrado.Id_acceso == 2)
                {
                    // No se ha encontrado ningún usuario con el id introducido o es admin
                    // Luego devolvemos false
                    return false;
                }

                // Si no es admin lo borramos de la base de datos
                // Configuramos la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el DELETE
                    Uri url = new Uri("https://localhost:7029/api/UsuarioControlador/" + id);

                    // Configurar la solicitud HTTP DELETE
                    HttpResponseMessage response = client.DeleteAsync(url).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        // Log
                        Util.LogInfo("UsuarioImplementacion", "BorraUsuarioPorId", "Usuario eliminado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido eliminar el usuario");

                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BorraUsuarioPorId", "No se ha podido borrar el usuario debido a la cancelacion de una tarea");
                return false;
            }
        }

        public async Task<UsuarioDTO> BuscaUsuarioPorId(long id)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "BuscaUsuarioPorId", "Ha entrado en BuscaUsuarioPorId");

                // URL de la API que deseas consultar
                string apiUrl = "https://localhost:7029/api/UsuarioControlador/" + id;

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
                        // En caso de error devolvemos null
                        return null;
                    }
                }

                // Deserializa la respuesta JSON a un objeto UsuarioDTO
                UsuarioDTO usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(responseData);

                // Si usuarioEncontrado es distinto de null lo devolvemos
                if (usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorId", "No se ha podido buscar el usuario por id debido a una operacion invalida");

                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorId", "No se ha podido buscar el usuario por id debido a un error en la solicitud HTTP");

                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "BuscaUsuarioPorId", "No se ha podido buscar el usuario por id debido a la cancelacion de una tarea");

                return null;
            }
        }

        public bool ActualizaUsuario(UsuarioDTO usuario)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "ActualizaUsuario", "Ha entrado en ActualizaUsuario");

                // Con el id del usuario pasado obtenemos el usuario de la base de datos
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(usuario.Id_usuario).Result;

                // Actualizamos algunos datos del usuarioEncontrado con el usuario
                usuarioEncontrado.Email_usuario = usuario.Email_usuario;
                usuarioEncontrado.Tlf_usuario = usuario.Tlf_usuario;
                usuarioEncontrado.Nombre_usuario = usuario.Nombre_usuario;
                if (usuario.RutaImagen_usuario != null)
                    usuarioEncontrado.RutaImagen_usuario = usuario.RutaImagen_usuario;

                // Convertimos el usuarioEncontrado a json
                string usuarioJson = JsonConvert.SerializeObject(usuarioEncontrado, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configuramos la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el UPDATE
                    Uri url = new Uri("https://localhost:7029/api/UsuarioControlador");

                    // Configurar la solicitud HTTP UPDATE
                    HttpResponseMessage response = client.PutAsync(url, new StringContent(usuarioJson, Encoding.UTF8, "application/json")).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        // Log
                        Util.LogInfo("UsuarioImplementacion", "ActualizaUsuario", "Usuario actualizado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario");

                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "ActualizaUsuario", "No se ha podido actualizar el usuario debido a la cancelacion de una tarea");
                return false;
            }
        }

        public bool AgregaUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                // Log
                Util.LogInfo("UsuarioImplementacion", "AgregaUsuario", "Ha entrado en AgregaUsuario");

                // Encriptamos la contraseña del usuario
                usuarioDTO.Psswd_usuario = Util.EncriptarContra(usuarioDTO.Psswd_usuario);

                // Convertimos el usuario a json
                string usuarioJson = JsonConvert.SerializeObject(usuarioDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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
                        // Log
                        Util.LogInfo("UsuarioImplementacion", "AgregaUsuario", "Usuario creado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario");

                        return false;
                    }
                }

            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("UsuarioImplementacion", "AgregaUsuario", "No se ha podido agregar el usuario debido a la cancelacion de una tarea");
                return false;
            }
        }
    }
}
