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
    }
}
