using System.Collections.ObjectModel;
using Newtonsoft.Json;
using froldanLavaXpress.Models;

namespace froldanLavaXpress.Views;

public partial class vClientes : ContentPage
{
    private const string Url = "http://192.168.2.108/lavaxpress/cliente.php";
    private readonly HttpClient _cliente = new HttpClient();
    private ObservableCollection<Cliente> _clientes = new();

    public vClientes()
    {
        InitializeComponent();
    }

    // Recarga la lista cada vez que se muestra la ventana
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MostrarClientes();
    }

    // GET: trae los clientes del servicio
    public async void MostrarClientes()
    {
        try
        {
            var content = await _cliente.GetStringAsync(Url);
            List<Cliente> _mostrar = JsonConvert.DeserializeObject<List<Cliente>>(content) ?? new();
            _clientes = new ObservableCollection<Cliente>(_mostrar);
            listaClientes.ItemsSource = _clientes;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error al listar", ex.Message, "OK");
        }
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAgregarCliente());
    }

    // Al tocar un cliente, abre la ventana para actualizar o eliminar
    private async void listaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;
        var objCliente = (Cliente)e.SelectedItem;
        listaClientes.SelectedItem = null;
        await Navigation.PushAsync(new vActualizarEliminarCliente(objCliente));
    }
}