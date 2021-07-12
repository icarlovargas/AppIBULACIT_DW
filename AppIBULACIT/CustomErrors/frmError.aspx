<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmError.aspx.cs" Inherits="AppIBULACIT.CustomErrors.frmError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>



    <div class="container">
        <h3><span aria-hidden="true" class="glyphicon-remove"></span>Ocurrio un error</h3>
        <div class="panel-group">
            <div class="panel panel-primary">
                <div class="panel-body">

                    <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Hubo un error al procesar la solicitud"></asp:Label>
                    <br />
                    <asp:Label ID="lblError" runat="server"></asp:Label>

                </div>
            </div>
        </div>
    </div>
</body>
</html>
