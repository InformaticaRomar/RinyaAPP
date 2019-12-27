<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="validacion_matricula.aspx.cs" Inherits="rinya_app.Calidad.validacion_matricula" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="col-md-8">
            <h2>CONTROL MATRICULA</h2>

            <div class="form-group">
                <div class="row">
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="SSCC_textBoxs" CssClass="col-md-8 control-label">Matricula:</asp:Label>
                    </div>
                    
                </div>
                <div class="row">

                    <div class="col-md-4">

                        <asp:TextBox ID="SSCC_textBoxs" CssClass="form-control"  runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>

                </div>
                </div>
            </div>
        </div>
</asp:Content>
