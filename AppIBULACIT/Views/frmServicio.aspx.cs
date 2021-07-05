using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;


namespace AppIBULACIT.Views
{
    public partial class frmServicio : System.Web.UI.Page
    {

        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();
        ServicioManager servicioManager = new ServicioManager();


        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();
        }

        private async void InicializarControles()
        {
            try
            {
                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                gvServicios.DataSource = servicios.ToList();
                gvServicios.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios.";
                lblStatus.Visible = true;
            }
        }

        protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}