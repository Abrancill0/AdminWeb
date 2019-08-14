<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="perfil.aspx.cs" Inherits="SCGESP.perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <!--Perfil
                            <a href="#" id="refreshTbl" style="margin: 0px 68px 0px 0px; height:25px" class="btn btn-primary btn-sm actions__item zmdi zmdi-refresh"></a>
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
                -->
            </div>
            <div class="panel-body">

                <div class="card" id="profile-main">
                    <div class="pm-overview c-overflow">

                        <div class="pmo-pic">
                            <div class="p-relative">
                                    <img class="img-perfil" src="img/usuarios/default.png" alt="">

                            </div>

                            <div class="pmo-stat">
                                <h4 class="c-white SgUsuNombre" style="color:white"></h4>
                                <b><span class="m-0 SgUsuId" style="color:darkgray;">1562</span></b>                                
                            </div>
                        </div>

                        <!--div class="pmo-block pmo-contact hidden-xs">
                            <h2>Contacto</h2>

                            <ul>
                                <li><i class="zmdi zmdi-phone"></i>00971 12345678 9</li>
                                <li><i class="zmdi zmdi-email"></i>malinda-h@gmail.com</li>
                                <li>
                                        <i class="zmdi zmdi-pin"></i>
                                        <address class="m-b-0 ng-binding">
                                            44-46 Morningside Road,<br>
                                            Edinburgh,<br>
                                            Scotland
                                        </address>
                                    </li>
                            </ul>
                        </div-->

                    </div>

                    <div class="pm-body clearfix">
                        <ul class="tab-nav tn-justified">
                            <li class="alert-success-pm"><a>Mi Perfil</a></li>
                            <li><a href="/empresa">Mi Empresa</a></li>
                        </ul>

                        <div class="pmb-block">
                            <div class="pmbb-header">
                                <h2><i class="zmdi zmdi-account m-r-10"></i> Información Basica de Usuario</h2>
                            </div>
                            <div class="pmbb-body p-l-30">
                                <div class="pmbb-view">
                                    <dl class="dl-horizontal">
                                        <dt>No. Empleado</dt>
                                        <dd class="GrEmpId bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Nombre Emp.</dt>
                                        <dd class="GrEmpNombre bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Apellido Paterno</dt>
                                        <dd class="GrEmpApellidoPaterno bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Apellido Materno</dt>
                                        <dd class="GrEmpApellidoMaterno bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Fecha Registro</dt>
                                        <dd class="GrEmpFechaAlta bold"></dd>
                                    </dl>
                                    
                                </div>

                            </div>
                        </div>

                        <div class="pmb-block">
                            <div class="pmbb-header">
                                <h2><i class="zmdi zmdi-phone m-r-10"></i> Contacto</h2>
                            </div>
                            <div class="pmbb-body p-l-30">
                                <div class="pmbb-view">
                                    <dl class="dl-horizontal">
                                        <dt>Correo Electronico</dt>
                                        <dd class="GrEmpCorreoElectronico bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Telefono Oficina</dt>
                                        <dd class="GrEmpTelefonoOficina bold"></dd>
                                    </dl>
                                </div>
                            </div>
                        </div>

                        <div class="pmb-block">
                            <div class="pmbb-header">
                                <h2><i class="zmdi zmdi-file m-r-10"></i> Información Predeterminada</h2>
                            </div>
                            <div class="pmbb-body p-l-30">
                                <div class="pmbb-view">
                                    <dl class="dl-horizontal">
                                        <dt>Centro</dt>
                                        <dd class="GrEmpCentroNombre bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Oficina</dt>
                                        <dd class="GrEmpOficinaNombre bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Tipos de Gasto</dt>
                                        <dd class="GrEmpTipoGastoNombre bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Tarjeta Toka</dt>
                                        <dd class="GrEmpTarjetaToka bold"></dd>
                                    </dl>
                                </div>
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
    <script type="text/javascript" src="js/perfil.js?123"></script>


</asp:Content>
