<%@ Page Title="Prueba"  Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="prueba_o.aspx.cs" Inherits="rinya_app.prueba_o" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2><%: Title %>.</h2>
    <h3>Buscar por Oracle</h3>
    <div id="formu">
    Buscar:

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Una zona" OnClick="Button1_Click"  />

        <asp:Button ID="Button2" runat="server" Text="Todas" OnClick="Button2_Click" />

        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Excel" />

        <asp:GridView ID="GridView1" runat="server" BackColor="#ccccff" 
      BorderColor="black"
      ShowFooter="false" 
      CellPadding=3 
      CellSpacing="0"
      Font-name="verdana"
      Font-Size="8pt"
      HeaderStyle-BackColor="#aaaadd"
      EnableViewState="false" Font-Names="Verdana">
        </asp:GridView>
    
    </div>
     </asp:Content>
