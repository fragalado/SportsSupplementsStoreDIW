using Newtonsoft.Json;
using ProyectoFinalDIW.Models;
using System.Text;

namespace ProyectoFinalDIW.Servicios
{
	/// <summary>
	/// Implementación de la interfaz Orden
	/// </summary>
	/// autor: Fran Gallego
	/// Fecha: 17/02/2024
	public class OrdenImplementacion : OrdenInterfaz
	{
		// Inicializamos las interfaces para utilizar sus métodos
		private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();
		private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();
		private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

		public bool ComprarCarritoUsuario(string emailUsuario)
		{
			try
			{
                // Log
                Util.LogInfo("OrdenImplementacion", "ComprarCarritoUsuario", "Ha entrado en ComprarCarritoUsuario");

                // Obtenemos el usuario de la base de datos
                UsuarioDTO usuarioDTO = usuarioInterfaz.BuscaUsuarioPorEmail(emailUsuario).Result;

				// Obtenemos los carritos del usuario
				List<CarritoDTO> listaCarritos = carritoInterfaz.ObtieneCarritoUsuario(emailUsuario).Result;

				// Comprobamos si la lista no esta vacia
				if (listaCarritos.Count == 0)
					return false;

				// Obtenemos todos los suplementos
				List<SuplementoDTO> listaSuplementos = suplementoInterfaz.ObtieneTodosLosSuplementos().Result;

				// Obtenemos el precio total del carrito
				float precioTotal = carritoInterfaz.ObtienePrecioTotalCarrito(listaCarritos, listaSuplementos);

				// Creamos un objeto OrdenDTO
				OrdenDTO ordenDTO = new OrdenDTO(usuarioDTO.Id_usuario, precioTotal, DateTime.Now);

				// Insertamos el objeto ordenDTO en la base de datos
				OrdenDTO ordenDevuelta = AgregarOrden(ordenDTO);

				// Comprobamos si se ha hecho el insert correctamente
				if (ordenDevuelta == null)
					return false;

				// Si llega aqui es porque es correcto
				// Ahora tendremos que crear objetos RelOrdenCarrito y añadirlos a la lista
				List<RelOrdenCarritoDTO> listaRelOrdenCarritoDTO = new List<RelOrdenCarritoDTO>();

				// Recorremos la lista de carritos
				foreach (var carrito in listaCarritos)
				{
					// Creamos un objeto RelOrdenCarrito
					RelOrdenCarritoDTO relOrdenCarritoDTO = new RelOrdenCarritoDTO();
					relOrdenCarritoDTO.Id_carrito = carrito.Id_carrito;
					relOrdenCarritoDTO.Id_orden = ordenDevuelta.Id_orden;

					// Añadimos el objeto creado a la lista
					listaRelOrdenCarritoDTO.Add(relOrdenCarritoDTO);
				}

				// Hacemos el POST de la lista
				bool okLista = AgregarListaRelOrdenCarrito(listaRelOrdenCarritoDTO);

                if (okLista)
				{
					// Ahora hacemos el update de los carritos
					foreach(var carrito in listaCarritos)
					{
						// Cambiamos el estado del carrito
						carrito.EstaComprado_carrito = true;

						// Actualizamos el carrito en la base de datos
						ActualizaCarrito(carrito);
					}

					// Devolvemos true
					return true;
				}
					

				return false;
			}
			catch (AggregateException)
			{
                // Log
                Util.LogError("OrdenImplementacion", "ComprarCarritoUsuario", "No se ha podido comprar el carrito del usuario debido a una excepcion agregada");
                return false;
			}
		}

		/// <summary>
		/// Método que realiza el INSERT de un objeto OrdenDTO a la base de datos
		/// </summary>
		/// <param name="ordenDTO">Objeto OrdenDTO a agregar</param>
		/// <returns>Devuelve el objeto OrdenDTO devuelto o null en caso de error</returns>
		private OrdenDTO AgregarOrden(OrdenDTO ordenDTO)
		{
			try
			{
                // Log
                Util.LogInfo("OrdenImplementacion", "AgregarOrden", "Ha entrado en AgregarOrden");

                // Convertimos la ordena JSON
			    string ordenJson = JsonConvert.SerializeObject(ordenDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Variable donde guardaremos los datos de la respuesta
                string responseData;

				// Configurar la solicitud HTTP
				using (HttpClient client = new HttpClient())
				{
					// Url a la que haremos el POST
					Uri url = new Uri("https://localhost:7029/api/OrdenControlador");

					// Configurar la solicitud HTTP POST
					HttpResponseMessage response = client.PostAsync(url, new StringContent(ordenJson, Encoding.UTF8, "application/json")).Result;

					// Verificar la respuesta del servidor
					if (response.IsSuccessStatusCode)
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "AgregarOrden", "Orden creada exitosamente");

                        // Obtenemos los datos de la respuesta
                        responseData = response.Content.ReadAsStringAsync().Result;

						// Convertimos los datos de la respuesta a un objeto OrdenDTO
						OrdenDTO ordenDevuelta = JsonConvert.DeserializeObject<OrdenDTO>(responseData);

						// Si el objeto OrdenDTO es distinto de null lo devolvemos
						if(ordenDevuelta != null)
							return ordenDevuelta;

						return null;
					}
					else
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "AgregarOrden", "No se ha podido crear la orden correctamente");
                        return null;
					}
				}
			}
            catch (AggregateException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a un excepcion agregada");
                return null;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a un argumento nulo");
                return null;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a un formato incorrecto del URI");
                return null;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a una operacion invalida");
                return null;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a un error en la solicitud HTTP");
                return null;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarOrden", "No se ha podido agregar la orden debido a la cancelacion de una tarea");
                return null;
            }
        }

		/// <summary>
		/// Método que realiza el INSERT de un objeto RelOrdenCarritoDTO a la base de datos
		/// </summary>
		/// <param name="listaRelOrdenCarritoDTO">Lista de objetos RelOrdenCarritoDTO</param>
		/// <returns>Devuelve true si se ha insertado corretamente o false en caso contrario</returns>
		private bool AgregarListaRelOrdenCarrito(List<RelOrdenCarritoDTO> listaRelOrdenCarritoDTO)
		{
			try
			{
                // Log
                Util.LogInfo("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "Ha entrado en AgregarListaRelOrdenCarrito");

                // Convertimos la ordena JSON
                string listaRelOrdenCarritoJson = JsonConvert.SerializeObject(listaRelOrdenCarritoDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
				{
					// Url a la que haremos el POST
					Uri url = new Uri("https://localhost:7029/api/RelOrdenCarritoControlador");

					// Configurar la solicitud HTTP POST
					HttpResponseMessage response = client.PostAsync(url, new StringContent(listaRelOrdenCarritoJson, Encoding.UTF8, "application/json")).Result;

					// Verificar la respuesta del servidor
					if (response.IsSuccessStatusCode)
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "Lista RelOrdenCarrito creada correctamente");

                        return true;
					}
					else
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito correctamente");
                        return false;
					}
				}
			}
            catch (AggregateException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a un excepcion agregada");
                return false;
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a un argumento nulo");
                return false;
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a un formato incorrecto del URI");
                return false;
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a una operacion invalida");
                return false;
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a un error en la solicitud HTTP");
                return false;
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "AgregarListaRelOrdenCarrito", "No se ha podido crear la lista RelOrdenCarrito debido a la cancelacion de una tarea");
                return false;
            }
        }

		/// <summary>
		/// Método que actualiza un carrito en la base de datos
		/// </summary>
		/// <param name="carritoDTO">Objeto CarritoDTO a actualizar</param>
		private void ActualizaCarrito(CarritoDTO carritoDTO)
		{
			try
			{
                // Log
                Util.LogInfo("OrdenImplementacion", "ActualizaCarrito", "Ha entrado en ActualizaCarrito");

                // Convertimos el carrito a  JSON
                string carritoJSON = JsonConvert.SerializeObject(carritoDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // Configurar la solicitud HTTP
                using (HttpClient client = new HttpClient())
				{
					// Url a la que haremos el PUT
					Uri url = new Uri("https://localhost:7029/api/CarritoControlador");

					// Configurar la solicitud HTTP PUT
					HttpResponseMessage response = client.PutAsync(url, new StringContent(carritoJSON, Encoding.UTF8, "application/json")).Result;

					// Verificar la respuesta del servidor
					if (response.IsSuccessStatusCode)
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "ActualizaCarrito", "Carrito actualizado correctamente");
                    }
					else
					{
                        // Log
                        Util.LogInfo("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito");
                    }
				}
			}
            catch (AggregateException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a un excepcion agregada");
            }
            catch (ArgumentNullException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a un argumento nulo");
            }
            catch (UriFormatException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a un formato incorrecto del URI");
            }
            catch (InvalidOperationException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a una operacion invalida");
            }
            catch (HttpRequestException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a un error en la solicitud HTTP");
            }
            catch (TaskCanceledException)
            {
                // Log
                Util.LogError("OrdenImplementacion", "ActualizaCarrito", "No se ha podido actualizar el carrito debido a la cancelacion de una tarea");
            }
        }
	}
}
