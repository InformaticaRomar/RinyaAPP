<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="A_Lote.aspx.cs" Inherits="rinya_app.Calidad.A_Lote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
    
    

      <script src="../Scripts/jquery-ui/jquery-ui.js"></script>
    
    
    <script src="../Scripts/jquery-jtemplates.js"></script>
    
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css" rel="stylesheet" />
<script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>

     <script type="text/javascript"> 
         function Confirm() {
             var confirm_value = document.createElement("INPUT");
             confirm_value.type = "hidden";
             confirm_value.name = "confirm_value";
             if (confirm("Seguro que quieres realizar la actualizacion?")) {
                 confirm_value.value = "Si";
             } else {
                 confirm_value.value = "No";
             }
             document.forms[0].appendChild(confirm_value);
         }

    jQuery(document).ready(function () {
       
        $("#MainContent_DropDown_Articulo").select2();
        $("#MainContent_Estados").select2();
       
    });
    </script> 
     <h2>Propagar al Lote</h2>
    <div class="panel panel-default">
     <div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

    <div class="panel-body">
            <div class="form-group">
               
              <div class="container">
                   
                  <div class="row " >
                      <div class="col-xs-2 col-md-2"> <asp:Label runat="server" CssClass="control-label" >Articulo:</asp:Label> </div>   
                      <div  class="col-xs-2 col-md-8"> <asp:DropDownList ID="DropDown_Articulo" CssClass="form-control" runat="server">  </asp:DropDownList ></div>
                      </div>
                      <div class="row">
                           <div class="col-xs-2 col-md-2"><asp:Label runat="server" AssociatedControlID="Lote_textBoxs" CssClass="control-label">Lote interno:</asp:Label></div>
                            <div class="col-xs-2 col-md-8"> <asp:TextBox ID="Lote_textBoxs" runat="server"  placeholder="Lote Interno" CssClass=" form-control"></asp:TextBox></div>
                      </div>
                  <div class="row">
                      <div class="col-xs-2 col-md-2"> <asp:Label runat="server" CssClass="control-label" >Estado:</asp:Label> </div>   
                      <div  class="col-xs-2 col-md-8"> <asp:DropDownList ID="Estados" CssClass="form-control" runat="server"> </asp:DropDownList> </div>     
                      </div>
                       <div class="col-xs-2 col-md-8"><asp:Button ID="BtBuscar" runat="server" Text="Actualizar" CssClass="btn btn-default" OnClick="BtBuscar_Click"  OnClientClick = "Confirm()" /></div>                       
                          </div>   
                  </div>
        
                </div>
        </div>
        
</asp:Content>
