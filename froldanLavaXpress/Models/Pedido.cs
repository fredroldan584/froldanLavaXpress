namespace froldanLavaXpress.Models
{
    public class Pedido
    {
        // Mismos nombres que los campos que devuelve pedido.php
        public int codigo { get; set; }
        public int codigo_cliente { get; set; }
        public string fecha { get; set; } = "";
        public string servicio { get; set; } = "";
        public double cantidad { get; set; }
        public double total { get; set; }
        public string estado { get; set; } = "";
        public string? foto { get; set; }          // puede venir vacío (NULL)
        public string cliente { get; set; } = "";  // nombre del cliente (viene del JOIN)
    }
}