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
    public partial class frmEstadistica : System.Web.UI.Page
    {

        IEnumerable<Estadistica> estadisticas = new ObservableCollection<Estadistica>();
        EstadisticaController estadisticaManager = new EstadisticaController();

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
                    estadisticas = await estadisticaManager.ObtenerEstadisticas();
                    InicializarControles();
                    ObtenerDatosGrafic();
                }

            }
        }


        private async void InicializarControles()
        {





            try
            {
                estadisticas = await estadisticaManager.ObtenerEstadisticas();
                gvSesiones.DataSource = estadisticas.ToList();
                gvSesiones.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmEstadistica.aspx",
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


        //private void ObtenerDatosGrafico()
        //{
        //    StringBuilder script = new StringBuilder();
        //    StringBuilder labelsGraficoVistas = new StringBuilder();
        //    StringBuilder dataGraficoVistas = new StringBuilder();
        //    StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();

        //    var random = new Random();

        //    foreach (var error in estadisticas.GroupBy(info => info.Vista).
        //        Select(group => new
        //        {
        //            Vista = group.Key,
        //            Cantidad = group.Count()
        //        }).OrderBy(x => x.Vista))
        //    {
        //        string color = String.Format("#{0:X6}", random.Next(0x1000000));
        //        labelsGraficoVistas.Append(string.Format("'{0}',", error.Vista)); // 'Correo','frmError',
        //        dataGraficoVistas.Append(string.Format("'{0}',", error.Cantidad)); // '2','3',
        //        backgroundcolorsGraficoVistas.Append(string.Format("'{0}',", color));

        //        labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
        //        dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
        //        backgroundcolorsGraficoVistasGlobal =
        //            backgroundcolorsGraficoVistas.ToString().Substring(backgroundcolorsGraficoVistas.Length - 1);
        //    }


        private void ObtenerDatosGrafic()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var error in estadisticas.GroupBy(info => info.Vista).

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
