<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminPlanes.aspx.cs" Inherits="presentacionWebForm.AdminPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="dashboard">
        <div class="planes-header">
            <h2>Planes de Membresía</h2>
            <button type="button" class="boton-principal" data-bs-toggle="modal" data-bs-target="#modalAgregarPlan">
                Agregar nuevo plan
            </button>
        </div>

        <div class="planes-grid">
            <!-- SECCIÓN IZQUIERDA: DETALLE DE PLAN -->
            <div class="planes-panel">
                <div class="planes-form-group">
                    <label>Plan</label>
                    <asp:DropDownList ID="ddlPlan" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged"
                        CssClass="form-input">
                    </asp:DropDownList>
                </div>

                <div class="planes-form-row">
                    <div class="planes-form-group">
                        <label>Horas x Semana</label>
                        <asp:TextBox ID="txtHorasSemana" runat="server" CssClass="form-input readonly" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="planes-form-group">
                        <label>Monto</label>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="form-input readonly" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

                <div class="planes-form-group">
                    <div class="planes-btn-group">
                        <asp:Button ID="btnEditarPlan" runat="server"
                            Text="Editar plan"
                            CssClass="boton-editar"
                            OnClick="btnEditarPlan_Click" />
                        <asp:Button ID="btnEliminarPlan" runat="server" Text="Eliminar Plan" CssClass="boton-editar" OnClick="btnEliminarPlan_Click" />
                    </div>
                </div>
            </div>

            <!-- SECCIÓN DERECHA: SOCIOS EN EL PLAN -->
            <%--            <div class="planes-panel">
                <div class="planes-socios-header">
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
            </div>--%>
        </div>
    </main>



    <!---------- VENTANA FLOTANTE *EDITAR PLAN* ---------->
    <div class="modal fade" id="modalEditarPlan" tabindex="-1" aria-labelledby="modalEditarPlanLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title" id="modalEditarPlanLabel">Editar plan</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body">
                    <!-- NOMBRE -->
                    <div class="mb-3">
                        <label for="txtNombrePlanEditar" class="form-label">Nombre Plan</label>
                        <asp:TextBox ID="txtNombrePlanEditar" runat="server" CssClass="form-input readonly" ReadOnly="true"></asp:TextBox>
                    </div>

                    <!-- FILA: CANTIDAD HORAS / MONTO -->
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="txthorasPlanEditar" class="form-label">Horas x Semana</label>
                            <asp:TextBox ID="txthorasPlanEditar" runat="server" TextMode="Number" CssClass="form-input readonly" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="txtMontoPlanEditar" class="form-label">Monto</label>
                            <asp:TextBox ID="txtMontoPlanEditar" runat="server" TextMode="Number" CssClass="form-input"></asp:TextBox>
                        </div>
                    </div>
                    <!-- MENSAJE ERROR -->
                    <div class="mb-3">
                        <asp:Label ID="lblErrorPlanEditar" runat="server" CssClass="text-danger" Visible="false" />
                    </div>
                </div>

                <!-- BOTONES EDITAR -->
                <div class="modal-footer border-secondary">
                    <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarCambios" runat="server" CssClass="boton-principal" Text="Guardar cambios" OnClick="btnGuardarCambios_Click" />
                </div>
            </div>
        </div>
    </div>


    <!---------- VENTANA FLOTANTE *AGREGAR PLAN* ---------->
    <div class="modal fade" id="modalAgregarPlan" tabindex="-1" aria-labelledby="modalAgregarPlan" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title" id="modalAgregarPlanTitulo">Agregar plan</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body">
                    <!-- NOMBRE -->
                    <div class="mb-3">
                        <label for="txtNombrePlanAgregar" class="form-label">Nombre Plan</label>
                        <asp:TextBox ID="txtNombrePlanAgregar" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <!-- FILA: CANTIDAD HORAS / MONTO -->
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="txthorasPlanAgregar" class="form-label">Horas x Semana</label>
                            <asp:TextBox ID="txthorasPlanAgregar" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="txtMontoPlanAgregar" class="form-label">Monto</label>
                            <asp:TextBox ID="txtMontoPlanAgregar" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- MENSAJE ERROR -->
                    <div class="mb-3">
                        <asp:Label ID="lblErrorPlanAgregar" runat="server" CssClass="text-danger" Visible="false" />
                    </div>
                </div>

                <!-- BOTONES AGREGAR -->
                <div class="modal-footer border-secondary">
                    <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnAceptarAltaPlan" runat="server" CssClass="boton-principal" Text="Aceptar" OnClick="btnAceptarAltaPlan_Click" />
                </div>
            </div>
        </div>
    </div>

    <!---------- VENTANA FLOTANTE *CONFIRMACIÓN ELIMINAR PLAN* ---------->
    <div class="modal fade" id="modalEliminarPlan" tabindex="-1" aria-labelledby="modalEliminarPlanLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title" id="modalEliminarPlanLabel">Eliminar plan</h5>
<%--                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>--%>
                </div>

                <div class="modal-body">
                    <p>¿Está seguro que desea eliminar el plan <strong><span id="planNombreEliminar"></span></strong>?</p>
                </div>

                <div class="modal-footer border-secondary">
                    <button type="button" class="boton-editar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminar" runat="server" CssClass="boton-principal" Text="Sí, eliminar" OnClick="btnConfirmarEliminar_Click" />
                </div>
            </div>
        </div>
    </div>


    <script>
        // Cuando se abre el modal "Agregar plan", limpia los campos
        var modalAgregar = document.getElementById('modalAgregarPlan');
        modalAgregar.addEventListener('show.bs.modal', function () {
            document.getElementById('<%= txtNombrePlanAgregar.ClientID %>').value = "";
            document.getElementById('<%= txthorasPlanAgregar.ClientID %>').value = "";
            document.getElementById('<%= txtMontoPlanAgregar.ClientID %>').value = "";
            document.getElementById('<%= lblErrorPlanAgregar.ClientID %>').style.display = "none";
        });
    </script>

</asp:Content>
