<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="control_tunel_4.aspx.cs" Inherits="rinya_app.Tunel.control_tunel_4" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
 


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowExportControls="False">
            <LocalReport ReportPath="Tunel\control_tunel_4_rpt.rdlc">
            </LocalReport>
       
    </rsweb:ReportViewer>
</asp:Content>
