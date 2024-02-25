using Newtonsoft.Json;
using ProyectoFinalDIW.Models;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Token
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public class TokenImplementacion : TokenInterfaz
    {
        public async Task<TokenDTO> ObtenerToken(string token)
        {
            try
            {
                // Log
                Util.LogInfo("TokenImplementacion", "ObtenerToken", "Ha entrado en ObtenerToken");

                // URL de la API que deseas consultar
                string apiUrl = "https://localhost:7029/api/TokenControlador/" + token;

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

                // Deserializa la respuesta JSON a un objeto TokenDTO
                TokenDTO tokenEncontrado = JsonConvert.DeserializeObject<TokenDTO>(responseData);

                // Ahora comprobamos si es distinto de null
                if (tokenEncontrado != null)
                {
                    return tokenEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("TokenImplementacion", "ObtenerToken", "No se ha podido obtener el token debido a una operacion invalida");
                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("TokenImplementacion", "ObtenerToken", "No se ha podido obtener el token debido a un error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("TokenImplementacion", "ObtenerToken", "No se ha podido obtener el token debido a la cancelacion de una tarea");
                return null;
            }
        }
    }
}
