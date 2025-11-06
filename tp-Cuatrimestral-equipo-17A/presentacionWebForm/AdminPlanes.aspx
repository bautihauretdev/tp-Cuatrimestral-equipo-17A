<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminPlanes.aspx.cs" Inherits="presentacionWebForm.AdminPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="dashboard">
        <div class="planes-header">
            <h2>Planes de Membresía</h2>
        </div>

        <div class="planes-grid">
            <!-- SECCIÓN IZQUIERDA: REGISTRAR NUEVO PLAN -->
            <div class="planes-panel">
                <div class="planes-form-group">
                    <label>Plan</label>
                    <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-input">
                        <asp:ListItem Text="Seleccionar plan" />
                    </asp:DropDownList>
                </div>

                <div class="planes-form-row">
                    <div class="planes-form-group">
                        <label>Horas x Semana</label>
                        <asp:TextBox ID="txtHorasSemana" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="0"></asp:TextBox>
                    </div>

                    <div class="planes-form-group">
                        <label>Monto</label>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="$ 0.00"></asp:TextBox>
                    </div>
                </div>

                <div class="planes-form-group">
                    <div class="planes-btn-group">
                        <asp:Button ID="btnEditarPlan" runat="server" Text="Editar Plan" CssClass="boton-editar" />
                        <asp:Button ID="btnEliminarPlan" runat="server" Text="Eliminar Plan" CssClass="boton-editar" />
                    </div>
                </div>
            </div>

            <!-- SECCIÓN DERECHA: SOCIOS EN EL PLAN -->
            <div class="planes-panel">
                <div class="planes-header">
                    <h2>Socios en el plan</h2>
                    <asp:Label ID="cantidadSociosPlan" runat="server" CssClass="cantidad-socios-plan" Text="#15" />
                </div>

                <asp:GridView ID="GridView1" runat="server" CssClass="tabla-cobros" AutoGenerateColumns="False">
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
