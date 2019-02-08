<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="traza.aspx.cs" Inherits="rinya_app.Calidad.Trazabilidad.traza" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ PreviousPageType VirtualPath="~/Calidad/trazabilidad/trazabilidad.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
         <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ExportContentDisposition="AlwaysInline">
             <LocalReport ReportPath="Calidad\Trazabilidad\Report2.rdlc">
             </LocalReport>
        </rsweb:ReportViewer>
    
</asp:Content>
