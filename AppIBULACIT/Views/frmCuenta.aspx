<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCuenta.aspx.cs" Inherits="AppIBULACIT.Views.frmCuenta" %>

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
        <asp:Label Text="Informacion de Cuentas" runat="server"></asp:Label></h1>
    <!--Paso 1 cambiamos el titulo-->
    <br />
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <!-- Paso 8 -->
    <br />
    <asp:GridView ID="gvCuentas" runat="server" AutoGenerateColumns="False" OnRowCommand="gvCuentas_RowCommand"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="IBAN" DataField="IBAN" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="CodigoMoneda" DataField="CodigoMoneda" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Saldo" DataField="Saldo" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <!--Paso 2 agregamos el gridview y modificamos las columnas-->
    <br />
    <asp:LinkButton type="button" CssClass="btn btn-success" OnClick="btnNuevo_Click" ID="btnNuevo" CausesValidation="false"
        runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <!-- Paso 3 Agregamos el LinkButton  /// Paso 9 creamos el evento onclick -->
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
                        <asp:Literal ID="ltrModalMensaje" runat="server" /><asp:Label ID="lblCodigoEliminar" runat="server" /></p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" CausesValidation="false" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
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
                <div class="modal-body ">
                    <table style="width: 100%; border-collapse: separate; border-spacing: 10px">
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" Enabled="false" runat="server" CssClass="form-control" /></td>

                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoUsuarioMan" Text="Codigo Usuario" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoUsuarioMan" runat="server" ValidateRequestMode="Enabled" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvCodigoUsuarioMan" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El codigo del Usuario es requerido" ControlToValidate="txtCodigoUsuarioMan"></asp:RequiredFieldValidator></td>



                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ForeColor="Maroon"
                                    ErrorMessage="La descripcion es necesaria" ControlToValidate="txtDescripcion"></asp:RequiredFieldValidator></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrIBAN" Text="IBAN" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtIBAN" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvIBAN" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El IBAN es requerido" ControlToValidate="txtIBAN"></asp:RequiredFieldValidator></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMonedaMan" Text="Codigo de Moneda" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddMonedas" Enabled="true" CssClass="form-control" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrSaldo" Text="Saldo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtSaldo" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvSaldo" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El saldo es requerido" ControlToValidate="txtSaldo"></asp:RequiredFieldValidator></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text="Estado" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlEstadoMant" Enabled="true" CssClass="form-control" runat="server">
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
