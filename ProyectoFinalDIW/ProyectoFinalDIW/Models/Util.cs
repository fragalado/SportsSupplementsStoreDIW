using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoFinalDIW.Models
{
    public class Util
    {
        public static string EncriptarContra(string password)
        {
            StringBuilder hexString = new StringBuilder();

            try
            {
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
                Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Algoritmo nulo.");
            }
            catch (TargetInvocationException e)
            {
                Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Error durante la invocación del método.");
            }
            catch (ObjectDisposedException e) 
            {
                Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Error el objeto está eliminado (disposed).");
            }
            catch (FormatException e)
            {
                Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Error formato no válido.");
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Error valor del argumento fuera del rango permitido.");
            }

            return hexString.ToString();
        }

        /// <summary>
        /// Método para controlar la sesión
        /// </summary>
        /// <param name="context">Contexto</param>
        /// <returns>Devuelve true si esta iniciada sesión o false si no</returns>
        public static bool ControlaSesion(HttpContext context)
        {
            try
            {
                string acceso = context.Session.GetString("acceso");

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
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool ControlaSesionAdmin(HttpContext context)
        {
            try
            {
                string acceso = context.Session.GetString("acceso");

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
                return false;
            }
        }
    }
}
