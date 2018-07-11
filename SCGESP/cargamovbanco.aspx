<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="cargamovbanco.aspx.cs" Inherits="SCGESP.cargamovbanco" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Confrontación [Cargar Movimientos Bancarios]
                <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <!--form btn cargar movimientos bancos-->
                <!--form id="frmcagarb" name="frmcagarb" class="form-inline" action="#" enctype='multipart/form-data' style="width: 50%; margin: 0 auto;"-->
                <table class="filtro">
                    <tr>
                        <td style="width: 100px;">Banco:</td>
                        <td style="width: 200px;">
                            <select id="banco" name="banco" class="select2" data-width="90%">
                                <option value="toka">TOKA</option>
                            </select>
                        </td>
                        <td>
                            <input id="filebanco" name="filebanco" onchange="cargaBanco()" type="file" accept=".xlsx" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">*<i>Para usar esta opción debes seguir los siguientes pasos:</i>
                            <ol>
                                <li>Descargar Estado de Cuenta que proporciona TOKA.</li>
                                <li>Click en el botón "Cargar Movimientos*"</li>
                                <li>Buscar y seleccionar el archivo Excel que contiene los movimientos (Estado de Cuenta TOKA).</li>
                                <li>Click en el botón "Abrir".</li>
                                <li>Si no hay errores en el contenido del Excel, los movimientos serán procesados y se te pedirá una confirmación para guardarlos.</li>
                            </ol>
                            <i>Nota:</i>
                            <ul>
                                <li>No alterar el archivo de Excel.
                                    <ul>
                                        <li>No cambiar la extensión del archivo (xlsx).</li>
                                        <li>No cambiar de posición las columnas.</li>
                                        <li>No cambiar el formato de las columnas.</li>
                                        <li>No eliminar el encabezado de las columnas.</li>
                                        <li>No agregar más columnas solo se guardaran las ya definidas en el Excel.</li>
                                    </ul>
                                </li>
                            </ul>
                            <img src="img/ejemploEdoCtaTOKA.PNG" width="50%" />
                        </td>
                    </tr>
                </table>
                <!--/form-->


                <!--movimientos bancos-->
                <div class="modal fade" id="MovBanco" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header titulo-modal">
                                Movimientotos
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close" onclick="$('#filebanco').filestyle('clear')"><i class="zmdi zmdi-close"></i>Cerrar</button>
                            </div>
                            <div class="modal-body" style="overflow-y: auto; max-height: 50%; height: 300px;">

                                <table class="filtro text-left" style="width: 99%; text-align: left">
                                    <tr>
                                        <td width="110px">No. Tarjeta</td>
                                        <td id="tdTarjeta"></td>
                                    </tr>
                                    <tr>
                                        <td>Nombre:</td>
                                        <td id="tdNombre"></td>
                                    </tr>
                                    <tr>
                                        <td>Nomina:</td>
                                        <td id="tdNomina"></td>
                                    </tr>
                                </table>

                                <span id="msnbanco">Guardar los siguientes movimientos para el banco</span>
                                <table id="tblMovimientos" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                                    <thead>
                                        <tr>
                                            <th>No. Tarjeta</th>
                                            <th width="70px">Fecha</th>
                                            <th>Concepto</th>
                                            <th width="70px">Imp. $</th>
                                            <th title="Indica si el movimiento ya existe en la BD.">Dup.?</th>
                                            <th title="Fecha de cuando el movimiento se cargo por ultima vez.">F.Carga</th>
                                            <th title="Confirma si deseas guardar el movimiento.">Conf.?</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot>
                                    </tfoot>
                                </table>
                            </div>
                            <div class="modal-footer">
                                <button id="guardarMBanco" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </section>
    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript">
        var pagina = "carga";
    </script>
    <script type="text/javascript" src="js/confrontacion.js?v.1"></script>
</asp:Content>
