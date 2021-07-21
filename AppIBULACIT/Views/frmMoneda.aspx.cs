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

namespace AppIBULACIT.Views
{
    public partial class frmMoneda : System.Web.UI.Page
    {
        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>(); //Paso 1 creamos la variables
        MonedaController monedaController = new MonedaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();
        }

        private async void InicializarControles() //Paso 2 creamos metodo para cargar datos
        {
            try
            {
                monedas = await monedaController.ObtenerMonedas(Session["Token"].ToString());
                gvMoneda.DataSource = monedas.ToList();
                gvMoneda.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmMoneda.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al cargar la lista de Monedas.";
                lblStatus.Visible = true;

            }
        }

        //Paso 3 le damos funcionalidad al boton nuevo
        protected async void btnNuevo_Click(object sender, EventArgs e) //aqui creamos la ventana emergente de nuevo (agregar nueva moneda)
        {
            try
            {
                ltrTituloMantenimiento.Text = "Nueva Moneda";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtDescripcion.Visible = true;
                ltrDescripcion.Visible = true;
                ddlEstadoMant.Enabled = false;
                txtCodigoMant.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
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
                    Vista = "frmMoneda.aspx",
                    Accion = "btnNuevo_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error con el boton de Nuevo";
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
                Vista = "frmMoneda.aspx.cs",
                Accion = "btnNuevo_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        //Paso 4 le damos la funcionalidad al boton eliminar y modificar
        protected async void gvMoneda_RowCommand(object sender, GridViewCommandEventArgs e) //funciones seleccionar los botones modificar o eliminar
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvMoneda.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        ltrTituloMantenimiento.Text = "Modificar Moneda";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();  //aqui traemos los datos de las finals
                        txtDescripcion.Text = row.Cells[1].Text.Trim();
                        btnAceptarMant.Visible = true;
                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar la Moneda #" + lblCodigoEliminar.Text + "?";
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
                    Vista = "frmMoneda.aspx",
                    Accion = "gvMoneda_RowCommand",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error con el gridView";
                lblStatus.Visible = true;

            }
        }

        //Paso 5 verificamos que esten las funciones de los botones

        //Esta funcionalidad agrega o modifica

        //Paso 8
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    Moneda moneda = new Moneda()
                    {
                        Descripcion = txtDescripcion.Text, //llenamos el objeto con los txt
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Moneda monedaIngresada = await monedaController.Ingresar(moneda, Session["Token"].ToString()); //cambiamos el manager

                    if (!string.IsNullOrEmpty(monedaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Moneda ingresada con exito";
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
                else //Modificar
                {
                    Moneda moneda = new Moneda() ///cambiamos el tipo de dato
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        Descripcion = txtDescripcion.Text,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Moneda monedaModificada = await monedaController.Actualizar(moneda, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(monedaModificada.Descripcion))
                    {
                        lblResultado.Text = "Servicio actualizado con exito";
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
                    Vista = "frmMoneda.aspx",
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
                Vista = "frmMoneda.aspx.cs",
                Accion = "btnAceptarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        // Paso 6 agregamos la funcionalidad de cancelar
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
                Vista = "frmMoneda.aspx.cs",
                Accion = "btnCancelarMant_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        //Esta funcionalidad elimina

        //Paso 9
        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                string resultado = string.Empty;
                resultado = await monedaController.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Moneda eliminada";
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
                    Vista = "frmMoneda.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al eliminar una moneda";
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
                Vista = "frmMoneda.aspx.cs",
                Accion = "btnAceptarModal_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }

        // Paso 7 agregamos la funcionalidad de cancelar
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
                Vista = "frmMoneda.aspx.cs",
                Accion = "btnCancelarModal_Click"
            };

            Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
        }
    }
}