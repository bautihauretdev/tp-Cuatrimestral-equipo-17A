<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AltaSocio.aspx.cs" Inherits="presentacionWebForm.AltaSocio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="main-container">
        <div class="content-wrapper">
            <div class="card" style="max-width:480px; margin:auto;">
                <h2 class="panel-title" style="text-align:center; margin-bottom:2rem;">
                    Alta de nuevo socio
                </h2>
                <asp:Label ID="lblMensajeAltaSocio" runat="server" CssClass="text-success" Visible="false" />
                <asp:Label ID="lblErrorAltaSocio" runat="server" CssClass="text-danger" Visible="false" />
                
                <asp:HiddenField ID="hfIdEditar" runat="server" />
                
                <div class="info-grid">
                    <div class="form-group">
                        <label class="info-label" for="txtNombreAltaSocio">Nombre</label>
                        <asp:TextBox ID="txtNombreAltaSocio" runat="server" CssClass="form-input" placeholder="Nombre" />
                    </div>
                    <div class="form-group">
                        <label class="info-label" for="txtApellidoAltaSocio">Apellido</label>
                        <asp:TextBox ID="txtApellidoAltaSocio" runat="server" CssClass="form-input" placeholder="Apellido" />
                    </div>
                    <div class="form-group">
                        <label class="info-label" for="txtDniAltaSocio">DNI</label>
                        <asp:TextBox ID="txtDniAltaSocio" runat="server" CssClass="form-input" placeholder="DNI" />
                    </div>
                    <div class="form-group">
                        <label class="info-label" for="txtFechaNacAltaSocio">Fecha de Nacimiento</label>
                        <asp:TextBox ID="txtFechaNacAltaSocio" runat="server" CssClass="form-input" TextMode="Date" placeholder="yyyy-mm-dd" />
                    </div>
                    <div class="form-group">
                        <label class="info-label" for="txtTelefonoAltaSocio">Teléfono</label>
                        <asp:TextBox ID="txtTelefonoAltaSocio" runat="server" CssClass="form-input" placeholder="Teléfono" />
                    </div>
                    <div class="form-group info-span-2">
                        <label class="info-label" for="txtEmailAltaSocio">Email</label>
                        <asp:TextBox ID="txtEmailAltaSocio" runat="server" CssClass="form-input" placeholder="Email" />
                    </div>
                </div>

                <div style="margin-top:2rem; text-align:center;">
                    <asp:Button ID="btnGuardarAltaSocio" runat="server" Text="Guardar" CssClass="boton-principal boton-guardar" OnClick="btnGuardarAltaSocio_Click" />
                     <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" CssClass="boton-principal boton-guardar"  Visible="false" OnClick="btnGuardarCambios_Click" />
                </div>
                <div style="margin-top:1rem; text-align:center;">
                    <span style="font-size:13px; color:#96c5a9;">La contraseña inicial será el DNI del socio</span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
