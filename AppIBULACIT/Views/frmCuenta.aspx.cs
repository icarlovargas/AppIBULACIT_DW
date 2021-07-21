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


//Paso 1 importamos las librerias
namespace AppIBULACIT.Views
{
    public partial class frmCuenta : System.Web.UI.Page
    {

        //Paso 2 creamos estas variables
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();
        MonedaController monedaController = new MonedaController();

        UsuarioManager2Controller usuarioManager = new UsuarioManager2Controller();

        protected async void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();

            btnAceptarModal.Visible = true;

            if (!IsPostBack)
            {
                monedas = await monedaController.ObtenerMonedas(Session["Token"].ToString());
                ddMonedas.DataSource = monedas.ToList();
                ddMonedas.DataTextField = "Descripcion";
                ddMonedas.DataValueField = "Codigo";
                ddMonedas.DataBind();
            }



        }

        //Paso 3 creamos el metodo que nos carga los elementos
        private async void InicializarControles()
        {
            try
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                gvCuentas.DataSource = cuentas.ToList();
                gvCuentas.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmCuenta.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas.";
                lblStatus.Visible = true;

            }
        }

        protected async void btnNuevo_Click(object sender, EventArgs e) //paso 10 creamos el evento
        {
            try
            {
                ltrTituloMantenimiento.Text = "Nueva Cuenta";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtCodigoMant.Text = string.Empty;
                ltrCodigoUsuarioMan.Visible = true;
                txtCodigoUsuarioMan.Visible = true;
                txtCodigoUsuarioMan.Text = string.Empty;
                ltrDescripcion.Visible = true;
                txtDescripcion.Visible = true;
                txtDescripcion.Text = string.Empty;
                ltrIBAN.Visible = true;
                txtIBAN.Visible = true;
                txtIBAN.Text = string.Empty;
                ltrCodigoMonedaMan.Visible = true;
                txtSaldo.Text = string.Empty;
                txtSaldo.Visible = true;
                ddMonedas.Enabled = true;
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
                    Vista = "frmCuenta.aspx",
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
                Vista = "frmCuenta.aspx.cs",
                Accion = "btnNuevo_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }



        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    Cuenta cuenta = new Cuenta()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuarioMan.Text),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIBAN.Text,
                        CodigoMoneda = Convert.ToInt32(ddMonedas.SelectedValue),
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Cuenta cuentaIngresada = await cuentaManager.Ingresar(cuenta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(cuentaIngresada.Descripcion))
                    {

                        lblResultado.Text = "Servicio ingresado con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();
                        //Correo correo = new Correo();

                        //correo.Enviar("Nuevo servicio incluido", cuenta.Descripcion, "vargascarlomario@gmail.com");
                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error al efectuar la operacion.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
                }
                else //Modificar
                {
                    ddlEstadoMant.Visible = true;
                    Cuenta cuenta = new Cuenta()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuarioMan.Text),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIBAN.Text,
                        CodigoMoneda = Convert.ToInt32(ddMonedas.SelectedValue),
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    

                    if (await validar_Usuario(txtCodigoUsuarioMan.Text))
                    {
                        Cuenta cuentaModificada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());
                        lblResultado.Text = "Cuenta actualizada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error al efectuar la operacion.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
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
                Vista = "frmCuenta.aspx.cs",
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
                Vista = "frmCuenta.aspx.cs",
                Accion = "btnCancelarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);

        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {

            try
            {
                string resultado = string.Empty;
                resultado = await cuentaManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Cuenta eliminada";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();

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
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo al eliminar la cuenta.";
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
                Vista = "frmCuenta.aspx.cs",
                Accion = "btnAceptarModal_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);

            EstadisticaController estadisticaManager = new EstadisticaController();
            Estadistica estadistica = new Estadistica()
            {
                CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                FechaHora = DateTime.Now,
                Navegador = HttpContext.Current.Request.Browser.Browser,
                PlataformaDispositivo = Request.Browser.Platform,
                FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                Vista = "frmCuenta.aspx.cs",
                Accion = "btnCancelarModal_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void gvCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCuentas.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        ltrTituloMantenimiento.Text = "Modificar Cuenta";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();
                        txtCodigoUsuarioMan.Text = row.Cells[1].Text.Trim();
                        txtDescripcion.Text = row.Cells[2].Text.Trim();
                        txtIBAN.Text = row.Cells[3].Text.Trim();
                        txtSaldo.Text = row.Cells[5].Text.Trim();
                        btnAceptarMant.Visible = true;
                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio # " + lblCodigoEliminar.Text + "?";
                        ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                        break;
                    default:
                        break;
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
                    Accion = "gvCuentas_RowCommand",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error con el gridView.";
                lblStatus.Visible = true;
            }
        }

        private async Task<bool> validar_Usuario(String id)
        {
            Usuario usuario = await usuarioManager.ObtenerUsuario(Session["Token"].ToString(), id);
            
            if (usuario.Codigo == Convert.ToInt32(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}
