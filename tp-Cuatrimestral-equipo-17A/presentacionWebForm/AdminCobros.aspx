<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminCobros.aspx.cs" Inherits="presentacionWebForm.AdminCobros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                    <asp:DropDownList ID="ddlSocio" runat="server" CssClass="form-input">
                        <asp:ListItem Text="Seleccionar socio" />
                    </asp:DropDownList>
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

                <div class="form-row">
                    <div class="form-group">
                        <label>Fecha de Cobro</label>
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="form-input readonly" ReadOnly="true" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Cuota</label>
                        <asp:TextBox ID="txtMes" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="Mes / Año"></asp:TextBox>
                    </div>
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

                <asp:Button ID="btnGuardarCobro" runat="server" Text="Guardar Cobro" CssClass="boton-principal boton-guardar" />
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
                        <asp:ButtonField Text="Eliminar" ButtonType="Button" ControlStyle-CssClass="btn-eliminar" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>
