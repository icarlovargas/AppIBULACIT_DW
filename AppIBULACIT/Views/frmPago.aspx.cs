using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;


namespace AppIBULACIT.Views
{
    public partial class frmPago : System.Web.UI.Page
    {
        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();
        ServicioManager ServicioManager = new ServicioManager();

        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();
        MonedaController monedaController = new MonedaController();

        IEnumerable<Pago> pagos = new ObservableCollection<Pago>();
        PagoController PagoManager = new PagoController();

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();


        protected async void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();



            if (!IsPostBack)
            {
                monedas = await monedaController.ObtenerMonedas(Session["Token"].ToString());
                ddMonedas.DataSource = monedas.ToList();
                ddMonedas.DataTextField = "Descripcion";
                ddMonedas.DataValueField = "Codigo";
                ddMonedas.DataBind();

                servicios = await ServicioManager.ObtenerServicios(Session["Token"].ToString());
                ddCodigoServicio.DataSource = servicios.ToList();
                ddCodigoServicio.DataTextField = "Descripcion";
                ddCodigoServicio.DataValueField = "Codigo";
                ddCodigoServicio.DataBind();

                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                ddCuentas.DataSource = cuentas.ToList();
                ddCuentas.DataTextField = "IBAN";
                ddCuentas.DataValueField = "Codigo";
                ddCuentas.DataBind();
            }
        }


        private async void InicializarControles()
        {
            try
            {
                pagos = await PagoManager.ObtenerPagos(Session["Token"].ToString());
                gvPagos.DataSource = pagos.ToList();
                gvPagos.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmPago.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al cargar la lista de pagos";
                lblStatus.Visible = true;

            }
        }

        protected async void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                ltrTituloMantenimiento.Text = "Nuevo Servicio";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtCodigoMant.Text = string.Empty;
                ddCodigoServicio.Enabled = true;
                ddCuentas.Enabled = true;
                ltrFecha.Visible = true;
                txtFecha.Text = DateTime.Now.ToString();
                txtFecha.Enabled = false;
                ltrMonto.Visible = true;
                txtMonto.Text = string.Empty;
                ddMonedas.Enabled = true;
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmPago.aspx",
                    Accion = "btnNuevo_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "No se puede ejecutar la siguiente accion";
                lblStatus.Visible = true;
            }

            EstadisticaController estadisticaManager = new EstadisticaController();
            Estadistica estadistica = new Estadistica()
            {
                CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                FechaHora = DateTime.Now,
                Navegador = HttpContext.Current.Request.Browser.Browser,
                PlataformaDispositivo = Request.Browser.Platform,
                FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                Vista = "frmPago.aspx.cs",
                Accion = "btnNuevo_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                Pago pago = new Pago()
                {
                    CodigoServicio = Convert.ToInt32(ddCodigoServicio.SelectedValue),
                    CodigoCuenta = Convert.ToInt32(ddCuentas.SelectedValue),
                    CodigoMoneda = Convert.ToInt32(ddMonedas.SelectedValue),
                    FechaHora = Convert.ToDateTime(txtFecha.Text),
                    Monto = Convert.ToDecimal(txtMonto.Text)
                };



                if (await validar_monto(ddCuentas.SelectedValue.ToString()))
                {
                    restar_Monto(ddCuentas.SelectedValue.ToString());
                    Pago pagoIngresado = await PagoManager.Ingresar(pago, Session["Token"].ToString());
                    lblResultado.Text = "Pago se realizó con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    //Correo correo = new Correo();

                    //correo.Enviar("Nuevo servicio incluido", cuenta.Descripcion, "vargascarlomario@gmail.com");
                }
                else
                {
                    lblResultado.Text = "Fondo insuficientes";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }



            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmCuenta.aspx",
                    Accion = "btnAceptarMant_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
            }

            EstadisticaController estadisticaManager = new EstadisticaController();
            Estadistica estadistica = new Estadistica()
            {
                CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                FechaHora = DateTime.Now,
                Navegador = HttpContext.Current.Request.Browser.Browser,
                PlataformaDispositivo = Request.Browser.Platform,
                FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                Vista = "frmPago.aspx.cs",
                Accion = "btnAceptarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            EstadisticaController estadisticaManager = new EstadisticaController();
            Estadistica estadistica = new Estadistica()
            {
                CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                FechaHora = DateTime.Now,
                Navegador = HttpContext.Current.Request.Browser.Browser,
                PlataformaDispositivo = Request.Browser.Platform,
                FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                Vista = "frmPago.aspx.cs",
                Accion = "btnCancelarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);

        }

        private async Task<bool> validar_monto(String id)
        {
            Cuenta cuenta = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), id);
            if (cuenta.Saldo >= Convert.ToDecimal(txtMonto.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void restar_Monto(string id)
        {
            Cuenta cuenta = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), id);
            cuenta.Saldo = cuenta.Saldo - Convert.ToDecimal(txtMonto.Text);
            Cuenta cuentaModificada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());
        }
    }
}