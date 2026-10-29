namespace ECepeda_Examen.Views;

public partial class Resumen : ContentPage
{
	string rutaExportacion = @"C:\Users\DesarrolloECep\source\repos\ECepeda_Examen\ECepeda_Examen\export";
	string usuarioConectado;
	string nombre;
	string apellido;
	int edad;
	DateTime fechaInicio;
	string plan;
	bool casillero;
	double montoInicial;
	double pagoMensual;
	double pagoTotal;

	public Resumen(string usuario, string nombre, string apellido, int edad, DateTime fechaInicio,
		string plan, bool casillero, double montoInicial, double pagoMensual, double pagoTotal)
	{
		InitializeComponent();

		usuarioConectado = usuario;
		this.nombre = nombre;
		this.apellido = apellido;
		this.edad = edad;
		this.fechaInicio = fechaInicio;
		this.plan = plan;
		this.casillero = casillero;
		this.montoInicial = montoInicial;
		this.pagoMensual = pagoMensual;
		this.pagoTotal = pagoTotal;

		lblUsuario.Text = "Usuario conectado: " + usuario;
		lblNombre.Text = nombre;
		lblApellido.Text = apellido;
		lblEdad.Text = edad.ToString();
		lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
		lblPlan.Text = plan;
		lblCasillero.Text = casillero ? "Sí" : "No";
		lblMontoInicial.Text = "$ " + montoInicial.ToString("0.00");
		lblPagoMensual.Text = "$ " + pagoMensual.ToString("0.00");
		lblPagoTotal.Text = "$ " + pagoTotal.ToString("0.00");
	}

	private async void btnNuevoRegistro_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopToRootAsync();
	}

	private async void btnExportar_Clicked(object sender, EventArgs e)
	{
		try
		{
			string nombreArchivo = "Resumen_" + nombre + "_" + apellido + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
			string carpeta = rutaExportacion;
			if (DeviceInfo.Platform != DevicePlatform.WinUI)
			{
				carpeta = FileSystem.AppDataDirectory;
			}

			Directory.CreateDirectory(carpeta);
			string rutaArchivo = Path.Combine(carpeta, nombreArchivo);

			string contenido =
				"RESUMEN DE SOCIO\n" +
				"Usuario conectado: " + usuarioConectado + "\n" +
				"Nombre: " + nombre + "\n" +
				"Apellido: " + apellido + "\n" +
				"Edad: " + edad + "\n" +
				"Fecha de inicio: " + fechaInicio.ToString("dd/MM/yyyy") + "\n" +
				"Plan: " + plan + "\n" +
				"Casillero: " + (casillero ? "Sí" : "No") + "\n" +
				"Monto Inicial: $ " + montoInicial.ToString("0.00") + "\n" +
				"Pago mensual: $ " + pagoMensual.ToString("0.00") + "\n" +
				"Pago Total: $ " + pagoTotal.ToString("0.00");

			File.WriteAllText(rutaArchivo, contenido);

			await DisplayAlertAsync("ARCHIVO GENERADO",
				"Nombre: " + nombreArchivo + "\nRuta: " + rutaArchivo,
				"Cerrar");
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}
}
