<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ubicacion.aspx.cs" Inherits="rinya_app.Ubicaciones.Ubicacion" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
     <!-- <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>-->
     <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
 <link rel="stylesheet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css" />
    <!--  <link href="../Scripts/jquery-ui/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>-->
      
    
   <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js" ></script>
    <script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>
      <script src="../Scripts/jquery-ui/jquery-ui.js"></script>
     <script >
         function EnterEvent(e) {
        if (e.keyCode == 13) {
            __doPostBack('<%=Button1.UniqueID%>', "");
        }
         }
         function tempAlert(msg, duration) {
             var el = document.createElement("div");
             el.setAttribute("style", "position:absolute;top:40%;left:20%;background-color:white;");
             el.innerHTML = msg;
             setTimeout(function () {
                 el.parentNode.removeChild(el);
             }, duration);
             document.body.appendChild(el);
         }
         function dialogo() {
             $('#table-dialogo').dialog({
                 height: 140,
                 modal: true,
                 open: function(event, ui){
                     setTimeout("$('#table-dialogo').dialog('close')", 4000);
                 }
             });
         }
         //  setTimeout(function () { alert("my message"); }, 10);
         
        /* function message(msg) {
             if (window.webkitNotifications) {
                 if (window.webkitNotifications.checkPermission() == 0) {
                     notification = window.webkitNotifications.createNotification(
                       'picture.png', 'Leeme', msg);
                     notification.onshow = function () { // when message shows up
                         setTimeout(function () {
                             notification.close();
                         }, 1000); // close message after one second...
                     };
                     notification.show();
                 } else {
                     window.webkitNotifications.requestPermission(); // ask for permissions
                 }
             }
             else {
                 alert(msg);// fallback for people who does not have notification API; show alert box instead
             }
         }*/
     </script>
    <title>Ubicación</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script src="../Scripts/js/bootstrap.min.js"></script>
    <form runat="server">
        
    <div class="container">
     <div class="row">
        <div class="col-md-6">
            <section id="ubicacionForm">
                <div class="form-horizontal">
                    <h4>Indica Ubicacion</h4>
                    <hr /> 
                    <div class="form-group">
                         <asp:Label runat="server" AssociatedControlID="txtMatricula" CssClass="col-md-2 control-label">Matricula:</asp:Label>
                         <div class="col-md-8">
                             <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" TextMode="SingleLine"/>
                         </div>
                    </div>
                     <div class="form-group">
                         <asp:Label runat="server" AssociatedControlID="txtUbicacion" CssClass="col-md-2 control-label">Ubicacion:</asp:Label>
                         <div class="col-md-8">
                             <asp:TextBox runat="server" ID="txtUbicacion" CssClass="form-control"  TextMode="SingleLine" onkeypress="return EnterEvent(event)"/>
                         </div>
                    </div>
             <div class="form-group">
                        <div class="col-md-offset-2 col-md-8">
                             <asp:Button runat="server" ID="Button1" Text="Guardar" CssClass="btn btn-default" OnClick="Guardar_click" />
                            <input type="button" Value="Guardar" runat="server" style="display:none" ID="cmdUBICA">
                            <hr /> <p></p>

                            

                              <div id="table-dialogo" style="display: none" >

                              <table id="orderedittable">
                                            <tr>
                                               <td >  <asp:Label id="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" /> </td>

                                            </tr>

                              </table>

                              </div>
                           

                        </div>
                    </div>
                </div>
                </section>
        </div>
     </div>
    
    
    </div>
    </form>
</body>
</html>
