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

        protected void btnFechaNacimiento_Click(object sender, EventArgs e)
        {
            clnFechaNacimiento.Visible = true;
        }

        protected void clnFechaNacimiento_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaNacimiento.Text = clnFechaNacimiento.SelectedDate.ToString("dd/MM/yyyy");
            clnFechaNacimiento.Visible = false;
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

                }
                catch (Exception)
                {
                    lblStatus.Text = "Hubo un error al registrar el usuario";
                    lblStatus.Visible = true;
                }
            }

            
        }
    }
}