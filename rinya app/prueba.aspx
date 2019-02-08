<%@ Page Title="Prueba"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="prueba.aspx.cs" Inherits="rinya_app.prueba" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Buscar por Zona</h3>
    <div id="Formu">Buscar:

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Una zona" OnClick="Button1_Click" />

        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Todas" />

    </div>

    <div id="grid"><ASP:DataGrid  ID="Table1" runat="server" Width="415px"
      BackColor="#ccccff" 
      BorderColor="black"
      ShowFooter="false" 
      CellPadding=3 
      CellSpacing="0"
      Font-Size="8pt"
      HeaderStyle-BackColor="#aaaadd"
      EnableViewState="false" Font-Names="Verdana">
        </ASP:DataGrid>
     <table border="0" width='100%' bgcolor='#ff00ff'>
<tr style='padding-bottom:1em;'>
<td>
    <table border="0">
        <tr valign='bottom'>
            <td width='50'>&nbsp;</td>
            <td>
                <img border="0" src="#" width="100" height="300">
            </td>
            <td width='25'>&nbsp;</td>
            <td>
                <span style='font-size:10px;color:#cccccc;'>Alpha testing</span>
            </td>
        </tr>
    </table>
</td>
<td align='right'>
    <table border="0" cellspacing="0">
        <tr>
            <td align='center' colspan='3'>
                <span>Another tesing text</span>
                <span style='color:#FFFFCC;'> - </span>
                <span style='font-size:11px;color:#fffff;'>Random text</span>
            </td>
        </tr>
    </table>
</td>
</tr>
        <asp:GridView ID="GridView1" runat="server" BackColor="#ccccff" BorderColor="black"
      ShowFooter="false" 
      CellPadding=3 
      CellSpacing="0"
      Font-name="verdana"
      Font-Size="8pt"
      HeaderStyle-BackColor="#aaaadd"
      EnableViewState="false" Font-Names="Verdana" AllowPaging="True" AllowSorting="True" EnableSortingAndPagingCallbacks="True">
        </asp:GridView>
    </div>
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
                                 <TextArea id="text_O_arriba"   class="auto-style1" ></TextArea>
                            </td>
                                         </tr>
                                   <tr id="Blck_57">          
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
                                 <TextArea id="text_O_medio"   class="auto-style1" ></TextArea>
                            </td>
                               </tr>
                                   <tr id="c_Blck_58">   
                  <td id ="c_lbl_58" style="width: 100px">Bajo </td>
                            <td class="cell">
                                 <input id="text_H_abajo" class="form-control" type="text" value="" style="width: 75px"/>
                            </td>
                            <td class="cell">
                                 <input id="text_E_abajo" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_abajo_A" type="checkbox" onclick="check_Abajo_A();">Incorrecto
                                               <input id="text_E_abajo_B" type="checkbox"  onclick="check_Abajo_B();">Correcto

                            </td>
                                        <td class="cell">
                                 <TextArea id="text_O_abajo"   class="auto-style1" ></TextArea>
                            </td>
                               
</tr>
                                   <tr id="c_Blck_59">   
                  <td id ="c_lbl_59" style="width: 250px">Resultado comité </td>
                           
                            <td class="cell">
                                 <input id="text_E" type="hidden" style="width: 46px" value="" />
                                <input id="text_E_A" type="checkbox" onclick="check_A();">Incorrecto
                                 <input id="text_E_B"  type="checkbox"  onclick="check_B();">Correcto

                            </td>
                               
</tr>
                                 <tr id="Obsevaciones_Comite"><td id ="lbl_comite">Observaciones Comité </td>
                            <td class="cell" colspan="3">
                                 <TextArea id="text_observa_comite" rows="3" class="auto-style1"> </TextArea>
                            </td></tr>
                            </table> 
                       

</asp:Content>
