using System.Net;
using froldanLavaXpress.Models;

namespace froldanLavaXpress.Views;

public partial class vDetallePedido : ContentPage
{
    private const string Url = "http://192.168.2.108/lavaxpress/pedido.php";
    private int codigoPedido;

    // Recibe el pedido tocado en la lista y muestra sus datos
    public vDetallePedido(Pedido datos)
    {
        InitializeComponent();
        codigoPedido = datos.codigo;
        lblCodigo.Text = datos.codigo.ToString();
        lblCliente.Text = datos.cliente;
        lblFecha.Text = datos.fecha;
        lblServicio.Text = datos.servicio;
        lblCantidad.Text = datos.cantidad.ToString("0.##");
        lblTotal.Text = "$ " + datos.total.ToString("0.00");
        pkEstado.SelectedItem = datos.estado;   // muestra el estado actual
    }

    // PUT: cambia solo el estado (datos por la URL, como en clientes)
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            string urlPut = Url + "?codigo=" + codigoPedido + "&estado=" + pkEstado.SelectedItem;
            cliente.UploadValues(urlPut, "PUT", parametros);

            await DisplayAlertAsync("Mensaje", "Estado actualizado a: " + pkEstado.SelectedItem, "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }

    // DELETE con confirmación
    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlertAsync("Confirmar",
            "¿Seguro que quieres eliminar el pedido N° " + codigoPedido + "?", "Sí", "No");
        if (!confirmar) return;

        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            cliente.UploadValues(Url + "?codigo=" + codigoPedido, "DELETE", parametros);

            await DisplayAlertAsync("Mensaje", "Pedido eliminado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }
}