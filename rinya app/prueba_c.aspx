<%@ Page Title="Prueba"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="prueba_c.aspx.cs" Inherits="rinya_app.prueba_c" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Buscar por Zona</h3>
    <div id="Formu">Buscar:

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Una zona" OnClick="Button1_Click" />

    </div>

    <div id="grid">
     

       



    </div>
   
</asp:Content>
