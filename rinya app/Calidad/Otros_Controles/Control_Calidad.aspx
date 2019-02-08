<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Control_Calidad.aspx.cs" Inherits="rinya_app.Calidad.Otros_Controles.Control_Calidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
      <h2>Control Laboratorio Producción</h2>
    <div class="panel panel-default">
     <div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
                    <div class="row">
                        <div class="col-xs-2 col-md-4"><asp:Label runat="server" AssociatedControlID="Lote_textBoxs" CssClass="control-label">Lote interno:</asp:Label></div>
                        <div class="col-xs-2 col-md-4"> <asp:Label runat="server" AssociatedControlID="Palet_text" CssClass="control-label" >Palet:</asp:Label> </div>                   
                    </div>
                    <div class="row " >
                            <div class="col-xs-2 col-md-4"> <asp:TextBox ID="Lote_textBoxs" runat="server"  placeholder="Lote Interno" CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="Palet_text"  placeholder="Numero Palet" runat="server" CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click"  /></div>                       
                          </div>   
                      
           
                    <div class="row">
                         
                            <div class="col-xs-2 col-md-4"><asp:Label id="label_sscc" runat="server" AssociatedControlID="SSCC_TextBox" CssClass="control-label">Matricula:</asp:Label></div>
                      
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="SSCC_TextBox"  placeholder="Matricula" runat="server" CssClass="form-control" OnTextChanged="SSCC_TextBox_TextChanged"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="Button1_Click" /></div>                       
                          </div>
                      

               

               </div>
            </div>
            </div>
           
    </div>

</asp:Content>
