using Newtonsoft.Json;
using ProyectoFinalDIW.Models;

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
    }
}
