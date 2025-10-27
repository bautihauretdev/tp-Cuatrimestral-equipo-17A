<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="AdminTurnos.aspx.cs" Inherits="presentacionWebForm.AdminTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row row-cols-1 row-cols-md-3 g-4">

        <!-- Card Miembros activos -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-people-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblActivos" runat="server" CssClass="tituloTurnos" Text="Miembros activos"></asp:Label>
                    </div>
                    <%-- En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblActivosNum" runat="server" CssClass="numeroTurnos" Text="174"></asp:Label> 

                </div>
            </div>
        </div>

        <!-- Card Socios que vienen hoy -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-person-check-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblSociosHoy" runat="server" CssClass="tituloTurnos" Text="Socios que vienen hoy"></asp:Label>
                    </div>
                    <%-- En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblSociosHoyNum" runat="server" CssClass="numeroTurnos" Text="51"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Card Horarios completos -->
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex align-items-center gap-2">
                        <i class="bi bi-clock-fill iconoColor fs-4"></i>
                        <asp:Label ID="lblCompletos" runat="server" CssClass="tituloTurnos" Text="Horarios completos"></asp:Label>
                    </div>
                    <%-- En el siguiente Label, el Text hay que ponerlo "-" y se irá modificando con el back --%>
                    <asp:Label ID="lblCompletosNum" runat="server" CssClass="numeroTurnos" Text="5"></asp:Label>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
