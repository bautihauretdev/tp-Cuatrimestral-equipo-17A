<%@ Page Title="" Language="C#" MasterPageFile="~/SocioMasterPage.Master" AutoEventWireup="true" CodeBehind="SocioMiPerfil.aspx.cs" Inherits="presentacionWebForm.SocioMiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="perfil-container">
        <div class="perfil-header">
            <h2>Mi Perfil</h2>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="perfil-mensaje" Visible="false"></asp:Label>

        <div class="perfil-seccion">
            <h3>Información Personal</h3>
            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtNombre">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
                <div class="perfil-campo">
                    <label for="txtApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtDni">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
                <div class="perfil-campo">
                    <label for="txtFechaNacimiento">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtTelefono">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" ReadOnly="true" runat="server" CssClass="perfil-input perfil-readonly" />
                </div>
                <div class="perfil-campo">
                    <label for="txtEmail">E-mail</label>
                   <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-grid">
                <div class="perfil-campo">
                    <label for="txtboxpsswrd">Contraseña</label>
                   <asp:TextBox ID="txtboxpsswrd" runat="server" Text="********" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>

                <div class="perfil-campo">
                    <label for="txtboxPlan">Plan</label>
                    <asp:TextBox ID="txtboxPlan" runat="server" ReadOnly="true" CssClass="perfil-input perfil-readonly" />
                </div>
            </div>

            <div class="perfil-botones">
                <asp:Button ID="btnEditarDatos" runat="server" Text="Editar Datos" CssClass="btn-perfil boton-editar" />
                <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" CssClass="btn-perfil boton-principal" />
            </div>
        </div>
    </div>
</asp:Content>
