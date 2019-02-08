<%@ Page Title="Prueba Filtro" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="prueba_c2.aspx.cs" Inherits="rinya_app.prueba_c2" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
   
    <h1><%: Title %></h1>
     <link href="https://code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.0.3/css/buttons.dataTables.min.css"  rel="stylesheet" type="text/css" />
    <link href="https://cdn.datatables.net/1.10.9/css/dataTables.jqueryui.min.css" rel="stylesheet" type="text/css" />
      <script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
     <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.0.3/js/dataTables.buttons.min.js"></script>
    
    <script type="text/javascript"  src="https://cdn.datatables.net/buttons/1.0.3/js/buttons.html5.min.js" ></script>
    <script type="text/javascript">
      
        jQuery(document).ready(function () {
            
           $('#tblData tfoot th').each(function () {
               var title = $('#tblData thead th').eq($(this).index()).text();
               $(this).html('<input type="text" placeholder="Buscar por ' + title + '" />');

           });

           $('#btnTodas').click(getZonas);
           
       });
        function getZonas() {
            $.ajaxSetup({
                cache: false
            });

            function showDetails() {
                alert("showing some details");
            }
            
         

            var table = $('#tblData').DataTable({
                
                dom: 'l<"toolbar"> Bfrtip',
                buttons: [
                'csvHtml5',
                'excelHtml5',
                'copyHtml5',
                {
                    extend: 'pdfHtml5',
                    text: 'PDF',
                    customize: function (doc) {
                        //alert("prueba");
                        // Splice the image in after the header, but before the table
                       
                        pdfMake.createPdf(doc).open();
                    }
                }
                ],
                "oLanguage": {
                    "sZeroRecords": "No hay datos",
                    "sLengthMenu": "Mostrar _MENU_ registros por pagina&nbsp;&nbsp;",
                    "sInfo": "Mostrando desde _START_ hasta _END_ del _TOTAL_ de registros",
                    "sInfoEmpty": "Mostrando desde 0 hasta 0 de 0 registro",
                    "sInfoFiltered": "(Filtrado _MAX_ del total de registros)",
                    "sEmptyTable": 'No hay filas que mostrar.....!',
                    "sSearch": "Buscar en todas las columnas:",

                    "sFirst": "Primero",
                    "sPrevious": "Anterior",
                    "sNext": "Siguente",
                    "sLast": "Ultimo"
                },
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                "iDisplayLength": 10,
                "filter": true,
                
                "pagingType": "simple_numbers",
                "orderClasses": false,
                "order": [[0, "asc"]],
                "info": false,
                
                "scrollCollapse": true,
                "bProcessing": true,
                "bServerSide": true,
                "sAjaxSource": "WebService1.asmx/GetTableData",
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
                            $("#tblData").show();
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
            table.columns().eq(0).each(function (colIdx) {
                $('input', table.column(colIdx).footer()).on('keyup change', function () {
                  
                    table
                        .column(colIdx)
                        .search(this.value)
                        .draw();
                });
            });
            function openDialog() {
                //alert("prueba");
                $("#dialog-modal").dialog({
                    height: 140,
                    modal: true
                });
            }
            $("body").on("click", "#tblData tbody tr", function (e) {
                e.preventDefault();                    

                var nTds = $('td', this);
                //example to show any cell data can be gathered, I used to get my ID from the first coumn in my final code
                var sBrowser = $(nTds[0]).text();
                var sGrade = $(nTds[1]).text();
                var dialogText = "Zona: " + sBrowser + " Nombre: ";
              
                var targetUrl = $(this).attr("href");
                
                $('#table-dialogo').dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: false,
                    width: 400,
                    heigth: 250,
                    open: function (event, ui) {
                        $('#text_dialog').val(sGrade);
                        $('#Texto_lbl').text(dialogText);
                      //  initialize();
                        //loadOrder($(this).data('orderId'));

                    },
                    close: function (event, ui) {

                        //limpia todos los textbox del popup
                        $('#table-dialogo :text').val("");

                    },
                    buttons: {
                        "Borrar": function () {
                            alert("Borrado, no");
                        },
                        "Update": function () {
                            
                            if (sGrade.length > 0) {
                                var zona = {};
                                zona.Zona = sBrowser;
                                zona.Nombre = $('#text_dialog').val();
                                $.ajax({
                                    type: "POST",
                                    url: "WebService1.asmx/update_Data",
                                    data: '{datos: ' + JSON.stringify(zona) + '}',
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
                $('#table-dialogo').dialog("open");
          
            });
        };
        </script>
 


    

 <input type="button" id="btnTodas" value="Obtener zonas" /> 
  
    <table id="tblData" class="display">
      <thead>
             <tr>
                 <th>Zona</th>
                 <th>Nombre</th>
                 
             </tr>
      </thead>
      <tbody></tbody>
            <tfoot>
            <tr>
                <th>Zona</th>
                 <th>Nombre</th>
                  </tr>
        </tfoot>
</table> 
    

  

<div id="table-dialogo" title="ZONA"   >
          <table id="orderedittable">
                        <tr>
                            <td id="Texto_lbl" style="width: 168px">Zona </td>
                            <td class="cell">
                                 <input id="text_dialog" type="text" value="" style="width: 190px"/>
                            </td>
                        </tr>
              </table>
       
 </div>

</asp:Content>
  

