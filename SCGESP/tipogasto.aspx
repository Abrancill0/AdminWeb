<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tipogasto.aspx.cs" Inherits="SCGESP.tipogasto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Tipo de Gastos
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height: 25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
                <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <table id="tblTipoGasto" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                    <thead>
                        <tr>
                            <th width="50px">#</th>
                            <th>Tipo de Gasto</th>
                            <th width="50px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>

        <!--Modal Tipo Gasto-->
        <div class="modal fade" id="verTipoGasto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header titulo-modal">
                        Tipo Gasto
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">

                        <table class="filtro" style="width: 100%">
                            <tr>
                                <td style="width: 150px">ID:</td>
                                <td id="FiTgaId"></td>
                            </tr>
                            <tr>
                                <td>Tipo Gasto:</td>
                                <td id="FiTgaNombre"></td>
                            </tr>
                        </table>

                    </div>
                    <div class="modal-footer account-controles">
                        <!--button id="guardara" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</!--button-->
                    </div>
                </div>
            </div>
        </div>


    </section>

    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript" src="js/tipogasto.js"></script>

</asp:Content>
