<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="requisiciones.aspx.cs" Inherits="SCGESP.requisiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Mis Requisiciones
        <a href="#" onclick='cerrarPanel(".panel")' style="color:#FFF;border-left:1px solid #FFF "  ><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">
                <form id="frmFiltrosRequsiciones" name="frmFiltrosRequsiciones" class="form-inline" action="#">
                    <table width="100%" id="filtro">
                        <tr>
                            <td >&nbsp;
                        <a id="anuevoa" class="btn btn-success" href="#" role="button"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;Nuevo</a>
                            </td>
                            <td style="text-align:right" >  
                                                                               <a id="btnReqPorAut" href="/requisicionesporautorizar" role="button" class="btn btn-primary" data-toggle='tooltip' data-placement='top' data-html='true' 
                                                  title='<div style=\"width: 170px;\">Requisiciones que requieren mi autorización</div>' aria-hidden='true'>
                            <i class="zmdi zmdi-search"></i> Ir a Requisiciones por autorizar
                        </a>
                                &nbsp;
                                    <a  id="refreshTbl"  class="btn btn-success" href="#" role="button"   style="color:#FFF"><i class="zmdi zmdi-refresh-alt"></i>&nbsp;Actualizar</a>


                                </td>

                        </tr>
                    </table>
                </form>

                <!--th width="80px">Monto</!--th>
                    <th width="80px">Disponible</th>
                    <th width="80px">Gastado</th-->
                <table id="tblRequisiciones" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                    <thead>
                        <tr>
                            <th width="10px">#</th>
                            <th width="250px">Justificaci&oacute;n</th>
                            <th width="250px">Responsable</th>
                            <th width="100px">Oficina</th>
                            <th width="100px">SubRamo</th>
                            <th width="250px">Estatus</th>
                            <th width="50px"></th>
                            <th width="50px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>

        <!--Modal Requisicion-->
        <div class="modal fade" id="verRequisiciones" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <!--style="width: 700px"-->
                    <div class="modal-header titulo-modal">                         
                            <table width="100%">
                                <tr>
                                    <td style="text-align:right">
                                        Requisiciones
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="text-align:right">
                                        <a  href="#" data-dismiss="modal" aria-label="Close" style="color:#FFF;border-left:1px solid #FFF ">&nbsp;<i class="zmdi zmdi-close"></i>Cerrar&nbsp;</a> 

                                    </td>
                                </tr>
                            </table>

                    </div>
                    <div class="modal-body">
                        <div id="btnAux"></div>
                        <!--a id="btndotacion" class="btn btn-success" href="#" role="button">Dotación</a-->
                        <form id="frmRequisicion" name="frmRequisicion" class="form-inline" action="#">
                            <table class="filtro" style="width: 50%">
                                <tr>
                                    <td>No. Requisición:</td>
                                    <td class="tdIdRequisicion"></td>
                                    <td class="tdBtnGeneraRequisicion">
                                        <button id="GeneraRequisicion" type="button" class="btn btn-primary"><span class="zmdi zmdi-check-all"></span>Genera Requisicion</button>
                                    </td>
                                </tr>
                            </table>
                            <div class="tab-container" style="width: 100%;">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li id="liSolicitud" class="active"><a data-toggle="tab" class="btn" href="#tabSolicitud">Solicitud <i id='fafasolicitar' class="" aria-hidden="true"></i></a></li>
                                    <!--fa fa-check-->
                                    <li id="liDetalleReq" onclick="guardadoAutomatico()" ><a data-toggle="tab" class="btn" href="#tabDetalleReq">Estimación <i id='fafapresupuesto' class="" aria-hidden="true"></i></a></li>

                                    <li id="liAutoriza" ><a data-toggle="tab" class="btn" href="#tabAutoriza">Flujo <i id='fafaautoriza' class="" aria-hidden="true"></i></a></li>
                                </ul>

                                <div class="tab-content">
                                    <div id="tabSolicitud" class="tab-pane fade in active">
                                        <table class="filtro" style="width: 100%">
                                            <tr>
                                                <td>Monto de Requisicion:</td>
                                                <td>
                                                    <label id="ReqMonto"></label>
                                                    <input type="hidden" id="idreq" name="idreq" value="0" />
                                                    <input type="hidden" id="monto" name="monto" style="width: 75%" onkeypress="return justNumbers(event);" class="form-control" placeholder="Monto $" readonly="readonly">
                                                    <!--div class="input-group">
                                                        <div class="form-group">
                                                            <span class="input-group-addon">
                                                                <i class="zmdi zmdi-money zmdi-hc-2x" style="padding: 3px 0px"></i>
                                                            </span>
                                                            <input type="text" id="monto" name="monto" style="width: 75%" onkeypress="return justNumbers(event);" class="form-control" placeholder="Monto $" readonly="readonly">
                                                            <i class="form-group__bar"></i>
                                                        </div>
                                                    </div-->
                                                </td>
                                                <td style="width: 80px"><!--Gastado:--></td>
                                                <td id="tdGastado" style="color: #b92c28; width: 100px"></td>
                                                <td style="width: 80px"><!--Disponible:--></td>
                                                <td id="tdDisponible" style="color: #419641; width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td>Justificacion:</td>
                                                <td colspan="5" id="tdJustificacion">
                                                    <span class="textotd "></span>
                                                    <div class="inputtd">
                                                        <div class="form-group">
                                                            <input type="text" id="RmReqJustificacion" name="RmReqJustificacion" style="width: 100%" class="form-control" placeholder="Favor de escribir la justificacion lo mas ampliamente posible - ¿Qué hara? ¿ Como lo Hara? ¿ Por que lo Hara?">
                                                            <i class="form-group__bar"></i>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tipo Requisición:</td>
                                                <td colspan="5" id="tdTipoRequisicion">
                                                    <span class="textotd"></span>
                                                    <span class="inputtd">
                                                        <select id="RmReqTipoRequisicion" name="RmReqTipoRequisicion" class="select2" data-width="100%"></select>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tipo de Gasto:</td>
                                                <td id="tdTipoGasto">
                                                    <span class="textotd "></span>
                                                    <span class="inputtd">
                                                        <select id="RmReqTipoDeGasto" name="RmReqTipoDeGasto" class="select2" data-width="100%" disabled></select>
                                                    </span>
                                                </td>
                                                <!--td>Forma de Pago:</!--td>
                                        <td><select id="formaPago" disabled="disabled" name="formaPago" class="select2" data-width="100%"></select>
                                            </td-->
                                                <td>Centro:
                                                </td>
                                                <td colspan="3" id="tdCentro">
                                                    <span class="textotd "></span>
                                                    <span class="inputtd">
                                                        <select id="RmReqCentro" name="RmReqCentro" class="select2" data-width="100%" onchange="" disabled ></select>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Oficina:</td>
                                                <td id="tdOficina">
                                                    <span class="textotd "></span>
                                                    <span class="inputtd">
                                                        <select id="RmReqOficina" name="RmReqOficina" class="select2" data-width="100%"></select>
                                                    </span>
                                                </td>
                                                <td>Subramo:
                                                </td>
                                                <td colspan="3" id="tdSubramo">
                                                    <span class="textotd "></span>
                                                    <span class="inputtd">
                                                        <select id="RmReqSubramo" name="RmReqSubramo" class="select2" data-width="100%">
                                                            <option value=''>- Subramo -</option>
                                                        </select>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>De:
                                                </td>
                                                <td>
                                                    <div class='input-group date' id='datetimepicker2'>
                                                        <input class="form-control reporte2" readonly="readonly" name="repde2" id="repde2" type="text" style="width: 80px;" />
                                                        <i class="form-group__bar"></i>
                                                        <span class="input-group-addon">
                                                            <span class="zmdi zmdi-calendar zmdi-hc-2x" style="padding: 3px 0px"></span>
                                                        </span>
                                                    </div>
                                                </td>
                                                <td>A:
                                                </td>
                                                <td colspan="3">
                                                    <div class="form-group">
                                                        <div class='input-group date' id='datetimepicker2'>
                                                            <input class="form-control reporte2" readonly="readonly" name="repa2" id="repa2" type="text" style="width: 80px;" />
                                                            <i class="form-group__bar"></i>
                                                            <span class="input-group-addon">
                                                                <span class="zmdi zmdi-calendar zmdi-hc-2x" style="padding: 3px 0px"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <input type="hidden" name="dias" id="dias" />
                                                    <input type="hidden" name="stAutorizado" id="stAutorizado" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><!--Estatus:-->
                                            <input type="hidden" id="idestatus" name="idestatus" value="0" />
                                                </td>
                                                <td id="tdEstatus" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td><!--Comentarios:--></td>
                                                <td id="tdComentarios" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="6"></td>
                                            </tr>
                                        </table>
                                        <div class="modal-footer account-controles">
                                            <button id="guardara" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
                                        </div>
                                    </div>

                                    <div id="tabDetalleReq" class="tab-pane fade">

                                        <table id="tblDetalleRequisicion" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                                            <thead>
                                                <tr>
                                                    <th width="10px">#</th>
                                                    <th>Categoria</th>
                                                    <th width="120px">Cantidad</th>
                                                    <th width="140px">Importe</th>
                                                    <th width="140px">SubTotal</th>
                                                    <th width="140px">Total C/IVA</th>
                                                    <th width="20px"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                            <tfoot>
                                            </tfoot>
                                        </table>
                                        <div id="selCtaPre" class='row account-controles'>
                                            Agregar Categoria &nbsp;
                                            <!--span class='help zmdi zmdi-help' data-toggle='tooltip' data-placement='top' data-html='true' title='LLena los tres campos y la categoria se agregara.' aria-hidden='true'></span-->
                                            <table class="filtro" border="1" style="border: 1px solid red; width: 100%;">
                                                <tr>
                                                    <td style="width:40%">
                                                        <select id="categoria" name="categoria" class="select2" data-width="100%">
                                                            <option value=''>- Categoria -</option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <!--div class="input-group" style="width: 80%; margin: 0px 20px;">
                                                            <div class="form-group form-group--float"-->
                                                                <input type="hidden" id="cantidadcat" name="cantidadcat" class="form-control" />
                                                                <!--label>Cantidad</label>
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                        <div-->
                                                    </td>
                                                    <td>
                                                        <div class="input-group" style="width: 80%; margin: 0px 20px;">
                                                            <div class="input-group">
                                                                <div class="form-group form-group--float">
                                                                    <input type="number" id="montocat" name="montocat" class="form-control" />
                                                                    <label>Importe $</label>
                                                                    <i class="form-group__bar"></i>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <button id="guardaDetalleReq" type="button" class="btn btn-primary" title='Guardar Categoria en la estimación.'><span class="glyphicon glyphicon-floppy-saved"></span> Guardar</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>


                                    </div>

                                    <div id="tabAutoriza" class="tab-pane fade">
                                        <table id="tblAutorizadoresReq" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                                            <thead>
                                                <tr>
                                                    <th width="10px"></th>
                                                    <th>Autorizador</th>
                                                    <th width="120px"></th>
                                                    <th width="120px"></th>
                                                    <th width="10px"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                            <tfoot>
                                            </tfoot>
                                        </table>
                                    </div>

                                </div>
                            </div>
                        </form>


                    </div>
                </div>
            </div>
        </div>

    </section>

    <!-- App functions and actions -->
    <script type="text/javascript" src="js/app.min.js"></script>
    <script type="text/javascript" src="js/js.js"></script>
    <script type="text/javascript" src="js/requisiciones.js?v.hnvghg19"></script>

</asp:Content>
