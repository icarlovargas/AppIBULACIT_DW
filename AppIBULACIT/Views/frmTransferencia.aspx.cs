using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;


namespace AppIBULACIT.Views
{
    public partial class frmTransferencia : System.Web.UI.Page
    {

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

        IEnumerable<Transferencia> transferencias = new ObservableCollection<Transferencia>();
        TransferenciaManager transferenciaManager = new TransferenciaManager();

        public string labelsGraficoVistasGlobal = string.Empty;
        public string dataGraficoVistasGlobal = string.Empty;
        public string backgroundcolorsGraficoVistasGlobal = string.Empty;


        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    transferencias = await transferenciaManager.ObtenerTransferencias(Session["Token"].ToString());
                    InicializarControles();
                    ObtenerDatosGraficoDias();

                }
            }


        }


        private async void InicializarControles()
        {
            try
            {
                transferencias = await transferenciaManager.ObtenerTransferencias(Session["Token"].ToString());
                gvTransferencia.DataSource = transferencias.ToList();
                gvTransferencia.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmTransferencia.aspx",
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

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                Transferencia transferencia = new Transferencia()
                {
                    CuentaOrigen = Convert.ToInt32(txtCuentaOrigen.Text),
                    CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                    FechaHora = Convert.ToDateTime(txtfecha.Text),
                    Descripcion = txtDescripcion.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };



                if (await validar_monto(txtCuentaOrigen.Text))
                {


                    if (await validar_cuentaDestino(txtCuentaDestino.Text))
                    {
                        Transferencia trasnferenciaIngresada = await transferenciaManager.Ingresar(transferencia, Session["Token"].ToString());
                        restar_Monto(txtCuentaOrigen.Text);
                        sumar_Monto(txtCuentaDestino.Text);
                        lblResultado.Text = "La transferencia se realizó con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();
                        //Correo correo = new Correo();

                        //correo.Enviar("Nuevo servicio incluido", cuenta.Descripcion, "vargascarlomario@gmail.com");
                    }
                    else
                    {
                        lblResultado.Text = "La cuenta destino no existe";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
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
                    Vista = "frmTrasnferencia.aspx",
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
                Vista = "frmTransferencia.aspx.cs",
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
                Vista = "frmTransferencia.aspx.cs",
                Accion = "btnCancelarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                ltrTituloMantenimiento.Text = "Nueva Transferencia";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtCodigoMant.Text = string.Empty;
                ltrCuentaOrigen.Visible = true;
                txtCuentaOrigen.Text = string.Empty;
                ltrCuentaDestino.Visible = true;
                txtCuentaDestino.Text = string.Empty;
                ltrDescripcion.Visible = true;
                txtDescripcion.Text = string.Empty;
                txtfecha.Text = DateTime.Now.ToString();
                txtfecha.Enabled = false;
                ltrMonto.Visible = true;
                txtMonto.Text = string.Empty;
                ddlEstadoMant.Enabled = false;
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
                Vista = "frmTransferencia.aspx.cs",
                Accion = "btnNuevo_Click"
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

        private async void sumar_Monto(string id)
        {
            Cuenta cuenta = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), id);
            cuenta.Saldo = cuenta.Saldo + Convert.ToDecimal(txtMonto.Text);
            Cuenta cuentaModificada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());
        }

        private async Task<bool> validar_cuentaDestino(String id)
        {
            Cuenta cuenta = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), id);
            if (cuenta.Codigo == Convert.ToInt32(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void ObtenerDatosGraficoDias()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var transferencias in transferencias.GroupBy(info => info.FechaHora.ToString("MM/dd/yyyy")).

                Select(group => new
                {
                    Fecha = group.Key,
                    Cantidad = group.Count()

                }).OrderBy(x => x.Fecha))
            {
                string color = string.Format("#{0:X6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", transferencias.Fecha));
                dataGraficoVistas.Append(string.Format("'{0}',", transferencias.Cantidad));
                backgroundcolorsGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobal = backgroundcolorsGraficoVistas.ToString().Substring(0, backgroundcolorsGraficoVistas.Length - 1);
            }
        }

    }
}