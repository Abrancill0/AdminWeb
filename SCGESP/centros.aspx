<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="centros.aspx.cs" Inherits="SCGESP.centros" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Centros
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height:25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <table id="tblCentros" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                    <thead>
                        <tr>
                            <th width="10px">#</th>
                            <th>Centro</th>
                            <th width="60px">Id Nivel</th>
                            <th width="100px">Nmb. Nivel</th>
                            <th width="50px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>

        <!--Modal Requisicion-->
        <div class="modal fade" id="verCentro" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <!--style="width: 700px"-->
                    <div class="modal-header titulo-modal">
                        Centro
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">

                        <table class="filtro" style="width: 100%">
                            <tr>
                                <td style="width: 100px">ID Centro:</td>
                                <td id="FiCenId" colspan="3"></td>
                            </tr>
                            <tr>
                                <td>Centro:</td>
                                <td id="FiCenNombre"></td>
                                <td>Monto Minimo:</td>
                                <td id="FiCenMontoMinimo"></td>
                            </tr>
                            <tr>
                                <td>ID Nivel:</td>
                                <td id="FiCenNivel"></td>
                                <td>Nivel:</td>
                                <td id="FiCenNivelNombre"></td>
                            </tr>
                            <tr>
                                <td>Responsable:</td>
                                <td id="FiCenResponsableNombre" colspan="3"></td>
                            </tr>
                            <tr>
                                <td>ID Centro Sup.:</td>
                                <td id="FiCenCentroSuperior"></td>
                                <td>Centro Sup.:</td>
                                <td id="FiCenCentroSuperiorNombre"></td>
                            </tr>
                        </table>

                    </div>
                    <div class="modal-footer account-controles">
                        <!--button id="guardara" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</!--button-->
                    </div>
                </div>            </div>
        </div>


    </section>

    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript" src="js/centros.js"></script>

</asp:Content>
