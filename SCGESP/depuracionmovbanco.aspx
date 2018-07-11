<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="depuracionmovbanco.aspx.cs" Inherits="SCGESP.depuracionmovbanco" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Confrontación [Depuraci&oacute;n Movimientos Bancarios]
                <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <!--form btn cargar movimientos bancos-->
                <!--form id="frmcagarb" name="frmcagarb" class="form-inline" action="#" enctype='multipart/form-data' style="width: 50%; margin: 0 auto;"-->
                <table class="filtro">
                    <tr>
                        <td class="centerxy" style="width: 30px">De:</td>
                        <td>
                            <div class="form-group" style="width: 120px">
                                <div class='input-group date datetimepicker2'>
                                    <input class="form-control reporte" readonly="readonly" name="repde" id="repde" type="text" />
                                    <i class="form-group__bar"></i>
                                    <span class="input-group-addon">
                                        <span class="zmdi zmdi-calendar zmdi-hc-2x" style="padding: 3px 0px"></span>
                                    </span>
                                </div>
                            </div>
                        </td>
                        <td class="centerxy" style="width: 30px">A:</td>
                        <td>
                            <div class="form-group" style="width: 120px">
                                <div class='input-group date datetimepicker2'>
                                    <input class="form-control reporte" readonly="readonly" name="repa" id="repa" type="text" />
                                    <i class="form-group__bar"></i>
                                    <span class="input-group-addon">
                                        <span class="zmdi zmdi-calendar zmdi-hc-2x" style="padding: 3px 0px"></span>
                                    </span>
                                </div>
                            </div>
                        </td>
                        <td class="centerxy">&nbsp;&nbsp;<a id="btnBuscar" href="#" role="button" class="btn btn-primary"><i class="zmdi zmdi-search"></i> Realizar</a>
                        </td>
                    </tr>
                </table>
                <!--/form-->

                <table id="tblDepuracionMovBanco" class="display browse" cellspacing="0" style="width:100%;">
                    <thead>
                        <tr>
                            <th rowspan="2"></th>
                            <th colspan="6">Movimiento Bancario</th>
                            <th colspan="3">Informe/Gasto</th>
                            <th rowspan="2"></th>
                        </tr>
                        <tr>
                            <th>Banco</th>
                            <th>Tarjeta</th>
                            <th>Tipo</th>
                            <th>Fecha</th>
                            <th>Observaciones</th>
                            <th>Importe</th>
                            <th>Informe</th>
                            <th>Gasto</th>
                            <th>Importe</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>



            </div>
        </div>
    </section>
    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript">
        var pagina = "depuracion";
    </script>
    <script type="text/javascript" src="js/confrontacion.js?v.1"></script>

</asp:Content>
