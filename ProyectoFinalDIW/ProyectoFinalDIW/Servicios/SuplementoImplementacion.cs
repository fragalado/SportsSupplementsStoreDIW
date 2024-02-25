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
            try
            {
                // Log
                Util.LogInfo("SuplementoImplementacion", "ObtieneTodosLosSuplementos", "Ha entrado en ObtieneTodosLosSuplementos");
                
                // URL que se desea consultar
                string apiUrl = "https://localhost:7029/api/SuplementoControlador";
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

                // Deserializa la respuesta JSON a una Lista de objetos Suplemento
                List<SuplementoDTO> listaSuplementos = JsonConvert.DeserializeObject<List<SuplementoDTO>>(responseData);

                // Devolvemos la lista
                return listaSuplementos;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ObtieneTodosLosSuplementos", "No se ha podido agregar la orden debido a una operacion invalida");
                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ObtieneTodosLosSuplementos", "No se ha podido agregar la orden debido a un error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ObtieneTodosLosSuplementos", "No se ha podido agregar la orden debido a la cancelacion de una tarea");
                return null;
            }
        }

        public async Task<SuplementoDTO> BuscaSuplementoPorId(long id)
        {
            try
            {
                // Log
                Util.LogInfo("SuplementoImplementacion", "BuscaSuplementoPorId", "Ha entrado en BuscaSuplementoPorId");

                // URL de la API que deseas consultar
                string apiUrl = "https://localhost:7029/api/SuplementoControlador/" + id;

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

                // Deserializa la respuesta JSON a un objeto SuplementoDTO
                SuplementoDTO suplementoEncontrado = JsonConvert.DeserializeObject<SuplementoDTO>(responseData);

                // Ahora comprobamos si el suplemento es distinto de null, si lo es lo devolvemos
                if (suplementoEncontrado != null)
                {
                    return suplementoEncontrado;
                }

                Console.WriteLine("No hay coincidencia");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BuscaSuplementoPorId", "No se ha podido buscar el suplemento por el id debido a una operacion invalida");

                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BuscaSuplementoPorId", "No se ha podido buscar el suplemento por el id debido a un error en la solicitud HTTP");

                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BuscaSuplementoPorId", "No se ha podido buscar el suplemento por el id debido a la cancelacion de una tarea");

                return null;
            }
        }

        public bool BorraSuplementoPorId(int id)
        {
            try
            {
                // Log
                Util.LogInfo("SuplementoImplementacion", "BorraSuplementoPorId", "Ha entrado en BorraSuplementoPorId");

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
                        // Log
                        Util.LogInfo("SuplementoImplementacion", "BorraSuplementoPorId", "Suplemento eliminado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido eliminar el suplemento");

                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "BorraSuplementoPorId", "No se ha podido borrar el suplemento debido a la cancelacion de una tarea");
                return false;
            }
        }

        public bool ActualizaSuplemento(SuplementoDTO suplemento)
        {
            try
            {
                // Log
                Util.LogInfo("SuplementoImplementacion", "ActualizaSuplemento", "Ha entrado en ActualizaSuplemento");

                // Obtenemos el suplemento de la base de datos
                SuplementoDTO suplementoEncontrado = BuscaSuplementoPorId(suplemento.Id_suplemento).Result;

                // Actualizamos los datos del suplementoEncontrado con los datos del suplemento pasado por parámetros
                suplementoEncontrado.Nombre_suplemento = suplemento.Nombre_suplemento;
                suplementoEncontrado.Desc_suplemento = suplemento.Desc_suplemento;
                suplementoEncontrado.Precio_suplemento = suplemento.Precio_suplemento;
                suplementoEncontrado.Tipo_suplemento = suplemento.Tipo_suplemento;
                if (suplemento.RutaImagen_suplemento != null)
                    suplementoEncontrado.RutaImagen_suplemento = suplemento.RutaImagen_suplemento;
                
                // Convertimos el suplemento a json
                string suplementoJson = JsonConvert.SerializeObject(suplementoEncontrado, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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
                        // Log
                        Util.LogInfo("SuplementoImplementacion", "ActualizaSuplemento", "Suplemento actualizado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento");

                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "ActualizaSuplemento", "No se ha podido actualizar el suplemento debido a la cancelacion de una tarea");
                return false;
            }
        }

        public bool AgregaSuplemento(SuplementoDTO suplemento)
        {
            try
            {
                // Log
                Util.LogInfo("SuplementoImplementacion", "AgregaSuplemento", "Ha entrado en AgregaSuplemento");

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
                        // Log
                        Util.LogInfo("SuplementoImplementacion", "AgregaSuplemento", "Suplemento agregado correctamente");

                        return true;
                    }
                    else
                    {
                        // Log
                        Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento");

                        return false;
                    }
                }
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("SuplementoImplementacion", "AgregaSuplemento", "No se ha podido agregar el suplemento debido a la cancelacion de una tarea");
                return false;
            }
        }
    }
}
