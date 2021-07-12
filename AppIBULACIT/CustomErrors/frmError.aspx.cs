using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.CustomErrors
{
    public partial class frmError : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            Exception error = Session["LastError"] as Exception;

            if(error != null)
            {
                error = error.GetBaseException();
                lblError.Text = error.Message;
                Session["LastError"] = null;

                ErrorManager errorManager = new ErrorManager();
                Error errorAPI = new Error()
                {
                    CodigoUsuario = 0,
                    Fecha = DateTime.Now,
                    Vista = "frmError.aspx",
                    Accion = "Page_Load",
                    Fuente = error.Source,
                    Numero = error.HResult.ToString(),
                    Descripcion = error.Message
                };

                Error errorIngresado = await errorManager.Ingresar(errorAPI);
            }
        }
    }
}