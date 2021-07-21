using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;


//importar librerias paso 1
namespace AppIBULACIT.Views
{
    public partial class frmUsuario : System.Web.UI.Page
    {
        //agregar variables paso 2
        ///IEnumerable<Usuario> usuarios = new  ObservableCollection<Usuario>();
        List<Usuario> listUsuarios = new List<Usuario>();
        UsuarioManager2Controller usuarioManager2 = new UsuarioManager2Controller();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //agregar metodo de postback
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    InicializarControles();
            }

        }
        //paso 3 crear metodo
        private async void InicializarControles()
        {
            try
            {
                Usuario usuario = await usuarioManager2.ObtenerUsuario(Session["Token"].ToString(), Session["CodigoUsuario"].ToString()); //aqui podemos cambiar el session
                listUsuarios.Add(usuario);
                gvUsuario.DataSource = listUsuarios.ToList();
                gvUsuario.DataBind();
            }

            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmUsuario.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al cargar la lista de servicios.";
                lblStatus.Visible = true;
            }
        }

        protected async void gvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvUsuario.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        ltrTituloMantenimiento.Text = "Modificar Usuario";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();
                        txtIdentificacion.Text = row.Cells[1].Text.Trim();
                        txtNombre.Text = row.Cells[2].Text.Trim();
                        txtUsername.Text = row.Cells[3].Text.Trim();
                        txtPassword.Text = row.Cells[4].Text.Trim();
                        txtemail.Text = row.Cells[5].Text.Trim();
                        txtFechaNacimiento.Text = row.Cells[6].Text.Trim();
                        btnAceptarMant.Visible = true;
                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar el Usuario #" + lblCodigoEliminar.Text + "?";
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
                    Vista = "frmUsuario.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error con el funcionamineto del gridView";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            //Modificar

            try
            {
                Usuario usuario = new Usuario() ///cambiamos el tipo de dato
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    Identificacion = txtIdentificacion.Text,
                    Nombre = txtNombre.Text,
                    Email = txtemail.Text,
                    FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text),
                    //FechaNacimiento = DateTime.Now, ///---por corregir
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Usuario usuarioModificado = await usuarioManager2.Actualizar(usuario, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(usuarioModificado.Nombre))
                {
                    lblResultado.Text = "Usuario Actualizado";
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
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmUsuario.aspx",
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
                Vista = "frmUsuario.aspx.cs",
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
                Vista = "frmUsuario.aspx.cs",
                Accion = "btnCancelarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                string resultado = string.Empty;
                resultado = await usuarioManager2.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Usuario Eliminado";
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
                    Vista = "frmUsuario.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error al eliminar el usuario.";
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
                Vista = "frmUsuario.aspx.cs",
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
                Vista = "frmUsuario.aspx.cs",
                Accion = "btnCancelarModal_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void btnFechaNac_Click(object sender, EventArgs e)
        {
            try
            {
                cldFechaNacimiento.Visible = true;
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
                    Vista = "frmUsuario.aspx",
                    Accion = "btnFechaNac_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error con el boton de fecha.";
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
                Accion = "btnFechaNac_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        protected async void cldFechaNacimiento_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if(DateTime.Now >= cldFechaNacimiento.SelectedDate)
                {
                    txtFechaNacimiento.Text = cldFechaNacimiento.SelectedDate.ToString("dd/MM/yyyy");
                    cldFechaNacimiento.Visible = false;
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Fecha invalida";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }



                
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmUsuario.aspx",
                    Accion = "cldFechaNacimiento_SelectionChanged",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un problema con el calendario.";
                lblStatus.Visible = true;
            }
        }
    }
}