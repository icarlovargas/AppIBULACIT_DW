using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;

namespace AppIBULACIT.Views
{
    public partial class frmError : System.Web.UI.Page
    {
        IEnumerable<Error> errores = new ObservableCollection<Error>();
        ErrorManager errorManager = new ErrorManager();

        public string labelsGraficoVistasGlobal = string.Empty;
        public string dataGraficoVistasGlobal = string.Empty;
        public string backgroundcolorsGraficoVistasGlobal = string.Empty;


        async protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    errores = await errorManager.ObtenerErrores(Session["Token"].ToString());
                    InicializarControles();
                    ObtenerDatosGrafic();
                }

            }
        }

        private async void InicializarControles()
        {
            try
            {
                
                gvError.DataSource = errores.ToList();
                gvError.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmError.aspx",
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



        private void ObtenerDatosGrafic()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var error in errores.GroupBy(info => info.Vista).

                Select(group => new
                {
                    Vista = group.Key,
                    Cantidad = group.Count()

                }).OrderBy(x => x.Vista))
            {
                string color = string.Format("#{0:X6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", error.Vista));
                dataGraficoVistas.Append(string.Format("'{0}',", error.Cantidad));
                backgroundcolorsGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobal = backgroundcolorsGraficoVistas.ToString().Substring(0, backgroundcolorsGraficoVistas.Length - 1);
            }
        }
    }
}