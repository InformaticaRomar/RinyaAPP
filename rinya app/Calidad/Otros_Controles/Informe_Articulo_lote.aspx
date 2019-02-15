<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Informe_Articulo_lote.aspx.cs" Inherits="rinya_app.Calidad.Otros_Controles.Informe_Articulo_lote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
     <script type="text/javascript"> 
     $.datepicker.regional['es'] = {
           closeText: 'Cerrar',
           prevText: '<Ant',
           nextText: 'Sig>',
           currentText: 'Hoy',
           monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
           monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
           dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
           dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
           dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
           weekHeader: 'Sm',
           dateFormat: 'dd-mm-yy',
           firstDay: 1,
           isRTL: false,
           showMonthAfterYear: false,
           yearSuffix: ''
       };
     $.datepicker.setDefaults($.datepicker.regional['es']);
     $(document).ready(function () {
   var hoy = new Date();
   var semana_dia = new Date();
   semana_dia.setDate(semana_dia.getDate() +1);
                // semana_dia.setDate(semana_dia.getDate() - 2);
                 
   $("#<%= TextBox_Desde.ClientID %>").datepicker().datepicker("setDate", new Date());

         $("#<%= TextBox_hasta.ClientID %>").datepicker().datepicker("setDate",  semana_dia);
         if (!($("[id$=TextBox_Desde]").datepicker().attr("Value") == undefined)) {
            
      if ($("[id$=TextBox_Desde]").datepicker().attr("Value").length > 0) {
          $("[id$=TextBox_Desde]").datepicker().datepicker("setDate", new Date($("[id$=TextBox_Desde]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
          $("[id$=TextBox_hasta]").datepicker().datepicker("setDate", new Date($("[id$=TextBox_hasta]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                // alert($("[id$=datepicker1]").datepicker().attr("Value"));
                 //alert($("[id$=datepicker2]").datepicker().attr("Value"));
             }
             
         }
     });
        </script>
     
      <h2>Control Liberacion Automatica</h2>
    <div class="panel panel-default">
     <div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
      <div class="row">
                         
                            <div class="col-xs-2 col-md-1"><asp:Label id="label" runat="server" AssociatedControlID="TextBox_Desde" CssClass="control-label">Desde</asp:Label></div>
                            <div class="col-xs-2 col-md-2"><asp:TextBox ID="TextBox_Desde"   runat="server" CssClass="form-control" ></asp:TextBox></div>
                            <div class="col-xs-2 col-md-1"><asp:Label id="label1" runat="server" AssociatedControlID="TextBox_hasta" CssClass="control-label">Hasta</asp:Label></div>
                            <div class="col-xs-2 col-md-2"><asp:TextBox ID="TextBox_hasta"   runat="server" CssClass="form-control" ></asp:TextBox></div>
           <div class="col-xs-2 col-md-1"><asp:Label id="label2" runat="server" AssociatedControlID="DropDownList1" CssClass="control-label">Turno</asp:Label></div>
                       
          <div class="col-xs-2 col-md-2"> <asp:DropDownList ID="DropDownList1" runat="server"  CssClass="form-control">
              <asp:ListItem Value="0">Todos</asp:ListItem>
              <asp:ListItem>1</asp:ListItem>
              <asp:ListItem>2</asp:ListItem>
              <asp:ListItem>3</asp:ListItem>
              </asp:DropDownList></div>
                            <div class="col-xs-2 col-md-2"><asp:Button ID="Button2" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="Button2_Click" /></div>                       
                          </div>
                                  </div>

               </div>
            </div>
            </div>

</asp:Content>