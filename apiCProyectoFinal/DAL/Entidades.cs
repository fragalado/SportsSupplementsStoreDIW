using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Usuario
    {
        // Atributos

        [Key] // Indica que es el PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que es autoincrementable
        public long id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public string tlf_usuario { get; set; }
        public string email_usuario { get; set; }
        public string psswd_usuario { get; set; }
        public long id_acceso { get; set; }
        [ForeignKey("id_acceso")]
        public Acceso? acceso { get; set; }
        public bool estaActivado_usuario { get; set; }
        public string? rutaImagen_usuario { get; set; }

        public List<Token_Tabla>? listaToken { get; set; }
        public List<Carrito>? listaCarrito { get; set; }
        public List<Orden>? listaOrden { get; set; }

        // Constructores
        public Usuario(string nombre_usuario, string tlf_usuario, string email_usuario, string psswd_usuario, long id_acceso, bool estaActivado_usuario, string? rutaImagen_usuario)
        {
            this.nombre_usuario = nombre_usuario;
            this.tlf_usuario = tlf_usuario;
            this.email_usuario = email_usuario;
            this.psswd_usuario = psswd_usuario;
            this.id_acceso = id_acceso;
            this.estaActivado_usuario = estaActivado_usuario;
            this.rutaImagen_usuario = rutaImagen_usuario;
        }

        public Usuario() {}
    }

    public class Acceso
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_acceso { get; set; }
        public string cod_acceso { get; set; }
        public string desc_acceso { get; set; }

        public List<Usuario>? listaUsuarios { get; set; }

        // Constructores
        public Acceso(string cod_acceso, string desc_acceso)
        {
            this.cod_acceso = cod_acceso;
            this.desc_acceso = desc_acceso;
        }

        public Acceso() { }
    }

    public class Token_Tabla
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_token { get; set; }
        public string cod_token { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime fch_fin_token { get; set; }
        public long id_usuario { get; set; }
        [ForeignKey("id_usuario")]
        public Usuario? usuario { get; set; }

        // Constructores
        public Token_Tabla(string cod_token, DateTime fch_fin_token, long id_usuario)
        {
            this.cod_token = cod_token;
            this.fch_fin_token = fch_fin_token;
            this.id_usuario = id_usuario;
        }

        public Token_Tabla() { }
    }

    public class Suplemento
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_suplemento { get; set; }
        public string nombre_suplemento { get; set; }
        public string desc_suplemento { get; set; }
        public float precio_suplemento { get; set; }
        public string tipo_suplemento { get; set; }
        public string marca_suplemento { get; set; }
        public string rutaImagen_suplemento { get; set; }

        public List<Carrito>? listaCarrito { get; set; }

        // Constructores

        public Suplemento(string nombre_suplemento, string desc_suplemento, float precio_suplemento, string tipo_suplemento, string marca_suplemento, string rutaImagen_suplemento)
        {
            this.nombre_suplemento = nombre_suplemento;
            this.desc_suplemento = desc_suplemento;
            this.precio_suplemento = precio_suplemento;
            this.tipo_suplemento = tipo_suplemento;
            this.marca_suplemento = marca_suplemento;
            this.rutaImagen_suplemento = rutaImagen_suplemento;
        }

        public Suplemento() { }
    }

    public class Carrito
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_carrito { get; set; }
        public long id_usuario { get; set; }
        [ForeignKey("id_usuario")]
        public Usuario? usuario { get; set; }
        public long id_suplemento { get; set; }
        [ForeignKey("id_suplemento")]
        public Suplemento? suplemento { get; set; }
        public int cantidad { get; set; }

        public List<Rel_Orden_Carrito>? listaRelacionOrdenCarrito { get; set; }

        // Constructores
        public Carrito(long id_usuario, long id_suplemento, int cantidad)
        {
            this.id_usuario = id_usuario;
            this.id_suplemento = id_suplemento;
            this.cantidad = cantidad;
        }

        public Carrito() { }
    }

    public class Orden
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_orden { get; set; }
        public long id_usuario { get; set; }
        [ForeignKey("id_usuario")]
        public Usuario? usuario { get; set; }
        public float precio_orden { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime fch_orden { get; set; }

        public List<Rel_Orden_Carrito>? listaRelacionOrdenCarrito { get; set; }

        // Constructores

        public Orden(long id_usuario, float precio_orden, DateTime fch_orden)
        {
            this.id_usuario = id_usuario;
            this.precio_orden = precio_orden;
            this.fch_orden = fch_orden;
        }

        public Orden() { }
    }

    public class Rel_Orden_Carrito 
    {
        // Atributos

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_rel_orden_carrito { get; set; }
        public long id_orden { get; set; }
        [ForeignKey("id_orden")]
        public Orden? orden { get; set; }
        public long id_carrito { get; set; }
        public Carrito? carrito { get; set; }

        // Constructores
        public Rel_Orden_Carrito(long id_orden, long id_carrito)
        {
            this.id_orden = id_orden;
            this.id_carrito = id_carrito;
        }

        public Rel_Orden_Carrito() { }
    }
}