namespace ProyectoFinalDIW.Models
{
    public class UsuarioDTO
    {
        // Atributos

        private long id_usuario;
        private string nombre_usuario;
        private string tlf_usuario;
        private string email_usuario;
        private string psswd_usuario;
        private long id_acceso = 1;
        private bool estaActivado_usuario = false;

        // Constructores

        public UsuarioDTO(long id_usuario, string nombre_usuario, string tlf_usuario, string email_usuario, string psswd_usuario)
        {
            this.id_usuario = id_usuario;
            this.nombre_usuario = nombre_usuario;
            this.tlf_usuario = tlf_usuario;
            this.email_usuario = email_usuario;
            this.psswd_usuario = psswd_usuario;
            id_acceso = 1;
            estaActivado_usuario = false;
        }

        public UsuarioDTO(string nombre_usuario, string tlf_usuario, string email_usuario, string psswd_usuario)
        {
            this.nombre_usuario = nombre_usuario;
            this.tlf_usuario = tlf_usuario;
            this.email_usuario = email_usuario;
            this.psswd_usuario = psswd_usuario;
            id_acceso = 1;
            estaActivado_usuario = false;
        }

        public UsuarioDTO()
        {
        }

        // Getter y Setter

        public long Id_usuario { get => id_usuario; set => id_usuario = value; }
        public string Nombre_usuario { get => nombre_usuario; set => nombre_usuario = value; }
        public string Tlf_usuario { get => tlf_usuario; set => tlf_usuario = value; }
        public string Email_usuario { get => email_usuario; set => email_usuario = value; }
        public string Psswd_usuario { get => psswd_usuario; set => psswd_usuario = value; }
        public long Id_acceso { get => id_acceso; set => id_acceso = value; }
        public bool EstaActivado_usuario { get => estaActivado_usuario; set => estaActivado_usuario = value; }

    }
}
