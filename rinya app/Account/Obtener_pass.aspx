<%@  Page Title="Obtener Password"  Language="C#"  AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Obtener_pass.aspx.cs" Inherits="rinya_app.Account.Obtener_pass" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <h2><%: Title %></h2>
   
     <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                     <h4>Utilice un usuario de quality para Conocer password.</h4>
                    <hr />
                      <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                     <div class="form-group">
                         <asp:Label runat="server" AssociatedControlID="txtUserName" CssClass="col-md-2 control-label">Usuario</asp:Label>
                         <div class="col-md-10">
                             <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" TextMode="SingleLine"/>
                                                          
                              </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                             <asp:Button runat="server" OnClick="LogIn" Text="Revelar password" CssClass="btn btn-default" />
                            <input type="submit" Value="Iniciar sesión" runat="server" style="display:none" ID="cmdLogin"><p></p>
                            <asp:Label id="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />

                        </div>
                    </div>
                </div>
                </section>
        </div>
     </div>
  </asp:Content>