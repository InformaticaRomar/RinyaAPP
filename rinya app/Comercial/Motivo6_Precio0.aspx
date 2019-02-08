<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Motivo6_Precio0.aspx.cs" Inherits="rinya_app.Comercial.Motivo6_Precio0" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
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
    jQuery(document).ready(function () {
        $("#datepicker1").datepicker().datepicker("setDate", semana_dia);
        $("#datepicker2").datepicker().datepicker("setDate", new Date());
    }
        </script>
     <div class="row">
           <div class="col-md-8">
               <h2> CONTROL STOCK</h2>
               <div class="form-group">
                   <div class="row">
                       <div class="col-md-4">
                   <asp:Label runat="server" AssociatedControlID="DropDownDesde" CssClass="col-md-2 control-label">Desde</asp:Label>
                           </div>
                       <div class="col-md-2">
                     <asp:Label runat="server" AssociatedControlID="DropDownHasta" CssClass="col-md-2 control-label">Hasta</asp:Label>
                           </div>
                       </div>
                   <div class="row">
                       
                      <div class="col-md-4">
                             
                         <asp:TextBox ID="datepicker1" runat="server"></asp:TextBox>
                         <!-- </div>-->
                  
                   <div class="col-md-4">  
                        <asp:TextBox ID="datepicker2" runat="server"></asp:TextBox>
                       </div>
                       <div class ="col-md-2"><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>
                    
                     </div>
                     <div class="row">
                           <hr />
                         <div class="container" >
                          <!-- <div class="container" style="overflow: scroll;HEIGHT:100%">-->
             
                   <cc1:SearchableGridView ID="GridView1" Visible="true"
                       
                        AllowSorting="true"
                        AutoGenerateColumns="false" CancelSearchImageURL="Icons/filterCancel.png" 
                        SearchGoFirstImageUrl="Icons/SearchGoFirst.png"
                        SearchGoImageUrl="Icons/SearchGo.png" SearchGoLastImageUrl="Icons/SearchGoLast.png"
                       ExcelImageURL="Icons/Excel.png"
                        SearchGoNextImageUrl="Icons/SearchGoNext.png" SearchGoPreviousImageUrl="Icons/SearchGoPrevious.png"
                        SearchImageURL="Icons/filter.png" ShowFooter="True" CurrentSortDirection="ASC"
                        
                        EmptyDataText="No se han encontrado datos"  runat="server"
                       OnFilterButtonClick="GridView1_FilterButtonClick"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnCancelFilterButtonClick="GridView1_CancelFilterButtonClick" OnNavigationButtonClick="GridView1_NavigationButtonClick" OnSorting="GridView1_Sorting" 
                     CellPadding="4" OnPageSizeChanged="GridView1_PageSizeChanged"
                         OnExcelButtonClick="GridView1_ExcelButtonClick" PageSize="3"   >
                         
                       <Columns>
                           <cc1:SearchBoundField DataField="N_albaran" HeaderText="N Albaran" SearchExpression="N_albaran">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="DESCRIPCION" HeaderText="Descripcion" SearchExpression="DESCRIPCION">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                           <cc1:SearchBoundField DataField="Kg_Actuales" HeaderText="KGs Actuales" SearchExpression="Kg_Actuales">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="UDs_Actuales" HeaderText="UDs Actuales" SearchExpression="UDs_Actuales">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="Dsc_Estado" HeaderText="Estado" SearchExpression="Dsc_Estado">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Almacen" HeaderText="Almacen" SearchExpression="Almacen">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="LoteInterno" HeaderText="Lote Interno" SearchExpression="LoteInterno">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="FechaCaducidad" HeaderText="F. Cad" SearchExpression="FechaCaducidad">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="FechaFabricacion" HeaderText="F. Fab" SearchExpression="FechaFabricacion">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="SSCC" HeaderText="Matricula" SearchExpression="SSCC">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Partida" HeaderText="Partida" SearchExpression="Partida">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Discriminador" HeaderText="Discriminador" SearchExpression="Discriminador">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Familia" HeaderText="Familia" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>


                           </Columns>
                        
                        <FooterStyle BackColor="#424242" Font-Bold="True" ForeColor="White"  />
                        <HeaderStyle BackColor="#424242" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#424242" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                       
                       
                           </cc1:SearchableGridView >
                  </div>
                                  <!-- </div> -->
                         </div>
                   </div>
               </div>
      </div>
</div>
</asp:Content>
