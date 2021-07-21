using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Models;
using AppIBULACIT.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Security;

namespace AppIBULACIT
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ObtenerInformacionAmbiente();
        }

        protected async  void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    LoginRequest loginRequest = new LoginRequest() { userName = txtUsername.Text, passWord = txtPassword.Text };
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario usuario = new Usuario();
                    usuario = await usuarioManager.Autenticar(loginRequest);

                    if (usuario != null)
                    {
                        JwtSecurityToken jwtSecurityToken;
                        var jwtHandler = new JwtSecurityTokenHandler();
                        jwtSecurityToken = jwtHandler.ReadJwtToken(usuario.Token); //estas tres lineas leen el token

                        
                        Session["CodigoUsuario"] = usuario.Codigo; //variables de sesion
                        Session["Identificacion"] = usuario.Identificacion;
                        Session["Nombre"] = usuario.Nombre;
                        Session["Email"] = usuario.Email;
                        Session["Estado"] = usuario.Estado;
                        Session["Token"] = usuario.Token;
                        Session["InicioSesion"] = jwtSecurityToken.ValidFrom.ToString("dd/MM/yyyy HH:mm:ss");
                        Session["FinSesion"] = jwtSecurityToken.ValidTo.ToString("dd/MM/yyyy HH:mm:ss");

                        FormsAuthentication.RedirectFromLoginPage(usuario.Username, false);

                        SesionController sesionManager = new SesionController();
                        Sesion sesion = new Sesion()
                        {
                            CodigoUsuario = usuario.Codigo,
                            FechaInicio = Convert.ToDateTime(jwtSecurityToken.ValidFrom.ToString("dd/MM/yyyy HH:mm:ss")),
                            FechaExpiracion = Convert.ToDateTime(jwtSecurityToken.ValidTo.ToString("dd/MM/yyyy HH:mm:ss")),
                            Estado = "A"
                        };

                        Sesion sesionIngresada = await sesionManager.Ingresar(sesion, usuario.Token);

                        EstadisticaController estadisticaManager = new EstadisticaController();
                        Estadistica estadistica = new Estadistica()
                        {
                            CodigoUsuario = usuario.Codigo,
                            FechaHora = DateTime.Now,
                            Navegador = HttpContext.Current.Request.Browser.Browser,
                            PlataformaDispositivo =  Request.Browser.Platform,
                            FabricanteDispositivo = Request.Browser.MobileDeviceManufacturer,
                            Vista = "Login.aspx.cs",
                            Accion = "btnAceptar_Click"
                        };

                        Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica);

                    }
                    else
                    {
                        lblStatus.Text = "Credenciales invalidas";
                        lblStatus.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = 0,
                    Fecha = DateTime.Now,
                    Vista = "login.aspx",
                    Accion = "btnAceptar_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
                lblStatus.Text = "Hubo un error al iniciar sesion";
                lblStatus.Visible = true;
            }
                    
            }


         private void ObtenerInformacionAmbiente()
        {
            HttpBrowserCapabilities bc = Request.Browser;
            var os = Environment.OSVersion;
        }
        }

    }
