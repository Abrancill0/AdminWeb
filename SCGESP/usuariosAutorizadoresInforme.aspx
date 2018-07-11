<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="usuariosAutorizadoresInforme.aspx.cs" Inherits="SCGESP.usuariosAutorizadoresInforme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Usuarios
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height:25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">
                <a id="btnUsu" href="/usuarios" role="button" class="btn btn-primary" aria-hidden='true'>Ver Lista de Usuarios</a>
                <table id="tblUsuarios" class="display browse" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th width="50px">Usuario</th>
                            <th>Nombre</th>
                            <th width="120px">No. Empleado</th>
                            <th width="250px">Nmb. Empleado</th>
                            <th width="120px" title="Solo puede existir un solo usuario principal">Aut. Principal</th>
                            <th width="120px" title="Usuario que puede autorizar en lugar del principal">Aut. Suplente</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="4" class="text-right">No. De Autorizadores:&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                            <td id="tdNoAutPrincipal"></td>
                            <td id="tdNoAutSuplente"></td>
                        </tr>
                    </tfoot>
                </table>

            </div>
        </div>
        
    </section>

    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript" src="js/usuariosAutInforme.js"></script>

</asp:Content>
