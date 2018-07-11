<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="empresa.aspx.cs" Inherits="SCGESP.empresa" %>
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
                                    <img class="img-perfil img-responsive" src="img/usuarios/default.png" alt="">

                                <label for="fileFotoUsuario" class="pmop-edit">
                                    <i class="zmdi zmdi-camera"></i><span
                                        class="hidden-xs">Actualizar Foto</span>
                                    <input id="fileFotoUsuario" name="fileFotoUsuario" class="hidden" type="file" accept="image/*"/>
                                </label>
                            </div>

                            <div class="pmo-stat">
                                <h2 class="m-0 c-white SgUsuId">1562</h2>
                                <span class="SgUsuNombre"></span>
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
                            <li><a href="/perfil">Mi Perfil</a></li>
                            <li class="active alert-success"><a>Mi Empresa</a></li>
                        </ul>

                        <div class="pmb-block">
                            <div class="pmbb-header">
                                <h2><i class="zmdi zmdi-file-text m-r-10"></i> Datos Fiscales</h2>
                            </div>
                            <div class="pmbb-body p-l-30">
                                <div class="pmbb-view">
                                    <dl class="dl-horizontal">
                                        <dt>Raz&oacute;n Social</dt>
                                        <dd class="GrConRazonSocial bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>R.F.C.</dt>
                                        <dd class="GrConRfc bold"></dd>
                                    </dl>
                                </div>

                            </div>
                        </div>

                        <div class="pmb-block">
                            <div class="pmbb-header">
                                <h2><i class="zmdi zmdi-pin m-r-10"></i> Domicilio Fiscal</h2>
                            </div>
                            <div class="pmbb-body p-l-30">
                                <div class="pmbb-view">
                                    <dl class="dl-horizontal">
                                        <dt>Calle</dt>
                                        <dd class="GrConCalle bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>No. Exterior</dt>
                                        <dd class="GrConNumExt bold"></dd>
                                    </dl>

                                    <dl class="dl-horizontal">
                                        <dt>Colonia</dt>
                                        <dd class="GrConColonia bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Ciudad</dt>
                                        <dd class="GrConCiudad bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Estado</dt>
                                        <dd class="GrConEstado bold"></dd>
                                    </dl>
                                    <dl class="dl-horizontal">
                                        <dt>Código Postal</dt>
                                        <dd class="GrConCodigoPostal bold"></dd>
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
    <script type="text/javascript" src="js/empresa.js?123"></script>

</asp:Content>
