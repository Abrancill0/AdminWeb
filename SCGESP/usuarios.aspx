<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="usuarios.aspx.cs" Inherits="SCGESP.usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Usuarios
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height:25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">
                <!--a id="btnUsuAut" href="/usuariosAutorizadoresInforme" role="button" class="btn btn-primary" aria-hidden='true'>Ver Lista de Usuarios Autorizadores de Informe</a-->
                <table id="tblUsuarios" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                    <thead>
                        <tr>
                            <th width="50px">Usuario</th>
                            <th>Nombre</th>
                            <th width="100px">No. Empleado</th>
                            <th width="250px">Nmb. Empleado</th>
                            <th width="50px">Activo</th>
                            <th width="50px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>

        <!--Modal Usuarios-->
        <div class="modal fade" id="verUsuario" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header titulo-modal">
                        Usuario
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">

                        <table class="filtro" style="width: 100%">
                            <tr>
                                <td style="width: 150px">ID Usuario:</td>
                                <td id="SgUsuId" colspan="3"></td>
                            </tr>
                            <tr>
                                <td>Usuario:</td>
                                <td id="SgUsuNombre"></td>
                                <td>Activo:</td>
                                <td id="SgUsuActivo"></td>
                            </tr>
                            <tr>
                                <td>ID Empleado:</td>
                                <td id="SgUsuEmpleado"></td>
                                <td>Nmb. Emp:</td>
                                <td id="SgUsuEmpleadoNombre"></td>
                            </tr>
                            <tr>                                
                                <td>ID Grupo:</td>
                                <td id="SgUsuGrupoUsuario"></td>
                                <td>Grupo:</td>
                                <td id="SgUsuGrupoUsuarioNombre"></td>
                            </tr>
                            <tr>
                                <td>Fecha Vencimiento:</td>
                                <td id="SgUsuFechaVencimiento" colspan="3"></td>
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
    <script type="text/javascript" src="js/usuarios.js"></script>

</asp:Content>
