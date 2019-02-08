Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Partial Class VB
    Inherits System.Web.UI.Page


    <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrentTime(ByVal name As String) As String
        Return "Hello " & name & Environment.NewLine & "The Current Time is: " & _
            DateTime.Now.ToString()
    End Function
End Class
