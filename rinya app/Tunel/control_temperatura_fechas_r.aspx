<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="control_temperatura_fechas_r.aspx.cs" Inherits="rinya_app.Tunel.control_temperatura_fechas_r" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowExportControls="true">
        <LocalReport ReportPath="Tunel\control_temperatura_turno_rpt.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
