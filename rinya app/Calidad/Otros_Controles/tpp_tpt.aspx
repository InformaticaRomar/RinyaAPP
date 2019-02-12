<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tpp_tpt.aspx.cs" Inherits="rinya_app.Calidad.Otros_Controles.tpp_tpt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Control TPP y TPT</h2>
    <div class="panel panel-default">
     <div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
       <div class="row">
                         
                            <div class="col-xs-2 col-md-4"><asp:Label id="label_sscc_" runat="server" AssociatedControlID="SSCC_TextBox_" CssClass="control-label">Matricula:</asp:Label></div>
                      
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="SSCC_TextBox_"  runat="server" CssClass="form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="Button1_" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="Button1_Click" /></div>                       
                          </div>
                    </div>
            </div>
            </div>
           
    </div>
</asp:Content>
