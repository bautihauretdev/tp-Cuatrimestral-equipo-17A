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
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="perfil-input" />
                </div>
                <div class="perfil-campo">
                    <label for="txtApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="perfil-input" />
                </div>
            </div>

            <div class="perfil-campo">
                <label for="txtDni">DNI</label>
                <asp:TextBox ID="txtDni" runat="server" CssClass="perfil-input perfil-readonly" ReadOnly="true" />
            </div>

            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtFechaNacimiento">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="perfil-input" />
                </div>
                <div class="perfil-campo">
                    <label for="txtTelefono">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="perfil-input" />
                </div>
            </div>

            <div class="perfil-campo">
                <label for="txtEmail">E-mail</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="perfil-input perfil-readonly" ReadOnly="true" />
            </div>
        </div>

        <div class="perfil-seccion">
            <h3>Mi Rutina</h3>
            <div class="perfil-rutina">
                <div class="perfil-rutina-header">
                    <h4>Día 1: Pecho y Tríceps</h4>
                    <span>Lunes</span>
                </div>
                <ul>
                    <li>Press de banca: 4x8</li>
                    <li>Aperturas con mancuernas: 3x12</li>
                    <li>Fondos en paralelas: 3 al fallo</li>
                    <li>Press francés: 4x10</li>
                </ul>
            </div>
        </div>

        <div class="perfil-botones">
            <asp:Button ID="btnEditarRutina" runat="server" Text="Editar Rutina" CssClass="btn-perfil" />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" CssClass="btn-perfil" />
        </div>
    </div>
</asp:Content>
