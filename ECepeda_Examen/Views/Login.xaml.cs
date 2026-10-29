namespace ECepeda_Examen.Views;

public partial class Login : ContentPage
{
	readonly string[,] usuarios =
	{
		{ "recepcion", "gym2026" },
		{ "entrenador", "fit_01" },
		{ "uisrael", "2026" },
		{ "gerente", "admin123" }
	};

	public Login()
	{
		InitializeComponent();
	}

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
		try
		{
			string usuarioIngresado = txtUsuario.Text;
			string contrasenaIngresada = txtContrasena.Text;
			bool accesoCorrecto = false;

			for (int i = 0; i < usuarios.GetLength(0); i++)
			{
				if (usuarios[i, 0] == usuarioIngresado && usuarios[i, 1] == contrasenaIngresada)
				{
					accesoCorrecto = true;
				}
			}

			if (accesoCorrecto)
			{
				await Navigation.PushAsync(new Registro(usuarioIngresado));
				txtUsuario.Text = "";
				txtContrasena.Text = "";
			}
			else
			{
				await DisplayAlertAsync("ERROR", "Su usuario o contraseña es incorrecto", "Cerrar");
				txtUsuario.Text = "";
				txtContrasena.Text = "";
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
    }

    private async void btnAcercaDe_Clicked(object sender, EventArgs e)
    {
		await DisplayAlertAsync("ACERCA DE",
			"Nombre: CEPEDA CAIZAGUANO EDISON BERNABE\nCarrera: SISTEMAS DE INFORMACIÓN\nParalelo: A",
			"Cerrar");
    }
}
