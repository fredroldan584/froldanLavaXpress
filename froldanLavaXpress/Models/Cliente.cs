using System;
using System.Collections.Generic;
using System.Text;

namespace froldanLavaXpress.Models
{
    public class Cliente
    {
        // Mismos nombres que los campos que devuelve cliente.php
        public int codigo { get; set; }
        public string nombre { get; set; } = "";
        public string apellido { get; set; } = "";
        public string telefono { get; set; } = "";
        public string direccion { get; set; } = "";
        public double? latitud { get; set; }    // ? = puede venir vacío (NULL)
        public double? longitud { get; set; }

        // Nombre completo para mostrar en el Picker de clientes
        public string nombreCompleto => nombre + " " + apellido;
    }
}