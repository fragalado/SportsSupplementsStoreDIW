namespace ProyectoFinalDIW.Models
{
	/// <summary>
	/// Clase CarritoDTO que hace referencia a la entidad Carrito
	/// </summary>
	/// autor: Fran Gallego
	/// Fecha: 11/02/2024
	public class CarritoDTO
	{
		// Atributos

		private long id_carrito;
		private long id_usuario;
		private long id_suplemento;
		private int cantidad;
		private bool estaComprado_carrito;

		// Constructores

		public CarritoDTO(long id_carrito, long id_usuario, long id_suplemento, int cantidad, bool estaComprado_carrito)
		{
			this.id_carrito = id_carrito;
			this.id_usuario = id_usuario;
			this.id_suplemento = id_suplemento;
			this.cantidad = cantidad;
			this.estaComprado_carrito = estaComprado_carrito;
		}

		public CarritoDTO()
		{
		}

		// Getter y Setter

		public long Id_carrito { get => id_carrito; set => id_carrito = value; }
		public long Id_usuario { get => id_usuario; set => id_usuario = value; }
		public long ID_suplemento { get => id_suplemento; set => id_suplemento = value; }
		public int Cantidad { get => cantidad; set => cantidad = value; }
		public bool EstaComprado_carrito { get => estaComprado_carrito; set => estaComprado_carrito = value; }

	}
}
