<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmUsuario.aspx.cs" Inherits="AppIBULACIT.Views.frmUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalMantenimiento() {
            $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
        }

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseMantenimiento() {
            $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvServicios tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>
    <!--Paso 7 Agregamos el script -->



    <h1>
        <asp:Label Text="Informacion de Usuario" runat="server"></asp:Label></h1>
    <!--Paso 1 cambiamos el titulo-->

    <!--<input id="myInput" placeholder="Buscar" class="form-control" type="text" />
     Paso 8 -->


        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
<link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
<script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
<script type="text/javascript" src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('[id*=gvUsuario]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            dom: 'Bfrtip',
            'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
            'iDisplayLength': 5,
            buttons: [
                { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Usuario_Excel', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Usuario_Csv', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Usuario_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
            ]
        });
    });
</script>





    <asp:GridView ID="gvUsuario" runat="server" OnRowCommand="gvUsuario_RowCommand" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Identificacion" DataField="Identificacion" />
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Username" DataField="Username" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Password" DataField="Password" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Email" DataField="Email" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Fecha Nacimiento" DataField="FechaNacimiento" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <!--Paso 2 agregamos el gridview y modificamos las columnas-->
    <!--<asp:LinkButton type="button" CssClass="btn btn-success" ID="btnNuevo"
        runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />-->
    <!-- Paso 3 Agregamos el LinkButton -->
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <!-- Paso 4 Agregamos el lblStatus -->

    <!--  Paso 5 Primera VENTANA MODAL  Quitamos el onClick-->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de servicios</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" /><asp:Label ID="lblCodigoEliminar" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" CausesValidation="false" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>


    <!-- Paso 6 Segunda VENTANA DE MANTENIMIENTO quitamos el onclick -->
    <div id="myModalMantenimiento" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%; border-collapse: separate; border-spacing: 10px">
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrIdentificacion" Text="Identificacion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvIdentificacion" runat="server" ForeColor="Maroon" 
                                    ErrorMessage="La identificacion es requerida" ControlToValidate="txtIdentificacion"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrNombre" Text="Nombre" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El nombre es requerido" ControlToValidate="txtNombre"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrUsername" Text="UserName" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El UserName es requerido" ControlToValidate="txtUsername"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrPassword" Text="Contraseña" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El Password es Requerido" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltremail" Text="Correo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvemail" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El correo es requerido" ControlToValidate="txtemail"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCalendar" Text="Fecha Nacimiento" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaNacimiento" Placeholder="Ingrese su fecha de nacimiento" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnFechaNac" runat="server" Text="Seleccionar fecha" CausesValidation="false" OnClick="btnFechaNac_Click" />
                            </td>
                            <td>
                                <asp:Calendar ID="cldFechaNacimiento" OnSelectionChanged="cldFechaNacimiento_SelectionChanged" runat="server" Visible="false"></asp:Calendar>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text="Estado" runat="server" EnableViewState="true" /></td>
                            <td>
                                <asp:DropDownList ID="ddlEstadoMant" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarMant" OnClick="btnAceptarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarMant" CausesValidation="false" OnClick="btnCancelarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
