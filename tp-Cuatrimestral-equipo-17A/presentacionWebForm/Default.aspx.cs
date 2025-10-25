using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            Usuario usuario = negocio.Login(txtBoxEmail.Text, txtBoxPassword.Text);

            if (usuario != null && usuario.Activo)
            {
                Session["usuario"] = usuario;
                if (usuario.Rol == RolUsuario.Administrador)
                    Response.Redirect("AdminHome.aspx");
                else
                    Response.Redirect("SocioHome.aspx");
            }
            else
            {
                lblError.Text = "Credenciales incorrectas o usuario inactivo.";
                lblError.Visible = true;
            }
        }
    }
}