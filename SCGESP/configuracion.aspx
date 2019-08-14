﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="configuracion.aspx.cs" Inherits="SCGESP.configuracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>


    </style>

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Configuración
                <a href="#" onclick='cerrarPanel(".panel")' style="color:#FFF;border-left:1px solid #FFF "><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <div id="tabsConfiguracion">
                    <ul>
                        <li><a href="#tabVoBo">VoBo</a></li>
                        <li><a href="#tabAutorizadores">Autorizadores de Requisiciones de Viaje</a></li>
                    </ul>
                    <div id="tabVoBo">
                        <div class="panel panel-default">
                            <div class="panel-heading">Usuario Visto Bueno</div>
                            <div class="panel-body">
                                <table width="100%">
                                    <tr>
                                        <td>Usuario:</td>
                                        <td colspan="3">
                                            <select id="SgUsuId" name="SgUsuId" class="select2" data-width="100%">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Validar Importe:</td>
                                        <td>
                                            <label class='custom-control custom-checkbox'>
                                                <input type='checkbox' id='ChkVoBoImporte' class='custom-control-input'>
                                                <span class='custom-control-indicator'></span>
                                                <span class='custom-control-description'>Validar solo requisiciones con un importe mayor o igual a: </span>
                                            </label>
                                        </td>
                                        <td>Importe:</td>
                                        <td>
                                            <input type="number" id='importemayor' name='importemayor' min="0" max="1000000" step="1" class='form-control' placeholder="0.00" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Control Activo:</td>
                                        <td colspan="3">
                                            <label class='custom-control custom-checkbox'>
                                                <input type='checkbox' id='ChkVoBoActivo' class='custom-control-input'>
                                                <span class='custom-control-indicator'></span>
                                                <span class='custom-control-description'>Mostrar Control Siempre Activo</span>
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Control Bloqueado:</td>
                                        <td colspan="3">
                                            <label class='custom-control custom-checkbox'>
                                                <input type='checkbox' id='ChkVoBoBloqueado' class='custom-control-input'>
                                                <span class='custom-control-indicator'></span>
                                                <span class='custom-control-description'></span>
                                                <span class='custom-control-description'>Mostrar Control Siempre Bloqueado</span>
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Ejemplo:</td>
                                        <td colspan="3">
                                            <img id="chkVoBo" src="img/chkVoBo/chkActivoBloqueado.PNG" class="img-responsive" style="width: 250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align:right">
                                            <button id="btnGuardaVoBo" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id="tabAutorizadores">
                        <table id="filtro" width="100%" >
						    <tr>
							    <td style="text-align:left"> &nbsp;
                                     <a id="btnUsu" href="/usuarios" role="button" class="btn btn-primary"  aria-hidden='true'>Ver Lista de Usuarios</a>

							
                                <%--<a class="btn btn-primary" href="#" role="button" onclick="Historico()"><i class="zmdi zmdi-refresh-alt"></i>Historico</a>--%>
                                </td>
                                <td style="text-align:right" >  
                                    <a  id="refreshTbl"  class="btn btn-success" href="#" role="button"   style="color:#FFF"><i class="zmdi zmdi-refresh-alt"></i>&nbsp;Actualizar</a>


                                </td>
                                </tr>
  					    </table>
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

            </div>
        </div>

    </section>

    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript" src="js/vobo.js?11"></script>
    <script type="text/javascript" src="js/usuariosAutInforme.js?kjh"></script>

    <script type="text/javascript">
        $("#tabsConfiguracion").tabs()
    </script>

</asp:Content>
