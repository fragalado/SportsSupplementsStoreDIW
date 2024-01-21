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
    }
}
