<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminCobros.aspx.cs" Inherits="presentacionWebForm.AdminCobros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&amp;display=swap" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="dashboard">
        <div class="cobros-header">
            <h2>Cobro de cuotas</h2>
        </div>

        <div class="cobros-grid">
            <!-- SECCIÓN IZQUIERDA: REGISTRAR NUEVO COBRO -->
            <div class="cobros-panel">
                <div class="form-group">
                    <label>Socio</label>

                    <!-- Búsqueda de socio (reemplaza el DropDownList) -->
                    <div class="search-box-container">
                         <span class="material-symbols-outlined search-icon">search</span>
                        <asp:TextBox ID="txtSearchSocio" runat="server" CssClass="search-input" placeholder="Buscar socio por DNI..."></asp:TextBox>
                        <asp:Button ID="btnBuscarSocio" runat="server" Text="Buscar" CssClass="boton-principal" OnClick="btnBuscarSocio_Click" />
                        <asp:HiddenField ID="hfIdSocioSeleccionado" runat="server" />
                    </div>

                    <asp:Label ID="lblSocioSeleccionado" runat="server" CssClass="info-value" Text=""></asp:Label>
                </div>

                <div class="form-group">
                    <label>Plan</label>
                    <asp:TextBox ID="txtPlan" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="Plan del socio"></asp:TextBox>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Recargo</label>
                        <asp:TextBox ID="txtRecargo" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="$ 0.00"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Total a cobrar</label>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="$ 0.00"></asp:TextBox>
                    </div>
                </div>

                <!-- Fecha de Cobro -->
             <div class="form-group">
             <label>Fecha de Cobro</label>
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-input readonly" ReadOnly="true" TextMode="Date"></asp:TextBox>
             </div>

                <!-- Cuotas pendientes debajo -->
             <div class="form-group">
              <label>Cuotas pendientes</label>
                <asp:DropDownList ID="ddlCuotasPendientes" runat="server" CssClass="form-input">
                </asp:DropDownList>
             </div>
           
                <div class="form-group">
                    <label>Forma de Pago</label>
                    <div class="radio-group">
                        <label class="radio-option">
                            <asp:RadioButton ID="rbEfectivo" runat="server" GroupName="FormaPago" Checked="true" />
                            <span>Efectivo</span>
                        </label>
                        <label class="radio-option">
                            <asp:RadioButton ID="rbTransferencia" runat="server" GroupName="FormaPago" />
                            <span>Transferencia</span>
                        </label>
                    </div>
                </div>

                <asp:Button ID="btnGuardarCobro" runat="server" Text="Guardar Cobro" CssClass="boton-principal boton-guardar" OnClick="btnGuardarCobro_Click" />
            </div>

            <!-- SECCIÓN DERECHA: HISTORIAL DE COBROS -->
            <div class="cobros-panel">
                <h2>Historial de Cobros</h2>
                <asp:GridView ID="gvHistorialCobros" runat="server" CssClass="tabla-cobros" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Socio" HeaderText="Miembro" />
                        <asp:BoundField DataField="FechaCobro" HeaderText="Fecha Cobro" />
                        <asp:BoundField DataField="Periodo" HeaderText="Mes/Año" />
                        <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="FormaPago" HeaderText="Forma Pago" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>