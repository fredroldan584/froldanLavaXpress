using System.Net;

namespace froldanLavaXpress.Views;

public partial class vAgregarCliente : ContentPage
{
    public vAgregarCliente()
    {
        InitializeComponent();
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtDireccion.Text))
        {
            await DisplayAlertAsync("Validación", "Complete todos los campos.", "OK");
            return;
        }
        if ((txtTelefono.Text ?? "").Length != 10 || !long.TryParse(txtTelefono.Text, out _))
        {
            await DisplayAlertAsync("Validación", "El teléfono debe tener 10 números.", "OK");
            return;
        }

        // POST: envía los datos al servicio PHP
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("nombre", txtNombre.Text.Trim());
            parametros.Add("apellido", txtApellido.Text.Trim());
            parametros.Add("telefono", txtTelefono.Text);
            parametros.Add("direccion", txtDireccion.Text.Trim());
            cliente.UploadValues("http://192.168.2.108/lavaxpress/cliente.php", "POST", parametros);

            await DisplayAlertAsync("Mensaje", "Cliente agregado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }

    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}