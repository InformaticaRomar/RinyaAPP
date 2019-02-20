<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="traza.aspx.cs" Inherits="rinya_app.Calidad.Trazabilidad.traza" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
 
<%@ PreviousPageType VirtualPath="~/Calidad/trazabilidad/trazabilidad.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
         <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ExportContentDisposition="AlwaysInline">
             <LocalReport ReportPath="Calidad\Trazabilidad\Report2.rdlc">
             </LocalReport>
        </rsweb:ReportViewer>
    
</asp:Content>
