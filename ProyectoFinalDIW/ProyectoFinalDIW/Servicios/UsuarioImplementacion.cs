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
            // Buscamos si existe un usuario con el email introducido.
            UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

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
                } else
                {
                    return null;
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-RegistrarUsuario] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-RegistrarUsuario] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-RegistrarUsuario] Error la tarea fue cancelada");
                return null;
            }
        }

        public async Task<UsuarioDTO> BuscaUsuarioPorEmail(string emailUsuario)
        {
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador/correo/" + emailUsuario;

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
                if (usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorEmail] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorEmail] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorEmail] Error la tarea fue cancelada");
                return null;
            }
        }

        public bool RecuperaPassword(UsuarioDTO usuario)
        {
            try
            {
                // Obtenemos el usuario de la base de datos
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorEmail(usuario.Email_usuario).Result;

                // Si usuarioEncontrado es null devolvemos false
                if (usuarioEncontrado == null)
                {
                    return false;
                }

                bool ok = emailIntefaz.EnviaCorreo(usuarioEncontrado, "https://localhost:7194/RestablecerPassword/VistaCambiarContrasenya", false);

                if (ok)
                    return true;
                return false; // Caso contrario
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-RecuperaPassword] Error la tarea fue cancelada");
                return false;
            }
        }

        public bool ModificaPassword(TokenDTO token, string password)
        {
            // Obtenemos el usuario por el id
            try
            {
                // Obtenemos el usuario
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(token.Id_usuario).Result;

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
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtenerToken] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtenerToken] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtenerToken] Error la tarea fue cancelada");
                return false;
            }
        }

        public bool ActivaCuenta(TokenDTO token)
        {
            // Activamos la cuenta del usuario
            // Para ello obtenemos el usuario de la base de datos y después hacemos un PUT a la base de datos con el usuario cambiado

            try
            {
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(token.Id_usuario).Result;

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

        public async Task<List<UsuarioDTO>> ObtieneTodosLosUsuarios()
        {
            // URL que se desea consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador";

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

                // Deserializa la respuesta JSON a una List de objetos Usuario
                List<UsuarioDTO> listaUsuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(responseData);

                // Devolvemos la lista
                return listaUsuarios;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtieneTodosLosUsuarios] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtieneTodosLosUsuarios] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ObtieneTodosLosUsuarios] Error la tarea fue cancelada");
                return null;
            }
        }

        public bool BorraUsuarioPorId(int id)
        {
            try
            {
                // Buscamos si el usuario por el id
                UsuarioDTO usuarioEncontrado = BuscaUsuarioPorId(id).Result;

                if (usuarioEncontrado == null)
                {
                    // No se ha encontrado ningún usuario con el id introducido
                    // Luego devolvemos false
                    return false;
                }

                // Si existe el usuario lo borramos de la base de datos
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
                        Console.WriteLine("Usuario eliminado exitosamente");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                        return false;
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BorrarUsuario] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BorrarUsuario] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BorrarUsuario] Error la tarea fue cancelada");
                return false;
            }
        }

        public async Task<UsuarioDTO> BuscaUsuarioPorId(long id)
        {
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/UsuarioControlador/" + id;

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
                if (usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorId] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorId] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-BuscaUsuarioPorId] Error la tarea fue cancelada");
                return null;
            }
        }

        public bool ActualizaUsuario(UsuarioDTO usuario)
        {
            try
            {
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
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ActualizaUsuario] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ActualizaUsuario] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-ActualizaUsuario] Error la tarea fue cancelada");
                return false;
            }
        }

        public bool AgregaUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                // Encriptamos la contraseña
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
                        Console.WriteLine("Usuario creado exitosamente");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
                        return false;
                    }
                }

            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-AgregaUsuario] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-AgregaUsuario] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-UsuarioImplementacion-AgregaUsuario] Error la tarea fue cancelada");
                return false;
            }
        }
    }
}
