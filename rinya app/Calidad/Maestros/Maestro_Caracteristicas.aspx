<%@ Page Title="Maestro de Caracteristicas" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Maestro_Caracteristicas.aspx.cs" Inherits="rinya_app.Calidad.Maestro.WebForm1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    
    <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/DataTables/css/buttons.dataTables.css"  rel="stylesheet" type="text/css" />
    <link href="../../Content/dataTables.jqueryui.css" rel="stylesheet" type="text/css" />

     <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
    <script src="../../Scripts/DataTables/jquery.dataTables.js"></script>
    <script src="../../Scripts/DataTables/jszip.min.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/DataTables/dataTables.buttons.min.js" ></script>
    <script src="../../Scripts/DataTables/buttons.html5.min.js"></script>

   <script type="text/javascript"> 
       jQuery(document).ready(function () {
           $.ajaxSetup({
               cache: false
           });

           function showDetails() {
               alert("showing some details");
           }
           var editor;
           var table = $('#tblData').DataTable({

               dom: '<"H"B<lfr>>t<"F"ip>',
               jQueryUI: true,
               responsive: true,
               buttons: [
                   {
                       text: 'Nueva',
                       action: function (e, dt, node, config) {
                           $("#table-dialogo").attr("style", "display: table");
                           var ID = "";
                           var Car = "";
                           var descrip = "";


                           $('#table-dialogo').dialog({
                               autoOpen: true,
                               modal: true,
                               resizable: false,
                               title: 'Nueva alta',
                               width: 400,
                               heigth: 250,

                               open: function (event, ui) {

                                   $('#lbl_id').hide();
                                   $('#lbl_caracter').hide();
                                   $('#Texto_ID').hide();
                                   $('#txt_caracteristica').hide();
                                   $('#text_Descripcion').val("");
                                   $('#blk_Tdato').hide();
                                  

                               },
                               close: function (event, ui) {


                                   $('#table-dialogo :text').val("");

                               },
                               buttons: {
                                   "Guardar": function () {
                                       var respuesta = confirm("Seguro que quieres añadir una nueva caracteristica?");
                                       if (respuesta) {
                                           var C_Maestros = {};
                                           C_Maestros.ID = $('#Texto_ID').text();
                                           C_Maestros.Cod_Caracteristica = $('#txt_caracteristica').val();
                                           C_Maestros.Caracteristica = $('#text_Descripcion').val();
                                           if (C_Maestros.Caracteristica.length > 0) {

                                               $.ajax({
                                                   type: "POST",
                                                   url: "WebService_maestro_caracteristicas.asmx/New_Data",
                                                   data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                                   contentType: "application/json; charset=utf-8",
                                                   dataType: "json",
                                                   success: function (response) {
                                                       //Success or failure message e.g. Record saved or not saved successfully
                                                       table.ajax.reload();
                                                       $('#table-dialogo').dialog("close");

                                                   },
                                                   error: function (response) {
                                                       alert(response.d);
                                                   }
                                               });
                                           }
                                       }
                          
                                   },

                                   "Cancelar": function () {
                                       $(this).dialog("close");
                                   }
                               }
                           })
                         
                       }
                   },
               'excelHtml5'
               , 'csvHtml5'
               ],
               "language": {
                   "sProcessing": "Procesando...",
                   "sLengthMenu": "Mostrar _MENU_ registros",
                   "sZeroRecords": "No se encontraron resultados",
                   "sEmptyTable": "Ningún dato disponible en esta tabla",
                   "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                   "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                   "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                   "sInfoPostFix": "",
                   "sSearch": "Buscar:",
                   "sUrl": "",
                   "sInfoThousands": ",",
                   "sLoadingRecords": "Cargando...",
                   "oPaginate": {
                       "sFirst": "Primero",
                       "sLast": "Último",
                       "sNext": "Siguiente",
                       "sPrevious": "Anterior"
                   },
                   "oAria": {
                       "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                       "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                   }
               },
               "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
               "iDisplayLength": 10,
               "filter": true,

               "pagingType": "simple_numbers",
               "orderClasses": false,
               "order": [[0, "asc"]],
               "info": true,
               "autoWidth": false,
               "sScrollY": "300px",
               "scrollX": true,

               'bScrollCollapse': true,

               "bProcessing": true,
               "bServerSide": true,
               "columnDefs": [
      {
          "targets": [0],
          "className" : "none"
      }],

               "sAjaxSource": "WebService_maestro_caracteristicas.asmx/GetTableData",
               "fnServerData": function (sSource, aoData, fnCallback) {
                   aoData.push({ "name": "roleId", "value": "admin" });
               
                   $.ajax({
                       "dataType": 'json',
                       "contentType": "application/json; charset=utf-8",
                       "type": "GET",
                       "url": sSource,
                       "data": aoData,
                       "success": function (msg) {
                           var json = jQuery.parseJSON(msg.d);
                           fnCallback(json);
                           $('#tblData').attr("style", "visibility: visible");
                           $('#tblData').show();
                       },
                       error: function (xhr, textStatus, error) {
                           if (typeof console == "object") {
                               console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
                           }
                       }
                   });
               },
               fnDrawCallback: function () {
                   $('.image-details').bind("click", showDetails);
               },
               fnInitComplete: function () {
                   table.columns.adjust().draw();
               }
           });



           table.columns().eq(0).each(function (colIdx) {
               $('input', table.column(colIdx).footer()).on('keyup change', function () {

                   table
                       .column(colIdx)
                       .search(this.value)
                       .draw();
               });
           });

           $("#tblData tbody").on("click", 'tr', function (e) {
               e.preventDefault();

               var nTds = $('td', this);

               var ID = $(nTds[0]).text();
               var Car = $(nTds[1]).text();
               var descrip = $(nTds[2]).text();
               var T_dato = 0;
               if ($(nTds[3]).text() == 'Rango')
               {
                   T_dato = 1;
               } else if ($(nTds[3]).text() == 'S/N')
               {
                   T_dato = 2;
               } else if ($(nTds[3]).text() == 'Info')
               {
                   T_dato = 3;
               }
               var V_tablet = 0;
               if ($(nTds[4]).text() == 'Si') {
                   V_tablet = 1;
               } else if ($(nTds[4]).text() == 'No') {
                   V_tablet = 0;
               }

               $("#table-dialogo").attr("style", "display: table");

               $('#table-dialogo').dialog({
                   autoOpen: true,
                   modal: true,
                   resizable: false,
                   title: 'Modificar',
                   width:400,
                   heigth: 250,

                   open: function (event, ui) {
                       $('#Texto_ID').text(ID.trim());
                       $('#txt_caracteristica').val(Car.trim());
                       $('#text_Descripcion').val(descrip.trim());
                       switch (T_dato) {
                           case 1:
                               $('#Tipo').selectedIndex = 0;
                               break;
                           case 2:
                               $('#Tipo').selectedIndex = 1;
                               break;
                           case 3:
                               $('#Tipo').selectedIndex = 2;
                               break;
                       }
                       $('#Tipo').val(T_dato);
                       $('#VTablet').val(V_tablet);
                   },
                   close: function (event, ui) {


                       $('#table-dialogo :text').val("");

                   },
                   buttons: {
                                   "Borrar": function () {
                                       var respuesta = confirm("Seguro que deseas borrar?");
                                       if (respuesta) {
                                           if (ID.length > 0) {

                                               var C_Maestros = {};
                                               C_Maestros.ID = $('#Texto_ID').text();
                                               C_Maestros.Cod_Caracteristica = $('#txt_caracteristica').val();
                                               C_Maestros.Caracteristica = $('#text_Descripcion').val();
                                               C_Maestros.Tipo_Dato = $('#Tipo').val();
                                               C_Maestros.Visible_Tablet = $('#VTablet').val();
                                               $.ajax({
                                                   type: "POST",
                                                   url: "WebService_maestro_caracteristicas.asmx/delete_Data",
                                                   data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                                   contentType: "application/json; charset=utf-8",
                                                   dataType: "json",
                                                   success: function (response) {
                                                       //Success or failure message e.g. Record saved or not saved successfully
                                                       table.ajax.reload();
                                                       $('#table-dialogo').dialog("close");

                                                   },
                                                   error: function (response) {
                                                       alert(response.d);
                                                   }
                                               });
                                           }
                                       }

                                   },
                          "Update": function () {
                              if (ID.length > 0) {

                                  var C_Maestros = {};
                                  C_Maestros.ID = $('#Texto_ID').text();
                                  C_Maestros.Cod_Caracteristica = $('#txt_caracteristica').val();
                                  C_Maestros.Caracteristica = $('#text_Descripcion').val();
                                  C_Maestros.Tipo_Dato = $('#Tipo').val();
                                  C_Maestros.Visible_Tablet = $('#VTablet').val();
                                  $.ajax({
                                      type: "POST",
                                      url: "WebService_maestro_caracteristicas.asmx/update_Data",
                                      data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                      contentType: "application/json; charset=utf-8",
                                      dataType: "json",
                                      success: function (response) {
                                          //Success or failure message e.g. Record saved or not saved successfully
                                          table.ajax.reload();
                                          $('#table-dialogo').dialog("close");

                                      },
                                      error: function (response) {
                                          alert(response.d);
                                      }
                                  });
                              }



                          },

                          "Cancelar": function () {
                              $(this).dialog("close");
                          }
                      }
                  });
           })
        
       });

   </script>
    <h1><%: Title %></h1>
    <table id="tblData" class="display">
      <thead>
             <tr>
                 <th>Id</th>
                 <th>Cod Caracteristica</th>
                <th>Caracteristica</th>
                 <th>Tipo Dato</th>
                 <th>Visible Tablet</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot>
            
        </tfoot>
</table> 
    <div id="table-dialogo"  style="display: none" >
          <table id="orderedittable">
                        
                             <tr>
                            <td id="lbl_id">ID: </td>
                                <td id="Texto_ID" style="width: 168px">1234 </td> 
                        </tr>
              <tr>
                            <td id="lbl_caracter" style="width: 168px">Caracteristica:</td>
                            <td class="cell">
                                 <input id="txt_caracteristica" style="width: 168px" type="text" value=""/>
                            </td>
                        </tr>
              <tr>
                            <td>Descripcion: </td>
                            <td class="cell">
                                 <input id="text_Descripcion" style="width: 168px" type="text" value="" />
                            </td>
                  
                        </tr>
              <tr id ="blk_Tdato">
                <td>Tipo de dato: </td>
                            <td class="cell">
                             <select id="Tipo" style="width: 168px" > 
                                     <option value="1">Rango</option>
                                     <option value="2">S/N</option>
                                     <option value="3">Info</option>
                             </select>
                                 </td>
                      </tr>  
              <tr id ="blk_VisibleT">
                <td>Visible Tablet: </td>
                            <td class="cell">
                             <select id="VTablet" style="width: 168px" > 
                                     <option value="1">Si</option>
                                     <option value="0">No</option>
                             </select>
                                 </td>
                      </tr>  
              </table>
       
 </div>
    
</asp:Content>