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
                lblStatus.Text = "Hubo un error al cargar la lista de servicios.";
                lblStatus.Visible = true;
            }
        }

        //Paso 3 le damos funcionalidad al boton nuevo
        protected void btnNuevo_Click(object sender, EventArgs e) //aqui creamos la ventana emergente de nuevo (agregar nueva moneda)
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

        //Paso 4 le damos la funcionalidad al boton eliminar y modificar
        protected void gvMoneda_RowCommand(object sender, GridViewCommandEventArgs e) //funciones seleccionar los botones modificar o eliminar
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

        //Paso 5 verificamos que esten las funciones de los botones

        //Esta funcionalidad agrega o modifica

        //Paso 8
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
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
                Moneda moneda= new Moneda() ///cambiamos el tipo de dato
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

        // Paso 6 agregamos la funcionalidad de cancelar
        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        //Esta funcionalidad elimina

        //Paso 9
        protected async void btnAceptarModal_Click(object sender, EventArgs e)
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

        // Paso 7 agregamos la funcionalidad de cancelar
        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }
    }
}