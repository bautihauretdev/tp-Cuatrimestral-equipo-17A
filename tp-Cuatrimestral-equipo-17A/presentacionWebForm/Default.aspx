<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="presentacionWebForm.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-container">
        <div class="login-left">
            <p class="login-quote">"El cuerpo logra lo que la mente cree."</p>
        </div>

        <div class="login-right">
            <div class="login-box">
                <div class="login-icon">🏋️</div>
                <h2>Bienvenido/a</h2>
                <p>Accede al portal.</p>

                <asp:TextBox ID="txtBoxEmail" runat="server" CssClass="input-field" placeholder="Introduce tu email"></asp:TextBox>
                <asp:TextBox ID="txtBoxPassword" runat="server" TextMode="Password" CssClass="input-field" placeholder="Introduce tu contraseña"></asp:TextBox>

                <div class="forgot">
                    <a href="#">¿Olvidaste tu contraseña?</a>
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Acceder" CssClass="btn-login" OnClick="btnLogin_Click" />
                <asp:Label ID="lblError" runat="server" CssClass="text-danger mt-2 d-block" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
