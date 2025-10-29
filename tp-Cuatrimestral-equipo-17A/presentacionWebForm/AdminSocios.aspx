<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminSocios.aspx.cs" Inherits="presentacionWebForm.AdminSocios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Styles/AdminSociosStyles.css") %>" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&amp;display=swap" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-container">
        <div class="content-wrapper">

            <div class="header-section">
                <div class="title-and-search">
                    <div class="search-box-container">
                        <span class="material-symbols-outlined search-icon">search</span>
                        <asp:TextBox ID="txtSearchDNI" runat="server" CssClass="search-input" placeholder="Buscar socio por DNI..."></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAgregarSocio" runat="server" Text="Agregar nuevo socio" CssClass="boton-principal" />
                </div>
            </div>

            <asp:Panel ID="pnlSocioPrincipal" runat="server" CssClass="socio-main-panel">

                <div class="card">
                    <h2 class="panel-title">Información Personal</h2>
                    <div class="info-grid">
                        <div class="info-item">
                            <p class="info-label">Nombre</p>
                            <asp:Label ID="lblNombre" runat="server" Text="Juan" CssClass="info-value" />
                        </div>
                        <div class="info-item">
                            <p class="info-label">Apellido</p>
                            <asp:Label ID="lblApellido" runat="server" Text="Pérez" CssClass="info-value" />
                        </div>
                        <div class="info-item">
                            <p class="info-label">DNI</p>
                            <asp:Label ID="lblDNI" runat="server" Text="12345678A" CssClass="info-value" />
                        </div>
                        <div class="info-item">
                            <p class="info-label">Fecha de nacimiento</p>
                            <asp:Label ID="lblFechaNacimiento" runat="server" Text="15/08/1990" CssClass="info-value" />
                        </div>
                        <div class="info-item info-span-2">
                            <p class="info-label">Teléfono</p>
                            <asp:Label ID="lblTelefono" runat="server" Text="+34 600 123 456" CssClass="info-value" />
                        </div>
                        <div class="info-item info-span-2">
                            <p class="info-label">Email</p>
                            <asp:Label ID="lblEmail" runat="server" Text="juan.perez@email.com" CssClass="info-value" />
                        </div>
                    </div>
                    <div class="action-buttons-container">
                        <asp:Button ID="btnEditarPerfil" runat="server" Text="Editar perfil del socio" CssClass="boton-editar" />
                    </div>
                </div>

                <div class="side-panels-wrapper">
                    <div class="card">
                        <div class="socio-status">
                            <asp:Label ID="lblNombreCompleto" runat="server" Text="Juan Pérez" CssClass="socio-name" />
                            <div class="status-indicator">
                                <span class="status-dot-animated"></span>
                                <span class="status-dot-solid"></span>
                                <asp:Label ID="lblEstado" runat="server" Text="Activo" CssClass="status-text-active" />
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <h2 class="panel-title">Plan de Membresía</h2>
                        <div class="membership-details">
                            <div class="info-item">
                                <p class="info-label">Plan Actual</p>
                                <asp:Label ID="lblPlanActual" runat="server" Text="Plan Premium Mensual" CssClass="info-value" />
                            </div>
                            <div class="info-item">
                                <p class="info-label">Fecha de Inicio</p>
                                <asp:Label ID="lblFechaInicio" runat="server" Text="01/03/2024" CssClass="info-value" />
                            </div>
                            <div class="info-item">
                                <p class="info-label">Próxima Renovación</p>
                                <asp:Label ID="lblProximaRenovacion" runat="server" Text="01/09/2024" CssClass="info-value" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
