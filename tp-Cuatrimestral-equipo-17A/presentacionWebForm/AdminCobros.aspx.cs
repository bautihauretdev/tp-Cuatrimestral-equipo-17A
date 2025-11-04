using negocio;
using presentacionWebForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class AdminCobros : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarSocios();
        }

        private void CargarSocios()
        {
            var cuotaNegocio = new CuotaNegocio();
            var socios = cuotaNegocio.ListarSocios();

            var sociosLista = socios.Select(s => new
            {
                IdSocio = s.IdSocio,
                NombreCompleto = s.Nombre + " " + s.Apellido
            }).ToList();

            ddlSocio.DataSource = sociosLista;
            ddlSocio.DataTextField = "NombreCompleto";
            ddlSocio.DataValueField = "IdSocio";
            ddlSocio.DataBind();
            ddlSocio.Items.Insert(0, new ListItem("Seleccionar socio...", ""));
        }
    }
}
