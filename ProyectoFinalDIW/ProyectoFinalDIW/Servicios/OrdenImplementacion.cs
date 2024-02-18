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
		private UsuarioInterfaz usuarioInterfaz = new UsuarioImplementacion();
		private CarritoInterfaz carritoInterfaz = new CarritoImplementacion();
		private SuplementoInterfaz suplementoInterfaz = new SuplementoImplementacion();

		public bool ComprarCarritoUsuario(string emailUsuario)
		{
			try
			{
				// Obtenemos el usuario de la base de datos
				UsuarioDTO usuarioDTO = usuarioInterfaz.BuscaUsuarioPorEmail(emailUsuario).Result;

				// Obtenemos los carritos del usuario
				List<CarritoDTO> listaCarritos = carritoInterfaz.ObtieneCarritoUsuario(emailUsuario).Result;

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

				// Primero obtenemos el orden de la base de datos
				//OrdenDTO ordenEncontrado = ObtieneOrden(ordenDTO).Result;
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
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ordenDTO"></param>
		/// <returns></returns>
		private OrdenDTO AgregarOrden(OrdenDTO ordenDTO)
		{
			// Convertimos la ordena JSON
			string ordenJson = JsonConvert.SerializeObject(ordenDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

			try
			{
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
						Console.WriteLine("Orden creado exitosamente");

						responseData = response.Content.ReadAsStringAsync().Result;

						OrdenDTO ordenDevuelta = JsonConvert.DeserializeObject<OrdenDTO>(responseData);

						if(ordenDevuelta != null)
							return ordenDevuelta;

						return null;
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
						return null;
					}
				}
			}
			catch (ArgumentNullException)
			{
				return null;
			}
			catch (UriFormatException)
			{
				return null;
			}
			catch (AggregateException)
			{
				return null;
			}
			catch (InvalidOperationException) 
			{
				return null;
			}
			catch (HttpRequestException)
			{
				return null;
			}
			catch (TaskCanceledException)
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listaRelOrdenCarritoDTO"></param>
		/// <returns></returns>
		private bool AgregarListaRelOrdenCarrito(List<RelOrdenCarritoDTO> listaRelOrdenCarritoDTO)
		{
			// Convertimos la ordena JSON
			string listaRelOrdenCarritoJson = JsonConvert.SerializeObject(listaRelOrdenCarritoDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

			try
			{
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
						Console.WriteLine("Lista RelOrdenCarrito creado exitosamente");

						return true;
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
						return false;
					}
				}
			}
			catch (ArgumentNullException)
			{
				return false;
			}
			catch (UriFormatException)
			{
				return false;
			}
			catch (AggregateException)
			{
				return false;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
			catch (HttpRequestException)
			{
				return false;
			}
			catch (TaskCanceledException)
			{
				return false;
			}
		}

		private void ActualizaCarrito(CarritoDTO carritoDTO)
		{
			// Convertimos el carrito a  JSON
			string carritoJSON = JsonConvert.SerializeObject(carritoDTO, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

			try
			{
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
						Console.WriteLine("Carrito actualziado exitosamente");
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.ReasonPhrase}");
					}
				}
			}
			catch (ArgumentNullException)
			{
			}
			catch (UriFormatException)
			{
			}
			catch (AggregateException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (HttpRequestException)
			{
			}
			catch (TaskCanceledException)
			{
			}
		}
	}
}
