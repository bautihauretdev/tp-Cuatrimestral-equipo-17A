<%@ Page Title="" Language="C#" MasterPageFile="~/SocioMasterPage.Master" AutoEventWireup="true" CodeBehind="SocioTurnos.aspx.cs" Inherits="presentacionWebForm.SocioTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard">
        <!-- CALENDARIO -->
        <div class="row g-4">
            <div class="col">
                <div class="card">

                    <!-- TÍTULO Y RANGO DE FECHAS -->
                    <div class="calendario-header d-flex align-items-center gap-3">
                        <h2>Calendario semanal</h2>

                        <div class="calendario-rango-container d-flex align-items-center gap-1">
                            <!-- Flecha atrás -->
                            <asp:LinkButton ID="btnSemanaAnterior" runat="server" CssClass="calendario-flecha">
                                <i class="bi bi-chevron-left"></i>
                            </asp:LinkButton>

                            <!-- Rango fechas semana  -->
                            <asp:Label ID="lblCalendarioRango" runat="server" CssClass="calendario-rango" Text="-"></asp:Label>

                            <!-- Flecha adelante -->
                            <asp:LinkButton ID="btnSemanaSiguiente" runat="server" CssClass="calendario-flecha">
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
                                    <!-- BOTÓN TURNO -->
                                    <asp:Repeater ID="rptTurnosDia" runat="server" DataSource='<%# Eval("Turnos") %>'>
                                        <ItemTemplate>
                                            <button type="button"
                                                class='calendario-turno <%# (bool)Eval("ReservadoPorSocio") ? "reservado-socio" : "" %>'
                                                onclick="abrirModalPedirTurno('<%# Eval("IdTurno") %>', '<%# Eval("FechaHoraTexto") %>', <%# ((bool)Eval("ReservadoPorSocio")).ToString().ToLower() %>)">
                                                <%# Eval("EstadoTexto") %>
                                            </button>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                    </div>

                    <!-- BOTON PEDIR TURNO FIJO -->
                    <div class="footer-calendario">
                        <button type="button" class="boton-editar" data-bs-toggle="modal" data-bs-target="#modalEditarTurno">
                            Pedir turno fijo
                        </button>
                    </div>

                </div>
            </div>
        </div>

        <!-- MODAL PEDIR TURNO -->
        <div class="modal fade" id="modalPedirTurno" tabindex="-1" aria-labelledby="modalPedirTurnoLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title" id="modalPedirTurnoLabel">Confirmar turno</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">
                        <p>Desea <span id="spanAccionTurno">pedir</span> el turno <span id="spanTurnoSeleccionado"></span>?</p>
                        <asp:HiddenField ID="hiddenTurno" runat="server" />
                        <asp:HiddenField ID="hiddenEsCancelacion" runat="server" />
                        <!-- Es para saber si es cancelación desde el server -->
                        <asp:Label ID="lblErrorPedirTurno" runat="server" CssClass="text-danger" Visible="false" />
                    </div>
                    <div class="modal-footer border-secondary">
                        <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnConfirmarTurno" runat="server" CssClass="boton-principal" Text="Aceptar" OnClick="btnConfirmarTurno_Click" />
                    </div>
                </div>
            </div>
        </div>


        <!-- VENTANA FLOTANTE SELECCIONAR TURNO FIJO -->
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
                        <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnGuardarCambios" runat="server" CssClass="boton-principal" Text="Guardar cambios" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function abrirModalPedirTurno(idTurno, fechaHora, reservado) {
            // Guardar ID en HiddenField
            document.getElementById('<%= hiddenTurno.ClientID %>').value = idTurno;

            // Guardar si es cancelación (true/false)
            document.getElementById('<%= hiddenEsCancelacion.ClientID %>').value = reservado ? "true" : "false";

            // Texto de acción
            document.getElementById('spanAccionTurno').innerText = reservado ? "cancelar" : "pedir";

            // Mostrar fecha y hora en el span
            document.getElementById('spanTurnoSeleccionado').innerText = fechaHora;

            // Abrir modal
            var myModal = new bootstrap.Modal(document.getElementById('modalPedirTurno'));
            myModal.show();
        }

        var abrirPorError = false;

        document.getElementById('modalPedirTurno').addEventListener('show.bs.modal', function () {

            // Si el modal se abre por el error del servidor no limpia la lbl de error
            if (abrirPorError) {
                abrirPorError = false;
                document.getElementById('<%= lblErrorPedirTurno.ClientID %>').style.display = "block";
                return;
            }

            // Si lo abre el usuario manualmente si limpia
            document.getElementById('<%= lblErrorPedirTurno.ClientID %>').style.display = "none";
            document.getElementById('<%= lblErrorPedirTurno.ClientID %>').innerText = "";
        });

    </script>

</asp:Content>
