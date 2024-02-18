namespace ProyectoFinalDIW.Models
{
	/// <summary>
	/// Entidad RelOrdenCarritoDTO que hace referencia a la tabla Rel_Orden_Carritos de la base de datos
	/// </summary>
	/// autor: Fran Gallego
	/// Fecha: 18/02/2024
	public class RelOrdenCarritoDTO
	{
		// Atributos

		private long id_rel_orden_carrito;
		private long id_orden;
		private long id_carrito;

		// Constructores


		// Getter y Setter

		public long Id_rel_orden_carrito { get => id_rel_orden_carrito; set => id_rel_orden_carrito = value; }
		public long Id_orden { get => id_orden; set => id_orden = value; }
		public long Id_carrito { get => id_carrito; set => id_carrito = value; }

	}
}
