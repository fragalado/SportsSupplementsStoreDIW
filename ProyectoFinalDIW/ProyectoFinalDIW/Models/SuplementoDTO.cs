﻿namespace ProyectoFinalDIW.Models
{
    public class SuplementoDTO
    {
        // Atributos

        private long id_suplemento;
        private string nombre_suplemento;
        private string desc_suplemento;
        private long precio_suplemento;
        private string tipo_suplemento;
        private string marca_suplemento;

        // Constructores

        public SuplementoDTO(string nombre_suplemento, string desc_suplemento, long precio_suplemento, string tipo_suplemento, string marca_suplemento)
        {
            this.nombre_suplemento = nombre_suplemento;
            this.desc_suplemento = desc_suplemento;
            this.precio_suplemento = precio_suplemento;
            this.tipo_suplemento = tipo_suplemento;
            this.marca_suplemento = marca_suplemento;
        }

        public SuplementoDTO()
        {
        }

        // Getter y Setter

        public long Id_suplemento { get => id_suplemento; set => id_suplemento = value; }
        public string Nombre_suplemento { get => nombre_suplemento; set => nombre_suplemento = value; }
        public string Desc_suplemento { get => desc_suplemento; set => desc_suplemento = value; }
        public long Precio_suplemento { get => precio_suplemento; set => precio_suplemento = value; }
        public string Tipo_suplemento { get => tipo_suplemento; set => tipo_suplemento = value; }
        public string Marca_suplemento { get => marca_suplemento; set => marca_suplemento = value; }

    }
}