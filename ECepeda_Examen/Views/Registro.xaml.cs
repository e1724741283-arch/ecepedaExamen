namespace ECepeda_Examen.Views;

public partial class Registro : ContentPage
{
	string usuarioConectado;
	double costoMembresia = 1200;
	int numeroCuotas = 5;
	double pagoMensual = 0;

	public Registro(string usuario)
	{
		InitializeComponent();
		usuarioConectado = usuario;
		lblUsuario.Text = "Usuario conectado: " + usuario;
		dpFechaInicio.MinimumDate = DateTime.Today;
		dpFechaInicio.Date = DateTime.Today;
	}

	private async Task<bool> ValidarDatos()
	{
		if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
		{
			await DisplayAlertAsync("ERROR", "Ingrese el nombre y el apellido", "Cerrar");
			return false;
		}

		if (!SoloLetras(txtNombre.Text) || !SoloLetras(txtApellido.Text))
		{
			await DisplayAlertAsync("ERROR", "El nombre y el apellido solo deben contener letras", "Cerrar");
			return false;
		}

		if (string.IsNullOrWhiteSpace(txtEdad.Text) || !SoloNumeros(txtEdad.Text))
		{
			await DisplayAlertAsync("ERROR", "La edad solo debe contener números enteros", "Cerrar");
			txtEdad.Text = "";
			return false;
		}

		if (int.Parse(txtEdad.Text) < 16)
		{
			await DisplayAlertAsync("ERROR", "La edad debe ser mayor o igual a 16", "Cerrar");
			txtEdad.Text = "";
			return false;
		}

		if (dpFechaInicio.Date == null || dpFechaInicio.Date < DateTime.Today)
		{
			await DisplayAlertAsync("ERROR", "La fecha de inicio no puede ser una fecha pasada", "Cerrar");
			return false;
		}

		if (pkPlan.SelectedIndex == -1)
		{
			await DisplayAlertAsync("ERROR", "Seleccione un plan", "Cerrar");
			return false;
		}

		if (!double.TryParse(txtMontoInicial.Text, out double montoInicial) || montoInicial < 120 || montoInicial > 600)
		{
			await DisplayAlertAsync("ERROR", "El monto inicial debe ser un número entre 120 y 600", "Cerrar");
			txtMontoInicial.Text = "";
			txtPagoMensual.Text = "";
			return false;
		}

		return true;
	}

	private bool SoloLetras(string texto)
	{
		foreach (char letra in texto.Trim())
		{
			if (!char.IsLetter(letra) && letra != ' ')
			{
				return false;
			}
		}
		return true;
	}

	private bool SoloNumeros(string texto)
	{
		foreach (char numero in texto.Trim())
		{
			if (!char.IsDigit(numero))
			{
				return false;
			}
		}
		return true;
	}

	private void CalcularPago()
	{
		double montoInicial = double.Parse(txtMontoInicial.Text);
		double saldo = costoMembresia - montoInicial;
		pagoMensual = saldo / numeroCuotas + costoMembresia * 0.02;

		if (swCasillero.IsToggled)
		{
			pagoMensual = pagoMensual + 10;
		}

		txtPagoMensual.Text = pagoMensual.ToString("0.00");
	}

	private async void btnCalcular_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (await ValidarDatos())
			{
				CalcularPago();
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private async void btnVerResumen_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (!await ValidarDatos())
			{
				return;
			}

			if (string.IsNullOrEmpty(txtPagoMensual.Text))
			{
				await DisplayAlertAsync("ERROR", "Primero calcule el pago mensual", "Cerrar");
				return;
			}

			CalcularPago();

			string nombre = txtNombre.Text.Trim();
			string apellido = txtApellido.Text.Trim();
			int edad = int.Parse(txtEdad.Text);
			DateTime fechaInicio = dpFechaInicio.Date ?? DateTime.Today;
			string plan = pkPlan.SelectedItem?.ToString() ?? "";
			bool casillero = swCasillero.IsToggled;
			double montoInicial = double.Parse(txtMontoInicial.Text);
			double pagoTotal = montoInicial + pagoMensual * numeroCuotas;

			await Navigation.PushAsync(new Resumen(usuarioConectado, nombre, apellido, edad, fechaInicio,
				plan, casillero, montoInicial, pagoMensual, pagoTotal));

			LimpiarCampos();
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private void LimpiarCampos()
	{
		txtNombre.Text = "";
		txtApellido.Text = "";
		txtEdad.Text = "";
		dpFechaInicio.Date = DateTime.Today;
		pkPlan.SelectedIndex = -1;
		swCasillero.IsToggled = false;
		txtMontoInicial.Text = "";
		txtPagoMensual.Text = "";
		pagoMensual = 0;
	}
}
