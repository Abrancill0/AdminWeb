<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="oficinas.aspx.cs" Inherits="SCGESP.oficinas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Oficinas
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height:25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <table id="tblOfinas" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                    <thead>
                        <tr>
                            <th width="10px">#</th>
                            <th>Oficina</th>
                            <th width="50px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>

        <!--Modal Requisicion-->
        <div class="modal fade" id="verOficina" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <!--style="width: 700px"-->
                    <div class="modal-header titulo-modal">
                        Oficina
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">

                        <table class="filtro" style="width: 100%">
                            <tr>
                                <td style="width: 100px">ID Oficina:</td>
                                <td id="GrOfiId">
                                </td>
                            </tr>
                            <tr>
                                <td>Oficina:</td>
                                <td id="GrOfiNombre">
                                </td>
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
    <script type="text/javascript" src="js/oficinas.js"></script>


</asp:Content>
