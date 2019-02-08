<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Control_Silo.aspx.cs" Inherits="rinya_app.Calidad.Otros_Controles.Control_Silo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/DataTables/css/buttons.dataTables.css"  rel="stylesheet" type="text/css" />
    <link href="../../Content/dataTables.jqueryui.css" rel="stylesheet" type="text/css" />

      <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
    <script src="../../Scripts/DataTables/jquery.dataTables.js"></script>
    <script src="../../Scripts/DataTables/jszip.min.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/DataTables/buttons.html5.min.js"></script>
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css" rel="stylesheet" />
<script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>
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
         dateFormat: 'dd/mm/yy',
         firstDay: 1,
         isRTL: false,
         showMonthAfterYear: false,
         yearSuffix: ''
     };
     $.datepicker.setDefaults($.datepicker.regional['es']);
     function chec_a() {
         var componente = '#txt_INH_L';
         var check1 = '#Check_a';
         var check2 = '#Check_b';
         if ($(check1).prop("checked")) {
             $(componente).val("0");
             $(check2).prop("checked", false);
         }
     }
     function chec_b() {
         var componente = '#txt_INH_L';
         var check1 = '#Check_a';
         var check2 = '#Check_b';
         if ($(check2).prop("checked")) {
             $(componente).val("1");
             $(check1).prop("checked", false);
         }
     }

     function chec_c() {
         var componente = '#txt_OLOR';
         var check1 = '#Check_c';
         var check2 = '#Check_d';
         if ($(check1).prop("checked")) {
             $(componente).val("0");
             $(check2).prop("checked", false);
         }
     }
     function chec_d() {
         var componente = '#txt_OLOR';
         var check1 = '#Check_c';
         var check2 = '#Check_d';
         if ($(check2).prop("checked")) {
             $(componente).val("1");
             $(check1).prop("checked", false);
         }
     }
     function chec_e() {
         var componente = '#txt_TEST_R';
         var check1 = '#Check_e';
         var check2 = '#Check_f';
         if ($(check1).prop("checked")) {
             $(componente).val("0");
             $(check2).prop("checked", false);
         }
     }
     function chec_f() {
         var componente = '#txt_TEST_R';
         var check1 = '#Check_e';
         var check2 = '#Check_f';
         if ($(check2).prop("checked")) {
             $(componente).val("1");
             $(check1).prop("checked", false);
         }
     }
     function nuevo() {
         
         $('#table-dialogo').dialog({
             autoOpen: true,
             modal: true,
             resizable: false,
             title: 'Nueva alta',
             width: 500,
             heigth: 250,

             open: function (event, ui) {
                 //alert("hola");
                 $('#Blc_id').hide();
                 $('#Blc_art').hide();
                 // $('#Blc_txt_carac').hide();
                 $('#Blc_Combo_art').show();
                 loadComboBox2('cmb_art', 'Get_all_Silos');
                 
                 $('#txt_PH').val("");
                 $('#txt_DORNIC').val("");
                 $('#txt_BRIX').val("");
                 //$('#txt_INH_L').prop('checked', false);
                 $('#txt_GRASA').val("");
                 $('#txt_PROTEINA').val("");
                 $('#txt_LACTOSA').val("");
                 $('#txt_SNF').val("");
                 $('#txt_TS').val("");
                 $('#txt_OLOR').val("");
                 $('#txt_TEST_R').val("");
                 $('#Check_a').prop('checked', false);
                 $('#Check_b').prop('checked', false);
                 $('#Check_c').prop('checked', false);
                 $('#Check_d').prop('checked', false);
                 $('#Check_e').prop('checked', false);
                 $('#Check_f').prop('checked', false);
                

             },
             close: function (event, ui) {
                 $('#table-dialogo :text').val("");

             },
             buttons: {
                 "Guardar": function () {
                     var respuesta = confirm("Seguro que quieres añadir datos Silo?");
                     if (respuesta) {
                         var C_Maestros = {};
                         C_Maestros.Id = "";
                         C_Maestros.SSCC = $('#cmb_art').val().toString();
                         C_Maestros.USUARIO = "";
                         C_Maestros.FECHA ="";
                         C_Maestros.PH = $('#txt_PH').val();
                         C_Maestros.DORNIC = $('#txt_DORNIC').val();
                         C_Maestros.BRIX = $('#txt_BRIX').val();
                         C_Maestros.INH_L = $('#txt_INH_L').val();
                         C_Maestros.GRASA = $('#txt_GRASA').val();
                         C_Maestros.PROTEINA = $('#txt_PROTEINA').val();
                         C_Maestros.LACTOSA = $('#txt_LACTOSA').val();
                         C_Maestros.SNF = $('#txt_SNF').val();
                         C_Maestros.TS=$('#txt_TS').val();
                         C_Maestros.OLOR= $('#txt_OLOR').val();
                         C_Maestros.TEST_R = $('#txt_TEST_R').val();

                         if (C_Maestros.SSCC.length > 0) {

                             $.ajax({
                                 type: "POST",
                                 url: "WebService_SILO.asmx/New_Data",
                                 data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 success: function (response) {
                                     //Success or failure message e.g. Record saved or not saved successfully
                                     loadComboBox('ddlsilo', 'Get_all_Silos');
                                     loadTable();
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
         });
     }
     function loadTable() {
         if ($.fn.dataTable.isDataTable('#tblData')) {
             var table = $('#tblData').DataTable();
             table.destroy();
         }
         $.ajaxSetup({
             cache: false
         });
         var valor = $("#<%= ddlsilo.ClientID %>").val().toString();
         $("#<%=comboseleccionado.ClientID %>").val(valor);
         $("#tblData").attr("style", "display: table");
         function showDetails() {
             alert("showing some details");
         }

         var table = $('#tblData').DataTable({

             dom: '<"H"B<lfr>>t<"F"ip>',
             jQueryUI: true,
             responsive: true,
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
             "lengthMenu": [[10, 25], [10, 25]],
             "iDisplayLength": 10,
             "filter": true,

             "pagingType": "simple_numbers",
             "orderClasses": false,
             "order": [[3, "asc"]],
             "info": true,
             "autoWidth": false,
             "sScrollY": "300px",
             "scrollX": true,
             "columnDefs": [
              {
                  "targets": [0],
                  "className": "none"
              }, {
                  "targets": [1],
                  "className": "none"
              }
                      ],
             'bScrollCollapse': true,

             "bProcessing": true,
             "bServerSide": true,

             "sAjaxSource": "WebService_SILO.asmx/GetTableData",
             "fnServerData": function (sSource, aoData, fnCallback) {
                 aoData.push({ "name": "roleId", "value": valor });
                 aoData.push({ "name": "fecha1", "value":  $("#<%= datepicker1.ClientID %>").val() });
                 aoData.push({ "name": "fecha2", "value": $("#<%= datepicker2.ClientID %>").val() });

                 $.ajax({
                     "dataType": 'json',
                     "contentType": "application/json; charset=utf-8",
                     "type": "POST",
                     "url": sSource,
                     "data": JSON.stringify({ myData: aoData }),
                     "success": function (msg) {
                         if (msg.d.length > 0) {
                         var json = jQuery.parseJSON(msg.d);
                         fnCallback(json);
                         $("#tblData").show();
                     } else {
                         $("#tblData").hide();
                     }
                     },
                     error: function (xhr, textStatus, error) {
                         if (typeof console == "object") {
                             console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
                             alert(error);
                         }
                     }
                 });
             },
             fnDrawCallback: function () {
                 $('.image-details').bind("click", showDetails);
             }
         });
         $("#tblData tbody").on("click", 'tr', function (e) {
             e.preventDefault();

             var nTds = $('td', this);
             var ID = $(nTds[0]).text();
             var SSCC = $(nTds[1]).text();
             var USUARIO = $(nTds[2]).text();
             var FECHA = $(nTds[3]).text();
             var PH = $(nTds[4]).text();
             var DORNIC = $(nTds[5]).text();
             var BRIX = $(nTds[6]).text();
             var INH_L = $(nTds[7]).text();
             var GRASA = $(nTds[8]).text();
             var PROTEINA = $(nTds[9]).text();
             var LACTOSA = $(nTds[10]).text();
             var SNF = $(nTds[11]).text();
             var TS = $(nTds[12]).text();
             var OLOR = $(nTds[13]).text();
             var TEST_R = $(nTds[14]).text();
             $("#table-dialogo").attr("style", "display: table");

             $('#table-dialogo').dialog({
                 autoOpen: true,
                 modal: true,
                 resizable: false,
                 title: 'Modificar',
                 width: 400,
                 heigth: 250,

                 open: function (event, ui) {
                     $('#Blc_id').hide();
                     $('#Blc_art').show();
                     $('#Blc_Combo_art').hide();
                     // $('#Blc_txt_carac').hide();
                     //loadComboBox('cmb_art', 'Get_all_Silos');
                     $('#Texto_Art').text(SSCC);
                     $('#Texto_ID').text(ID);
                     $('#txt_PH').val(PH);
                     $('#txt_DORNIC').val(DORNIC);
                     $('#txt_BRIX').val(BRIX);
                     $('#txt_INH_L').val(INH_L);

                     if (INH_L == '1') {
                         $('#Check_a').prop('checked', false);
                         $('#Check_b').prop('checked', true);
                     } else if (INH_L == '0') {
                         $('#Check_a').prop('checked', true);
                         $('#Check_b').prop('checked', false);
                     }
                     else  {
                         $('#Check_a').prop('checked', false);
                         $('#Check_b').prop('checked', false);
                     }
                    // $('#txt_INH_L').prop('checked', false);
                     $('#txt_GRASA').val(GRASA);
                     $('#txt_PROTEINA').val(PROTEINA);
                     $('#txt_LACTOSA').val(LACTOSA);
                     $('#txt_SNF').val(SNF);
                     $('#txt_TS').val(TS);
                     $('#txt_OLOR').val(OLOR);
                     if (OLOR == '1') {
                         $('#Check_c').prop('checked', false);
                         $('#Check_d').prop('checked', true);
                     } else if (OLOR == '0') {
                         $('#Check_c').prop('checked', true);
                         $('#Check_d').prop('checked', false);
                     }
                     else {
                         $('#Check_c').prop('checked', false);
                         $('#Check_d').prop('checked', false);
                     }
                     $('#txt_TEST_R').val(TEST_R);
                     if (TEST_R == '1') {
                         $('#Check_e').prop('checked', false);
                         $('#Check_f').prop('checked', true);
                     } else if (TEST_R == '0') {
                         $('#Check_e').prop('checked', true);
                         $('#Check_f').prop('checked', false);
                     }
                     else {
                         $('#Check_e').prop('checked', false);
                         $('#Check_f').prop('checked', false);
                     }


                 },
                 close: function (event, ui) {
                     $('#table-dialogo :text').val("");

                 },
                 buttons: {
                     "Actualizar": function () {
                         var respuesta = confirm("Seguro que quieres actualizar datos?");
                         if (respuesta) {
                             var C_Maestros = {};
                             C_Maestros.Id = $('#Texto_ID').text();
                             C_Maestros.SSCC = $('#Texto_Art').text();
                             C_Maestros.USUARIO = "";
                             C_Maestros.FECHA = "";
                             C_Maestros.PH = $('#txt_PH').val();
                             C_Maestros.DORNIC = $('#txt_DORNIC').val();
                             C_Maestros.BRIX = $('#txt_BRIX').val();
                             C_Maestros.INH_L = $('#txt_INH_L').val();
                             C_Maestros.GRASA = $('#txt_GRASA').val();
                             C_Maestros.PROTEINA = $('#txt_PROTEINA').val();
                             C_Maestros.LACTOSA = $('#txt_LACTOSA').val();
                             C_Maestros.SNF = $('#txt_SNF').val();

                             C_Maestros.TS = $('#txt_TS').val();
                             C_Maestros.OLOR = $('#txt_OLOR').val();
                             C_Maestros.TEST_R = $('#txt_TEST_R').val();

                             if (C_Maestros.SSCC.length > 0) {

                                 $.ajax({
                                     type: "POST",
                                     url: "WebService_SILO.asmx/update_Data",
                                     data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                     contentType: "application/json; charset=utf-8",
                                     dataType: "json",
                                     success: function (response) {
                                         //Success or failure message e.g. Record saved or not saved successfully
                                         // loadComboBox('ddlArticulo', 'GetArticulos');
                                         loadComboBox('ddlsilo', 'Get_all_Silos');
                                         loadTable();
                                         $('#table-dialogo').dialog("close");

                                     },
                                     error: function (response) {
                                         alert(response.d);
                                     }
                                 });
                             }
                         }

                     },
                     "Borrar": function () {
                         var respuesta = confirm("Seguro que quieres borrar datos?");
                         if (respuesta) {
                             var C_Maestros = {};
                             C_Maestros.Id = $('#Texto_ID').text();
                             C_Maestros.SSCC = $('#Texto_Art').text();
                             C_Maestros.USUARIO = "";
                             C_Maestros.FECHA = "";
                             C_Maestros.PH = $('#txt_PH').val();
                             C_Maestros.DORNIC = $('#txt_DORNIC').val();
                             C_Maestros.BRIX = $('#txt_BRIX').val();
                             C_Maestros.INH_L = $('#txt_INH_L').val();
                             C_Maestros.GRASA = $('#txt_GRASA').val();
                             C_Maestros.PROTEINA = $('#txt_PROTEINA').val();
                             C_Maestros.LACTOSA = $('#txt_LACTOSA').val();
                             C_Maestros.SNF = $('#txt_SNF').val();

                             C_Maestros.TS = $('#txt_TS').val();
                             C_Maestros.OLOR = $('#txt_OLOR').val();
                             C_Maestros.TEST_R = $('#txt_TEST_R').val();

                             if (C_Maestros.SSCC.length > 0) {

                                 $.ajax({
                                     type: "POST",
                                     url: "WebService_SILO.asmx/Delete_Data",
                                     data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                     contentType: "application/json; charset=utf-8",
                                     dataType: "json",
                                     success: function (response) {
                                         //Success or failure message e.g. Record saved or not saved successfully
                                         // loadComboBox('ddlArticulo', 'GetArticulos');
                                         loadComboBox('ddlsilo', 'Get_all_Silos');
                                         loadTable();
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
             });

            

         });
     }
    function loadComboBox(comboname, methodname) {

        var template = "{#foreach $T as record}\
                         <option value='{$T.record.SSCC}'>{$T.record.DESCRIPCION}</option>\
                    {#/for}";

        var combo = $("#<%= ddlsilo.ClientID %>");

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "WebService_SILO.asmx/" + methodname,
            data: '{}',
            dataType: "json",

            success: function (data) {


                combo.setTemplate(template);
                combo.processTemplate(JSON.parse(data.d));

            },
            error: function (request, status, error) {
                alert(JSON.parse(request.responseText).Message);
            }
        })
        combo.select2();
        combo.on('change', function () {
             
            loadTable();
           
            //alert("hola");
        });
       
    }
     function loadComboBox2(comboname, methodname) {

        var template = "{#foreach $T as record}\
                         <option value='{$T.record.SSCC}'>{$T.record.DESCRIPCION}</option>\
                    {#/for}";

        var combo = $("#"+comboname);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "WebService_SILO.asmx/" + methodname,
            data: '{}',
            dataType: "json",

            success: function (data) {


                combo.setTemplate(template);
                combo.processTemplate(JSON.parse(data.d));

            },
            error: function (request, status, error) {
                alert(JSON.parse(request.responseText).Message);
            }
        })
        combo.select2();
        combo.on('change', function () {
             
            loadTable();
           
            //alert("hola");
        });
       
    }


    jQuery(document).ready(function () {
        var hoy = new Date();
        var semana_dia = new Date();
        semana_dia.setDate(semana_dia.getDate() - 2);

         $("#<%= datepicker1.ClientID %>").datepicker().datepicker("setDate", semana_dia);
         $("#<%= datepicker2.ClientID %>").datepicker().datepicker("setDate", new Date());
        loadComboBox('ddlsilo', 'Get_all_Silos');
    });
     </script>

            <h2><%: Title %></h2>
           

    <table><tbody>
        
        <tr><th>Silo:</th>  <p></p>
         <td>
        <select id="ddlsilo"  runat="server" class="js-example-basic-single" style="width: 492px"   ></select>
        </td>
            <td> <p>Desde: <input type="text"  class="form-control" id="datepicker1" runat="server" style="width: 98px"></p>
              </td>
<td>
  <p>Hasta: <input type="text" id="datepicker2" runat="server"  class="form-control" style="width: 98px"></p>
    <input type="hidden" id="comboseleccionado" value="prue" runat="server" />
    </td>

            
            <td> &nbsp;&nbsp;<input id="btnTodas" type="button" Value="Nuevo" class="btn btn-default" onclick="nuevo();"/></td>
              <td> <input ID="BtBuscar" type="button" Value="Buscar" Class="btn btn-default" onclick="loadTable();"/> </td>
                <td> <asp:Button ID="btnexcel" runat="server" Text="Excel" CssClass="btn btn-default" OnClick="btnexcel_Click"  /> </td>
             </tr>
        </tbody>
    </table>
    <p></p>
    <p></p>
                          
    <table id="tblData" class="display" style="display: none" >
      <thead>
             <tr>
                 <th>ID</th>
                 <th>SSCC</th>
                 <th>USUARIO</th>
                 <th>FECHA</th>
                 <th>PH</th>
                 <th>DORNIC</th>
                 <th>BRIX</th>
                 <th>INH_L</th>
                 <th>GRASA</th>
                 <th>PROTEINA</th>
                 <th>LACTOSA</th>
                 <th>SNF</th>
                 <th>TS</th>
                 <th>OLOR</th>
                 <th>TEST_R</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot>
            
        </tfoot>
</table> 
       

  <div id="table-dialogo"  style="display: none" >
          <table id="orderedittable">
                        
                             <tr id="Blc_id">
                                <td id="lbl_id">ID: </td>
                                <td id="Texto_ID" style="width: 168px">1234 </td> 
                            </tr>
                           
                            <tr id="Blc_art" >
                                <td id="lbl_art">Silo: </td>
                                <td id="Texto_Art" style="width: 168px">1234 </td> 
                            </tr>
                            <tr id="Blc_Combo_art">
                                <td id="lbl_art2">Silo: </td>
                                <td ><select id="cmb_art" class="js-example-basic-single" style="width: 300px"> </select> </td> 
                            </tr>
                            
                            <tr id="blk_PH">
                            <td id="lbl_caracter" style="width: 168px">Ph ( 6.7 | 6.9 ): </td>
                            <td class="cell">
                                 <input id="txt_PH" type="text" value="" style="width: 46px"/>
                            </td>
                        </tr>
                         <tr id="blk_DORNIC">
                            <td>Dornic ( 0 | 17): </td>
                            <td class="cell">
                                 <input id="txt_DORNIC" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
              
                         <tr id="blk_BRIX">
                            <td>Brix ( 9 | 100): </td>
                            <td class="cell">
                                 <input id="txt_BRIX" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>

               <tr id="blk_INH_L">
                            <td>INH L: </td>
                            <td class="cell">
                                 <input id="txt_INH_L" type="hidden" style="width: 46px" value="" />
                                <input type="checkbox" id="Check_a" onclick="chec_a();" >Incorrecto
                                <input type="checkbox" id="Check_b"  onclick="chec_b();" >Correcto
                            </td>
                        </tr>
              <tr id="blk_GRASA">
                            <td>Grasa ( 3.5 | 4 ): </td>
                            <td class="cell">
                                 <input id="txt_GRASA" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
               <tr id="blk_PROTEINA">
                            <td>Proteina ( 3.05 | 3.3 ): </td>
                            <td class="cell">
                                 <input id="txt_PROTEINA" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
               <tr id="blk_LACTOSA">
                            <td>Lactosa ( 4.5 | 4.75 ): </td>
                            <td class="cell">
                                 <input id="txt_LACTOSA" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
               <tr id="blk_SNF">
                            <td>SNF ( 9 | 9.3 ): </td>
                            <td class="cell">
                                 <input id="txt_SNF" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
               <tr id="blk_TS">
                            <td>TS ( 12.6 |13.4 ): </td>
                            <td class="cell">
                                 <input id="txt_TS" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
            
                   <tr id="blk_OLOR">
                            <td>OLOR (Correcto): </td>
                            <td class="cell">
                                 <input id="txt_OLOR" type="hidden" style="width: 46px" value="" />
                                <input type="checkbox" id="Check_c" onclick="chec_c();" >Incorrecto
                                <input type="checkbox" id="Check_d"  onclick="chec_d();" >Correcto
                            </td>
                        </tr>
              <tr id="blk_TEST_R">
                            <td>Test R (Correcto) : </td>
                            <td class="cell">
                                 <input id="txt_TEST_R" type="hidden" style="width: 46px" value="" />
                                <input type="checkbox" id="Check_e" onclick="chec_e();" >Incorrecto
                                <input type="checkbox" id="Check_f"  onclick="chec_f();" >Correcto
                            </td>
                        </tr>
                        
              </table>
       
     </div>

</asp:Content>
