<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminReportes.aspx.cs" Inherits="presentacionWebForm.AdminReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="dashboard">
        <h2>Resumen general del estado del gimnasio</h2>

        <!-- MÉTRICAS PRINCIPALES -->
        <section class="metricas">
            <div class="card">
                <h3>Socios activos</h3>
                <asp:Label ID="lblSociosActivos" runat="server" Text="154" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Ingresos del mes</h3>
                <asp:Label ID="lblIngresosMes" runat="server" Text="$12,450" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Socios morosos</h3>
                <asp:Label ID="lblMorosos" runat="server" Text="13" CssClass="numero-metricas text-red"></asp:Label>
            </div>
            <div class="card">
                <h3>Ocupación prom. turnos</h3>
                <asp:Label ID="lblOcupacionProm" runat="server" Text="1%" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Día con más concurrencia</h3>
                <asp:Label ID="lblDiaMasConcurrencia" runat="server" Text="Lunes" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Franja con más concurrencia</h3>
                <asp:Label ID="lblFranjaMasConcurrencia" runat="server" Text="18:00 - 19:00" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Día con menos concurrencia</h3>
                <asp:Label ID="lblDiaMenosConcurrencia" runat="server" Text="Domingo" CssClass="numero-metricas"></asp:Label>
            </div>
            <div class="card">
                <h3>Franja con menos concurrencia</h3>
                <asp:Label ID="lblFranjaMenosConcurrencia" runat="server" Text="12:00 - 13:00" CssClass="numero-metricas"></asp:Label>
            </div>
        </section>

        <!-- LISTADOS DE INTERÉS -->
        <section class="listas">
            <h2>Listados de interés</h2>
            <div class="listas-grid">
                <!-- Morosos -->
                <div class="lista-card">
                    <h3>Socios morosos</h3>
                    <asp:Repeater ID="rptMorosos" runat="server">
                        <ItemTemplate>
                            <div class="lista-item">
                                <div>
                                    <p class="nombre"><%# Eval("Nombre") %></p>
                                    <p class="detalle"><%# Eval("DiasVencido") %> días vencido</p>
                                </div>
                                <span class="monto negativo">-<%# Eval("MontoAdeudado") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Próximos pagos -->
                <div class="lista-card">
                    <h3>Próximos pagos por vencer</h3>
                    <asp:Repeater ID="rptPorVencer" runat="server">
                        <ItemTemplate>
                            <div class="lista-item">
                                <div>
                                    <p class="nombre"><%# Eval("Nombre") %></p>
                                    <p class="detalle">Vence en <%# Eval("DiasRestantes") %> días</p>
                                </div>
                                <span class="monto aviso">$<%# Eval("Monto") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Top activos -->
                <div class="lista-card">
                    <h3>Top 5 socios más activos</h3>
                    <asp:Repeater ID="rptActivos" runat="server">
                        <ItemTemplate>
                            <div class="lista-item top">
                                <span class="nombre"><%# Container.ItemIndex + 1 %>. <%# Eval("Nombre") %></span>
                                <span class="detalle"><%# Eval("Asistencias") %> asistencias</span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </section>
    </main>

</asp:Content>
