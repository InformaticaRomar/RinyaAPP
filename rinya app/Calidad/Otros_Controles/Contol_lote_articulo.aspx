<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contol_lote_articulo.aspx.cs" Inherits="rinya_app.Calidad.Otros_Controles.Contol_lote_articulo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="3pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowExportControls="False">
 </rsweb:ReportViewer>
</asp:Content>
