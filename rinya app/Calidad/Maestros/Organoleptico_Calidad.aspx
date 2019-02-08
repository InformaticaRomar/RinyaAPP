<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Organoleptico_Calidad.aspx.cs" Inherits="rinya_app.Calidad.Maestros.Organoleptico_Calidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
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

       function load_combo2()
       {
           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Cod_Caracteristica}'>{$T.record.Caracteristica}</option>\
                    {#/for}";

           var combo = $('#cmb_carac');

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organoleptic.asmx/GetCaracteristicas" ,
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

       }
       function load_Combo1(comboname, methodname, selected) {

           var template = "{#foreach $T as record}\
                        <option value='{$T.record.ID_Estado}'>{$T.record.Estado}</option>\
                    {#/for}";

           var combo = $('#ddlEstado' );

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organoleptic.asmx/GetEstados",
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

           combo.val(selected).change();
           combo.selectmenu();
           combo.select2();
       }


       function loadTable() {
           if ($.fn.dataTable.isDataTable('#tblData')) {
               var table = $('#tblData').DataTable();
               table.destroy();
           }
           $.ajaxSetup({
               cache: false
           });

           function showDetails() {
               alert("showing some details");
           }
           
           $("#tblData").attr("style", "display: table");
           $('#tblData tfoot th').each(function () {
               var columna = $('#tblData thead th').eq($(this).index()).text();
               var pie = $('#tblData tfoot th').eq($(this).index()).text();
               pie = pie.trim();
               var tamanyo_p = pie.length//&nbsp;
               var tamanyo_c = columna.length
               if (pie == "SSCC") {
                   tamanyo_p = 19;
                   tamanyo_c = 19;
               }

               if (columna == "Fecha  ") {
                   tamanyo_c = tamanyo_c
               }
               if (tamanyo_p > 0) {

                   $(this).html('<input type="text" size=' + tamanyo_p + ' placeholder="' + pie + '" />');
               } else if (tamanyo_c > 0) {
                   $(this).html('<input type="text" style="visibility:hidden" size=' + tamanyo_c + '" />');
               }

           });
          var table = $('#tblData').DataTable({

               dom: '<"H"B<lfr>>t<"F"ip>',
               jQueryUI: true,
               responsive: true,
               buttons: [
                    'excelHtml5'
               
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
               "order": [[1, "asc"]],
               "info": true,
               "autoWidth": true,
               "sScrollY": "400px",
               "scrollX": true,
               "columnDefs": [
                      {
                          "targets": [0],
                          "className": "none"
                      },
                       {
                           "targets": [9],
                           "className": "none"
                       },
                   {
                   "targets": [36],
                   "className" : "none"
                   }, {
                       "targets": [37],
                       "className": "none"
                   },
               {
                   "aTargets": [6],
                   "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                       if (sData.indexOf("Liberad") > -1) {
                           $(nTd).addClass('liberado');
                       } else { $(nTd).addClass('retenido'); }
                   }
               }
               ],
               'bScrollCollapse': true,

               "bProcessing": true,
               "bServerSide": true,
               "sAjaxSource": "WebService_Organoleptic.asmx/GetTableData",
               
               "fnServerData": function (sSource, aoData, fnCallback) {
                   aoData.push({ "name": "fecha1", "value": $("#datepicker1").val() });
                   aoData.push({ "name": "fecha2", "value": $("#datepicker2").val() });
                  
                   $.ajax({
                       "dataType": 'json',
                       "contentType": "application/json; charset=utf-8",
                       "type": "POST",
                       "url": sSource,
                       "data":  JSON.stringify({ myData:aoData}),
                       "success": function (msg) {
                           var json = jQuery.parseJSON(msg.d);
                           fnCallback(json);
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
              var C_Datos = {};
              C_Datos.ID = $(nTds[0]).text();
              C_Datos.ARTICULO = $(nTds[1]).text();
              C_Datos.FECHA_SSCC_CON = $(nTds[3]).text();
              C_Datos.UD_ACTUAL = $(nTds[4]).text();
              C_Datos.KG_ACTUAL = $(nTds[5]).text();
              C_Datos.ESTADO = $(nTds[6]).text();
              C_Datos.SSCC = $(nTds[7]).text();
              C_Datos.ID_LOTE = $(nTds[9]).text();
              C_Datos.ID_ESTADO = $(nTds[37]).text();
              
              $('#blck_ID').hide();
              $('#blck_ID_LOTE').hide();
              
              for (var j = 1; j <= 24; j++){
                  $('#Blck_' + j).hide();
                 
              }
              $('#table-dialogo').dialog({
                  autoOpen: true,
                  modal: true,
                  resizable: false,
                  title: 'Modificar',
                  width: 550,
                  heigth: 390,

                  open: function (event, ui) {
                      if (C_Datos.ARTICULO.length > 0) {
                          $('#Texto_articulo').text($(nTds[1]).text() + ' | ' + $(nTds[2]).text());
                          $('#Texto_matricula').text($(nTds[7]).text());
                          $('#Texto_Lote_Interno').text($(nTds[8]).text());
                          $('#Texto_Fecha_Caducidad').text($(nTds[12]).text());
                          $('#Texto_ID_LOTE').text($(nTds[13]).text());
                          $('#Texto_ID').text($(nTds[0]).text());
                          $('#text_24').val($(nTds[38]).text());
                          $('#Blck_24').show();
                          loadComboBox('ddlEstado', 'GetEstados', C_Datos.ID_ESTADO);
                          $('#ddlEstado').selectmenu().selectmenu('refresh', true);
                          
                          $.ajax({
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              url: "WebService_Organoleptic.asmx/GetCaracteristicas_articulo",
                              data: '{datos: ' + JSON.stringify(C_Datos) + '}',
                              dataType: "json",

                              success: function (data) {

                                  var dato = JSON.parse(data.d);
                                  if (dato.length > 0)
                                      for (var i = 0; i < dato.length; i++) {

                                          var valor = dato[i].Caracteristica;
                                          var texto_lbl =  $('#lbl_' + valor).text();
                                          $('#lbl_' + valor).text(dato[i].lbl);
                                          $('#text_' + valor).val($(nTds[12 + valor]).text());
                                          $('#Blck_' + valor).show();
                                      }
                              },
                              error: function (request, status, error) {
                                  alert(JSON.parse(request.responseText).Message);
                              }
                          });
                      }
                  },
                  close: function (event, ui) {


                      $('#table-dialogo :text').val("");

                  },
                  buttons: {

                      "Update": function () {
                          C_Datos.PH_CRUDO_AP = $('#text_1').val();
                          C_Datos.PH_CRUDO_DP = $('#text_2').val();
                          C_Datos.BRIX_CRUDO_AP = $('#text_3').val();
                          C_Datos.BRIX_CRUDO_DP = $('#text_4').val();
                          C_Datos._HUMEDAD = $('#text_5').val();
                          C_Datos._ES = $('#text_6').val();
                          C_Datos.HC = $('#text_7').val();
                          C_Datos.SACAROSA = $('#text_8').val();
                          C_Datos.GRASA = $('#text_9').val();
                          C_Datos.PROTEINA = $('#text_10').val();
                          C_Datos.LACTOSA = $('#text_11').val();
                          C_Datos.TEMPERATURA = $('#text_12').val();
                          C_Datos.PH = $('#text_13').val();
                          C_Datos.COLOR = $('#text_14').val();
                          C_Datos.SABOR = $('#text_15').val();
                          C_Datos.CORTE = $('#text_16').val();
                          C_Datos.FILM = $('#text_17').val();
                          C_Datos.CATA = $('#text_18').val();
                          C_Datos.GLUTEN = $('#text_19').val();
                          C_Datos.CASEINA = $('#text_20').val();
                          C_Datos.LISTERIA = $('#text_21').val();
                          C_Datos.SALMONELLA = $('#text_22').val();
                          C_Datos.PPC = $('#text_23').val();
                          C_Datos.OBS_ESTADO = $('#text_24').val();
                          if (C_Datos.ID_LOTE.length > 0) {

                              $.ajax({
                                  type: "POST",
                                  url: "WebService_Organoleptic.asmx/update_Data",
                                  data: '{datos: ' + JSON.stringify(C_Datos) + '}',
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

          });

         
          
       }
       function loadComboBox(comboname, methodname, selected) {

           var template = "{#foreach $T as record}\
                        <option value='{$T.record.ID_Estado}'>{$T.record.Estado}</option>\
                    {#/for}";

           var combo = $('#' + comboname);

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organoleptic.asmx/" + methodname,
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

           combo.val(selected).change();
           //combo.selectmenu();
           //combo.select2();
       }
    
      
       jQuery(document).ready(function () {
           loadComboBox('ddlEstado', 'GetEstados', 1);
           var hoy = new Date();
           var semana_dia = new Date();
           semana_dia.setDate(semana_dia.getDate() - 2);

           $("#datepicker1").datepicker().datepicker("setDate", semana_dia);
           $("#datepicker2").datepicker().datepicker("setDate", new Date());
           $('#btnTodas').click(loadTable());
          
       });

   </script>

    <h1><%: Title %></h1>

      <div>
    <table>
        <tr>
          <th>
<p>Desde: <input type="text" id="datepicker1" style="width: 89px"></p>
              </th>
<th>
  <p>Hasta: <input type="text" id="datepicker2" style="width: 89px"></p>

    </th>
       <th>
        <input type="button" id="btnTodas" value="Obtener Matriculas" style="width: 160px" onclick="loadTable()" />&nbsp;
     </th>
            </tr>
    </table>
   
        </div>
    <p></p>
    <p></p>
    <table id="tblData" class="display" style="display: none" >
      <thead>
             <tr>
                 <th>ID</th>
                  <th>ARTICULO</th>
                 <th>DESCRIPCION</th>
				  <th>FECHA_SSCC_CON</th>
				  <th>UD_ACTUAL</th>
				  <th>KG_ACTUAL</th>
				  <th>ESTADO</th>
				  <th>SSCC</th>
				  <th>LOTE_INTERNO</th>
				  <th>ID_LOTE</th>
				  <th>FECHA_CREACION</th>
				  <th>HORA</th>
				  <th>FECHACADUCIDAD</th>
				  <th>PH_CRUDO_AP</th>
				  <th>PH_CRUDO_DP</th>
				  <th>BRIX_CRUDO_AP</th>
				  <th>BRIX_CRUDO_DP</th>
				  <th>%_HUMEDAD</th>
				  <th>%_ES</th>
				  <th>HC</th>
				  <th>SACAROSA</th>
				  <th>GRASA</th>
                  <th>PROTEINA</th>
                  <th>LACTOSA</th>
                  <th>TEMPERATURA</th>
				  <th>PH</th>
				  <th>COLOR</th>
				  <th>SABOR</th>
				  <th>CORTE</th>
				  <th>FILM</th>
				  <th>CATA</th>
				  <th>GLUTEN</th>
				  <th>CASEINA</th>
				  <th>LISTERIA</th>
				  <th>SALMONELLA</th>
				  <th>PPC</th>
                 <th>ID_ESTADO</th>
                 <th>OBS_ESTADO</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot  style="text-align:center">
            <tr  style="text-align:center ; padding:100px">
                  <th>ID</th>
                  <th>ARTICULO</th>
                 <th>DESCRIPCION</th>
				  <th>FECHA_SSCC_CON</th>
				  <th></th>
				  <th></th>
				  <th>ESTADO</th>
				  <th>SSCC</th>
				  <th>LOTE_INTERNO</th>
				  <th>ID_LOTE</th>
				  <th>FECHA_CREACION</th>
				  <th>HORA</th>
				  <th>FECHACADUCIDAD</th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
				  <th></th>
                <th>ID_ESTADO</th>
                <th></th>
                  </tr>
        </tfoot>
</table> 
   
      <div id="table-dialogo"   style="display: none" >

          <table id="orderedittable">
                        <tr>
                            <td id="Texto_lbl" style="width: 168px">Articulo: </td>
                                 <td id="Texto_articulo" style="width: 250px">Descripcion </td> 
                        </tr>
                        <tr>
                            <td>Matricula: </td>
                            <td id="Texto_matricula" style="width: 250px">1234 </td> 
                        </tr>
						<tr>
                            <td>Estado</td>
                            <td class="cell">
                                <select id="ddlEstado" style="width: 250px">
                                </select>
                            </td>
                        </tr>
						<tr>
                            <td>Lote Interno: </td>
                            <td id="Texto_Lote_Interno" style="width: 250px">1234 </td> 
                        </tr>
						<tr>
                            <td>Fecha Caducidad: </td>
                            <td id="Texto_Fecha_Caducidad" style="width: 250px">1234 </td> 
                        </tr>
						<tr id="Blck_1">
                            <td id ="lbl_1" style="width: 250px">PH Antes Paste </td>
                            <td class="cell">
                                 <input id="text_1" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_2">
                            <td id ="lbl_2" style="width: 250px">PH Despues Paste </td>
                            <td class="cell">
                                 <input id="text_2" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_3">
                            <td id ="lbl_3" style="width: 250px">BRIX Antes Paste </td>
                            <td class="cell">
                                 <input id="text_3" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_4">
                            <td id ="lbl_4" style="width: 250px">BRIX Despues Paste </td>
                            <td class="cell">
                                 <input id="text_4" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_5">
                            <td id ="lbl_5" style="width: 250px">HUMEDAD </td>
                            <td class="cell">
                                 <input id="text_5" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_6">
                            <td id ="lbl_6" style="width: 250px">ES </td>
                            <td class="cell">
                                 <input id="text_6" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_7">
                            <td id ="lbl_7" style="width: 250px">HC </td>
                            <td class="cell">
                                 <input id="text_7" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_8">
                            <td id ="lbl_8" style="width: 250px">Sacarosa </td>
                            <td class="cell">
                                 <input id="text_8" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_9">
                            <td id ="lbl_9"  style="width: 250px">Grasa </td>
                            <td class="cell">
                                 <input id="text_9" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_10">
                            <td id ="lbl_10"  style="width: 250px">Proteina </td>
                            <td class="cell">
                                 <input id="text_10" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_11">
                            <td id ="lbl_11" style="width: 250px">Lactosa </td>
                            <td class="cell">
                                 <input id="text_11" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_12">
                            <td id ="lbl_12" style="width: 250px">Temperatura </td>
                            <td class="cell">
                                 <input id="text_12" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_13">
                            <td id ="lbl_13">Ph </td>
                            <td class="cell">
                                 <input id="text_13" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr >
                         <tr id="Blck_14" >
                            <td id ="lbl_14"  style="width: 250px">Color </td>
                            <td class="cell">
                                 <input id="text_14" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_15">
                            <td id ="lbl_15" style="width: 250px">Sabor </td>
                            <td class="cell">
                                 <input id="text_15" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                         <tr id="Blck_16">
                            <td id ="lbl_16" style="width: 250px">Corte </td>
                            <td class="cell">
                                 <input id="text_16" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_17">
                            <td id ="lbl_17" style="width: 250px">Film </td>
                            <td class="cell">
                                 <input id="text_17" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_18">
                            <td id ="lbl_18" style="width: 250px">Cata </td>
                            <td class="cell">
                                 <input id="text_18" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_19">
                            <td id ="lbl_19" style="width: 250px">Gluten </td>
                            <td class="cell">
                                 <input id="text_19" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_20">
                            <td id ="lbl_20" style="width: 250px">Caseina </td>
                            <td class="cell">
                                 <input id="text_20" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_21">
                            <td id ="lbl_21" style="width: 250px">Listeria </td>
                            <td class="cell">
                                 <input id="text_21" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_22">
                            <td id ="lbl_22" style="width: 250px">Salmonela </td>
                            <td class="cell">
                                 <input id="text_22" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_23">
                            <td id ="lbl_23" style="width: 250px">PPC </td>
                            <td class="cell">
                                 <input id="text_23" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_24">
                            <td id ="lbl_24">ObsEstado </td>
                            <td class="cell">
                                 <TextArea id="text_24" cols="40" rows="3" style="width: 250px"> </TextArea>
                            </td>
                        </tr>
                        <tr id="blck_ID_LOTE">
                            <td id="lbl_ID_LOTE" style="width: 250px">Id_Lote: </td>

                                 <td id="Texto_ID_LOTE" style="width: 250px">1234 </td> 
                        </tr>
               <tr id="blck_ID">
                            <td id="lbl_ID" style="width: 250px">Id: </td>
                                 <td id="Texto_ID" style="width: 250px">1234 </td> 
                        </tr>
              </table>       
 </div>

</asp:Content>
