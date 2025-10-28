<%@ Page Title="" Language="C#" MasterPageFile="~/SocioMasterPage.Master" AutoEventWireup="true" CodeBehind="SocioTurnos.aspx.cs" Inherits="presentacionWebForm.SocioTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- CALENDARIO -->
    <div class="row g-4">
        <div class="col">
            <div class="card">

                <!-- TÍTULO Y RANGO DE FECHAS -->
                <div class="header-calendario">
                    <asp:Label ID="lblCalendarioTitulo" runat="server" CssClass="calendarioTitulo" Text="Calendario Semanal"></asp:Label>
                    <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblCalendarioRango" runat="server" CssClass="calendarioRango" Text="20 Oct - 26 Oct"></asp:Label>
                </div>

                <!-- DIAS / HORA / TURNO -->
                <div class="calendar-scroll">
                    <div class="calendar-grid">
                        <!-- DIAS DE LA SEMANA -->
                        <div class="cabeceraDias"></div>
                        <asp:Label ID="lblLunes" runat="server" CssClass="cabeceraDias" Text="Lunes"></asp:Label>
                        <asp:Label ID="lblMartes" runat="server" CssClass="cabeceraDias" Text="Martes"></asp:Label>
                        <asp:Label ID="lblMiercoles" runat="server" CssClass="cabeceraDias" Text="Miércoles"></asp:Label>
                        <asp:Label ID="lblJueves" runat="server" CssClass="cabeceraDias" Text="Jueves"></asp:Label>
                        <asp:Label ID="lblViernes" runat="server" CssClass="cabeceraDias" Text="Viernes"></asp:Label>
                        <asp:Label ID="lblSabado" runat="server" CssClass="cabeceraDias" Text="Sábado"></asp:Label>
                        <asp:Label ID="lblDomingo" runat="server" CssClass="cabeceraDias" Text="Domingo"></asp:Label>

                        <!-- (!) ATENCION: AHORA ESTA COPIADO PEGADO MANUAL, CUANDO ESTÉ LA BD SE HACE CON REPEATER (ESTÁ PREPARADO ABAJO) -->
                        <%-- Hora --%>
                        <asp:Label ID="lblHora0800" runat="server" CssClass="horaCol text-end text-secondary font-monospace pe-2" Text="08:00"></asp:Label>
                        <%-- Turnos de la hora por día--%>
                        <asp:Button ID="btn0800Lunes" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Martes" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Miercoles" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Jueves" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Viernes" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Sabado" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="btn0800Domingo" runat="server" CssClass="turnoButton" Text="Disponible" />

                        <%-- Hora --%>
                        <asp:Label ID="lblHora0900" runat="server" CssClass="horaCol text-end text-secondary font-monospace pe-2" Text="09:00"></asp:Label>
                        <%-- Turnos de la hora por día--%>
                        <asp:Button ID="Button1" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button2" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button3" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button4" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button5" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button6" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button7" runat="server" CssClass="turnoButton" Text="Disponible" />

                        <%-- Hora --%>
                        <asp:Label ID="lblHora1000" runat="server" CssClass="horaCol text-end text-secondary font-monospace pe-2" Text="10:00"></asp:Label>
                        <%-- Turnos de la hora por día--%>
                        <asp:Button ID="Button8" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button9" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button10" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button11" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button12" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button13" runat="server" CssClass="turnoButton" Text="Disponible" />
                        <asp:Button ID="Button14" runat="server" CssClass="turnoButton" Text="Disponible" />

                        <!-- (!) CUANDO ESTÉ CONECTADA LA BD HACER CON REPEATER -->
                        <!-- Filas de horarios dinámicas -->
                        <%--                        <asp:Repeater ID="rptHoras" runat="server">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server" CssClass="horaCol text-end text-secondary font-monospace pe-2"
                                    Text='<%# Eval("Hora") %>'></asp:Label>

                                <asp:Repeater ID="rptTurnosDia" runat="server" DataSource='<%# Eval("Turnos") %>'>
                                    <ItemTemplate>
                                        <asp:Button ID="btnTurno" runat="server" CssClass="turnoButton" Text='<%# Eval("Estado") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
