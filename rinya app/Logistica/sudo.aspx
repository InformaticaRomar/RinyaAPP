<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="sudo.aspx.cs" Inherits="rinya_app.Logistica.sudo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script src="../Scripts/singleton.sudoku.js"></script>
    <script type="text/javascript">
		$( document ).ready( function() {
			var game = Sudoku.getInstance();
			$( '#container').append( game.getGameBoard() );
			$( '#solve').click( function() {
				game.solve();
			} );
			$( '#validate').click( function() {
				game.validate();
			} );
			$( '#reset').click( function() {
				game.reset();
			} );
		} );
		</script>
		<h1>Sudoku</h1>
		<div id="container"></div>
		<div id="menu" class="sudoku-menu">
			<button id="solve">Solucionar</button>
			<button id="validate">Validar</button>
			<button id="reset">Reset</button>
</div>
</asp:Content>
