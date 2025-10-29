<%@ Page Title="" Language="C#" MasterPageFile="~/SocioMasterPage.Master" AutoEventWireup="true" CodeBehind="SocioMiPerfil.aspx.cs" Inherits="presentacionWebForm.SocioMiPerfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="perfil-container">
        <div class="perfil-header">
            <span class="perfil-icon material-icons"></span>
            <div>
                <h2>Mi Perfil</h2>
                <p>Gestiona tu información personal y tu rutina.</p>
            </div>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="perfil-mensaje" Visible="false"></asp:Label>

        <div class="perfil-seccion">
            <h3>Información Personal</h3>
            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtNombre">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" Text= "Juan" ReadOnly="true" CssClass ="perfil-input perfil-readonly" />
                </div>
                <div class="perfil-campo">
                    <label for="txtApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" Text="Perez" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-campo">
                <label for="txtDni">DNI</label>
                <asp:TextBox ID="txtDni" runat="server" Text= "12345678"  CssClass="perfil-input perfil-readonly" ReadOnly="true" />
            </div>

            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtFechaNacimiento">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" Text="1990-05-10" ReadOnly="true"  CssClass="perfil-input perfil-readonly" />
                </div>
                <div class="perfil-campo">
                    <label for="txtTelefono">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" Text="1122334455" ReadOnly="true" runat="server" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-campo">
                <label for="txtEmail">E-mail</label>
                <asp:TextBox ID="txtEmail" runat="server" Text="juanperez@mail.com" CssClass="perfil-input perfil-readonly" ReadOnly="true" />
            </div>
        </div>

        <div class="perfil-grid">
     <div class="perfil-campo">
          <label for="txtboxpsswrd">Contraseña</label>
        <asp:TextBox ID="txtboxpsswrd" runat="server"  Text="********" ReadOnly="true"  CssClass="perfil-input perfil-readonly" />
     </div>

     <div class="perfil-campo">
         <label for="txtboxPlan">Plan</label>
        <asp:TextBox ID="txtboxPlan" runat="server" Text="Premium" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
     </div>
 </div>

          <div class="perfil-botones">
            <asp:Button ID="btnEditarDatos" runat="server" Text="Editar Datos" CssClass="btn-perfil btn-izq" />
            <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" CssClass="btn-perfil btn-der" />
            </div>
        </div>
</asp:Content>
