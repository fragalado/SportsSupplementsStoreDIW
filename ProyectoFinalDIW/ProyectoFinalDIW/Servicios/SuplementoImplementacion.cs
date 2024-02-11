using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Suplemento
    /// </summary>
    /// author: Fran Gallego
    /// Fecha: 07/02/2024
    public class SuplementoImplementacion : SuplementoInterfaz
    {
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
                List<SuplementoDTO> listaSuplementos = ConviertePrecioSuplementoAFloat(JsonConvert.DeserializeObject<List<SuplementoDTO>>(responseData));

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

        private List<SuplementoDTO> ConviertePrecioSuplementoAFloat(List<SuplementoDTO> listaSuplementos)
        {
            try
            {
                // Recorremos los suplementos
                foreach (var suplemento in listaSuplementos)
                {
                    // Asignamos el nuevo precio
                    suplemento.Precio_suplemento = suplemento.Precio_suplemento / 100;
                }

                // Devolvemos la lista
                return listaSuplementos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<SuplementoDTO> BuscaSuplementoPorId(long id)
        {
            // URL de la API que deseas consultar
            string apiUrl = "https://localhost:7029/api/SuplementoControlador/" + id;

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
                SuplementoDTO suplementoEncontrado = JsonConvert.DeserializeObject<SuplementoDTO>(responseData);

                // Ahora comprobamos si el suplemento es distinto de null, si lo es lo devolvemos
                if (suplementoEncontrado != null)
                {
                    return suplementoEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaSuplementoPorId] Error operación no válida");
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaSuplementoPorId] Error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BuscaSuplementoPorId] Error la tarea fue cancelada");
                return null;
            }
        }

        public bool BorraSuplementoPorId(int id)
        {
            try
            {
                // Buscamos el suplemento por el id
                SuplementoDTO suplementoEncontrado = BuscaSuplementoPorId(id).Result;

                if (suplementoEncontrado == null)
                {
                    // No se ha encontrado ningún suplemento con el id introducido
                    // Luego devolvemos false
                    return false;
                }

                // Si existe el suplementoEncontrado lo borramos de la base de datos
                // Configuramos la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el DELETE
                    Uri url = new Uri("https://localhost:7029/api/SuplementoControlador/" + id);

                    // Configurar la solicitud HTTP DELETE
                    HttpResponseMessage response = client.DeleteAsync(url).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("SuplementoEncontrado eliminado exitosamente");

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
                Console.WriteLine("[ERROR-AdminImplementacion-BorraSuplementoPorId] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BorraSuplementoPorId] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-BorraSuplementoPorId] Error la tarea fue cancelada");
                return false;
            }
        }

        public bool ActualizaSuplemento(SuplementoDTO suplemento)
        {
            try
            {
                // Convertimos el suplemento a json
                string suplementoJson = JsonConvert.SerializeObject(suplemento, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configuramos la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el UPDATE
                    Uri url = new Uri("https://localhost:7029/api/SuplementoControlador");

                    // Configurar la solicitud HTTP UPDATE
                    HttpResponseMessage response = client.PutAsync(url, new StringContent(suplementoJson, Encoding.UTF8, "application/json")).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Suplemento actualizado exitosamente");

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
                Console.WriteLine("[ERROR-AdminImplementacion-ActualizaSuplemento] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ActualizaSuplemento] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-ActualizaSuplemento] Error la tarea fue cancelada");
                return false;
            }
        }

        public bool AgregaSuplemento(SuplementoDTO suplemento)
        {
            try
            {
                // Convertimos el suplemento a json
                string suplementoJson = JsonConvert.SerializeObject(suplemento, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configuramos la solicitud HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Url a la que haremos el POST
                    Uri url = new Uri("https://localhost:7029/api/SuplementoControlador");

                    // Configurar la solicitud HTTP POST
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(suplementoJson, Encoding.UTF8, "application/json")).Result;

                    // Verificar la respuesta del servidor
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Suplemento agregado exitosamente");

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
                Console.WriteLine("[ERROR-AdminImplementacion-AgregaSuplemento] Error operación no válida");
                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-AgregaSuplemento] Error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("[ERROR-AdminImplementacion-AgregaSuplemento] Error la tarea fue cancelada");
                return false;
            }
        }
    }
}
