<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminPlanes.aspx.cs" Inherits="presentacionWebForm.AdminPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="dashboard">


        <div class="planes-grid">
            <h2>Planes de Membresía</h2>

            <div class="planes-panel">
                <div class="form-group">
                    <label>Plan</label>
                    <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-input">
                        <asp:ListItem Text="Seleccionar plan" />
                    </asp:DropDownList>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Horas x Semana</label>
                        <asp:TextBox ID="txtHorasSemana" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="0"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Monto</label>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="$ 0.00"></asp:TextBox>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Cantidad de Socios Inscriptos:</label>
                        <asp:TextBox ID="txtMes" runat="server" CssClass="form-input readonly" ReadOnly="true" placeholder="#"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="planes-group">
                        <asp:Button ID="btnEditarPlan" runat="server" Text="Editar Plan" CssClass="boton-editar" />
                        <asp:Button ID="btnEliminarPlan" runat="server" Text="Eliminar Plan" CssClass="boton-editar" />
                    </div>
                </div>

            </div>
        </div>
    </main>
</asp:Content>
