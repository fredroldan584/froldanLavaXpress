namespace froldanLavaXpress.Views;

public partial class vMenu : ContentPage
{
    public vMenu(string usuario)
    {
        InitializeComponent();
        lblBienvenida.Text = "Bienvenido, " + usuario;
    }

    private async void btnClientes_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vClientes());
    }

    // Pantallas que haremos en las siguientes iteraciones
    private async void btnProximamente_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("En desarrollo", "Esta opción estará disponible en la siguiente iteración.", "OK");
    }

    // Vuelve a la primera ventana (Login), como en el examen
    private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}