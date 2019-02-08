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

function loadComboBox(comboname, methodname, selected) {

    var template = "{#foreach $T as record}\
                        <option value='{$T.record.ID_Estado}'>{$T.record.Estado}</option>\
                    {#/for}";

    var combo = $('#' + comboname);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "WebService_calidad.asmx/" + methodname,
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
    //combo.trigger("chosen:updated").selectmenu().selectmenu('refresh',true);
 

  

}
function Create_Table_Organo(Tablename, methodname) {
    if ($.fn.dataTable.isDataTable('#' + Tablename)) {
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
  
}
function Create_Table(Tablename, methodname) {
    $.ajaxSetup({ cache: false });
   
    
    function showDetails() {
        alert("showing some details");
    }

   

    var table = $('#' + Tablename).DataTable({
        //dom: 'l<"toolbar"> Bfrtip',
        dom: '<"H"B<lfr>>t<"F"ip>',
        jQueryUI: true,
        responsive: true,
        buttons: [
        'csvHtml5',
        'excelHtml5',
        'copyHtml5'
        ],
        "language": {
            "sProcessing":    "Procesando...",
            "sLengthMenu":    "Mostrar _MENU_ registros",
            "sZeroRecords":   "No se encontraron resultados",
            "sEmptyTable":    "Ningún dato disponible en esta tabla",
            "sInfo":          "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty":     "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered":  "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix":   "",
            "sSearch":        "Buscar:",
            "sUrl":           "",
            "sInfoThousands":  ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst":    "Primero",
                "sLast":    "Último",
                "sNext":    "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending":  ": Activar para ordenar la columna de manera ascendente",
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
        "columnDefs": [
       {
           "targets": [18],
           "className" : "none"
       }, 
       {
           "aTargets": [2],
           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
               if (sData.indexOf("Liberad") > -1) {
                   $(nTd).addClass('liberado');
               } else { $(nTd).addClass('retenido'); }
           }
       }
        ],
         
       // "scrollCollapse": true,
        "bProcessing": true,
        "bServerSide": true,
       
        "sAjaxSource": "WebService_calidad.asmx/"+ methodname,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "fecha1", "value": $("#datepicker1").val() },
            { "name": "fecha2", "value": $("#datepicker2").val() }
         );
            $.ajax({
                "dataType": 'json',
                "contentType": "application/json; charset=utf-8",
                "type": "GET",
                "url": sSource,
                "data": aoData,
                "success": function (msg) {
                    var json = jQuery.parseJSON(msg.d);
                    fnCallback(json);
                    $('#' + Tablename).attr("style", "visibility: visible");
                    $('#' + Tablename).show();
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
    
    $('#' + Tablename+" tbody").on("click", 'tr', function (e) {
        e.preventDefault();

        var nTds = $('td', this);
        
        var NomEs = $(nTds[2]).text();
        var aRDescrip = $(nTds[3]).text();
        var SSCC = $(nTds[6]).text();
        var Color = $(nTds[9]).text();
        var Sabor = $(nTds[10]).text();
        var Textura = $(nTds[11]).text();
        var Corte = $(nTds[12]).text();
        var Film = $(nTds[13]).text();
        var Temperatura = $(nTds[14]).text();
        var Ph = $(nTds[15]).text();
        var Brix = $(nTds[16]).text();
        var ObsEstado = $(nTds[17]).text();
        var estado = $(nTds[18]).text();

        $("#table-dialogo").attr("style", "display: table");

        $('#table-dialogo').dialog({
            autoOpen: true,
            modal: true,
            resizable: false,
            title: 'Modificar',
            width: 500,
            heigth: 250,
           
            open: function (event, ui) {
                
                $('#Texto_articulo').text(aRDescrip.trim());
                $('#Texto_matricula').text(SSCC.trim());
                $('#text_color').val(Color.trim());
                $('#text_Sabor').val(Sabor.trim());
                $('#text_Textura').val(Textura.trim());
                $('#text_Corte').val(Corte.trim());
                $('#text_Film').val(Film.trim());
                $('#text_Temperatura').val(Temperatura.trim());
                $('#text_Ph').val(Ph.trim());
                $('#text_Brix').val(Brix.trim());
                $('#text_ObsEstado').val(ObsEstado.trim());
                loadComboBox('ddlEstado', 'GetEstados', estado.trim());
                $('#ddlEstado').selectmenu().selectmenu('refresh', true);
                

            },
            close: function (event, ui) {

               
                $('#table-dialogo :text').val("");

            },
            buttons: {
                "Borrar": function () {
                    alert("Borrado, no");
                },
                "Update": function () {

                    if (SSCC.length > 0) {

                        var C_calidad = {};
                        C_calidad.aRDescrip = $('#Texto_articulo').text();
                        C_calidad.SSCC = $('#Texto_matricula').text();
                        C_calidad.color = $('#text_color').val();
                        C_calidad.Sabor = $('#text_Sabor').val();
                        C_calidad.Textura = $('#text_Textura').val();
                        C_calidad.Corte = $('#text_Corte').val();
                        C_calidad.Film = $('#text_Film').val();
                        C_calidad.Temperatura = $('#text_Temperatura').val();
                        C_calidad.Ph = $('#text_Ph').val();
                        C_calidad.Brix = $('#text_Brix').val();
                        C_calidad.ObsEstado = $('#text_ObsEstado').val();
                        $.ajax({
                            type: "POST",
                            url: "WebService_calidad.asmx/update_Data",
                            data: '{datos: ' + JSON.stringify(C_calidad) + '}',
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
        //simple dialog example here
        // $('#table-dialogo').dialog("open");

    });

   
   
}