<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminTurnos.aspx.cs" Inherits="presentacionWebForm.AdminTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- CARDS INFORMACIÓN -->
    <div class="row row-cols-1 row-cols-md-3 g-4">
        <!-- Miembros activos -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-people-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblActivos" runat="server" CssClass="tituloTurnos" Text="Miembros activos"></asp:Label>
                    </div>
                    <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblActivosNum" runat="server" CssClass="numeroTurnos" Text="174"></asp:Label> 

                </div>
            </div>
        </div>

        <!-- Socios que vienen hoy -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-person-check-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblSociosHoy" runat="server" CssClass="tituloTurnos" Text="Socios que vienen hoy"></asp:Label>
                    </div>
                    <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblSociosHoyNum" runat="server" CssClass="numeroTurnos" Text="51"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Horarios completos -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-clock-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblCompletos" runat="server" CssClass="tituloTurnos" Text="Horarios completos"></asp:Label>
                    </div>
                    <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblCompletosNum" runat="server" CssClass="numeroTurnos" Text="5"></asp:Label>
                </div>
            </div>
        </div>
    </div>

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
            <div class="calendar-grid">
<%--            <div class="d-grid" style="grid-template-columns: auto repeat(7, minmax(140px, 1fr)); gap: 0.5rem; min-width:1100px;">--%>
                <!-- DIAS DE LA SEMANA -->
                <div class="cabeceraDias"></div>
                <asp:Label ID="lblLunes" runat="server" CssClass="cabeceraDias" Text="Lunes"></asp:Label>
                <asp:Label ID="lblMartes" runat="server" CssClass="cabeceraDias" Text="Martes"></asp:Label>
                <asp:Label ID="lblMiercoles" runat="server" CssClass="cabeceraDias" Text="Miércoles"></asp:Label>
                <asp:Label ID="lblJueves" runat="server" CssClass="cabeceraDias" Text="Jueves"></asp:Label>
                <asp:Label ID="lblViernes" runat="server" CssClass="cabeceraDias" Text="Viernes"></asp:Label>
                <asp:Label ID="lblSabado" runat="server" CssClass="cabeceraDias" Text="Sábado"></asp:Label>
                <asp:Label ID="lblDomingo" runat="server" CssClass="cabeceraDias" Text="Domingo"></asp:Label>

                <!-- FILAS DE HORARIOS -->
                <%-- Hora --%>
                <asp:Label ID="lblHora0800" runat="server" CssClass="mt-2 text-end text-secondary font-monospace pe-2" Text="08:00"></asp:Label>
                
                <%-- Turnos por día--%>
                <asp:Button ID="btn0800Lunes" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Martes" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Miercoles" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Jueves" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Viernes" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Sabado" runat="server" CssClass="turnoButton" Text="Disponible" />
                <asp:Button ID="btn0800Domingo" runat="server" CssClass="turnoButton" Text="Disponible" />
        
                <%-- Repetir bloque anterior por cada hora que quieras mostrar --%>
            </div>
        </div>
    </div>


</div>

</asp:Content>
