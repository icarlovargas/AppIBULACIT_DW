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
    public partial class frmSesion : System.Web.UI.Page
    {
        IEnumerable<Sesion> sesiones = new ObservableCollection<Sesion>();
        SesionController sesionManager = new SesionController();

        public string labelsGraficoVistasGlobalusuario = string.Empty;
        public string dataGraficoVistasGlobalusuario = string.Empty;
        public string backgroundcolorsGraficoVistasGlobalusuario = string.Empty;


        public string labelsGraficoVistasGlobaldias = string.Empty;
        public string dataGraficoVistasGlobaldias = string.Empty;
        public string backgroundcolorsGraficoVistasGlobaldias = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    sesiones = await sesionManager.ObtenerSesiones(Session["Token"].ToString());
                    InicializarControles();
                    ObtenerDatosGraficoDias();
                    ObtenerDatosGraficoUsuarios();
                }

            }
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

        private void ObtenerDatosGraficoUsuarios()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var sesion in sesiones.GroupBy(info => info.CodigoUsuario).

                Select(group => new
                {
                    Usuario = group.Key,
                    Cantidad = group.Count()

                }).OrderBy(x => x.Usuario))
            {
                string color = string.Format("#{0:X6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", sesion.Usuario));
                dataGraficoVistas.Append(string.Format("'{0}',", sesion.Cantidad));
                backgroundcolorsGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobalusuario = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                dataGraficoVistasGlobalusuario = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobalusuario = backgroundcolorsGraficoVistas.ToString().Substring(0, backgroundcolorsGraficoVistas.Length - 1);
            }
        }

        private void ObtenerDatosGraficoDias()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder backgroundcolorsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var session in sesiones.GroupBy(info => info.FechaInicio.ToString("MM/dd/yyyy")).

                Select(group => new
                {
                    Fecha = group.Key,
                    Cantidad = group.Count()

                }).OrderBy(x => x.Fecha))
            {
                string color = string.Format("#{0:X6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", session.Fecha));
                dataGraficoVistas.Append(string.Format("'{0}',", session.Cantidad));
                backgroundcolorsGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobaldias = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                dataGraficoVistasGlobaldias = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobaldias = backgroundcolorsGraficoVistas.ToString().Substring(0, backgroundcolorsGraficoVistas.Length - 1);
            }
        }
    }
}
