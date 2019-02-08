<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Picking.aspx.cs" Inherits="rinya_app.Logistica.Picking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type = "text/javascript">
function DisableButton() {
    document.getElementById("<%=Button1.ClientID%>").disabled = true;
}
window.onbeforeunload = DisableButton;
</script>
    <asp:Button ID="Button1" runat="server" Text="Reiniciar" CssClass="btn btn-default" OnClick="Button1_Click"  />
    <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>

</asp:Content>
