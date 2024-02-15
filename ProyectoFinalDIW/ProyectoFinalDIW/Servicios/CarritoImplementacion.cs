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
						Console.WriteLine("Carrito creado exitosamente");

                        return true;
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
						return false;
					}
				}
				return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool BorraCarrito(long id_carrito)
        {
            try
            {
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
						Console.WriteLine("Carrito borrado exitosamente");

						return true;
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
						return false;
					}
				}
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CarritoDTO>> ObtieneCarritoUsuario(string emailUsuario)
        {
            try
            {
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

                // Deserializa la respuesta JSON a un objeto C#
                List<CarritoDTO> listaCarrito = JsonConvert.DeserializeObject<List<CarritoDTO>>(responseData);

                // Una vez que tenemos la lista carrito y el usuario vamos a quedarnos solo con los carritos del usuario que no esten comprados
                List<CarritoDTO> listaCarritoFiltrado = listaCarrito.FindAll(x => x.EstaComprado_carrito == false && x.Id_usuario == usuarioDTO.Id_usuario).ToList();

                // Devolvemos la lista filtrada
                return listaCarritoFiltrado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public float ObtienePrecioTotalCarrito(List<CarritoDTO> listaCarrito, List<SuplementoDTO> listaSuplementos)
        {
            try
            {
                float total = 0;
                // Recorremos la lista
                foreach (var carrito in listaCarrito)
                {
                    // Obtenemos el suplemento por el id
                    float precioSuplemento = listaSuplementos.Find(x => x.Id_suplemento == carrito.ID_suplemento).Precio_suplemento;
                    
                    // Sumamos el total
                    total += carrito.Cantidad * precioSuplemento;
                }

                // Devolvemos el precio total
                return total;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
