<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminTurnos.aspx.cs" Inherits="presentacionWebForm.AdminTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard">
        <section class="metricas">
            <div class="card">
                <div class="header-turnos">
                    <i class="bi bi-people-fill iconoColor fs-4"></i>
                    <h3>Socios activos</h3>
                </div>
                <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                <asp:Label ID="lblSociosActivosNum" runat="server" CssClass="numero-metricas" Text="174"></asp:Label>
            </div>

            <div class="card">
                <div class="header-turnos">
                    <i class="bi bi-person-check-fill iconoColor fs-4"></i>
                    <h3>Socios que vienen hoy</h3>
                </div>
                    <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                <asp:Label ID="lblSociosHoyNum" runat="server" CssClass="numero-metricas" Text="51"></asp:Label>
            </div>

            <div class="card">
                <div class="header-turnos">
                    <i class="bi bi-clock-fill iconoColor fs-4"></i>
                    <h3>Horarios completos</h3>
                </div>
                <%-- (!) VER: En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                <asp:Label ID="lblHorariosCompletosNum" runat="server" CssClass="numero-metricas" Text="5"></asp:Label>

            </div>
        </section>

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

                    <!-- BOTONES -->
                    <%--                <div class="footer-calendario">
                    <asp:Button ID="btnAgregarTurno" runat="server" Text="Agregar turno" CssClass="agregarTurno" />
                    <asp:Button ID="btnEditarTurno" runat="server" Text="Editar turno" CssClass="modificarTurno" />
                </div>--%>

                    <div class="footer-calendario">
                        <button type="button" class="boton-editar" data-bs-toggle="modal" data-bs-target="#modalEditarTurno">
                            Editar turno
                        </button>
                    </div>

                </div>
            </div>
        </div>

        <!-- VENTANA FLOTANTE EDITAR TURNO -->
        <div class="modal fade" id="modalEditarTurno" tabindex="-1" aria-labelledby="modalEditarTurnoLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title" id="modalEditarTurnoLabel">Editar turnos</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>

                    <div class="modal-body">

                        <!-- FILA: FECHAS -->
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="txtFechaDesde" class="form-label">Fecha desde</label>
                                <asp:TextBox ID="txtFechaDesde" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="txtFechaHasta" class="form-label">Fecha hasta</label>
                                <asp:TextBox ID="txtFechaHasta" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- FILA: HORAS -->
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="txtHoraDesde" class="form-label">Hora desde</label>
                                <asp:TextBox ID="txtHoraDesde" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="txtHoraHasta" class="form-label">Hora hasta</label>
                                <asp:TextBox ID="txtHoraHasta" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- CAPACIDAD -->
                        <div class="mb-3">
                            <label for="txtCapacidad" class="form-label">Capacidad máxima</label>
                            <asp:TextBox ID="txtCapacidad" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="modal-footer border-secondary">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnGuardarCambios" runat="server" CssClass="boton-principal" Text="Guardar cambios" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
