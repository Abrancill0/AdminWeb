<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="confrontacion.aspx.cs" Inherits="SCGESP.confrontacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Confrontación [Depuraci&oacute;n Movimientos Bancarios]
                <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <table class="filtro" style="width: 100%">
                    <tr>
                        <td style="width: 80px">Usuario:</td>
                        <td>
                            <select id="usuario" name="usuario" class="select2" data-width="100%" data-live-search="true" data-size="10">
                            </select>
                        </td>
                        <td style="width: 80px">Informe:</td>
                        <td>
                            <select id="informe" name="informe" multiple class="select2" data-placeholder=" - Informe - " data-width="100%" data-live-search="true" data-size="10">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <button id="confrontarInforme" type="button" class="btn btn-primary centerxy"><i class="zmdi zmdi-thumb-up"></i>Confrontar Informe</button>
                        </td>
                    </tr>
                </table>


                <table id="tblGastosInforme" class="display browse" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th></th>
                            <th># Inf.</th>
                            <th>Concepto</th>
                            <th>Negocio</th>
                            <th>Forma P.</th>
                            <th>F.Gasto</th>
                            <th>Monto $</th>
                            <th>Banco?</th>
                            <th>Asig.Mov.</th>
                            <th>
                                <span class='helpth glyphicon glyphicon-question-sign' title="Confirmar Confrontación del Gasto"></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                    </tfoot>
                </table>


                <!--model movimientos bancarios sin gastos-->
                <div class="modal fade" id="MovBanSinGasto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header titulo-modal">
                                Movimientos Bancarios Sin Gasto
                                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                            </div>
                            <div class="modal-body">
                                <span id="msnmb"></span>

                                <form id="frmGastoSinXML" name="frmMovBanSinGasto" class="form-inline" action="#" style="width: 100%;">
                                    <table class="filtro table-bordered" style="width: 100%;">
                                        <tr><td colspan="8">Fitros</td></tr>
                                        <tr>
                                            <td style="width: 80px;">Fecha De:</td>
                                            <td>
                                                <input class="form-control reporte2" readonly="readonly" name="repde2" id="repde2" type="text" style="width: 80px;" />
                                            </td>
                                            <td style="width: 80px;">Fecha A:</td>
                                            <td>
                                                <input class="form-control reporte2" readonly="readonly" name="repa2" id="repa2" type="text" style="width: 80px;" />
                                            </td>
                                            <td>Importe De:
                                            </td>
                                            <td>
                                                <input type="number" id="importede" name="importede" onchange="BuscarMovSinGasto('', '', '', '', '', 'importede');" class="form-control text-right" style="width: 80px;" />
                                            </td>
                                            <td>Importe A:
                                            </td>
                                            <td>
                                                <input type="number" id="importea" name="importea" onchange="BuscarMovSinGasto('', '', '', '', '', 'importea');" class="form-control text-right" style="width: 80px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </form>
                                <div id="scrolltblMovSinGasto">
                                    <table id="tblMovSinGasto" class="display browse" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Banco</th>
                                                <th>Cuenta/Tarjeta</th>
                                                <th>Descripción</th>
                                                <th width="70px">Fecha</th>
                                                <th width="70px">Monto $</th>
                                            </tr>
                                        </thead>
                                        <tfoot>
                                        </tfoot>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">

                                <button id="btnAsignarMovBanGasto" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span><span id="lblbtnAsignarGastoXML">Asignar Mov. al Gasto</span></button>
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
        var pagina = "confrontacion";
    </script>
    <script type="text/javascript" src="js/confrontacion.js?v.1"></script>

</asp:Content>
