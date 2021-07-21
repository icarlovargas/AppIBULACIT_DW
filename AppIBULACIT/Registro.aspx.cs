using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnFechaNacimiento_Click(object sender, EventArgs e)
        {
            try
            {
                cldFechaNacimiento.Visible = true;
                EstadisticaController estadisticaManager = new EstadisticaController();
                Estadistica estadistica = new Estadistica()
                {
                    CodigoUsuario = 0,
                    FechaHora = DateTime.Now,
                    Navegador = HttpContext.Current.Request.Browser.Browser,
                    PlataformaDispositivo = Request.Browser.Platform,
                    FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                    Vista = "Registro.aspx.cs",
                    Accion = "btnFechaNacimiento_Click"
                };

                Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmRegistro.aspx",
                    Accion = "btnFechaNacimiento_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error con el Boton de Fecha de Nacimiento.";
                lblStatus.Visible = true;
            }
        }

        protected async void cldFechaNacimiento_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtFechaNacimiento.Text = cldFechaNacimiento.SelectedDate.ToString("dd/MM/yyyy");
                cldFechaNacimiento.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    Fecha = DateTime.Now,
                    Vista = "frmRegistro.aspx",
                    Accion = "cldFechaNacimiento_SelectionChanged",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hay un error con Calendar";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptar_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {

                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();

                    Usuario usuario = new Usuario()
                    {
                        Identificacion = txtIdentificacion.Text,
                        Nombre = txtNombre.Text,
                        Email = txtEmail.Text,
                        FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text),
                        Username = txtUsername.Text,
                        Password = txtPassword.Text,
                        Estado = "A"
                    };

                    Usuario usuarioRegistrado = await usuarioManager.Registrar(usuario);

                    if (!string.IsNullOrEmpty(usuario.Identificacion))
                    {
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        lblStatus.Text = "Hubo un error al registrar el usuario";
                        lblStatus.Visible = true;
                    }

                    EstadisticaController estadisticaManager = new EstadisticaController();
                    Estadistica estadistica = new Estadistica()
                    {
                        CodigoUsuario = 0,
                        FechaHora = DateTime.Now,
                        Navegador = HttpContext.Current.Request.Browser.Browser,
                        PlataformaDispositivo = Request.Browser.Platform,
                        FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                        Vista = "Registro.aspx.cs",
                        Accion = "btnAceptar_Click"
                    };

                    Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);

                }
                catch (Exception ex)
                {
                    ErrorManager errorManager = new ErrorManager();
                    Error error = new Error()
                    {
                        CodigoUsuario = 0,
                        Fecha = DateTime.Now,
                        Vista = "registro.aspx",
                        Accion = "btnAceptar_Click",
                        Fuente = ex.Source,
                        Numero = ex.HResult.ToString(),
                        Descripcion = ex.Message
                    };

                    Error errorIngresado = await errorManager.Ingresar(error);
                    lblStatus.Text = "Hubo un error al registrar el usuario";
                    lblStatus.Visible = true;
                }
            }

            
        }
    }
}