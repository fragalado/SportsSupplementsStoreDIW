namespace ProyectoFinalDIW.Models
{
    /// <summary>
    /// Clase Token que representará a la entidad Token de la base de datos
    /// </summary>
    /// autor: Fran Gallego
    public class TokenDTO
    {
        // Atributos

        private long id_token;
        private string cod_token;
        private DateTime fch_fin_token;
        private long id_usuario;

        // Constructores

        public TokenDTO(string cod_token, DateTime fch_fin_token, long id_usuario)
        {
            this.cod_token = cod_token;
            this.fch_fin_token = fch_fin_token;
            this.id_usuario = id_usuario;
        }

        public TokenDTO()
        {
        }

        // Getter y Setter

        public long Id_token { get => id_token; set => id_token = value; }
        public string Cod_token { get => cod_token; set => cod_token = value; }
        public DateTime Fch_fin_token { get => fch_fin_token; set => fch_fin_token = value; }
        public long Id_usuario { get => id_usuario; set => id_usuario = value; }
    }
}
