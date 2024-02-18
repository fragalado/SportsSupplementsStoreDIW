namespace ProyectoFinalDIW.Models
{
	/// <summary>
	/// Clase OrdenDTO que hace referencia a la tabla Orden de la base de datos
	/// </summary>
	/// autor: Fran Gallego
	/// Fecha: 17/02/2024
	public class OrdenDTO
	{
		// Atributos

		private long id_orden;
		private long id_usuario;
		private float precio_orden;
		private DateTime fch_orden;

		// Constructores

		public OrdenDTO(long id_usuario, float precio_orden, DateTime fch_orden)
		{
			this.id_usuario = id_usuario;
			this.precio_orden = precio_orden;
			this.fch_orden = fch_orden;
		}

		// Getter y Setter

		public long Id_orden { get => id_orden; set => id_orden = value; }
		public long Id_usuario { get => id_usuario; set => id_usuario = value; }
		public float Precio_orden { get => precio_orden; set => precio_orden = value; }
		public DateTime Fch_orden { get => fch_orden; set => fch_orden = value; }

	}
}
