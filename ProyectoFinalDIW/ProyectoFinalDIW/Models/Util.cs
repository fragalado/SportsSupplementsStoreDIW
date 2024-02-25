using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoFinalDIW.Models
{
    /// <summary>
    /// Clase Util que contiene los métodos que se usarán varias veces en el codigo
    /// </summary>
    /// autor: Fran Gallego
    public class Util
    {
        /// <summary>
        /// Método que encripta la contraseña pasada por parametros
        /// </summary>
        /// <param name="password">Contraseña a encriptar</param>
        /// <returns>Devuelve la contraseña encriptada</returns>
        public static string EncriptarContra(string password)
        {
            StringBuilder hexString = new StringBuilder();

            try
            {
                // Log
                LogInfo("Util", "EncriptarContra", "Ha entrado en EncriptarContra");

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                    foreach (byte b in hash)
                    {
                        hexString.Append(b.ToString("x2"));
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                // Log
                LogError("Util", "EncriptarContra", "Algoritmo nulo");
            }
            catch (TargetInvocationException e)
            {
                // Log
                LogError("Util", "EncriptarContra", "durante la invocación del método");
            }
            catch (ObjectDisposedException e) 
            {
                // Log
                LogError("Util", "EncriptarContra", "el objeto está eliminado (disposed)");
            }
            catch (FormatException e)
            {
                // Log
                LogError("Util", "EncriptarContra", "formato no válido");
            }
            catch(ArgumentOutOfRangeException e)
            {
                // Log
                LogError("Util", "EncriptarContra", "valor del argumento fuera del rango permitido");
            }

            return hexString.ToString();
        }

        /// <summary>
        /// Método para controlar la sesión
        /// </summary>
        /// <param name="context">Objeto HttpContext que contiene los datos del contexto</param>
        /// <returns>Devuelve true si esta iniciada sesión o false si no</returns>
        public static bool ControlaSesion(HttpContext context)
        {
            try
            {
                // Log
                LogInfo("Util", "ControlaSesion", "Ha entrado en ControlaSesion");

                // Obtenemos el acceso del contexto
                string acceso = context.Session.GetString("acceso");

                // Si el acceso es igual a 1 o 2 quiere decir que el usuario ha iniciado sesion
                if (acceso == "1" || acceso == "2")
                {
                    // El usuario ha iniciado sesión, luego devolvemos true
                    return true;
                }

                // Si el usuario no ha iniciado sesión devolvemos false
                return false;
            }
            catch (Exception)
            {
                // Log
                LogError("Util", "ControlaSesion", "Se ha producido un error al controlar la sesion");
                return false;
            }
        }

        /// <summary>
        /// Método que controla si el usuario es admin o no
        /// </summary>
        /// <param name="context">Objeto HttpContext con los datos del contexto</param>
        /// <returns>Devuelve true si el usuario es admin o false si no</returns>
        public static bool ControlaSesionAdmin(HttpContext context)
        {
            try
            {
                // Log
                LogInfo("Util", "ControlaSesionAdmin", "Ha entrado en ControlaSesionAdmin");

                // Obtenemos el acceso del contexto
                string acceso = context.Session.GetString("acceso");

                // Comprobamos si el acceso es igual a 2, quiere decir que será Admin
                if (acceso == "2")
                {
                    // El usuario es admin
                    return true;
                }

                // Si el usuario no es admin devolvemos false
                return false;
            }
            catch (Exception)
            {
                // Log
                LogError("Util", "ControlaSesionAdmin", "Se ha producido un error al controlar la sesion para admin");

                return false;
            }
        }

        /// <summary>
        /// Método que realiza la escritura para la informacion en fichero log
        /// </summary>
        /// <param name="nombreClase">Nombre de la clase</param>
        /// <param name="nombreMetodo">Nombre del método</param>
        /// <param name="mensaje">Mensaje a escribir en el fichero</param>
        public static void LogInfo(string nombreClase, string nombreMetodo, string mensaje)
        {
            try
            {
                // Objeto StreamWriter para poder crear y escribir en un fichero de texto
                StreamWriter sw = new StreamWriter("C:\\FicherosProg\\logsC\\fichero.txt", true);

                // Escribimos
                sw.WriteLine("["+ DateTime.Now + "]-[INFO-" + nombreClase + "-" + nombreMetodo + "] Info: " + mensaje);

                // Cerramos el StreamWriter
                sw.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-Util-LogInfo] Error al escribir info en fichero");
            }
        }

        /// <summary>
        /// Método que realiza la escritura para los errores en un fichero log
        /// </summary>
        /// <param name="nombreClase">Nombre de la clase</param>
        /// <param name="nombreMetodo">Nombre del método</param>
        /// <param name="mensaje">Mensaje a escribir</param>
        public static void LogError(string nombreClase, string nombreMetodo, string mensaje)
        {
            try
            {
                // Objeto StreamWriter para poder crear y escribir en un fichero de texto
                StreamWriter sw = new StreamWriter("C:\\FicherosProg\\logsC\\fichero.txt", true);

                // Escribimos
                sw.WriteLine("["+ DateTime.Now + "]-[ERROR-" + nombreClase + "-" + nombreMetodo + "] Error: " + mensaje);

                // Cerramos el StreamWriter
                sw.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR-Util-LogError] Error al escribir error en fichero");
            }
        }
    }
}
