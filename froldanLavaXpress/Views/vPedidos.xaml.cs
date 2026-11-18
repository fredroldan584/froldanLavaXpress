using System.Collections.ObjectModel;
using Newtonsoft.Json;
using froldanLavaXpress.Models;

namespace froldanLavaXpress.Views;

public partial class vPedidos : ContentPage
{
    private const string Url = "http://192.168.2.108/lavaxpress/pedido.php";
    private readonly HttpClient _cliente = new HttpClient();
    private ObservableCollection<Pedido> _pedidos = new();

    public vPedidos()
    {
        InitializeComponent();
    }

    // Recarga la lista cada vez que se muestra (al volver del detalle)
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MostrarPedidos();
    }

    // GET: trae los pedidos con el nombre del cliente
    public async void MostrarPedidos()
    {
        try
        {
            var content = await _cliente.GetStringAsync(Url);
            List<Pedido> _mostrar = JsonConvert.DeserializeObject<List<Pedido>>(content) ?? new();
            _pedidos = new ObservableCollection<Pedido>(_mostrar);
            listaPedidos.ItemsSource = _pedidos;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error al listar", ex.Message, "OK");
        }
    }

    // Al tocar un pedido, abre su detalle
    private async void listaPedidos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;
        var objPedido = (Pedido)e.SelectedItem;
        listaPedidos.SelectedItem = null;
        await Navigation.PushAsync(new vDetallePedido(objPedido));
    }
}