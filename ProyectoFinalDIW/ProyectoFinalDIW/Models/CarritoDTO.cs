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

		// Constructores -> Vacio

		// Getter y Setter

		public long Id_carrito { get => id_carrito; set => id_carrito = value; }
		public long Id_usuario { get => id_usuario; set => id_usuario = value; }
		public long ID_suplemento { get => id_suplemento; set => id_suplemento = value; }
		public int Cantidad { get => cantidad; set => cantidad = value; }
		public bool EstaComprado_carrito { get => estaComprado_carrito; set => estaComprado_carrito = value; }

	}
}
