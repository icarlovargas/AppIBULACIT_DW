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
    public partial class frmSesion : System.Web.UI.Page
    {
        IEnumerable<Sesion> sesiones = new ObservableCollection<Sesion>();
        SesionController sesionManager = new SesionController();

        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();
        }

        private async void InicializarControles()
        {
            try
            {
                sesiones = await sesionManager.ObtenerSesiones(Session["Token"].ToString());
                gvSesiones.DataSource = sesiones.ToList();
                gvSesiones.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "SesionControle.aspx",
                    Accion = "InicializarControles",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                
                lblStatus.Text = ex.ToString();
                lblStatus.Visible = true;

            }
        }
    }
}