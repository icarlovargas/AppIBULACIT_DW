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


//Paso 1 importamos las librerias
namespace AppIBULACIT.Views
{
    public partial class frmCuenta : System.Web.UI.Page
    {

        //Paso 2 creamos estas variables
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();
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
                lblStatus.Text = "Hubo un error al cargar la lista de servicios.";
                lblStatus.Visible = true;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e) //paso 10 creamos el evento
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
            txtCodigoMonedaMan.Visible = true;
            txtCodigoMonedaMan.Text = string.Empty;
            ddlEstadoMant.Enabled = false;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
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
                        CodigoMoneda = Convert.ToInt32(txtCodigoMonedaMan.Text),
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
                    Cuenta cuenta = new Cuenta()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuarioMan.Text),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIBAN.Text,
                        CodigoMoneda = Convert.ToInt32(txtCodigoMonedaMan.Text),
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Cuenta cuentaModificada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(cuentaModificada.Descripcion))
                    {
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
                    Vista = "frmServici.aspx",
                    Accion = "btnAceptarMant_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
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
                    Vista = "frmServici.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);

            }
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void gvCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    txtCodigoMonedaMan.Text = row.Cells[4].Text.Trim();
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
    }
}
