<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Organoleptico_comite.aspx.cs" Inherits="rinya_app.Calidad.Maestros.Organoleptico_comite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
      <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
      <link href="../../Scripts/jquery-ui/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/DataTables/css/buttons.dataTables.css"  rel="stylesheet" type="text/css" />
    <link href="../../Content/dataTables.jqueryui.css" rel="stylesheet" type="text/css" />

    <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
    <script src="../../Scripts/DataTables/jquery.dataTables.js"></script>
    <script src="../../Scripts/DataTables/jszip.min.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/DataTables/dataTables.buttons.min.js" ></script>
    <script src="../../Scripts/DataTables/buttons.html5.min.js"></script>
         <style> select {
           width: 300px;
       }
       .overflow {
           height: 300px;
       }
             .auto-style1 {
                 width: 511px;
             }
       </style>
   <script type="text/javascript"> 
       function check_Arriba_A() {
           var componente = '#txt_E_arriba';
           var check1 = '#text_E_arriba_A';
           var check2 = '#text_E_arriba_B';
           if ($(check1).prop("checked")) {
               $(componente).val("0");
               $(check2).prop("checked", false);
           }
       }
       function check_Arriba_B() {
           var componente = '#txt_E_arriba';
           var check1 = '#text_E_arriba_A';
           var check2 = '#text_E_arriba_B';
           if ($(check2).prop("checked")) {
               $(componente).val("1");
               $(check1).prop("checked", false);
           }
       }

       function check_Medio_A() {
           var componente = '#txt_E_medio';
           var check1 = '#text_E_medio_A';
           var check2 = '#text_E_medio_B';
           if ($(check1).prop("checked")) {
               $(componente).val("0");
               $(check2).prop("checked", false);
           }
       }
       function check_Medio_B() {
           var componente = '#txt_E_medio';
           var check1 = '#text_E_medio_A';
           var check2 = '#text_E_medio_B';
           if ($(check2).prop("checked")) {
               $(componente).val("1");
               $(check1).prop("checked", false);
           }
       }

       function check_Abajo_A() {
           var componente = '#text_E_abajo';
           var check1 = '#text_E_abajo_A';
           var check2 = '#text_E_abajo_B';
           if ($(check1).prop("checked")) {
               $(componente).val("0");
               $(check2).prop("checked", false);
           }
       }
       function check_Abajo_B() {
           var componente = '#text_E_abajo';
           var check1 = '#text_E_abajo_A';
           var check2 = '#text_E_abajo_B';
           if ($(check2).prop("checked")) {
               $(componente).val("1");
               $(check1).prop("checked", false);
           }
       }

       function check_A() {
           var componente = '#text_E';
           var check1 = '#text_E_A';
           var check2 = '#text_E_B';
           if ($(check1).prop("checked")) {
               $(componente).val("0");
               $(check2).prop("checked", false);
           }
       }
       function check_B() {
           var componente = '#text_E';
           var check1 = '#text_E_A';
           var check2 = '#text_E_B';
           if ($(check2).prop("checked")) {
               $(componente).val("1");
               $(check1).prop("checked", false);
           }
       }
       function ES_HR(){
           var _ES = parseFloat($('#text_6').val());
           var _HR = 100 - _ES;
           $('#text_30').val(_HR);
       }
       function HR_ES() {
           var  _HR= parseFloat($('#text_30').val());
           var _ES = 100 - _HR;
           $('#text_6').val(_ES);
       }
       $(window).bind('resize', function () {
           // table.fnAdjustColumnSizing();
           //table.columns.adjust().draw();
           $('#tblData').DataTable().columns.adjust().draw(false, true);
       });
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
       function cambiarvalor(dato)
       {
           //if (dato.toString.length>0)
           var selectedval = ($("#ddlEstado option:selected").val());
           $('#Texto_est').val(dato);
           alert(selectedval);
       }
       function chec_a(x) {
           var componente = '#text_' + x;
           var check1 = '#Check_a_' + x;
           var check2 = '#Check_b_' + x;
           if ($(check1).prop("checked")) {
               $(componente).val("1");
               $(check2).prop("checked", false);
           }
       }
       function chec_b(x) {
           var componente = '#text_' + x;
           var check1 = '#Check_a_' + x;
           var check2 = '#Check_b_' + x;
           if ( $(check2).prop("checked")) {
               $(componente).val("0");
               $(check1).prop("checked", false);
           }
       }
       function load_combo2()
       {
           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Cod_Caracteristica}'>{$T.record.Caracteristica}</option>\
                    {#/for}";

           var combo = $('#cmb_carac');

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organo_Comite.asmx/GetCaracteristicas",
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
               url: "WebService_Organo_Comite.asmx/GetEstados",
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


       function load_Combo3(comboname, methodname, selected) {

           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Cod_NC}'>{$T.record.Nc_des}</option>\
                    {#/for}";

           var combo = $('#ddlMerma');

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organo_Comite.asmx/GetMotivoMerma",
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
        
       }
       function loadTable() {
           if ($.fn.dataTable.isDataTable('#tblData')) {

               var table = $('#tblData').DataTable();
               table.destroy();
              // table.clear();
               //table.fnDestroy();
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
                 //  $(this).html('<input type="text" style="visibility:hidden" size=' + tamanyo_c + '" />');
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
               "lengthMenu": [[10, 25, 30], [10, 25, 30]],
               "iDisplayLength": 10,
               "filter": true,

               "pagingType": "simple_numbers",
               "orderClasses": false,
               "order": [[1, "asc"]],
               "info": true,
               "autoWidth": true,
               "sScrollY": "400px",
               "sScrollX": '100%',
               "columnDefs": [
                      {
                          "targets": [0],
                          "className": "none"
                      },
                       {
                           "targets": [4],
                           "className": "none"
                       },
                        {
                            "targets": [5],
                            "className": "none"
                        },
                       {
                           "targets": [9],
                           "className": "none"
                       }, {
                           "targets": [13],
                           "className": "none"
                       }, {
                           "targets": [14],
                           "className": "none"
                       },
				   {
				       "targets": [15],
				       "className": "none"
				   }, {
				       "targets": [16],
				       "className": "none"
				   }, {
				       "targets": [17],
				       "className": "none"
				   }, {
				       "targets": [18],
				       "className": "none"
				   },
				   {
				       "targets": [19],
				       "className": "none"
				   }, {
				       "targets": [20],
				       "className": "none"
				   }, {
				       "targets": [21],
				       "className": "none"
				   }, {
				       "targets": [22],
				       "className": "none"
				   },
				   {
				       "targets": [23],
				       "className": "none"
				   }, {
				       "targets": [24],
				       "className": "none"
				   }, {
				       "targets": [25],
				       "className": "none"
				   }, {
				       "targets": [26],
				       "className": "none"
				   },
				   {
				       "targets": [27],
				       "className": "none"
				   }, {
				       "targets": [28],
				       "className": "none"
				   }, {
				       "targets": [29],
				       "className": "none"
				   }, {
				       "targets": [30],
				       "className": "none"
				   },
				   {
				       "targets": [31],
				       "className": "none"
				   }, {
				       "targets": [32],
				       "className": "none"
				   }, {
				       "targets": [33],
				       "className": "none"
				   }, {
				       "targets": [34],
				       "className": "none"
				   },
				   {
				       "targets": [35],
				       "className": "none"
				   },  {
				       "targets": [37],
				       "className": "none"
				   }, {
				       "targets": [38],
				       "className": "none"
				   },
				   {
				       "targets": [39],
				       "className": "none"
				   }, {
				       "targets": [40],
				       "className": "none"
				   }, {
				       "targets": [41],
				       "className": "none"
				   }, {
				       "targets": [42],
				       "className": "none"
				   },
				   {
				       "targets": [43],
				       "className": "none"
				   }, {
				       "targets": [44],
				       "className": "none"
				   }, {
				       "targets": [45],
				       "className": "none"
				   },
				   {
				       "targets": [46],
				       "className": "none"
				   }, {
				       "targets": [47],
				       "className": "none"
				   }, {
				       "targets": [48],
				       "className": "none"
				   }, {
				       "targets": [49],
				       "className": "none"
				   },
				   {
				       "targets": [50],
				       "className": "none"
				   },
                    {
                        "targets": [51],
                        "className": "none"
                    },
                     {
                         "targets": [52],
                         "className": "none"
                     }, {
                         "targets": [53],
                         "className": "none"
                     }, {
                         "targets": [54],
                         "className": "none"
                     },
                     {
                         "targets": [55],
                         "className": "none"
                     }, {
                         "targets": [56],
                         "className": "none"
                     }, {
                         "targets": [57],
                         "className": "none"
                     }, {
                         "targets": [58],
                         "className": "none"
                     }, {
                         "targets": [59],
                         "className": "none"
                     }, {
                         "targets": [60],
                         "className": "none"
                     }, {
                         "targets": [61],
                         "className": "none"
                     }, 
                    {
                        "targets": [62],
                        "className": "none"
                    }, {
                        "targets": [64],
                        "className": "none"
                    },
                   {
                       "targets": [65],
                       "className": "none"
                   }, {
                       "targets": [66],
                       "className": "none"
                   }, {
                       "targets": [67],
                       "className": "none"
                   },
                   {
                       "targets": [68],
                       "className": "none"
                   },
                   {
                       "targets": [69],
                       
                       'searchable': false,
                    'orderable': false,
                    'render': function (data, type, full, meta) {

                        var check = parseFloat(data);

                        if (check == 0) {
                            return '<input type="checkbox" id="Vmin_chk" value="' + $('<div/>').text(data).html() + '">';
                        } else if (check == 1) {
                            return '<input type="checkbox" id="Vmin_chk" checked value="' + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return $('<div/>').text(data).html();
                        }
                    }
              
                   },
                   {
                       "targets": [70],
                       "className": "none"
                   },
                   {
                       "targets": [71],
                       "className": "none"
                   },
                   {
                       "targets": [72],
                       "className": "none"
                   },
                   {
                       "targets": [73],
                       "className": "none"
                   },
                   {
                       "targets": [74],
                       "className": "none"
                   },
                   {
                       "targets": [75],
                       "className": "none"
                   },
                   {
                       "targets": [76],
                       "className": "none"
                   },
                   {
                       "targets": [77],
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
               "bDestroy": true,
               'bScrollCollapse': true,

               "bProcessing": true,
               "bServerSide": true,
               "sAjaxSource": "WebService_Organo_Comite.asmx/GetTableData",
               
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
               }
          });
        
          function ltrim(str, filter) {
              var i = str.length;
              filter || (filter = '');
              for (var k = 0; k < i && filtering(str.charAt(k), filter) ; k++);
              return str.substring(k, i);
          }

          function filtering(charToCheck, filter) {
              filter || (filter = " \t\n\r\f");
              return (filter.indexOf(charToCheck) != -1);
          }
          table.columns().every(function () {
              var that = this;
             // var data = that[index].name;
             

              if ($(that.footer()).hasClass('tfoot_search')) {

                      var that = this;
                      $('input', this.footer()).on('keyup change', function () {
                          if (that.search() !== ltrim(this.value, "0")) {
                              that
                                  .search(ltrim(this.value, "0"))
                                  .draw();
                          }
                      });   

              } else if ($(that.footer()).hasClass('tfoot_Enter')) {

                  var that = this;
                      $('input', this.footer()).on('keyup change', function (event) {
                          var key = event.keyCode || event.which;
                          if (key == 13 || key == 10) {
                              if (that.search() !== ltrim(this.value, "0")) {
                                  that
                                      .search(ltrim(this.value, "0"))
                                      .draw();
                                  $(this).select();
                              }
                          } else if ((key == 46 || key == 08) && this.value.length == 0) {

                              if (that.search() !== ltrim(this.value, "0")) {
                                  that
                                      .search(ltrim(this.value, "0"))
                                      .draw();
                                  $(this).select();
                              }
                          }
                          return false;
                      });
                  }
             /* $('input', this.footer()).on('keyup change', function () {
                 
                  if (that.search() !== ltrim(this.value,"0")) {
                      that
                          .search(ltrim(this.value,"0"))
                          .draw();
                  }
              });*/
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
              C_Datos.ID_ESTADO = $(nTds[77]).text();
              C_Datos.OBS_ESTADO = $(nTds[36]).text();
              $('#blck_ID').hide();
              $('#blck_ID_LOTE').hide();
              C_Datos.MERMA = $(nTds[65]).text();
              for (var j = 1; j <= 52; j++){
                  $('#Blck_' + j).hide();
                 
              }
              $('#table-dialogo').dialog({
                  autoOpen: true,
                  modal: true,
                  resizable: false,
                  title: 'Modificar',
                  width: 750,
                  heigth: 390,
                 

                  open: function (event, ui) {
                      if (C_Datos.ARTICULO.length > 0) {
                          $('#Texto_articulo').text($(nTds[1]).text() + ' | ' + $(nTds[2]).text());
                          $('#Texto_matricula').text($(nTds[7]).text());
                          $('#Texto_Lote_Interno').text($(nTds[8]).text());
                          $('#Texto_Fecha_Caducidad').text($(nTds[12]).text());
                          $('#Texto_ID_LOTE').text($(nTds[13]).text());
                          $('#Texto_ID').text($(nTds[0]).text());
                          $('#text_0').val($(nTds[36]).text());
                          var estado_com = parseFloat($(nTds[69]).children('#Vmin_chk').val());
                          $('#text_E').val(estado_com);
                          if (estado_com == '1') {
                              $('#text_E_A').prop('checked', false);
                              $('#text_E_B').prop('checked', true);
                          }
                          else if (estado_com == '0') {
                              $('#text_E_A').prop('checked', true);
                              $('#text_E_B').prop('checked', false);
                          }
                          else {
                              $('#text_E_A').prop('checked', false);
                              $('#text_E_B').prop('checked', false);
                          }
                          
                          $('#text_observa_comite').val($(nTds[76]).text());
                          $('#text_H_arriba').val($(nTds[70]).text());
                          
                          var arriba = $(nTds[71]).text();
                          $('#txt_E_arriba').val(arriba);
                          if (arriba == '1') {
                              $('#text_E_arriba_A').prop('checked', false);
                              $('#text_E_arriba_B').prop('checked', true);
                          }
                          else if (arriba == '0') {
                              $('#text_E_arriba_A').prop('checked', true);
                              $('#text_E_arriba_B').prop('checked', false);
                      }
                      else {
                              $('#text_E_arriba_A').prop('checked', false);
                              $('#text_E_arriba_B').prop('checked', false);
                       }
                          $('#text_H_medio').val($(nTds[72]).text());
                          
                          var medio = $(nTds[73]).text();
                          $('#txt_E_medio').val(medio);
                          if (medio == '1') {
                              $('#text_E_medio_A').prop('checked', false);
                              $('#text_E_medio_B').prop('checked', true);
                          }
                          else if (medio == '0') {
                              $('#text_E_medio_A').prop('checked', true);
                              $('#text_E_medio_B').prop('checked', false);
                          }
                          else {
                              $('#text_E_medio_A').prop('checked', false);
                              $('#text_E_medio_B').prop('checked', false);
                          }

                          $('#text_H_abajo').val($(nTds[74]).text());

                          var abajo = $(nTds[75]).text();
                          $('#text_E_abajo').val(abajo);
                          if (abajo == '1') {
                              $('#text_E_abajo_A').prop('checked', false);
                              $('#text_E_abajo_B').prop('checked', true);
                          }
                          else if (abajo == '0') {
                              $('#text_E_abajo_A').prop('checked', true);
                              $('#text_E_abajo_B').prop('checked', false);
                          }
                          else {
                              $('#text_E_abajo_A').prop('checked', false);
                              $('#text_E_abajo_B').prop('checked', false);
                          }

                          $('#Blck_0').show();
                        //  loadComboBox('ddlEstado', 'GetEstados', C_Datos.ID_ESTADO);
                          //$("#ddlEstado option:selected").val(C_Datos.ID_ESTADO);
                          $('#ddlEstado').val(C_Datos.ID_ESTADO);
                         // $("#ddlEstado option:selected").text(C_Datos.ESTADO);
                          $('#ddlEstado').selectmenu().selectmenu('refresh', true);
                          $('#ddlMerma').val(C_Datos.MERMA);
                          $('#ddlMerma').selectmenu().selectmenu("menuWidget").addClass("overflow");
                         
                          if (C_Datos.ID_ESTADO == 4 || C_Datos.ID_ESTADO == 7 || C_Datos.ID_ESTADO == 8)
                          {
                              
                              //$('#ddlMerma').selectmenu().selectmenu('selectmenuchange');
                              $('#ddlMerma').selectmenu().selectmenu('refresh', true);
                          $('#Merma').show();
                          } else { $('#Merma').hide(); $('#ddlMerma option:selected').val("0"); }
                          

                        /*  $('#ddlEstado').selectmenu().selectmenu('change', function (event, data) {
                              alert(data.item.value);
                          });*/
                          $('#ddlEstado').on('selectmenuchange', function (e, ui) {
                            
                              if (ui.item.value == 4 || ui.item.value == 7 || ui.item.value == 8) {
                                  $('#ddlMerma').selectmenu().selectmenu('refresh', true);
                                  $('#Merma').show();
                              } else {
                                  $('#ddlMerma option:selected').val("0");
                                  $('#Merma').hide();
                              }
                            
                             
                          });
                         

                         // $('#ddlEstado').selectmenu().selectmenu('change', cambiarvalor($('#ddlEstado').val));
                          
                          $.ajax({
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              url: "WebService_Organo_Comite.asmx/GetCaracteristicas_articulo",
                              data: '{datos: ' + JSON.stringify(C_Datos) + '}',
                              dataType: "json",

                              success: function (data) {

                                  var dato = JSON.parse(data.d);
                                  if (dato.length > 0)
                                      for (var i = 0; i < dato.length; i++) {
                                          
                                          var valor = dato[i].Caracteristica;
                                          var tipo_dato = dato[i].Tipo_Dato
                                          var val = 12;
                                          if (valor >= 24) {
                                              val = 13;
                                          }
                                          if (valor >= 36)
                                          {
                                              val = 14;
                                          }
                                          if (valor >= 50) {
                                              val = 16;
                                          }
                                          
                                          var texto_lbl = $('#lbl_' + valor).text();
                                          var pr = val + valor;
                                          $('#lbl_' + valor).text(dato[i].lbl);
                                          if (tipo_dato==1){
                                              $('#text_' + valor).val($(nTds[val + valor]).text());
                                          }
                                          else if (tipo_dato == 2) {
                                              // $('#text_' + valor).type = "Checkbox";
                                              var componente = '#text_' + valor;
                                              var check1 = '#Check_a_' + valor;
                                              var check2 = '#Check_b_' + valor;
                                              if ($(check1).length) {
                                                  var dato_colum = $(nTds[val + valor]).text();
                                                  if (dato_colum.length > 0) {
                                                      if (dato_colum == 1) {
                                                          $(componente).val("1");
                                                          $(check1).prop('checked', true);
                                                          $(check2).prop('checked', false);

                                                      } else {

                                                          $(componente).val("0");
                                                          $(check1).prop('checked', false);
                                                          $(check2).prop('checked', true);

                                                      }
                                                  }
                                                  else {
                                                      $(componente).val("");
                                                      $(check1).prop('checked', false);
                                                      $(check2).prop('checked', false);
                                                  }
                                              }else{
                                                  $(componente).prop("type", "hidden");
                                                  $(componente).after('<li><input type="checkbox" id="Check_a_' + valor + '" onclick="chec_a('+valor+');" >Incorrecto</li><li><input type="checkbox" id="Check_b_' + valor + '"  onclick="chec_b('+valor+');" >Correcto</li>');

                                                  var dato_colum = $(nTds[val + valor]).text();
                                                  if (dato_colum.length>0){
                                                      if (dato_colum == 1) {
                                                          $(componente).val("1");
                                                          $(check1).prop('checked', true);
                                                          $(check2).prop('checked', false);

                                                      } else{

                                                          $(componente).val("0");
                                                          $(check1).prop('checked', false);
                                                          $(check2).prop('checked', true);

                                                      }
                                                  }
                                                  else {
                                                      $(componente).val("");
                                                      $(check1).prop('checked', false);
                                                      $(check2).prop('checked', false);
                                                  }
                                              }
                                          }

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
                          C_Datos.OBS_ESTADO = $('#text_0').val();
                          C_Datos.IMPUREZAS = $('#text_26').val();
                          C_Datos.BOLETIN = $('#text_24').val();
                          C_Datos.BRIX = $('#text_25').val();
                          C_Datos.CONSISTENCIA = $('#text_27').val();
                          C_Datos.OLOR = $('#text_28').val();
                          C_Datos.EMULSION = $('#text_29').val();
                          C_Datos.HR = $('#text_30').val();
                          C_Datos.TS = $('#text_31').val();
                          C_Datos.SNF = $('#text_32').val();
                          C_Datos.ASPECTO = $('#text_33').val();
                          C_Datos.DORNIC = $('#text_34').val();
                          C_Datos.INHIBIDORES = $('#text_35').val();
                          C_Datos.NC = "";
                          C_Datos.SILO = $('#text_36').val();
                          C_Datos.TTRANSP = $('#text_37').val();
                          C_Datos.LAVADO = $('#text_38').val();
                          C_Datos.SILODEST = $('#text_39').val();
                          C_Datos.EDAD_LECHE = $('#text_40').val();
                          C_Datos.T_CISTERNA = $('#text_41').val();
                          C_Datos.T_MUESTRA = $('#text_42').val();
                          C_Datos.INH_R = $('#text_43').val();
                          C_Datos.INH_L = $('#text_44').val();
                          C_Datos.D = $('#text_45').val();
                          C_Datos.ALCOHOL = $('#text_46').val();
                          C_Datos.C_FILTRO = $('#text_47').val();
                          C_Datos.INH_BOB = $('#text_48').val();
                          C_Datos.FLATOXINA = $('#text_49').val();
                          C_Datos.CONTROL_PESO_CALIDAD = $('#text_50').val();
                          C_Datos.CONTROL_PESO_LINEA = $('#text_51').val();
                          C_Datos.CURVA_PH = $('#text_52').val();
                          C_Datos.ESTADO_COMITE = $('#text_E').val();
                          C_Datos.HORA_ARRIBA = $('#text_H_arriba').val();
                          C_Datos.ESTADO_ARRIBA = $('#txt_E_arriba').val();
                          C_Datos.HORA_MEDIO = $('#text_H_medio').val();
                          C_Datos.ESTADO_MEDIO = $('#txt_E_medio').val();
                          C_Datos.HORA_ABAJO = $('#text_H_abajo').val();
                          C_Datos.ESTADO_ABAJO = $('#text_E_abajo').val();
                          C_Datos.OBSEVACIONES_COMITE = $('#text_observa_comite').val();
                          C_Datos.MERMA = $("#ddlMerma option:selected").val();
                          C_Datos.ID_ESTADO = $("#ddlEstado option:selected").val();
                          C_Datos.ESTADO = $("#ddlEstado option:selected").text();
                          if ((C_Datos.ID_ESTADO == 4 || C_Datos.ID_ESTADO == 7 || C_Datos.ID_ESTADO == 8) && C_Datos.MERMA == 0)
                          {
                              alert("No se va a guardar por no indicar motivo de merma");
                          }else{
                          //alert($('#ddlEstado').selectmenu().selectmenu("value", value));
                              if (C_Datos.ID_LOTE.length > 0) {

                                  $.ajax({
                                      type: "POST",
                                      url: "WebService_Organo_Comite.asmx/update_Data",
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
                          }
                      },
                      "Cancelar": function () {
                          $(this).dialog("close");
                      }
                  }
              });

          });
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
     
       }
       function loadComboBox(comboname, methodname, selected) {

           var template = "{#foreach $T as record}\
                        <option value='{$T.record.ID_Estado}'>{$T.record.Estado}</option>\
                    {#/for}";

           var combo = $('#' + comboname);

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_Organo_Comite.asmx/" + methodname,
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
           //combo.on("change", cambiarvalor($(this).val));
          // combo.selectmenu();
           //combo.select2();
       }
    
      
       jQuery(document).ready(function () {
           loadComboBox('ddlEstado', 'GetEstados', 1);
           load_Combo3('ddlMerma', 'GetMotivoMerma', 1);
           //$('#ddlEstado').on('selectmenuchange', cambiarvalor(this.value));
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
<p>Desde: <input type="text" id="datepicker1" class="form-control" style="width: 89px"></p>
              </th>
<th>
  <p>Hasta: <input type="text" id="datepicker2" class="form-control" style="width: 89px"></p>

    </th>
       <th>
        <input type="button" id="btnTodas" class="btn btn-default" value="Obtener Matriculas" style="width: 160px" onclick="loadTable()" />&nbsp;
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
                 <th>OBS_ESTADO</th>
                 <th>BOLETIN</th>
                    <th>BRIX</th>  
                    <th>IMPUREZAS</th>
                    <th>CONSISTENCIA</th>
                    <th>OLOR</th>
                    <th>EMULSION</th>
                    <th>HR</th>
                    <th>TS</th>
                    <th>SNF</th>
                    <th>ASPECTO</th>
                    <th>DORNIC</th>
                    <th>INHIBIDORES</th>
                    <th>NC</th>
                  
                 <th>SILO</th>
<th>TTRANSP</th>
<th>LAVADO</th>
<th>SILODEST</th>
<th>EDAD_LECHE</th>
<th>T_CISTERNA</th>
<th>T_MUESTRA</th>
<th>INH_R</th>
<th>INH_L</th>
<th>D</th>
<th>ALCOHOL</th>
<th>C_FILTRO</th>
<th>INH_BOB</th><th>NUMPALET</th><th>FLATOXINA</th>
                 <th>MERMA</th>
                 <th>CONTROL_PESO_CALIDAD</th>
                 <th>CONTROL_PESO_LINEA</th>
                 <th>CURVA_PH</th>
                 <th>ESTADO_COMITE</th>
                <th>HORA_ARRIBA</th>
                <th>ESTADO_ARRIBA</th>
                <th>HORA_MEDIO</th>
                 <th>ESTADO_MEDIO</th>
                <th>HORA_ABAJO</th>
                <th>ESTADO_ABAJO</th>
                <th>OBSEVACIONES_COMITE</th>
                 <th>ID_ESTADO</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot  style="text-align:center">
            <tr  style="text-align:center ; padding:100px">
                  <th class="tfoot_search">ID</th>
                  <th class="tfoot_Enter">ARTICULO</th>
                 <th class="tfoot_search">DESCRIPCION</th>
				  <th class="tfoot_Enter">FECHA_SSCC_CON</th>
				  <th ></th>
				  <th></th>
				  <th class="tfoot_search">ESTADO</th>
				  <th class="tfoot_Enter">SSCC</th>
				  <th class="tfoot_Enter">LOTE_INTERNO</th>
				  <th class="tfoot_search">ID_LOTE</th>
				  <th class="tfoot_search">FECHA_CREACION</th>
				  <th class="tfoot_search">HORA</th>
				  <th class="tfoot_search">FECHACADUCIDAD</th>
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
                <th></th>
                <th></th>
                <th></th>
                <th></th>
                 <th class="tfoot_Enter">NumPalet</th>
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
                  </tr>
        </tfoot>
</table> 
   
      <div id="table-dialogo" style="display: none" >

          <table id="orderedittable" class="table table-bordered">
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
                                <select id="ddlEstado"  style="width: 250px">
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

              <tr id="Merma"  class="ui-field-contain" >
                  
                   <td>Motivo Merma</td>
               <td class="cell">
                                <select id="ddlMerma"  style="width: 250px">
                                </select>
                            </td>
                    </tr>
						<tr id="Blck_1">
                            <td id ="lbl_1" style="width: 250px">PH Antes Paste </td>
                            <td class="cell">
                                 <input id="text_1" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_2">
                            <td id ="lbl_2" style="width: 250px">PH Despues Paste </td>
                            <td class="cell">
                                 <input id="text_2" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_3">
                            <td id ="lbl_3" style="width: 250px">BRIX Antes Paste </td>
                            <td class="cell">
                                 <input id="text_3" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_4">
                            <td id ="lbl_4" style="width: 250px">BRIX Despues Paste </td>
                            <td class="cell">
                                 <input id="text_4" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_5">
                            <td id ="lbl_5" style="width: 250px">HUMEDAD </td>
                            <td class="cell">
                                 <input id="text_5" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_6">
                            <td id ="lbl_6" style="width: 250px">ES </td>
                            <td class="cell">
                                 <input id="text_6" type="text" value="" class="form-control" style="width: 250px" onblur="ES_HR()"/>
                            </td>
                        </tr>
						<tr id="Blck_7">
                            <td id ="lbl_7" style="width: 250px">HC </td>
                            <td class="cell">
                                 <input id="text_7" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_8">
                            <td id ="lbl_8" style="width: 250px">Sacarosa </td>
                            <td class="cell">
                                 <input id="text_8" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_9">
                            <td id ="lbl_9"  style="width: 250px">Grasa </td>
                            <td class="cell">
                                 <input id="text_9" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_10">
                            <td id ="lbl_10"  style="width: 250px">Proteina </td>
                            <td class="cell">
                                 <input id="text_10" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_11">
                            <td id ="lbl_11" style="width: 250px">Lactosa </td>
                            <td class="cell">
                                 <input id="text_11" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_12">
                            <td id ="lbl_12" style="width: 250px">Temperatura </td>
                            <td class="cell">
                                 <input id="text_12" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_13">
                            <td id ="lbl_13">Ph </td>
                            <td class="cell">
                                 <input id="text_13" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr >
                         <tr id="Blck_14" >
                            <td id ="lbl_14"  style="width: 250px">Color </td>
                            <td class="cell">
                                 <input id="text_14" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_15">
                            <td id ="lbl_15" style="width: 250px">Sabor </td>
                            <td class="cell">
                                 <input id="text_15" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                         <tr id="Blck_16">
                            <td id ="lbl_16" style="width: 250px">Corte </td>
                            <td class="cell">
                                 <input id="text_16" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_17">
                            <td id ="lbl_17" style="width: 250px">Film </td>
                            <td class="cell">
                                 <input id="text_17" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_18">
                            <td id ="lbl_18" style="width: 250px">Cata </td>
                            <td class="cell">
                                 <input id="text_18" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_19">
                            <td id ="lbl_19" style="width: 250px">Gluten </td>
                            <td class="cell">
                                 <input id="text_19" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                        <tr id="Blck_20">
                            <td id ="lbl_20" style="width: 250px">Caseina </td>
                            <td class="cell">
                                 <input id="text_20" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_21">
                            <td id ="lbl_21" style="width: 250px">Listeria </td>
                            <td class="cell">
                                 <input id="text_21" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_22">
                            <td id ="lbl_22" style="width: 250px">Salmonela </td>
                            <td class="cell">
                                 <input id="text_22" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
						<tr id="Blck_23">
                            <td id ="lbl_23" style="width: 250px">PPC </td>
                            <td class="cell">
                                 <input id="text_23" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
               <tr id="Blck_24">
                            <td id ="lbl_24" style="width: 250px">Boletin </td>
                            <td class="cell">
                                 <input id="text_24" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>        
              <tr id="Blck_25">
                            <td id ="lbl_25" style="width: 250px">Brix </td>
                            <td class="cell">
                                 <input id="text_25" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_26">
                            <td id ="lbl_26" style="width: 250px">Impurezas </td>
                            <td class="cell">
                                 <input id="text_26" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>

              <tr id="Blck_27">
                            <td id ="lbl_27" style="width: 250px">CONSISTENCIA </td>
                            <td class="cell">
                                 <input id="text_27" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_28">
                            <td id ="lbl_28" style="width: 250px">OLOR </td>
                            <td class="cell">
                                 <input id="text_28" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_29">
                            <td id ="lbl_29" style="width: 250px">EMULSION </td>
                            <td class="cell">
                                 <input id="text_29" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_30">
                            <td id ="lbl_30" style="width: 250px">HR </td>
                            <td class="cell">
                                 <input id="text_30" type="text" class="form-control" value="" style="width: 250px" onblur="HR_ES()"/>
                            </td>
                        </tr>
                <tr id="Blck_31">
                            <td id ="lbl_31" style="width: 250px">TS </td>
                            <td class="cell">
                                 <input id="text_31" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_32">
                            <td id ="lbl_32" style="width: 250px">SNF </td>
                            <td class="cell">
                                 <input id="text_32" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_33">
                            <td id ="lbl_33" style="width: 250px">ASPECTO </td>
                            <td class="cell">
                                 <input id="text_33" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_34">
                            <td id ="lbl_34" style="width: 250px">DORNIC </td>
                            <td class="cell">
                                 <input id="text_34" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_35">
                            <td id ="lbl_35" style="width: 250px">INHIBIDORES </td>
                            <td class="cell">
                                 <input id="text_35" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_36">
                            <td id ="lbl_36" style="width: 250px">SILO </td>
                            <td class="cell">
                                 <input id="text_36" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_37">
                            <td id ="lbl_37" style="width: 250px">TTRANSP </td>
                            <td class="cell">
                                 <input id="text_37" type="text"class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_38">
                            <td id ="lbl_38" style="width: 250px">LAVADO </td>
                            <td class="cell">
                                 <input id="text_38" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_39">
                            <td id ="lbl_39" style="width: 250px">SILODEST </td>
                            <td class="cell">
                                 <input id="text_39" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
               <tr id="Blck_40">
                            <td id ="lbl_40" style="width: 250px">EDAD_LECHE </td>
                            <td class="cell">
                                 <input id="text_40" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
               <tr id="Blck_41">
                            <td id ="lbl_41" style="width: 250px">T_CISTERNA </td>
                            <td class="cell">
                                 <input id="text_41" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_42">
                            <td id ="lbl_42" style="width: 250px">T_MUESTRA </td>
                            <td class="cell">
                                 <input id="text_42" type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_43">
                            <td id ="lbl_43" style="width: 250px">INH_R </td>
                            <td class="cell">
                                 <input id="text_43" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_44">
                            <td id ="lbl_44" style="width: 250px">INH_L </td>
                            <td class="cell">
                                 <input id="text_44" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_45">
                            <td id ="lbl_45" style="width: 250px">D </td>
                            <td class="cell">
                                 <input id="text_45" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_46">
                            <td id ="lbl_46" style="width: 250px">ALCOHOL </td>
                            <td class="cell">
                                 <input id="text_46" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_47">
                            <td id ="lbl_47" style="width: 250px">C_FILTRO </td>
                            <td class="cell">
                                 <input id="text_47" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_48">
                            <td id ="lbl_48" style="width: 250px">INH_BOB </td>
                            <td class="cell">
                                 <input id="text_48" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_49">
                            <td id ="lbl_49" style="width: 250px">FLATOXINA </td>
                            <td class="cell">
                                 <input id="text_49" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                        </tr>
                <tr id="Blck_50">
                            <td id ="lbl_50" style="width: 250px">CONTROL_PESO_CALIDAD </td>
                            <td class="cell">
                                 <input id="text_50" type="text" class="form-control"  value="" style="width: 250px"/>
                            </td>
                        </tr>
               <tr id="Blck_51">
                            <td id ="lbl_51" style="width: 250px">CONTROL_PESO_LINEA </td>
                            <td class="cell">
                                 <input id="text_51" class="form-control"  type="text" value="" style="width: 250px"/>
                            </td>
                        </tr>
              <tr id="Blck_52">

                            <td id ="lbl_52" style="width: 250px">CURVA_PH </td>
                            <td class="cell">
                                 <input id="text_52" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              
                             <% if (Roles.IsUserInRole(HttpContext.Current.User.Identity.GetUserName(), "ADMIN"))
                                                                                    { %>  
              <tr id="Blck_53">
                         <td id ="td_Comite" colspan="2">
                             <table id ="table comite"  style="margin: auto;background-color:hsl(0, 0%, 20%)">
                                   <tr id="bloques_Comite"><td colspan="3" style="text-align:center">
                                       Cata comité
                                                           </td></tr>
                                   <tr id="Blck_CC">
                            <td id ="lbl_CCa" style="width: 100px"> </td>
                           <td id ="lbl_CCb" style="width: 100px">Hora muestra </td>
                           <td id ="lbl_CCc" style="width: 250px">Resultado Muestra </td>
                                         </tr>
                                   <tr id="Blck_54">
                            <td id ="lbl_53a" style="width: 150px">Arriba </td>
                            <td class="cell">
                                 <input id="text_H_arriba" type="text" class="form-control" value="" style="width: 75px"/>
                            </td>
                                           <td class="cell">
                                               <input id="txt_E_arriba" type="hidden" style="width: 46px" value="" />
                                               <input id="text_E_arriba_A" type="checkbox" onclick="check_Arriba_A();">Incorrecto
                                               <input id="text_E_arriba_B" type="checkbox"  onclick="check_Arriba_B();">Correcto
                            </td>
                                         </tr>
                                   <tr id="Blck_55">          
                  <td id ="lbl_54" style="width: 250px">Medio </td>
                            <td class="cell">
                                 <input id="text_H_medio"  class="form-control" type="text" value="" style="width: 75px"/>
                            </td>
                            <td class="cell">
                                <input id="txt_E_medio" type="hidden" style="width: 46px" value="" />
                                               <input id="text_E_medio_A" type="checkbox" onclick="check_Medio_A();">Incorrecto
                                               <input id="text_E_medio_B" type="checkbox"  onclick="check_Medio_B();">Correcto
                            </td>
                               </tr>
                                   <tr id="Blck_56">   
                  <td id ="lbl_55" style="width: 100px">Bajo </td>
                            <td class="cell">
                                 <input id="text_H_abajo" class="form-control" type="text" value="" style="width: 75px"/>
                            </td>
                            <td class="cell">
                                 <input id="text_E_abajo" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_abajo_A" type="checkbox" onclick="check_Abajo_A();">Incorrecto
                                               <input id="text_E_abajo_B" type="checkbox"  onclick="check_Abajo_B();">Correcto

                            </td>
                               
</tr>
                                   <tr id="Blck_57">   
                  <td id ="lbl_56" style="width: 250px">Resultado comité </td>
                           
                            <td class="cell">
                                 <input id="text_E" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_A" type="checkbox" onclick="check_A();">Incorrecto
                                 <input id="text_E_B"  type="checkbox"  onclick="check_B();">Correcto

                            </td>
                               
</tr>
                                 <tr id="Obsevaciones_Comite"><td id ="lbl_comite">Observaciones Comité </td>
                            <td class="cell" colspan="2">
                                 <TextArea id="text_observa_comite" rows="3" class="auto-style1"> </TextArea>
                            </td></tr>
                            </table> 
                          </td>
                      </tr>
               <% }  %>
               <tr id="Blck_0">
                            <td id ="lbl_0">ObsEstado </td>
                            <td class="cell">
                                 <TextArea id="text_0"  rows="3"  class="auto-style1"> </TextArea>
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
              <tr id="blck_ID2" style="display: none"> <td> <input id="Texto_est" type="hidden"  value="" style="width: 250px"/>  </td>  </tr>
              </table>       
 </div>
</asp:Content>
