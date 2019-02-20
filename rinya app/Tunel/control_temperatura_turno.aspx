<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="control_temperatura_turno.aspx.cs" Inherits="rinya_app.Tunel.control_temperatura_turno" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowExportControls="true">
        <LocalReport ReportPath="Tunel\control_temperatura_turno_rpt.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>

</asp:Content>
