using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
    /// <summary>
    /// Implementación de la interfaz Carrito
    /// </summary>
    /// autor: Fran Gallego
    /// Fecha: 11/02/2024
    public class CarritoImplementacion : CarritoInterfaz
    {
        // Inicializamos la interfaz Usuario para usar sus métodos
        private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();

        // Inicializamos la interfaz Suplemento para usar sus métodos
        private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

        public bool AgregaSuplemento(long id_suplemento, string emailUsuario)
        {
            try
            {
                // Log
                Util.LogInfo("CarritoImplementacion", "AgregaSuplemento", "Ha entrado en AgregaSuplemento");

                // Obtenemos el usuario por el email
                UsuarioDTO usuarioDTO = usuarioInterfaz.BuscaUsuarioPorEmail(emailUsuario).Result;

                // Construimos un objeto carritoDTO
                CarritoDTO carritoDTO = new CarritoDTO();
                carritoDTO.ID_suplemento = id_suplemento;
                carritoDTO.EstaComprado_carrito = false;
                carritoDTO.Id_usuario = usuarioDTO.Id_usuario;
                carritoDTO.Cantidad = 1;

                // Convertimos el carrito a JSON
                string carritoJson = JsonConvert.SerializeObject(carritoDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
				{
					// Url a la que haremos el POST
					Uri url = new Uri("https://localhost:7029/api/CarritoControlador");

					// Configurar la solicitud HTTP POST
					HttpResponseMessage response = client.PostAsync(url, new StringContent(carritoJson, Encoding.UTF8, "application/json")).Result;

					// Verificar la respuesta del servidor
					if (response.IsSuccessStatusCode)
					{
                        // Log
                        Util.LogInfo("CarritoImplementacion", "AgregaSuplemento", "Carrito creado correctamente");

                        return true;
					}
					else
					{
                        // Log
                        Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito");

						return false;
					}
				}
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "AgregaSuplemento", "No se ha podido crear el carrito debido a la cancelacion de una tarea");
                return false;
            }
        }

        public bool BorraCarrito(long id_carrito)
        {
            try
            {
                // Log
                Util.LogInfo("CarritoImplementacion", "BorraCarrito", "Ha entrado en BorraCarrito");

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
				{
					// Url a la que haremos el POST
					Uri url = new Uri("https://localhost:7029/api/CarritoControlador/" + id_carrito);

					// Configurar la solicitud HTTP POST
					HttpResponseMessage response = client.DeleteAsync(url).Result;

					// Verificar la respuesta del servidor
					if (response.IsSuccessStatusCode)
					{
                        // Log
                        Util.LogInfo("CarritoImplementacion", "BorraCarrito", "Carrito borrado correctamente");

                        return true;
					}
					else
					{
                        // Log
                        Util.LogInfo("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito");
                        return false;
					}
				}
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "BorraCarrito", "No se ha podido borrar el carrito debido a la cancelacion de una tarea");
                return false;
            }
        }

        public async Task<List<CarritoDTO>> ObtieneCarritoUsuario(string emailUsuario)
        {
            try
            {
                // Log
                Util.LogInfo("CarritoImplementacion", "ObtieneCarritoUsuario", "Ha entrado en ObtieneCarritoUsuario");

                // Obtenemos el usuario por el email
                UsuarioDTO usuarioDTO = usuarioInterfaz.BuscaUsuarioPorEmail(emailUsuario).Result;

                // Ahora que tenemos el usuario vamos a obtener el carrito
                string apiUrl = "https://localhost:7029/api/CarritoControlador";

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

                // Deserializa la respuesta JSON a una lista de CarritoDTO
                List<CarritoDTO> listaCarrito = JsonConvert.DeserializeObject<List<CarritoDTO>>(responseData);

                // Una vez que tenemos la lista carrito y el usuario vamos a quedarnos solo con los carritos del usuario que no esten comprados
                List<CarritoDTO> listaCarritoFiltrado = listaCarrito.FindAll(x => x.EstaComprado_carrito == false && x.Id_usuario == usuarioDTO.Id_usuario).ToList();

                // Devolvemos la lista filtrada
                return listaCarritoFiltrado;
            }
            catch (AggregateException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del usuario debido a un excepcion agregada");
                return null;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del usuario debido a un argumento nulo");
                return null;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del usuario debido a un formato incorrecto del URI");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del usuario debido a una operacion invalida");
                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del uusario debido a un error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtieneCarritoUsuario", "No se ha podido obtener los carritos del usuario debido a la cancelacion de una tarea");
                return null;
            }
        }

        public float ObtienePrecioTotalCarrito(List<CarritoDTO> listaCarrito, List<SuplementoDTO> listaSuplementos)
        {
            try
            {
                // Log
                Util.LogInfo("CarritoImplementacion", "ObtienePrecioTotalCarrito", "Ha entrado en ObtienePrecioTotalCarrito");

                float total = 0;
                // Recorremos la lista
                foreach (var carrito in listaCarrito)
                {
                    // Obtenemos el precio del suplemento por el id del suplemento
                    float precioSuplemento = listaSuplementos.Find(x => x.Id_suplemento == carrito.ID_suplemento).Precio_suplemento;
                    
                    // Sumamos el total
                    total += carrito.Cantidad * precioSuplemento;
                }

                // Devolvemos el precio total
                return total;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("CarritoImplementacion", "ObtienePrecioTotalCarrito", "No se ha podido calcualr el precio total del carrito debido a un argumento nulo");
                return 0;
            }
        }
    }
}
