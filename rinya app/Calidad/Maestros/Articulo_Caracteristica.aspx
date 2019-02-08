<%@ Page Title="Articulo y Caracteristicas" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Articulo_Caracteristica.aspx.cs" Inherits="rinya_app.Calidad.Maestros.Articulo_Caracteristica" %>

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
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css" rel="stylesheet" />
<script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>

   <script type="text/javascript"> 
       function check_prop(checkbox)
       {
           if (checkbox.checked) {
             
               $('#txt_min').val('1');
               $('#txt_max').val('1');
           } else {
               $('#txt_min').val('0');
               $('#txt_max').val('0');
               
           }
        }
       function nuevo() {
           load_combo2();
           load_combo3();
           $('#table-dialogo').dialog({
               autoOpen: true,
               modal: true,
               resizable: false,
               title: 'Nueva alta',
               width: 500,
               heigth: 250,

               open: function (event, ui) {

                   $('#Blc_id').hide();
                   $('#Blc_art').hide();
                   $('#Blc_txt_carac').hide();
                   $('#Blc_Combo_carac').show();
                   $('#Blc_Combo_art').show();
                   //$('#cmb_carac').selectmenu().selectmenu('refresh', true);
                   $('#txt_min').val("");
                   $('#txt_max').val("");
                   $('#text_obliga').prop('checked', false);
                   $('#text_lote').prop('checked', false);
                   $('#cmb_carac').select2().on("change", function (e) {

           var C_Maestros = {};
           C_Maestros.Cod_Caracteristica = $(this).select2('data')[0].id;
           // mostly used event, fired to the original element when the value changes
           // console.log(JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_articulo_caracteristicas.asmx/GetCaracteristicas2",
               data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
               dataType: "json",

               success: function (data) {
                   var Tipo_Datos = JSON.parse(data.d)[0].Tipo_Dato;
                   if (Tipo_Datos == 2) {
                       $('#blk_Max').hide();
                       $('#blk_Min').hide();
                       $('#blk_SN').show();

                   } else {
                       $('#blk_Max').show();
                       $('#blk_Min').show();
                       $('#blk_SN').hide();
                   }

               },
               error: function (request, status, error) {
                   alert(JSON.parse(request.responseText).Message);
               }
           });
           // log("change val=" + e.val);
       });

               },
               close: function (event, ui) {
                   $('#table-dialogo :text').val("");

               },
               buttons: {
                   "Guardar": function () {
                       var respuesta = confirm("Seguro que quieres añadir una nueva caracteristica?");
                       if (respuesta) {
                           var C_Maestros = {};
                           C_Maestros.Id = "";
                           C_Maestros.articulo = $('#cmb_art').val().toString();
                           C_Maestros.art_descripcion = "";
                           C_Maestros.Cod_Caracteristica = $('#cmb_carac').val().toString();
                           C_Maestros.Caracteristica = "";
                           C_Maestros.V_Max = $('#txt_max').val();
                           C_Maestros.V_Min = $('#txt_min').val();
                           if ($('#text_obliga').is(':checked')) {
                               C_Maestros.Obligatorio = "1";
                           } else {
                               C_Maestros.Obligatorio = "0";
                           }
                           if ($('#text_lote').is(':checked')) {
                               C_Maestros.A_lote = "1";
                           } else {
                               C_Maestros.A_lote = "0";
                           }


                           if (C_Maestros.Cod_Caracteristica.length > 0) {

                               $.ajax({
                                   type: "POST",
                                   url: "WebService_articulo_caracteristicas.asmx/New_Data",
                                   data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                   contentType: "application/json; charset=utf-8",
                                   dataType: "json",
                                   success: function (response) {
                                       //Success or failure message e.g. Record saved or not saved successfully
                                       loadComboBox('ddlArticulo', 'GetArticulos');
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
       function load_combo2()
       {
           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Cod_Caracteristica}'>{$T.record.Caracteristica}</option>\
                    {#/for}";

           var combo = $('#cmb_carac');

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_articulo_caracteristicas.asmx/GetCaracteristicas" ,
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
       function load_combo3() {
           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Articulo}'>{$T.record.Descripcion}</option>\
                    {#/for}";

           var combo = $('#cmb_art');

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_articulo_caracteristicas.asmx/Get_all_Articulos",
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
       function copia_articulo(){ 
           $("#table-dialogo2").attr("style", "display: table");

           $('#table-dialogo2').dialog({
               autoOpen: true,
               modal: true,
               resizable: false,
               title: 'Copiar',
               width: 400,
               heigth: 250,
               open: function (event, ui) {
                   $('#Texto_Art3').text($('#ddlArticulo').val().toString());
               },
               buttons: {
                   "Copiar": function () {
                       var respuesta = confirm("Seguro que quieres copiar las caracteristicas ?");
                       if (respuesta) {
                           var C_Maestros = {};
                           C_Maestros.articuloA = $('#Texto_Art3').text();
                           C_Maestros.articuloB = $('#txt_Art_C').val();
                        //   var valor = $('#ddlArticulo').val().toString();

                           $.ajax({
                               type: "POST",
                               contentType: "application/json; charset=utf-8",
                               url: "WebService_articulo_caracteristicas.asmx/copia_Articulo",
                               data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                               dataType: "json",

                               success: function (data) {
                                   var Tipo_Datos = data.d;
                                  if (Tipo_Datos == true) {
                                      alert("Articulo copiado con exito!!");
                                      $('#txt_Art_C').val("");

                                   } else {
                                      alert("Error en copia!!");
                                     
                                   }

                               },
                               error: function (request, status, error) {
                                   alert(JSON.parse(request.responseText).Message);
                               }
                           });
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

           function showDetails() {
               alert("showing some details");
           }
           var valor = $('#ddlArticulo').val().toString();
           $("#tblData").attr("style", "display: table");
           var table = $('#tblData').DataTable({

               dom: '<"H"B<lfr>>t<"F"ip>',
               jQueryUI: true,
               responsive: true,
               buttons: [
                   {
                       text: 'Nueva',
                       action: function (e, dt, node, config) {
                           load_combo2();
                           $('#table-dialogo').dialog({
                               autoOpen: true,
                               modal: true,
                               resizable: false,
                               title: 'Nueva alta',
                               width: 400,
                               heigth: 250,

                               open: function (event, ui) {

                                   $('#Blc_id').hide();
                                   $('#Blc_art').hide();
                                   $('#Blc_txt_carac').hide();
                                   $('#Blc_Combo_carac').show();
                                   $('#Blc_Combo_art').hide();
                                   //$('#cmb_carac').selectmenu().selectmenu('refresh', true);
                                   $('#txt_min').val("");
                                   $('#txt_max').val("");
                                   $('#text_obliga').prop('checked', false);
                                   $('#text_lote').prop('checked', false);
                                   $('#cmb_carac').select2()
       .on("change", function (e) {

           var C_Maestros = {};
           C_Maestros.Cod_Caracteristica = $(this).select2('data')[0].id;
           // mostly used event, fired to the original element when the value changes
          // console.log(JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_articulo_caracteristicas.asmx/GetCaracteristicas2",
               data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
               dataType: "json",

               success: function (data) {
                   var Tipo_Datos = JSON.parse(data.d)[0].Tipo_Dato;
                   if(Tipo_Datos==2) {
                           $('#blk_Max').hide();
                           $('#blk_Min').hide();
                           $('#blk_SN').show();
                           
                       } else {
                           $('#blk_Max').show();
                           $('#blk_Min').show();
                           $('#blk_SN').hide();
                       }

               },
               error: function (request, status, error) {
                   alert(JSON.parse(request.responseText).Message);
               }
           });
           // log("change val=" + e.val);
       });



                               },
                               close: function (event, ui) {
                                   $('#table-dialogo :text').val("");

                               },
                               buttons: {
                                   "Guardar": function () {
                                       var respuesta = confirm("Seguro que quieres añadir una nueva caracteristica?");
                                       if (respuesta) {
                                           var C_Maestros = {};
                                           C_Maestros.Id = "";
                                           C_Maestros.articulo = valor;
                                           C_Maestros.art_descripcion = "";
                                           C_Maestros.Cod_Caracteristica = $('#cmb_carac').val().toString();
                                           C_Maestros.Caracteristica = "";
                                           C_Maestros.V_Max = $('#txt_max').val();
                                           C_Maestros.V_Min = $('#txt_min').val();
                                           if ($('#text_obliga').is(':checked')) {
                                               C_Maestros.Obligatorio = "1";
                                           } else {
                                               C_Maestros.Obligatorio = "0";
                                           }
                                           if ($('#text_lote').is(':checked')) {
                                               C_Maestros.A_lote = "1";
                                           } else {
                                               C_Maestros.A_lote = "0";
                                           }
                                          
                                           if (C_Maestros.Cod_Caracteristica.length > 0) {

                                               $.ajax({
                                                   type: "POST",
                                                   url: "WebService_articulo_caracteristicas.asmx/New_Data",
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
                           });
                          }
                       // Splice the image in after the header, but before the table

                      
                   }, 'excelHtml5'
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
               "order": [[3, "asc"]],
               "info": true,
               "autoWidth": false,
               "sScrollY": "300px",
               "scrollX": true,
               "columnDefs": [
                {
                    "targets": [0],
                    "className" : "none"
                }, {
                    "targets": [1],
                    "className" : "none"
                },{
                    "targets": [2],
                    "className" : "none"
                },  {
                    "targets": [8],
                    "className": "none"
                }, {
                    "targets": [5],
                    'searchable': false,
                    'orderable': false,

                    'render': function (data, type, full, meta) {
                        var TDato =full[8]
                        var check = parseFloat(data);
                        if (TDato == 2){
                         if (check == 0 ) {
                                return '<input type="checkbox" id="Vmin_chk" value="' + $('<div/>').text(data).html() + '">';
                            } else {
                             return '<input type="checkbox" id="Vmin_chk" checked value="' + $('<div/>').text(data).html() + '">';
                            }
                        } else {
                            return $('<div/>').text(data).html();;
                        }
                    }
                }, {
                    "targets": [6],
                    'searchable': false,
                    'orderable': false,

                    'render': function (data, type, full, meta) {
                        var TDato = full[8]
                        var check = parseFloat(data);
                        if (TDato == 2) {
                            if (check == 0) {
                                return '<input type="checkbox" id="Vmax_chk" value="' + $('<div/>').text(data).html() + '">';
                            } else {
                                return '<input type="checkbox" id="Vmax_chk" checked value="' + $('<div/>').text(data).html() + '">';
                            }
                        }
                        else {
                            return $('<div/>').text(data).html();;
                        }
                    }
                },
                        {
                    "targets": [7],
                    'searchable':false,
                    'orderable':false,
                  
                    'render': function (data, type, full, meta) {
                        var check = parseInt(data);
                        if (check == 1) {
                            return '<input type="checkbox" id="obliga" checked value="' + $('<div/>').text(data).html() + '">';
                        } else
                        {
                            return '<input type="checkbox" id="obliga" value="' + $('<div/>').text(data).html() + '">';
                        }
                      
               }
                        },
           {
                    "targets": [9],
               'searchable':false,
               'orderable':false,
                  
               'render': function (data, type, full, meta) {
                   var check = parseInt(data);
                   if (check == 1) {
                       return '<input type="checkbox" id="A_Lote" checked value="' + $('<div/>').text(data).html() + '">';
                   } else
                   {
                       return '<input type="checkbox" id="A_Lote" value="' + $('<div/>').text(data).html() + '">';
                   }
                      
               }
           }],
               'bScrollCollapse': true,

               "bProcessing": true,
               "bServerSide": true,
               "sAjaxSource": "WebService_articulo_caracteristicas.asmx/GetTableData",
               "fnServerData": function (sSource, aoData, fnCallback) {
                   aoData.push({ "name": "roleId", "value": valor });
                   $.ajax({
                       "dataType": 'json',
                       "contentType": "application/json; charset=utf-8",
                       "type": "GET",
                       "url": sSource,
                       "data": aoData,
                       "success": function (msg) {
                           if (msg.d.length >0){
                           var json = jQuery.parseJSON(msg.d);
                           fnCallback(json);
                           $("#tblData").show();
                           } else
                           {
                               $("#tblData").hide();
                           }
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
               }
           });

           $("#tblData tbody").on("click", 'tr', function (e) {
               e.preventDefault();

               var nTds = $('td', this);

               var ID = $(nTds[0]).text();
               var articulo = $(nTds[1]).text();
               var art_descripcion = $(nTds[2]).text();
               var Carac = $(nTds[3]).text();
               var desc_car = $(nTds[4]).text();
               var V_Min = $(nTds[5]).text();
               var V_Max = $(nTds[6]).text();
               var Obligatorio = $(nTds[7]).children('#obliga').val();
               var A_lote = $(nTds[9]).children('#A_Lote').val();
               var Tipo_Dato = $(nTds[8]).text();
                   //blk_SN
               $("#table-dialogo").attr("style", "display: table");

               $('#table-dialogo').dialog({
                   autoOpen: true,
                   modal: true,
                   resizable: false,
                   title: 'Modificar',
                   width: 400,
                   heigth: 250,

                   open: function (event, ui) {
                       $('#Blc_Combo_art').hide();
                       $('#Blc_Combo_carac').hide();
                       $('#Texto_ID').text(ID.trim());
                       $('#Texto_Art').text(articulo.trim() + " " + art_descripcion.trim());
                       $('#Texto_carac').text(Carac.trim() + " " + desc_car.trim());
                       $('#txt_min').val(V_Min.trim());
                       $('#txt_max').val(V_Max.trim());
                       if (Tipo_Dato == 2) {
                           $('#blk_Max').hide();
                           $('#blk_Min').hide();
                           $('#blk_SN').show();
                           
                           var check = parseFloat($(nTds[5]).children('#Vmin_chk').val());
                           if (check == 0) {

                               $('#text_sn').prop('checked', false);
                               $('#text_sn').val(V_Min.trim());
                           } else {
                               $('#text_sn').prop('checked', true);
                               $('#text_sn').val(V_Min.trim());
                           }
                       } else {
                           $('#blk_Max').show();
                           $('#blk_Min').show();
                           $('#blk_SN').hide();
                       }

                    
                       if (Obligatorio == 0) {
                           $('#text_obliga').prop('checked', false);
                           $('#text_obliga').val(Obligatorio.trim());
                       }
                       else {
                           $('#text_obliga').prop('checked', true);
                           $('#text_obliga').val(Obligatorio.trim());
                       }
                       if (A_lote == 0) {
                           $('#text_lote').prop('checked', false);
                           $('#text_lote').val(A_lote.trim());
                       }
                       else {
                           $('#text_lote').prop('checked', true);
                           $('#text_lote').val(A_lote.trim());
                       }

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
                                   C_Maestros.Id = $('#Texto_ID').text();
                                   C_Maestros.articulo = "";
                                   C_Maestros.art_descripcion = "";
                                   C_Maestros.Carac = "";
                                   C_Maestros.desc_car = "";
                                   C_Maestros.V_Max = $('#txt_max').val();
                                   C_Maestros.V_Min = $('#txt_min').val();
                                   C_Maestros.Cod_Caracteristica = "";
                                   C_Maestros.Caracteristica = "";
                                   if ($('#text_obliga').is(':checked')) {
                                       C_Maestros.Obligatorio = "1";
                                   } else {
                                       C_Maestros.Obligatorio = "0";
                                   }
                                   if ($('#text_lote').is(':checked')) {
                                       C_Maestros.A_lote = "1";
                                   } else {
                                       C_Maestros.A_lote = "0";
                                   }
                                   $.ajax({
                                       type: "POST",
                                       url: "WebService_articulo_caracteristicas.asmx/delete_Data",
                                       data: '{datos: ' + JSON.stringify(C_Maestros) + '}',
                                       contentType: "application/json; charset=utf-8",
                                       dataType: "json",
                                       success: function (response) {
                                           //Success or failure message e.g. Record saved or not saved successfully
                                           loadComboBox('ddlArticulo', 'GetArticulos');
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
                               C_Maestros.Id = $('#Texto_ID').text();
                               C_Maestros.articulo = "";
                               C_Maestros.art_descripcion = "";
                               C_Maestros.Carac = "";
                               C_Maestros.desc_car = "";
                               C_Maestros.V_Max = $('#txt_max').val();
                               C_Maestros.V_Min = $('#txt_min').val();
                               C_Maestros.Cod_Caracteristica = "";
                               C_Maestros.Caracteristica = "";
                               if (Tipo_Dato == 2) {
                                   if ($('#text_sn').is(':checked')) {
                                       C_Maestros.V_Max = "1";
                                       C_Maestros.V_Min = "1";
                                   } else {
                                       C_Maestros.V_Max = "0";
                                       C_Maestros.V_Min = "0";
                                   }
                               }
                               if ($('#text_obliga').is(':checked')) {
                                   C_Maestros.Obligatorio = "1";
                               } else {
                                   C_Maestros.Obligatorio = "0";
                               }
                               if ($('#text_lote').is(':checked')) {
                                   C_Maestros.A_lote = "1";
                               } else {
                                   C_Maestros.A_lote = "0";
                               }
                               $.ajax({
                                   type: "POST",
                                   url: "WebService_articulo_caracteristicas.asmx/update_Data",
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
           });
        


       }
    
       function loadComboBox(comboname, methodname) {

           var template = "{#foreach $T as record}\
                        <option value='{$T.record.Articulo}'>{$T.record.Descripcion}</option>\
                    {#/for}";

           var combo = $('#' + comboname);

           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "WebService_articulo_caracteristicas.asmx/" + methodname,
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
               loadTable(); // or $(this).val()
           });
         //  combo.val(selected).change();
       }

       jQuery(document).ready(function () {
           loadComboBox('ddlArticulo', 'GetArticulos');
           $('#text_obliga').change(function () {
               if ($(this).attr('checked')) {
                   $(this).val('1');
               } else {
                   $(this).val('0');
               }
           });
           $('#text_lote').change(function () {
               if ($(this).attr('checked')) {
                   $(this).val('1');
               } else {
                   $(this).val('0');
               }
           });

        
       });

   </script>
     
      
            <h2><%: Title %></h2>
           

    <table><tbody>
        
        <tr><th>Articulo:</th>
         <td>
        <select id="ddlArticulo" class="js-example-basic-single" style="width: 392px"   ></select>
        </td>
            
            <td> &nbsp;&nbsp;<input id="btnTodas" type="button" Value="Nuevo" class="btn btn-default" onclick="nuevo();" /></td>
                <!--<input type="button" id="btnTodas" value="Nuevo"  onclick="nuevo()" OnClick="javascript:nuevo();"  /></td>-->
               <td> <asp:Button ID="BtBuscar" runat="server" Text="Limites Excel" CssClass="btn btn-default" OnClick="BtBuscar_Click"  /> </td>
            <td> <input id="btnCopia" type="button" Value="Copiar A" class="btn btn-default" onclick="copia_articulo();" /></td>
             </tr>
        </tbody>
    </table>
    <p></p>
    <p></p>
                          
    <table id="tblData" class="display" style="display: none" >
      <thead>
             <tr>
                 <th>Id</th>
                 <th>Articulo</th>
                 <th>Descripcion Articulo</th>
                 <th>Cod Caracteristica</th>
                <th>Caracteristica</th>
                 <th>Valor Min</th>
                 <th>Valor Max</th>
                 <th>obliga</th>
                 <th>Tipo Dato</th>
                  <th>Propagar a todo lote</th>
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot>
            
        </tfoot>
</table> 
     <div id="table-dialogo2"  style="display: none" >
          <table id="orderedittable2">
              <tr id="Blc_art2" >
                                <td id="lbl_art3">Articulo: </td>
                                <td id="Texto_Art3">1234 </td> 
                   <td id="lbl_art4"> a Articulo: </td>
                    <td class="cell">
                                 <input id="txt_Art_C" type="text" value="" style="width: 46px"/>
                            </td>
                            </tr>
              </table>
              </div>
    

    

      <div id="table-dialogo"  style="display: none" >
          <table id="orderedittable">
                        
                             <tr id="Blc_id">
                                <td id="lbl_id">ID: </td>
                                <td id="Texto_ID" style="width: 168px">1234 </td> 
                            </tr>
                           
                            <tr id="Blc_art" >
                                <td id="lbl_art">Articulo: </td>
                                <td id="Texto_Art" style="width: 168px">1234 </td> 
                            </tr>
                            <tr id="Blc_Combo_art">
                                <td id="lbl_art2">Articulo: </td>
                                <td ><select id="cmb_art" class="js-example-basic-single" style="width: 300px"> </select> </td> 
                            </tr>
                            <tr id="Blc_txt_carac">
                                <td id="lbl_carac">Caracteristica: </td>
                                <td id="Texto_carac" style="width: 168px">1234 </td> 
                            </tr>
                            <tr id="Blc_Combo_carac">
                                <td id="lbl_carac2">Caracteristica: </td>
                                <td ><select id="cmb_carac" class="js-example-basic-single" style="width: 200px"> </select> </td> 
                            </tr>
                            <tr id="blk_Min">
                            <td id="lbl_caracter" style="width: 168px">Valor Minimo:</td>
                            <td class="cell">
                                 <input id="txt_min" type="text" value="" style="width: 46px"/>
                            </td>
                        </tr>
                         <tr id="blk_Max">
                            <td>Valor Maximo: </td>
                            <td class="cell">
                                 <input id="txt_max" type="text" style="width: 46px" value="" />
                            </td>
                        </tr>
               <tr id="blk_SN">
                            <td>Si / No: </td>
                            <td class="cell">
                                 <input id="text_sn" type="checkbox"  onclick="check_prop(this)"  style="width: 46px" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>Obligatorio: </td>
                            <td class="cell">
                                 <input id="text_obliga" type="checkbox" style="width: 46px" value="0" />
                            </td>
                        </tr>
                   <tr>
                            <td>Propagar a lote: </td>
                            <td class="cell">
                                 <input id="text_lote" type="checkbox" style="width: 46px" value="0" />
                            </td>
                        </tr>
                        
              </table>
       
     </div>
</asp:Content>