<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminTurnos.aspx.cs" Inherits="presentacionWebForm.AdminTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard">
        <!-- METRICAS -->
        <section class="reportes-metricas">
            <div class="card">
                <div class="turnos-metricas-card">
                    <i class="bi bi-people-fill metricas-icono fs-4"></i>
                    <h3>Socios activos</h3>
                </div>
                <asp:Label ID="lblSociosActivosNum" runat="server" CssClass="metricas-numero" Text="174"></asp:Label>
            </div>

            <div class="card">
                <div class="turnos-metricas-card">
                    <i class="bi bi-person-check-fill metricas-icono fs-4"></i>
                    <h3>Socios que vienen hoy</h3>
                </div>
                <asp:Label ID="lblSociosHoyNum" runat="server" CssClass="metricas-numero" Text="51"></asp:Label>
            </div>

            <div class="card">
                <div class="turnos-metricas-card">
                    <i class="bi bi-clock-fill metricas-icono fs-4"></i>
                    <h3>Horarios completos</h3>
                </div>
                <asp:Label ID="lblHorariosCompletosNum" runat="server" CssClass="metricas-numero" Text="5"></asp:Label>

            </div>
        </section>

        <!-- CALENDARIO -->
        <div class="row g-4">
            <div class="col">
                <div class="card">

                    <!-- TÍTULO Y RANGO DE FECHAS -->
                    <div class="calendario-header d-flex align-items-center gap-3">
                        <h2>Calendario semanal</h2>

                        <div class="calendario-rango-container d-flex align-items-center gap-1">
                            <!-- Flecha atrás -->
                            <asp:LinkButton ID="btnSemanaAnterior" runat="server" CssClass="calendario-flecha" OnClick="btnSemanaAnterior_Click">
                                <i class="bi bi-chevron-left"></i>
                            </asp:LinkButton>

                            <!-- Rango fechas semana  -->
                            <asp:Label ID="lblCalendarioRango" runat="server" CssClass="calendario-rango" Text="-"></asp:Label>

                            <!-- Flecha adelante -->
                            <asp:LinkButton ID="btnSemanaSiguiente" runat="server" CssClass="calendario-flecha" OnClick="btnSemanaSiguiente_Click">
                                <i class="bi bi-chevron-right"></i>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <!-- C A L E N D A R I O -->
                    <div class="calendario-scroll">
                        <div class="calendario-grid">
                            <!-- DIAS DE LA SEMANA -->
                            <div class="calendario-cabecera-dias"></div>
                            <asp:Label ID="lblLunes" runat="server" CssClass="calendario-cabecera-dias" Text="Lunes"></asp:Label>
                            <asp:Label ID="lblMartes" runat="server" CssClass="calendario-cabecera-dias" Text="Martes"></asp:Label>
                            <asp:Label ID="lblMiercoles" runat="server" CssClass="calendario-cabecera-dias" Text="Miércoles"></asp:Label>
                            <asp:Label ID="lblJueves" runat="server" CssClass="calendario-cabecera-dias" Text="Jueves"></asp:Label>
                            <asp:Label ID="lblViernes" runat="server" CssClass="calendario-cabecera-dias" Text="Viernes"></asp:Label>
                            <asp:Label ID="lblSabado" runat="server" CssClass="calendario-cabecera-dias" Text="Sábado"></asp:Label>
                            <asp:Label ID="lblDomingo" runat="server" CssClass="calendario-cabecera-dias" Text="Domingo"></asp:Label>

                            <!-- R E P E A T E R -->
                            <asp:Repeater ID="rptHoras" runat="server">
                                <ItemTemplate>
                                    <!-- HORAS -->
                                    <asp:Label
                                        ID="lblHora"
                                        runat="server"
                                        CssClass="calendario-hora text-end text-secondary font-monospace pe-2"
                                        Text='<%# Eval("HoraTexto") %>'>
                                    </asp:Label>

                                    <!-- TURNOS (uno por cada día de la semana) -->
                                    <asp:Repeater ID="rptTurnosDia" runat="server" DataSource='<%# Eval("Turnos") %>'>
                                        <ItemTemplate>
                                            <!-- BOTÓN DEL TURNO -->
                                            <asp:Button
                                                ID="btnTurno"
                                                runat="server"
                                                CssClass="calendario-turno"
                                                Text='<%# Eval("EstadoTexto") %>'
                                                CommandArgument='<%# Eval("IdTurno") %>'
                                                OnClick="btnTurno_Click" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                    </div>

                    <!-- BOTON EDITAR TURNO -->
                    <div class="footer-calendario">
                        <button type="button" class="boton-editar" data-bs-toggle="modal" data-bs-target="#modalEditarTurno">
                            Editar turnos
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

                        <!-- Fila: Fechas -->
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

                        <!-- Fila: Horas -->
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="txtHoraDesde" class="form-label">Hora desde</label>
                                <asp:TextBox ID="txtHoraDesde" runat="server" TextMode="Time" CssClass="form-control" step="3600"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="txtHoraHasta" class="form-label">Hora hasta</label>
                                <asp:TextBox ID="txtHoraHasta" runat="server" TextMode="Time" CssClass="form-control" step="3600"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Capacidad -->
                        <div class="mb-3">
                            <label for="txtCapacidad" class="form-label">Capacidad máxima</label>
                            <asp:TextBox ID="txtCapacidad" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                        </div>

                        <!-- Mensaje error para editar turno -->
                        <div class="mb-3">
                            <asp:Label ID="lblErrorEditarTurno" runat="server" CssClass="text-danger" Visible="false" />
                        </div>
                    </div>

                    <div class="modal-footer border-secondary">
                        <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnGuardarCambios" runat="server" CssClass="boton-principal" Text="Guardar cambios" OnClick="btnGuardarCambios_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- VENTANA FLOTANTE DETALLE SOCIOS DEL TURNO -->
        <div class="modal fade" id="modalDetalleTurno" tabindex="-1" aria-labelledby="modalDetalleTurnoLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title" id="modalDetalleTurnoLabel">
                            <asp:Label ID="lblDetalleTurnoTitulo" runat="server" Text="Socios del turno"></asp:Label>
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>

                    <div class="modal-body">
                        <!-- Lista de socios del turno -->
                        <asp:Repeater ID="rptSociosTurno" runat="server">
                            <HeaderTemplate>
                                <ul class="list-group list-group-flush">
                            </HeaderTemplate>

                            <ItemTemplate>
                                <li class="list-group-item bg-dark text-light border-secondary d-flex flex-column">
                                    <span><strong><%# Eval("Apellido") %>, <%# Eval("Nombre") %></strong></span>
                                    <small class="text-secondary">DNI: <%# Eval("Dni") %> - Email: <%# Eval("Email") %>
                                    </small>
                                </li>
                            </ItemTemplate>

                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>

                        <!-- Mensaje si no hay socios -->
                        <asp:Label ID="lblSinSociosTurno" runat="server"
                            CssClass="text-secondary"
                            Visible="false"
                            Text="No hay socios anotados en este turno.">
                        </asp:Label>
                    </div>

                    <div class="modal-footer border-secondary">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- Para el modal "Editar turno" -->
    <script>
        var limpiarCamposEditar = false; // Variable global para controlar si limpiar campos

        // Cuando se abre el modal "Editar turno", limpia los campos
        var modalEditar = document.getElementById('modalEditarTurno');

        modalEditar.addEventListener('show.bs.modal', function () {
            // Solo limpiar si NO fue un postback con error
            if (limpiarCamposEditar) {
                document.getElementById('<%= txtFechaDesde.ClientID %>').value = "";
                document.getElementById('<%= txtFechaHasta.ClientID %>').value = "";
                document.getElementById('<%= txtHoraDesde.ClientID %>').value = "";
                document.getElementById('<%= txtHoraHasta.ClientID %>').value = "";
                document.getElementById('<%= txtCapacidad.ClientID %>').value = "";
                document.getElementById('<%= lblErrorEditarTurno.ClientID %>').style.display = "none";
            } else {
                // Cuando hay error, no limpiar campos ni ocultar label
                limpiarCamposEditar = true; // resetea para la próxima vez
            }
        });
    </script>

</asp:Content>
