using System.Net;
using froldanLavaXpress.Models;

namespace froldanLavaXpress.Views;

public partial class vActualizarEliminarCliente : ContentPage
{
    private const string Url = "http://192.168.2.108/lavaxpress/cliente.php";

    // Recibe el cliente tocado en la lista y llena las cajas
    public vActualizarEliminarCliente(Cliente datos)
    {
        InitializeComponent();
        txtCodigo.Text = datos.codigo.ToString();
        txtNombre.Text = datos.nombre;
        txtApellido.Text = datos.apellido;
        txtTelefono.Text = datos.telefono;
        txtDireccion.Text = datos.direccion;
    }

    // PUT: los datos van en la URL porque cliente.php los lee con $_GET
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        if ((txtTelefono.Text ?? "").Length != 10)
        {
            await DisplayAlertAsync("Validación", "El teléfono debe tener 10 números.", "OK");
            return;
        }

        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            string urlPut = Url + "?codigo=" + txtCodigo.Text
                          + "&nombre=" + Uri.EscapeDataString(txtNombre.Text)
                          + "&apellido=" + Uri.EscapeDataString(txtApellido.Text)
                          + "&telefono=" + txtTelefono.Text
                          + "&direccion=" + Uri.EscapeDataString(txtDireccion.Text);
            cliente.UploadValues(urlPut, "PUT", parametros);

            await DisplayAlertAsync("Mensaje", "Cliente actualizado correctamente", "OK");
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
            "¿Seguro que quieres eliminar a " + txtNombre.Text + " " + txtApellido.Text + "?",
            "Sí", "No");
        if (!confirmar) return;

        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            cliente.UploadValues(Url + "?codigo=" + txtCodigo.Text, "DELETE", parametros);

            await DisplayAlertAsync("Mensaje", "Cliente eliminado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }
}