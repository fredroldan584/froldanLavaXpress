namespace froldanLavaXpress.Views;

public partial class vLogin : ContentPage
{
    // Matriz de usuarios (como en el examen): columna 0 = usuario, columna 1 = contraseña
    string[,] usuarios =
    {
        { "admin",      "lava2026" },
        { "atencion",   "lx2026"   },
        { "repartidor", "entrega1" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    // Cada vez que se muestra el Login (también al cerrar sesión) limpia las cajas
    protected override void OnAppearing()
    {
        base.OnAppearing();
        txtUsuario.Text = "";
        txtClave.Text = "";
    }

    private async void btnIngresar_Clicked(object sender, EventArgs e)
    {
        string usuario = (txtUsuario.Text ?? "").Trim();
        string clave = txtClave.Text ?? "";
        bool acceso = false;

        for (int i = 0; i < usuarios.GetLength(0); i++)
        {
            if (usuarios[i, 0] == usuario && usuarios[i, 1] == clave)
            {
                acceso = true;
                break;
            }
        }

        if (acceso)
        {
            // Abre el Menú y le envía el usuario por el constructor
            await Navigation.PushAsync(new vMenu(usuario));
        }
        else
        {
            await DisplayAlertAsync("Acceso denegado", "Usuario o contraseña incorrectos.", "OK");
        }
    }
}