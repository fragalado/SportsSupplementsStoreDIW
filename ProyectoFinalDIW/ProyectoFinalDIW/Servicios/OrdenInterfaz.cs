namespace ProyectoFinalDIW.Servicios
{
	/// <summary>
	/// Interfaz que define los métodos que darán servicio a Orden
	/// </summary>
	/// autor: Fran Gallego
	/// Fecha: 17/02/2024
	public interface OrdenInterfaz
	{
		/// <summary>
		/// Realiza la compra de los carritos del usuario
		/// </summary>
		/// <param name="emailUsuario">Email del usuario</param>
		/// <returns>Devuelve true si se ha comprado la compra o false si no.</returns>
		public bool ComprarCarritoUsuario(String emailUsuario);
	}
}
