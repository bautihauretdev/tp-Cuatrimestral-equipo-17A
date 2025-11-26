<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminSocios.aspx.cs" Inherits="presentacionWebForm.AdminSocios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Styles/AdminSociosStyles.css") %>" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&amp;display=swap" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- BUSQUEDA + AGREGAR SOCIO -->
    <div class="socios-header">
        <div class="title-and-search">
            <div class="search-box-container">
                <span class="material-symbols-outlined search-icon">search</span>

                <asp:TextBox ID="txtBuscarDNI" runat="server"
                    CssClass="search-input"
                    placeholder="Buscar socio por DNI..."></asp:TextBox>

                <asp:Button ID="btnBuscarDNI" runat="server"
                    Text="Buscar" CssClass="boton-principal"
                    OnClick="btnBuscarDNI_Click" />
            </div>

            <asp:Button ID="btnAgregarSocio" runat="server"
                Text="Agregar nuevo socio" CssClass="boton-principal"
                OnClick="btnAgregarSocio_Click" />
        </div>
    </div>

    <asp:Label ID="lblErrorBusqueda" runat="server"
        CssClass="text-danger" Visible="false" />

    <asp:HiddenField ID="hfIdSocioSeleccionado" runat="server" />


    <!-- LISTA DE SOCIOS -->
    <div class="card" style="margin-top:1rem;">
        <h2 class="panel-title">Listado de socios</h2>

       <asp:Repeater ID="rptSocios" runat="server" OnItemCommand="rptSocios_ItemCommand"> 
    <HeaderTemplate>
        <table class="socios-table">
            <thead>
                <tr>
                    <th style="width:60px;">Id</th>
                    <th>Nombre</th>
                    <th>Apellido</th>
                    <th>DNI</th>
                    <th>Email</th>
                    <th>Plan</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>

    <ItemTemplate>
        <tr class="<%# Eval("Activo") != null && !(bool)Eval("Activo") ? "estado-inactivo-row" : "" %>">

            <td><%# Eval("IdSocio") %></td>

            <td>
                <asp:LinkButton ID="lnkNombre" runat="server"
                    CommandName="SelectSocio"
                    CommandArgument='<%# Eval("IdSocio") %>'
                    CssClass="link-socio">
                    <%# Eval("Nombre") %>
                </asp:LinkButton>
            </td>

            <td>
                <asp:LinkButton ID="lnkApellido" runat="server"
                    CommandName="SelectSocio"
                    CommandArgument='<%# Eval("IdSocio") %>'
                    CssClass="link-socio">
                    <%# Eval("Apellido") %>
                </asp:LinkButton>
            </td>

            <td><%# Eval("Dni") %></td>
            <td><%# Eval("Email") %></td>

            <td><%# Eval("Plan.Nombre") ?? Eval("PlanNombre") ?? "Sin plan" %></td>

        </tr>
    </ItemTemplate>

    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

    </div>



    <!-- MODAL TARJETA SOCIO -->
    <div class="modal fade" id="modalSocioDetalle" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-xl">
            <div class="modal-content bg-dark text-white">

                <div class="modal-header border-secondary">
                    <h5 class="modal-title">Detalle del socio</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <asp:Panel ID="pnlSocioPrincipal" runat="server" CssClass="socio-main-panel">

                        <!-- INFORMACIÓN PERSONAL -->
                        <div class="card">
                            <h2 class="panel-title">Información Personal</h2>

                            <div class="info-grid">

                                <div class="info-item">
                                    <p class="info-label">Nombre</p>
                                    <asp:Label ID="lblNombre" runat="server" CssClass="info-value" />
                                </div>

                                <div class="info-item">
                                    <p class="info-label">Apellido</p>
                                    <asp:Label ID="lblApellido" runat="server" CssClass="info-value" />
                                </div>

                                <div class="info-item">
                                    <p class="info-label">DNI</p>
                                    <asp:Label ID="lblDNI" runat="server" CssClass="info-value" />
                                </div>

                                <div class="info-item">
                                    <p class="info-label">Fecha de nacimiento</p>
                                    <asp:Label ID="lblFechaNacimiento" runat="server" CssClass="info-value" />
                                </div>

                                <div class="info-item info-span-2">
                                    <p class="info-label">Teléfono</p>
                                    <asp:Label ID="lblTelefono" runat="server" CssClass="info-value" />
                                </div>

                                <div class="info-item info-span-2">
                                    <p class="info-label">Email</p>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="info-value" />
                                </div>

                            </div>

                            <div class="socio-section-button">
                                <asp:Button ID="btnEditarPerfil" runat="server"
                                    Text="Editar perfil"
                                    CssClass="boton-editar"
                                    OnClick="btnEditarPerfil_Click" />
                                
                                <asp:Button ID="btnEliminarLogico" runat="server"
                                    Text="Dar de baja"
                                    CssClass="boton-editar"
                                    OnClick="btnEliminarLogico_Click" />
                            </div>
                        </div>


                        <!-- LATERAL DERECHO -->
                        <div class="side-panels-wrapper">

                            <div class="card">
                                <div class="socio-status">
                                    <asp:Label ID="lblNombreCompleto" runat="server" CssClass="socio-name" />

                                    <div class="status-indicator">
                                        <span class="status-dot-animated"></span>
                                        <span class="status-dot-solid"></span>
                                        <asp:Label ID="lblEstado" runat="server" CssClass="status-text-active" />
                                    </div>
                                </div>
                            </div>

                            <div class="card">
                                <h2 class="panel-title">Información Adicional</h2>

                                <div class="membership-details">

                                    <div class="info-item">
                                        <p class="info-label">Plan Actual</p>
                                        <asp:Label ID="lblPlanActual" runat="server" CssClass="info-value" />
                                    </div>

                                </div>
                            </div>

                        </div>

                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>



    <!-- MODAL: SOCIO INACTIVO -->
    <div class="modal fade" id="modalEditarInactivo" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content bg-dark text-white">

                <div class="modal-header border-secondary">
                    <h5 class="modal-title">Socio inactivo</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <p>Este socio se encuentra marcado como <strong>inactivo</strong>.</p>
                    <p>¿Desea reactivarlo para poder editar su información?</p>

                    <asp:HiddenField ID="hfIdSocioInactivo" runat="server" />
                </div>

                <div class="modal-footer border-secondary">
                    <asp:Button ID="Button1" runat="server"
                        CssClass="boton-principal"
                        Text="Reactivar socio"
                        OnClick="btnReactivarDesdeEdicion_Click" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>
