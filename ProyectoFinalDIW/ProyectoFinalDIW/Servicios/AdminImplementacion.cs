using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Admin
    /// </summary>
    public class AdminImplementacion : AdminInterfaz
    {
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
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosUsuarios] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosUsuarios] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosUsuarios] Error la tarea fue cancelada");
                return null;
            }
        }

        public async Task<List<SuplementoDTO>> ObtieneTodosLosSuplementos()
        {
            // URL que se desea consultar
            string apiUrl = "https://localhost:7029/api/SuplementoControlador";

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

                // Deserializa la respuesta JSON a una List de objetos Suplemento
                List<SuplementoDTO> listaSuplementos = JsonConvert.DeserializeObject<List<SuplementoDTO>>(responseData);

                // Devolvemos la lista
                return listaSuplementos;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosSuplementos] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosSuplementos] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ObtieneTodosLosSuplementos] Error la tarea fue cancelada");
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
                Console.WriteLine("[ERROR-AdminImplementacion-BorrarUsuario] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BorrarUsuario] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BorrarUsuario] Error la tarea fue cancelada");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<UsuarioDTO> BuscaUsuarioPorId(int id)
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
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaUsuarioPorId] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaUsuarioPorId] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaUsuarioPorId] Error la tarea fue cancelada");
                return null;
            }
        }
    }
}
