<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Organoleptico_new.aspx.cs" Inherits="rinya_app.Calidad.Organoleptico_new" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
   
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
      <link href="../Scripts/jquery-ui/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
    <link href="../Content/DataTables/css/buttons.dataTables.css"  rel="stylesheet" type="text/css" />
    <link href="../Content/dataTables.jqueryui.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-ui/jquery-ui.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <script src="../Scripts/DataTables/jszip.min.js"></script>
    <script src="../Scripts/jquery-jtemplates.js"></script>
    <script src="../Scripts/DataTables/dataTables.buttons.min.js" ></script>
    <script src="../Scripts/DataTables/buttons.html5.js"></script>
   
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
       function prote_dil () {
           var _prote = parseFloat($('#text_10').val());
           var _prote_dil = 9 * _prote;
           $('#text_62').val(_prote_dil);
       }
       function dil_prote() {
           var _prote_dil = parseFloat($('#text_62').val());
           var _prote =  _prote_dil/9;
           $('#text_10').val(_prote);
       }
       function grasa_dil() {
           var _gras = parseFloat($('#text_9').val());
           var _gras_dil = 9 * _gras;
           $('#text_63').val(_gras_dil);
       }
       function dil_grasa() {
           var  _gras_dil= parseFloat($('#text_63').val());
           var _gras = 9 * _gras_dil;
           $('#text_9').val(_gras);
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
               url: "WebService_Organoleptico.asmx/GetCaracteristicas",
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
               url: "WebService_Organoleptico.asmx/GetEstados",
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
               url: "WebService_Organoleptico.asmx/GetMotivoMerma",
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


       function loadTable( tipo) {
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
                    {
                        text: 'Excel',
                       
                        action: function (e, dt, node, config) {
                          
                $.ajax({
                    type: "POST",
                    url: "WebService_Organoleptico.asmx/GetDocument",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var res = msg.d;
                        if (res == true) {
                            window.location.href = 'documentos/Organoleptico.xlsx';
                        }
                   
                   
                    },
                    error: function (err) {
                        alert(err.d);
                    }
                });
                        }
                    }
               
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
                   "deferRender": true,
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
                           "targets": [6],
                           "className": "none"
                       },
                        {
                            "targets": [7],
                            "className": "none"
                        },
                        {
                            "targets": [11],
                            "className": "none"
                        },
                        {
                           "targets": [13],
                           "className": "none"
                        },
                        {
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
				   }, {
				       "targets": [36],
				       "className": "none"
				   }, {
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
                        "targets": [63],
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
                        "className": "none"
                    }, {
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
                       
                       'searchable': false,
                    'orderable': false,
                    'render': function (data, type, full, meta) {

                        var check = parseFloat(data);

                        if (check == 0) {
                            return '<input type="checkbox" id="Vmin_chk" checked value="' + $('<div/>').text(data).html() + '"> Incorrecto';
                        } else if (check == 1) {
                            return '<input type="checkbox" id="Vmin_chk"  value="' + $('<div/>').text(data).html() + '"> Correcto';
                        }
                        else {
                            return $('<div/>').text(data).html();
                        }
                    }
              
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
                       "targets": [78],
                       "className": "none"
                   },
                   {
                       "targets": [79],
                       "className": "none"
                   },
                    {
                        "targets": [80],
                        "className": "none"
                    },
                    {
                        "targets": [81],
                        "className": "none"
                    },
                     {
                         "targets": [82],
                         "className": "none"
                     },
                 
                  {
                      "targets": [84],
                      "className": "none"
                  },
                  {
                      "targets": [85],
                      "className": "none"
                  },
                  {
                      "targets": [86],
                      "className": "none"
                  }, {
                      "targets": [87],
                      "className": "none"
                  },
                  {
                      "targets": [88],
                      "className": "none"
                  }, {
                      "targets": [89],
                      "className": "none"
                  },
                  {
                      "targets": [90],
                      "className": "none"
                  },
                  {
                      "targets": [91],
                      "className": "none"
                  },
                  {
                      "targets": [92],
                      "className": "none"
                  },
                  {
                      "targets": [93],
                      "className": "none"
                  },
                  {
                      "targets": [94],
                      "className": "none"
                  },
                  {
                      "targets": [95],
                      "className": "none"
                  }, 
                  {
                      "targets": [96],
                      "className": "none"
                  },
                  
                  {
                      "targets": [97],
                      "className": "none"
                  },
                  {
                      "targets": [98],
                      "className": "none"
                  },
                  {
                      "targets": [99],
                      "className": "none"
                  },
                  
                  {
                      "targets": [100],
                      "className": "none"
                  },
                   {
                       "targets": [101],
                       "className": "none"
                   },
                    {
                        "targets": [102],
                        "className": "none"
                    },
                    {
                        "targets": [103],
                        "className": "none"
                    },
                    
                    {
                        "targets": [104],
                        "className": "none"
                    },
                   

                    {
                        "targets": [105],
                        "className": "none"
                    },

                    {
                        "targets": [106],
                        "className": "none"
                    },
                     {
                         "targets": [107],
                         "className": "none"
                     },
                      {
                          "targets": [108],
                          "className": "none"
                      },
               {
                   "aTargets": [8],
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
               "sAjaxSource": "WebService_Organoleptico.asmx/GetTableData",
               
               "fnServerData": function (sSource, aoData, fnCallback) {
                   aoData.push({ "name": "fecha1", "value": $("#datepicker1").val() });
                   aoData.push({ "name": "fecha2", "value": $("#datepicker2").val() });
                   aoData.push({ "name": "tipo", "value": tipo });
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
              C_Datos.FECHA_SSCC_CON = $(nTds[5]).text();
              C_Datos.UD_ACTUAL = $(nTds[6]).text();
              C_Datos.KG_ACTUAL = $(nTds[7]).text();
              C_Datos.ESTADO = $(nTds[8]).text();
              C_Datos.SSCC = $(nTds[9]).text();
              C_Datos.ID_LOTE = $(nTds[11]).text();
              C_Datos.ID_ESTADO = $(nTds[84]).text();
              C_Datos.OBS_ESTADO = $(nTds[10]).text();
              $('#blck_ID').hide();
              $('#blck_ID_LOTE').hide();
              C_Datos.MERMA = $(nTds[65]).text();
              for (var j = 1; j <= 82; j++){
                  $('#Blck_' + j).hide();
                 
              }
              $('#table-dialogo').dialog({
                  autoOpen: true,
                  modal: true,
                  resizable: false,
                  title: 'Modificar',
                  width: 900,
                  heigth: 490,
                 

                  open: function (event, ui) {
                      if (C_Datos.ARTICULO.length > 0) {
                          $('#Texto_articulo').text($(nTds[1]).text() + ' | ' + $(nTds[2]).text());
                          $('#Texto_matricula').text($(nTds[9]).text());
                          $('#Texto_Lote_Interno').text($(nTds[3]).text());
                          $('#Texto_Fecha_Caducidad').text($(nTds[14]).text());
                          $('#Texto_ID_LOTE').text($(nTds[11]).text());
                          $('#Texto_ID').text($(nTds[0]).text());
                          $('#text_0').val($(nTds[10]).text());
                          var temp =parseFloat($(nTds[26]).text().split(",").join("."));
                          var estado_com = parseFloat($(nTds[73]).children('#Vmin_chk').val());
                          $('#text_E').val(estado_com);
                          // $('#content').css('background-color', '#FF4000');
                          var Usuario = $('#Usuario').val();
                          if (Usuario=='1'){
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
                              
                              $('#text_observa_comite').val($(nTds[80]).text());
                              $('#text_H_arriba').val($(nTds[74]).text());
                              $('#text_O_arriba').val($(nTds[81]).text());
                              var arriba = $(nTds[75]).text();
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
                              $('#text_H_medio').val($(nTds[76]).text());
                              $('#text_O_medio').val($(nTds[82]).text());
                              var medio = $(nTds[77]).text();
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

                              $('#text_H_abajo').val($(nTds[78]).text());
                              $('#text_O_abajo').val($(nTds[83]).text());
                              var abajo = $(nTds[79]).text();
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
                              for (var j = 1; j <= 82; j++) {
                                  $('#Blck_' + j).hide();

                              }
                          }

                          $('#Blck_0').show();
                        //  loadComboBox('ddlEstado', 'GetEstados', C_Datos.ID_ESTADO);
                          //$("#ddlEstado option:selected").val(C_Datos.ID_ESTADO);
                          $('#ddlEstado').val(C_Datos.ID_ESTADO);
                         // $("#ddlEstado option:selected").text(C_Datos.ESTADO);
                          $('#ddlEstado').selectmenu().selectmenu('refresh', true).addClass("overflow");
                        
                          $('#ddlMerma').val(C_Datos.MERMA);
                          $('#ddlMerma').selectmenu().selectmenu("menuWidget").addClass("overflow");
                         
                          if (C_Datos.ID_ESTADO == 4 || C_Datos.ID_ESTADO == 7 || C_Datos.ID_ESTADO == 8)
                          {
                              
                              //$('#ddlMerma').selectmenu().selectmenu('selectmenuchange');
                              $('#ddlMerma').selectmenu().selectmenu('refresh', true);
                          $('#Merma').show();
                          } else { $('#Merma').hide();  }
                          

                        /*  $('#ddlEstado').selectmenu().selectmenu('change', function (event, data) {
                              alert(data.item.value);
                          });*/
                          $('#ddlEstado').on('selectmenuchange', function (e, ui) {
                            
                              if (ui.item.value == 4 || ui.item.value == 7 || ui.item.value == 8) {
                                  $('#ddlMerma').selectmenu().selectmenu('refresh', true);
                                  $('#Merma').show();
                              } else {
                                 // $('#ddlMerma option:selected').val("0");
                                  $('#Merma').hide();
                              }
                            
                             
                          });
                         

                         // $('#ddlEstado').selectmenu().selectmenu('change', cambiarvalor($('#ddlEstado').val));
                          
                          $.ajax({
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              url: "WebService_Organoleptico.asmx/GetCaracteristicas_articulo",
                              data: '{datos: ' + JSON.stringify(C_Datos) + '}',
                              dataType: "json",

                              success: function (data) {

                                  var dato = JSON.parse(data.d);
                                  if (dato.length > 0)
                                      for (var i = 0; i < dato.length; i++) {
                                          
                                          var valor2 = dato[i].Caracteristica;
                                          var tipo_dato = dato[i].Tipo_Dato
                                          var val = 14;
                                          if (valor2 >= 70) {
                                              var valor = valor2 - 4;
                                              
                                          } else
                                          {
                                              var valor = valor2;

                                          }
                                          
                                          if (valor >= 36)
                                          {
                                              val = 15;
                                          }
                                          if (valor >= 50) {
                                              val = 16;
                                          }
                                          if (valor >= 55) {
                                              val = 13;
                                          }
                                          if (valor >= 60) {
                                              val = 25;
                                          }
                                          var texto_lbl = $('#lbl_' + valor).text();
                                          var pr = val + valor;
                                         
                                          
                                          $('#lbl_' + valor).text(dato[i].lbl);
                                          if (valor == 64 || valor == 65) {
                                              $('#text_' + valor).text($(nTds[val + valor]).text());
                                          }else if (tipo_dato==1){
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
                                          
                                          if(valor ==12){
                                          if (temp>8) 
                                              $('#text_12').css('background-color', '#FF0000');
                                          else
                                              $('#text_12').css('background-color', '#00FF00');
                                          } 
                                          
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
                          if (document.getElementById("userAuth").value == "False")
                          {
                              alert("No se va a guardar por no estar autentificado!!");
                          }
                       else{
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
                              C_Datos.TEXTURA = $('#text_53').val();
                              C_Datos.ABSORVANCIA = $('#text_54').val();
                              C_Datos.WEB = $('#text_58').val();
                              C_Datos.CAG = $('#text_59').val();
                              C_Datos.SILO_DESTINO = $('#text_60').val();
                              C_Datos.CROMATOGRAMAS_FILM = $('#text_61').val();
                              C_Datos.PROTEINA_DILUCION = $('#text_62').val();
                              C_Datos.GRASA_DILUCION = $('#text_63').val();
                              C_Datos.TTP = '';
                              C_Datos.TPP = '';
                              C_Datos.SABOR_MERENGUE = $('#text_66').val();
                              C_Datos.OLOR_TORTILLA = $('#text_67').val();
                              C_Datos.SABOR_TORTILLA = $('#text_68').val();
                              C_Datos.Espectofotometro = $('#text_69').val();
                              C_Datos.BOLETIN_GRASA = $('#text_70').val();
                              C_Datos.BOLETIN_PROTEINA = $('#text_71').val();
                              C_Datos.BRIX_PASTE_2500 = $('#text_72').val();
                              C_Datos.BRIX_PASTE_6000 = $('#text_73').val();
                              C_Datos.GLUTEN_BOLETIN = $('#text_74').val();
                              C_Datos.BOLETIN_LISTERIA = $('#text_75').val();
                              C_Datos.BOLETIN_SALMONELLA = $('#text_76').val();
                              C_Datos.PRE_BOLETIN_FQ = $('#text_77').val();
                              C_Datos.BOLETIN_SOJA = $('#text_78').val();
                              C_Datos.BOLETIN_CARAMELO = $('#text_79').val();
                              C_Datos.ETIQUETA_CORRECTA = $('#text_80').val();
                              C_Datos.CURVA_FERMENTACION = $('#text_81').val();
                              C_Datos.ATP_APERTURA = $('#text_82').val();
                              var Usuario = $('#Usuario').val();
                              if (Usuario == '100') {
                                  C_Datos.ESTADO_COMITE = $('#text_E').val();
                                  C_Datos.HORA_ARRIBA = $('#text_H_arriba').val();
                                  C_Datos.ESTADO_ARRIBA = $('#txt_E_arriba').val();
                                  C_Datos.HORA_MEDIO = $('#text_H_medio').val();
                                  C_Datos.ESTADO_MEDIO = $('#txt_E_medio').val();
                                  C_Datos.HORA_ABAJO = $('#text_H_abajo').val();
                                  C_Datos.ESTADO_ABAJO = $('#text_E_abajo').val();
                                  C_Datos.OBSEVACIONES_COMITE = $('#text_observa_comite').val();
                                  C_Datos.OBSERVA_ARRIBA = $('#text_O_arriba').val();
                                  C_Datos.OBSERVA_MEDIO = $('#text_O_medio').val();
                                  C_Datos.OBSERVA_ABAJO = $('#text_O_abajo').val();
                              } else {
                                  C_Datos.ESTADO_COMITE = '';
                                  C_Datos.HORA_ARRIBA = '';
                                  C_Datos.ESTADO_ARRIBA = '';
                                  C_Datos.HORA_MEDIO = '';
                                  C_Datos.ESTADO_MEDIO = '';
                                  C_Datos.HORA_ABAJO = '';
                                  C_Datos.ESTADO_ABAJO = '';
                                  C_Datos.OBSEVACIONES_COMITE = '';
                                  C_Datos.OBSERVA_ARRIBA = '';
                                  C_Datos.OBSERVA_MEDIO = '';
                                  C_Datos.OBSERVA_ABAJO = '';
                              }
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
                                          url: "WebService_Organoleptico.asmx/update_Data",
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
               url: "WebService_Organoleptico.asmx/" + methodname,
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
           $('#btnTodas').click(loadTable('1'));
        
          
       });
    

   </script>

    <h1><%: Title %></h1>

      <div>
    <table>
        <tr>

          <th>
<p>Desde: <input type="text" id="datepicker1" class="form-control" style="width: 98px"></p>
              </th>
<th> <p>Hasta: <input type="text" id="datepicker2" class="form-control" style="width: 98px"></p></th>
             <th>&nbsp;&nbsp; <p>&nbsp;&nbsp;<input id="btnToda" type="button" Value="Obtener Matriculas" class="btn btn-default" onclick="loadTable('1');"/></p></th>
           <th> &nbsp;&nbsp;<p>&nbsp;&nbsp;<input id="btnFormula" type="button" Value="Obtener Formulas" class="btn btn-default" onclick="loadTable('2');"/> </p></th>
             <th> &nbsp;&nbsp;<p>&nbsp;&nbsp;<input id="btnventa" type="button" Value="Obtener Articulo" class="btn btn-default" onclick="loadTable('3');"/> </p></th>
             <th> &nbsp;&nbsp;<p>&nbsp;&nbsp;<input id="btnMprima" type="button" Value="Obtener M. Prima" class="btn btn-default" onclick="loadTable('4');"/> </p></th>
     
            </tr>

       
    </table>
   
        </div>
    <p></p>
    <p></p>
    <table id="tblData" class="dataTable"  data-searching="true" style="display: none" >
      <thead>
             <tr>
                    <th>ID</th>
                    <th>ART</th>
                    <th>DESCRIPCION</th>
                    <th>LOTE</th>
                    <th>Nº</th>
				    <th>F.CREA</th>
				    <th>UD_ACTUAL</th>
				    <th>KG_ACTUAL</th>
				    <th>ESTADO</th>
				    <th>SSCC</th>
				    <th>&nbsp;&nbsp;OBS&nbsp;ESTADO&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
				    <th>ID_LOTE</th>
				    <th>F.FAB</th>
				    <th>HORA</th>
				    <th>F.CAD</th>
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
                    <th>INH_BOB</th>
                    <th>FLATOXINA</th>
                    <th>MERMA</th>
                    <th>CONTROL_PESO_CALIDAD</th>
                    <th>CONTROL_PESO_LINEA</th>
                    <th>CURVA_PH</th>
                    <th>TEXTURA</th>
                    <th>ABSORVANCIA</th>
                    <th>WEB</th>
                    <th>CAG</th>
                    <th>ESTADO COMITE</th>
                    <th>HORA_ARRIBA</th>
                    <th>ESTADO_ARRIBA</th>
                  <th>OBS_ARRIBA</th>
                    <th>HORA_MEDIO</th>
                    <th>ESTADO_MEDIO</th>
                 <th>OBS_MEDIO</th>
                    <th>HORA_ABAJO</th>
                    <th>ESTADO_ABAJO</th>
                  <th>OBS_ABAJO</th>
                    <th>OBS COMITE</th>
                     <th>ID_ESTADO</th>
                  <th>SILO_DESTINO</th>
                    <th>CROMATOGRAMAS_FILM</th>
                 <th>PROTEINA_DILUCION</th>
                    <th>GRASA_DILUCION</th>
                  <th>TTP</th>
                    <th>TPP</th>
                <th>SABOR_MERENGUE<th>
                  <th>OLOR_TORTILLA</th>
                    <th>SABOR_TORTILLA</th>
                 <th>Espectofotometro</th>
                  <th>BOLETIN_GRASA</th>
                 <th>BOLETIN_PROTEINA</th>
                 <th>BRIX_PASTE_2500</th>
                 <th>BRIX_PASTE_6000</th>
                <th> GLUTEN_BOLETIN</th>
                 <th> BOLETIN_LISTERIA</th>
                 <th> BOLETIN_SALMONELLA</th>
                 <th> PRE_BOLETIN_FQ</th>
                 <th> BOLETIN_SOJA</th>
                 <th> BOLETIN_CARAMELO</th>
                 <th> ETIQUETA_CORRECTA</th>
                 <th> CURVA_FERMENTACION</th>
                 <th> ATP_APERTURA</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot  style="text-align:center">
            <tr  style="text-align:center ; padding:100px">
                  <th ></th>
                  <th class="tfoot_Enter">ART</th>
                 <th class="tfoot_search">DESCRIPCION</th>
				  <th class="tfoot_Enter">LOTE</th>
				  <th class="tfoot_Enter">Nº</th>
				  <th class="tfoot_Enter">F.CREA</th>
                  <th></th>
				  <th></th>
				  <th class="tfoot_search">ESTADO</th>
				  <th class="tfoot_Enter">SSCC</th>
				  <th></th>
				  <th></th>
				  <th class="tfoot_search">F.FAB</th>
				  <th ></th>
				  <th class="tfoot_search">F.CAD</th>
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
                 <th>NumPalet</th>
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
                                 <input id="text_9" type="text" class="form-control" value="" style="width: 250px" onblur="grasa_dil()"/>
                            </td>
                        </tr>
						<tr id="Blck_10">
                            <td id ="lbl_10"  style="width: 250px">Proteina </td>
                            <td class="cell">
                                 <input id="text_10" type="text" class="form-control" value="" style="width: 250px" onblur="prote_dil()"/>
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
                                 <input id="text_38" type="text" class="form-control"  value="" style="width: 250px"/>
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
                                 <input id="text_40" type="text" class="form-control" value="" style="width: 250px"/>
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
               <tr id="Blck_53">

                            <td id ="lbl_53" style="width: 250px">TEXTURA </td>
                            <td class="cell">
                                 <input id="text_53" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
               <tr id="Blck_54">

                            <td id ="lbl_54" style="width: 250px">ABSORVANCIA </td>
                            <td class="cell">
                                 <input id="text_54" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_58">

                            <td id ="lbl_58" style="width: 250px">WEB </td>
                            <td class="cell">
                                 <input id="text_58" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_59">

                            <td id ="lbl_59" style="width: 250px">CAG </td>
                            <td class="cell">
                                 <input id="text_59" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_60">

                            <td id ="lbl_60" style="width: 250px">SILO_DESTINO</td>
                            <td class="cell">
                                 <input id="text_60" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_61">

                            <td id ="lbl_61" style="width: 250px">CROMATOGRAMAS_FILM</td>
                            <td class="cell">
                                 <input id="text_61" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
                 <tr id="Blck_62">

                            <td id ="lbl_62" style="width: 250px">Proteina Dilucion</td>
                            <td class="cell">
                                 <input id="text_62" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_63">

                            <td id ="lbl_63" style="width: 250px">Grasa Dilucion</td>
                            <td class="cell">
                                 <input id="text_63" class="form-control" type="text" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_64">

                            <td id ="lbl_64" style="width: 250px">TTP</td>
                            <td id="text_64" style="width: 250px">1234 </td> 
                           
                       </tr>
              <tr id="Blck_65">

                            <td id ="lbl_65" style="width: 250px">TPP</td>
                                <td id="text_65" style="width: 250px">1234 </td> 
                            
                       </tr>
                <tr id="Blck_66">

                            <td id ="lbl_66" style="width: 250px">Sabor Merengue</td>   
                            <td class="cell">
                                 <input id="text_66" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
                      <tr id="Blck_67">

                            <td id ="lbl_67" style="width: 250px">Olor Tortilla</td>   
                            <td class="cell">
                                 <input id="text_67" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
                <tr id="Blck_68">

                            <td id ="lbl_68" style="width: 250px">Sabor Tortilla</td>   
                            <td class="cell">
                                 <input id="text_68" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_69">

                            <td id ="lbl_69" style="width: 250px">Espectofotometro</td>   
                            <td class="cell">
                                 <input id="text_69" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_70">

                            <td id ="lbl_70" style="width: 250px">BOLETIN_GRASA</td>   
                            <td class="cell">
                                 <input id="text_70" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
               <tr id="Blck_71">

                            <td id ="lbl_71" style="width: 250px">BOLETIN_PROTEINA</td>   
                            <td class="cell">
                                 <input id="text_71" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
               <tr id="Blck_72">

                            <td id ="lbl_72" style="width: 250px">BRIX_PASTE_2500</td>   
                            <td class="cell">
                                 <input id="text_72" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
               <tr id="Blck_73">

                            <td id ="lbl_73" style="width: 250px">BRIX_PASTE_6000</td>   
                            <td class="cell">
                                 <input id="text_73" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
               <tr id="Blck_74">

                            <td id ="lbl_74" style="width: 250px">GLUTEN_BOLETIN</td>   
                            <td class="cell">
                                 <input id="text_74" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
                <tr id="Blck_75">

                            <td id ="lbl_75" style="width: 250px">BOLETIN_LISTERIA</td>   
                            <td class="cell">
                                 <input id="text_75" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                       </tr>
              <tr id="Blck_76">

                            <td id ="lbl_76" style="width: 250px">BOLETIN_SALMONELLA</td>   
                            <td class="cell">
                                 <input id="text_76" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>  
              <tr id="Blck_77">

                            <td id ="lbl_77" style="width: 250px">PRE_BOLETIN_FQ</td>   
                            <td class="cell">
                                 <input id="text_77" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
               <tr id="Blck_78">

                            <td id ="lbl_78" style="width: 250px">BOLETIN_SOJA</td>   
                            <td class="cell">
                                 <input id="text_78" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
               <tr id="Blck_79">

                            <td id ="lbl_79" style="width: 250px">BOLETIN_CARAMELO</td>   
                            <td class="cell">
                                 <input id="text_79" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
              <tr id="Blck_80">

                            <td id ="lbl_80" style="width: 250px">ETIQUETA_CORRECTA</td>   
                            <td class="cell">
                                 <input id="text_80" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
              <tr id="Blck_81">

                            <td id ="lbl_81" style="width: 250px">CURVA_FERMENTACION</td>   
                            <td class="cell">
                                 <input id="text_81" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
              <tr id="Blck_82">

                            <td id ="lbl_82" style="width: 250px">ATP_APERTURA</td>   
                            <td class="cell">
                                 <input id="text_82" type="text" class="form-control" value="" style="width: 250px"/>
                            </td>
                </tr>
                             <% if (Roles.IsUserInRole(HttpContext.Current.User.Identity.GetUserName(), "COMITE_CALIDAD"))
                                              { %>  
              <tr id="c_Blck_55"> 
                         <td id ="td_Comite" colspan="2">
                            <table id ="table comite"  style="margin: auto;background-color:hsl(0, 0%, 20%)">
                                   <tr id="bloques_Comite"><td colspan="3" style="text-align:center">
                                       Cata comité
                                                           </td></tr>
                                   <tr id="Blck_CC">
                            <td id ="lbl_CCa" style="width: 100px"> </td>
                           <td id ="lbl_CCb" style="width: 100px">Hora muestra </td>
                           <td id ="lbl_CCc" style="width: 250px">Resultado Muestra </td>
                            <td id ="lbl_CCd" style="width: 250px">Observaciones Muestra </td>
                                         </tr>
                                   <tr id="c_Blck_56">
                            <td id ="lbl_56a" style="width: 150px">Arriba </td>
                            <td class="cell">
                                 <input id="text_H_arriba" type="text" class="form-control" value="" style="width: 75px"/>
                            </td>
                                           <td class="cell">
                                               <input id="txt_E_arriba" type="hidden" style="width: 46px" value="" />
                                               <input id="text_E_arriba_A" type="checkbox" onclick="check_Arriba_A();">Incorrecto
                                               <input id="text_E_arriba_B" type="checkbox"  onclick="check_Arriba_B();">Correcto
                            </td>
                                        <td class="cell">
                                 <TextArea id="text_O_arriba"   rows="3" class="auto-style1" ></TextArea>
                            </td>
                                         </tr>
                                   <tr id="c_Blck_57">          
                  <td id ="lbl_57" style="width: 250px">Medio </td>
                            <td class="cell">
                                 <input id="text_H_medio"  class="form-control" type="text" value="" style="width: 75px"/>
                            </td>
                            <td class="cell">
                                <input id="txt_E_medio" type="hidden" style="width: 46px" value="" />
                                               <input id="text_E_medio_A" type="checkbox" onclick="check_Medio_A();">Incorrecto
                                               <input id="text_E_medio_B" type="checkbox"  onclick="check_Medio_B();">Correcto
                            </td>
                                <td class="cell">
                                 <TextArea id="text_O_medio"  rows="3"  class="auto-style1" ></TextArea>
                            </td>
                               </tr>
                                   <tr id="c_Blck_58">   
                  <td id ="c_lbl_58" style="width: 100px">Bajo </td>
                            <td class="cell">
                                 <input id="text_H_abajo"  class="form-control" type="text" value="" style="width: 75px"/>
                            </td>
                            <td class="cell">
                                 <input id="text_E_abajo" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_abajo_A" type="checkbox" onclick="check_Abajo_A();">Incorrecto
                                               <input id="text_E_abajo_B" type="checkbox"  onclick="check_Abajo_B();">Correcto

                            </td>
                                        <td class="cell">
                                 <TextArea id="text_O_abajo"   rows="3" class="auto-style1" ></TextArea>
                            </td>
                               
</tr>
                                   <tr id="c_Blck_59">   
                  <td id ="c_lbl_59" style="width: 250px">Resultado comité </td>
                           
                            <td class="cell">
                                 <input id="text_E" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_A" type="checkbox" onclick="check_A();">Incorrecto
                                 <input id="text_E_B"  type="checkbox"  onclick="check_B();">Correcto

                            </td>
                                  <td>   </td>    
                               
</tr>
                                 <tr id="Obsevaciones_Comite"><td id ="lbl_comite">Observaciones Comité </td>
                            <td class="cell" colspan="3">
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
          <input id="userAuth" type="hidden" value="<%=HttpContext.Current.User.Identity.IsAuthenticated %>" />      
         <input id="Usuario" type="hidden" style="width: 46px" value="<%  if (Roles.IsUserInRole(HttpContext.Current.User.Identity.GetUserName(), "ADMIN")) { Response.Write("1"); }
             else
             { Response.Write("0"); } %>"/>

 </div>

</asp:Content>
