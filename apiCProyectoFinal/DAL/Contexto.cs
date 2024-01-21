using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Declaración de la clase Contexto que hereda de DbContext.
    /// (DbContext es parte de EntityFramework Core y éste es responsable de establecer la conexión con la base de datos y realizar operaciones)
    /// </summary>
    public class Contexto : DbContext
    {
        /// <summary>
        /// Constructor que toma DbContextOptions como parámetros.
        /// (DbContextOptions sirve para configurar el comportamiento del contexto de la base de datos).
        /// </summary>
        /// <param name="options">Opciones de configuración para el contexto de la base de datos.</param>
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        // Los DbSet representan la tabla en la base de datos y sirven para realizar operaciones CRUD.
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Acceso> Accesos { get; set; }
        public DbSet<Token_Tabla> Tokens { get; set; }
        public DbSet<Suplemento> Suplementos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Rel_Orden_Carrito> Rel_Orden_Carritos { get; set; }
        public DbSet<Orden> Ordenes { get;set; }

    }
}
