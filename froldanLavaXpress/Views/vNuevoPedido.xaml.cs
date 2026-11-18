using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using froldanLavaXpress.Models;

namespace froldanLavaXpress.Views;

public partial class vNuevoPedido : ContentPage
{
    private const string UrlCliente = "http://192.168.2.108/lavaxpress/cliente.php";
    private const string UrlPedido = "http://192.168.2.108/lavaxpress/pedido.php";
    private readonly HttpClient _cliente = new HttpClient();

    // Precios en el MISMO orden que el Picker de servicios
    // Posición 0 y 1 = por libra | desde la 2 = por unidad
    double[] precios = { 0.50, 0.25, 3.00, 2.50, 2.00, 1.25, 1.50 };
    string[] servicios = { "Lavado y secado", "Secado", "Edredón", "Cobija mediana",
                           "Cobija pequeña", "Zapatos de color", "Zapatos blancos" };

    public vNuevoPedido()
    {
        InitializeComponent();
        dpFecha.MinimumDate = DateTime.Today;   // no permite fechas pasadas
        CargarClientes();
    }

    // GET: llena el Picker con los clientes de la API
    public async void CargarClientes()
    {
        try
        {
            var content = await _cliente.GetStringAsync(UrlCliente);
            List<Cliente> lista = JsonConvert.DeserializeObject<List<Cliente>>(content) ?? new();
            pkCliente.ItemsSource = lista;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error al cargar clientes", ex.Message, "OK");
        }
    }

    // Cambia el texto según se cobre por libra o por unidad
    private void pkServicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pkServicio.SelectedIndex <= 1)
            lblCantidad.Text = "Libras:";
        else
            lblCantidad.Text = "Unidades:";

        txtTotal.Text = "";
    }

    // Devuelve el total, o -1 si los datos no son válidos
    private double CalcularTotal()
    {
        if (pkServicio.SelectedIndex == -1)
            return -1;

        if (!double.TryParse(txtCantidad.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double cantidad) || cantidad <= 0)
            return -1;

        // Edredones y cobijas: solo números enteros
        if (pkServicio.SelectedIndex >= 2 && cantidad % 1 != 0)
            return -1;

        return cantidad * precios[pkServicio.SelectedIndex];
    }

    private async void btnCalcular_Clicked(object sender, EventArgs e)
    {
        double total = CalcularTotal();
        if (total == -1)
        {
            await DisplayAlertAsync("Validación", "Seleccione el servicio e ingrese una cantidad válida (unidades sin decimales).", "OK");
            return;
        }
        txtTotal.Text = total.ToString("0.00");
    }

    // POST: guarda el pedido
    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (pkCliente.SelectedIndex == -1)
        {
            await DisplayAlertAsync("Validación", "Seleccione el cliente.", "OK");
            return;
        }

        double total = CalcularTotal();   // se recalcula por si cambió algo
        if (total == -1)
        {
            await DisplayAlertAsync("Validación", "Seleccione el servicio e ingrese una cantidad válida.", "OK");
            return;
        }

        var cliente = (Cliente)pkCliente.SelectedItem;

        try
        {
            WebClient wc = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("codigo_cliente", cliente.codigo.ToString());
            parametros.Add("fecha", dpFecha.Date?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd"));
            parametros.Add("servicio", servicios[pkServicio.SelectedIndex]);
            parametros.Add("cantidad", txtCantidad.Text.Replace(",", "."));
            parametros.Add("total", total.ToString("0.00", CultureInfo.InvariantCulture));
            wc.UploadValues(UrlPedido, "POST", parametros);

            await DisplayAlertAsync("Pedido registrado",
                "Cliente: " + cliente.nombreCompleto +
                "\nServicio: " + servicios[pkServicio.SelectedIndex] +
                "\nTotal: $" + total.ToString("0.00"), "OK");
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