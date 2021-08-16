<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmTransferencia.aspx.cs" Inherits="AppIBULACIT.Views.frmTransferencia" %>

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
        <asp:Label Text="Lista de Transferencias" runat="server"></asp:Label></h1>
    

<!--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>-->
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
        $('[id*=gvTransferencia]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            dom: 'Bfrtip',
            'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
            'iDisplayLength': 5,
            buttons: [
                { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Transferencias_Excel', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Transferencias_Csv', exportOptions: { modifier: { page: 'all' } } },
                { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Transferencias_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
            ]
        });
    });
</script>




    <br />
    <asp:GridView ID="gvTransferencia" runat="server" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Cuenta Origen" DataField="CuentaOrigen" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Cuenta Destino" DataField="CuentaDestino" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Fecha" DataField="FechaHora" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />

        </Columns>
    </asp:GridView>

    <br />
    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnNuevo" OnClick="btnNuevo_Click" CausesValidation="false"
        runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <!-- Paso 3 Agregamos el LinkButton  /// Paso 9 creamos el evento onclick -->
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <!-- Paso 4 Agregamos el lblStatus -->


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
                                <asp:Literal ID="ltrCuentaOrigen" Text="Cuenta Origen" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCuentaOrigen" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvCuentaOrigen" runat="server" ForeColor="Maroon"
                                    ErrorMessage="Debe de indicar la Cuenta de Origen" ControlToValidate="txtCuentaOrigen"></asp:RequiredFieldValidator>
                            </td>
                        </tr>



                        <tr>
                            <td>
                                <asp:Literal ID="ltrCuentaDestino" Text="Cuenta Destino" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCuentaDestino" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvCuentaDestino" runat="server" ForeColor="Maroon"
                                    ErrorMessage="Debe de indicar la cuenta Destino" ControlToValidate="txtCuentaDestino"></asp:RequiredFieldValidator>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrfecha" Text="Fecha" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtfecha" runat="server" Enabled="false" CssClass="form-control" /></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ForeColor="Maroon"
                                    ErrorMessage="La descripcion es requerida" ControlToValidate="txtDescripcion"></asp:RequiredFieldValidator>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvMonto" runat="server" ForeColor="Maroon"
                                    ErrorMessage="El monto de requerido" ControlToValidate="txtMonto"></asp:RequiredFieldValidator></td>

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
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarMant" OnClick="btnCancelarMant_Click" CausesValidation="false" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>

     <div class="row">
            <div class="col-sm">
     <div id="canvas-holder" style="width:40%">
		            <canvas id="vistas-chart"></canvas>
	            </div>
              <script >
                  new Chart(document.getElementById("vistas-chart"), {
                      type: 'line',
                      data: {
                          labels: [<%= this.labelsGraficoVistasGlobal %>],
                          datasets: [{
                              label: "Estadistica por fechas de transferencias",
                              backgroundColor: [<%= this.backgroundcolorsGraficoVistasGlobal %>],
                        data: [<%= this.dataGraficoVistasGlobal %>]
                          }]
                      },
                      options: {
                          title: {
                              display: true,
                              text: 'Fechas de transferencias'
                          }
                      }
                  });
              </script>
                </div>
            </div>




</asp:Content>
