<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Autorizaciones.aspx.cs" Inherits="SCGESP.Autorizaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<style type="text/css">

		</style>
    <section class="content">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Autorizaciones Requisición por Comprobar
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
            </div>
            <div class="panel-body">

                <div id="AlertInfoDisp" class="alert alert-danger" hidden="hidden">
                    <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                    <span id="infoDisp"></span>
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                </div>
				<div id="divListInformes" style="padding: 0px; width: 100%; margin: 0 auto; white-space: nowrap; display: block;">
					<table id="tblProyectos" class="display browse" style="overflow-x: auto; white-space: nowrap;" cellspacing="0" data-page-length="10">
                    <thead>
                        <tr style="text-align:center;">
                            <th width="50px" style="text-align:center;">Requisici&oacute;n</th>
                            <th width="40px" style="text-align:center;">Informe</th>
                            <th width="200px" style="text-align:center; width: 200px !important;">Justificaci&oacute;n</th>
                            <th width="100px" style="text-align:center;">Importe<br />Autorizado</th>
                            <th width="100px" style="text-align:center;">Empleado</th>
							<th width="100px" style="text-align:center;">Estatus</th>
                            <th width="20px"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
</div>
            </div>
        </div>
        
        <!-- Modal Comprobante -->
        <div class="modal fade" id="verComprobante" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header titulo-modal">
                        <h4 class="modal-title" id="ModalLabelComprobante">Ver</h4>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">

                        <div id="controles_compXML" align="center">
                        </div>
                        <div id="print_compXML" style="width: 100%; height: 100%">
                            <table class="imprimir" style="width: 100%">
                                <tr>
                                    <td id="version" class="text-right" colspan="4"></td>
                                </tr>
                                <tr>
                                    <td id="nmbEmisor" colspan="2"></td>
                                    <td>Folio Fiscal:</td>
                                    <td id="folioFiscal" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td width="110px">RFC Emisor</td>
                                    <td id="rfcEmisor" style="font-weight: normal; font-size: 12px"></td>
                                    <td>No de Serie del CSD:</td>
                                    <td id="noSerie" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="top">
                                        <p>Domicilio Fiscal del Emisor:</p>
                                        <p id="domFiscalEmisor" style="font-weight: normal; font-size: 12px"></p>
                                    </td>
                                    <td colspan="2" valign="top">
                                        <p>Lugar, Fecha y hora de emisión:</p>
                                        <p id="lugarHora" style="font-weight: normal; font-size: 12px"></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Sucursal:</td>
                                    <td id="sucursal" style="font-weight: normal; font-size: 12px"></td>
                                    <td>Efecto del Comprobante:</td>
                                    <td id="efectoComprobante" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td>RFC Receptor:</td>
                                    <td id="rfcReceptor" style="font-weight: normal; font-size: 12px"></td>
                                    <td>Folio y Serie</td>
                                    <td id="folioSerie" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td id="nmbRemisor" colspan="2"></td>
                                    <td colspan="2" valign="top">
                                        <p>Régimen Fiscal:</p>
                                        <p id="regimenFiscal" style="font-weight: normal; font-size: 12px"></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="domFiscalReceptor" colspan="4" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                            </table>

                            <table id="tblConceptosXML" class="display browse imprimir" cellspacing="0" width="100%">
                                <thead>
                                    <tr>
                                        <th>Cantidad</th>
                                        <th>Unidad de Medida</th>
                                        <!--th>Número de Identificación</th-->
                                        <th>Descripción</th>
                                        <th>Precio Unitario</th>
                                        <th>Importe</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="6"></td>
                                    </tr>
                                </tbody>
                            </table>

                            <table class="imprimir" style="width: 100%; word-wrap: break-word; table-layout: fixed;">
                                <tr>
                                    <td width="150px">Motivo del Descuento:</td>
                                    <td id="motivoDescuento" colspan="3" style="font-weight: normal; font-size: 12px"></td>
                                    <td width="250px" rowspan="6" valign="top">
                                        <table class="imprimir" style="width: 100%">
                                            <tr>
                                                <td width="70px">Subtotal:</td>
                                                <td id="subtotal" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Impuestos Trasladados</td>
                                            </tr>
                                            <!--tr><td id="lblIVA">IVA 16 %</td><td id="iva" style="font-weight: normal; font-size: 12px" align="right"></td></tr-->
                                            <tr>
                                                <td id="lblImp1"></td>
                                                <td id="imp1" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td id="lblImp2"></td>
                                                <td id="imp2" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td id="lblImp3"></td>
                                                <td id="imp3" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td id="lblImp4"></td>
                                                <td id="imp4" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td id="lblImp5"></td>
                                                <td id="imp5" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td id="lblImp6"></td>
                                                <td id="imp6" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                            <tr>
                                                <td>Total</td>
                                                <td id="total" style="font-weight: normal; font-size: 12px" align="right"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Moneda:</td>
                                    <td id="moneda" style="font-weight: normal; font-size: 12px"></td>
                                    <td width="120px">Tipo de cambio:</td>
                                    <td id="tipoCambio" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td id="lblformaPago">Forma de Pago:</td>
                                    <td id="formaPago" colspan="3" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td id="lblmetodoPago">Método de Pago:</td>
                                    <td id="metodoPago" colspan="3" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td>Número de cuenta de Pago:</td>
                                    <td id="nCuentaPago" colspan="3" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td>Condiciones de Pago:</td>
                                    <td id="condPago" colspan="3" style="font-weight: normal; font-size: 12px"></td>
                                </tr>
                                <tr>
                                    <td colspan="5">Total con letra:</td>
                                </tr>
                                <tr>
                                    <td id="totalLetra" style="font-weight: normal; font-size: 12px" colspan="5"></td>
                                </tr>
                                <tr>
                                    <td colspan="5">Sello digital del CFDI:</td>
                                </tr>
                                <tr>
                                    <td style="font-weight: normal; font-size: 12px" colspan="5">
                                        <p id="SD_CFDI" style='font-weight: normal'></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">Sello del SAT:</td>
                                </tr>
                                <tr>
                                    <td style="font-weight: normal; font-size: 12px" colspan="5">
                                        <p id="S_SAT" style='font-weight: normal'></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="5">
                                        <img id="qrxml" src="" alt="QR" style="display: block; margin: 0 auto;" />
                                    </td>
                                    <td colspan="4">Cadena Original del complemento de certificación digital del SAT</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <p id="COCCD_SAT" style='font-weight: normal'></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">No de Serie del Certificado del SAT: <span id="noSC_SAT" style="font-weight: normal; font-size: 12px"></span></td>
                                </tr>
                                <tr>
                                    <td colspan="4">Fecha y hora de certificación: <span id="FH_certificacion" style="font-weight: normal; font-size: 12px"></span></td>
                                </tr>
                            </table>
                        </div>

                        <div id="divPDF">
                            <div id="controles_compPDF" align="center"></div>
                            <object id="compPDF" data="" type="application/pdf" width="100%" height="500px"></object>
                        </div>

                        <div id="controles_compOTRO" align="center">
                        </div>
                        <div id="print_compOTRO" style="width: 100%; height: 100%; padding:10px;">
                            <img id="compOTRO" src="" alt="Comprobante" class="img-thumbnail" />
                        </div>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Datos Adicionales -->
        <div class="modal fade" data-modal-color="bluegray" id="verDatosAdicionales" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header titulo-modal">
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                        Datos Adicionales
                    </div>
                    <div class="modal-body">

                        <form id="frmDatosAdicionales" name="informe" class="form-inline" action="#">
                            <table style="width: 100%; border:hidden">
                                <tr class="hidden">
                                    <td style="width:100px;">No. Comensales</td>
                                    <td colspan="3">
                                <div class="input-group mb-3 form-group form-group--float text-left" style="margin-top: 0px;margin-bottom: 10px;">
                                    <input type="number" id='ncomensalesda' name='ncomensalesda' min="0" max="50" step="1" class='form-control' placeholder="No. Comensales" style="width:20%" />
                                    <i class="form-group__bar"></i>
                                </div>
                                    </td>
                                </tr>
                                <tr><td>Nmb. Comensales</td><td colspan="3">
                                    <div id="inpComensalesda" style="width:100%"></div>
                                    </td>
                                </tr>
								<!--
                                <tr>
                                    <td>R.F.C</td>
                                    <td>
                                        <input type="hidden" id="idgasto" name="idgasto" />
                                        <input type="hidden" id="idinforme" name="idinforme" />
                                        <input type="hidden" id="idproyecto" name="idproyecto" />
                                        <div class="form-group">
                                            <input type="text" id="rfc" name="rfc" style="width: 100%" maxlength="15" class="form-control" placeholder="R.F.C.">
                                            <i class="form-group__bar"></i>
                                        </div>
                                    </td>
                                    <td>Contacto</td>
                                    <td>
                                        <div class="form-group">
                                            <input type="text" id="contacto" name="contacto" style="width: 100%" maxlength="100" class="form-control" placeholder="Contacto">
                                            <i class="form-group__bar"></i>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Telefono</td>
                                    <td>
                                        <div class="form-group">
                                            <input type="text" id="telefono" name="telefono" style="width: 100%" maxlength="15" class="form-control" placeholder="Telefono">
                                            <i class="form-group__bar"></i>
                                        </div>
                                    </td>
                                    <td>Correo</td>
                                    <td>
                                        <div class="form-group">
                                            <input type="text" id="correo" name="correo" style="width: 100%" maxlength="100" class="form-control" placeholder="Correo Electronico">
                                            <i class="form-group__bar"></i>
                                        </div>
                                    </td>
                                </tr>
									-->
                            </table>
                        </form>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                        <button id="btnDA" type="button" class="btn btn-primary" onclick="GuardarDatosAdicionales()"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
                    </div>
                </div>
            </div>
        </div>
       
        <!-- Modal ver Informe -->
        <div id="verInformeGastos" class="modal fade"  tabindex="1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-xlg" role="document">
                <div class="modal-content">
                    <div class="modal-header titulo-modal">
                        <!--style="height: 50px;"-->
                        Informe
                <table cellspacing="0" width="80%">
                    <tr>
                        <td valign="middle">
                            <!--Opciones-->
                             <input id="HFUsuariovobo" type="hidden" />
                             <a id="aenvia" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-envelope"></span> Enviar a Comprobacion</a>
                             <a id="aautoriza" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-envelope"></span> Autorizar</a>
                             <a id="arechaza" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-remove"></span> Rechazar</a>
                             <a id="autorizadores" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-user"></span> Enviar a Autorizadores</a>

                            <a id="averhorag" class="btn btn-primary btn-md" href="#" role="button" data-placement='top' data-html='true'
                                title="<div style='width: 170px;'>Ver la hora en que se realizo el Gasto.</div>" aria-hidden='true'><span class="zmdi zmdi-time"></span>Ver Hora Gasto</a>

                            <!--a id="aagregarg" class="btn btn-warning btn-md" href="#" role="button"><span class="glyphicon glyphicon-plus"></span> Agregar Gasto</a-->

                        </td>
                    </tr>
                </table>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div-- class="modal-body" style="width: 100%">
                        <!--id="mInformeGastos"-->

                        <div class="card" style="margin: 0px; padding: 0px;">
                            <div class="card-header card-info" role="tab" style="margin: 0px; padding: 2px 5px;">
                                
                            </div>
                            <div id="infoInforme" class="collapse in" style="margin: 0px; padding: 0px;">
                                <div class="card-block" style="margin: 0px; padding: 2px;">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6">
                                            <div id="verinforme" name="informe" class="form-inline">
                                                <table class="filtro text-left" style="width: 99%; text-align: left">
                                                    <tr>
                                                        <td width="110px">No. Requisicion</td>
                                                        <td colspan="3">
															<table>
																<tr>
																	<td>
																		<label id="lblRequisicion"></label>
																	</td>
																	<td style="padding: 0px 30px;">No. Informe:</td>
																	<td>
																		<label id="tdninforme"></label>
																	</td>
																</tr>
															</table>
                                                            
                                                        </td>
                                                        <!--td>Proyecto:</td>
                                                        <td>
                                                            <div class="form-group">
                                                                <input type="text" id="proycontable" name="proycontable" style="width: 100%" class="typeahead form-control" placeholder="Nombre Proyecto">
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                            <label id="lblproycontable"></label>
                                                        </td-->
                                                    </tr>
                                                    <tr>
                                                        <td>Justificaci&oacute;n:</td>
                                                        <td>
                                                            <input type="hidden" id="idinforme" name="idinforme" />
                                                            <input type="hidden" id="idproyecto" name="idproyecto" />
                                                            <input type="hidden" id="estatus" name="estatus" />
                                                            <input type="hidden" id="ConfBanco" name="ConfBanco" />
                                                            <input type="hidden" id="inputCabeceraFormaPago" name="inputCabeceraFormaPago" />
															<input id='totalg' type='hidden' value='0'/>
															<input id='montog' type='hidden' value='0'/>
                                                            <div class="form-group">
                                                                <input type="text" id="proyecto" readonly="readonly" name="proyecto" style="width: 100%" class="form-control" placeholder="Nombre Informe">
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                            <label id="lblproyecto"></label>
                                                        </td>
                                                        <!--td width="70px">Motivo:</!--td>
                                                        <td>
                                                            <div class="form-group">
                                                                <input type="text" id="motivo" readonly="readonly" name="motivo" style="width: 100%" class="form-control" placeholder="Motivo...">
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                            <label id="lblmotivo"></label>
                                                        </td-->
                                                    </tr>
                                                    <tr>
                                                        <td>Responsable:</td>
                                                        <td>
                                                            <div id="divresponsablever">
                                                                <select id="responsablever" disabled="disabled" name="responsablever" class="select2" data-width="200px">
                                                                </select>
                                                                <input type="hidden" id="usuResponsable" name="usuResponsable" value="" />
                                                            </div>
                                                            <label id="lblresponsablever"></label>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            
                                                        </td>
                                                    </tr>
                                                    <!--tr>
                                                        <td>Nota:</td>
                                                        <td colspan="3">
                                                            <div class="form-group">
                                                                <input type="text" id="nota" readonly="readonly" name="nota" style="width: 100%" class="form-control" placeholder="Notas...">
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                            <label id="lblnota"></label>
                                                        </td>
                                                    </tr-->
                                                    <tr>
                                                        <td>Inicio:</td>
                                                        <td colspan="3">
															<table>
																<tr>
																	<td>
																		<label id="tddel"></label>
																	</td>
																	<td style="padding: 0px 30px;">Vigencia:</td>
																	<td>
																		<label id="tdal"></label>
																	</td>
																</tr>
															</table>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdcomAut" colspan="4"></td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdConBancaria" colspan="4"></td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdContabilizar" colspan="4"></td>
                                                    </tr>
													<tr>
														<td>Estatus:</td>
                                                        <td colspan="3">
                                                            <label id="tdestatus"></label>
                                                        </td>
													</tr>
                                                </table>
                                            </div>
                                        </div>

                                        <div class="hidden-xs col-xs-12 col-sm-12 col-md-12 col-lg-6">
                                            <table id="filtro" class="display" cellspacing="0" width="100%" data-page-length="10">
                                                <tr>
                                                    <td colspan="3" align="center">Importes Requisicí&oacute;n</td>
                                                </tr>
                                                <tr>
                                                    <td width="25%">Autorizado</td>
                                                    <td width="25%" align="center">Gastado</td>
                                                    <td width="25%">Por Comprobar<input type="hidden" id="disAnticipo" name="disAnticipo" value="0" /></td>
                                                    <td width="25%" align="center">Decrementado</td>
                                                </tr>
                                                <tr style="height: 31px;">
                                                    <td id="tdanticipo" align="right"></td>
                                                    <td id="tdgastado" align="right"></td>
                                                    <td id="tddisponible" align="right"></td>
                                                    <td id="tddecrementado" align="right"></td>
                                                </tr>
                                            </table>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                        <input id="HFRmRdeRequisicion" type="hidden" />
                        <!--Lista de gastos del informe-->
                        <!--div id="gastos" class="row inner-content scrollbar-dynamic" style="width: 100%; margin: 0px; padding: 0px;"-->
                            <form id='nuevoGasto' name='nuevoGasto' style="padding: 0px; width: 100%; margin: 0 auto; white-space: nowrap;" class='form-inline' action='#' method="POST" enctype='multipart/form-data'>                                
					<table id="tblGastos" class="browse display nowrap" style="overflow-x: auto; white-space: nowrap;" cellspacing="0" data-page-length="-1">
                                    <!--data-page-length="25"-->
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th style='width: 80px;'>Día</th>
                                            <th style='width: 40px;'>Hora</th>
                                            <th>Justificaci&oacute;n</th>
											<th>Categoria</th>
                                            <th>Proveedor</th>
                                            <th>Gasto $</th>
                                            <!--th>F. de Pago</th-->
                                            <th>XML</th>
                                            <th>PDF</th>
                                            <th>IMG</th>
                                            <th>Monto</th>
                                            <th title="Importe aceptable">Aceptable</th>
                                            <th title="Importe fuera de política">Fuera de pol&iacute;tica</th>
                                            <th title="Importe no deducible">No Deducible</th>
                                            <th>Objetivo / Comensales</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot>
                                    </tfoot>
                                </table>
                                <div id="inputnuevogasto"></div>
                            </form>
                        <!--/div-->
                    </div>
                </div>
            </div>
        </div>

        <div id="tabAutoriza" class="modal fade" tabindex="1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <!--style="width: 700px"-->
                    <div class="modal-header titulo-modal">
                        Autorizadores
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
                    </div>
                    <div class="modal-body">
                        <label id="lblAutOpcional">
                            Autorizadores Opcionales:
                            <select id="mAutOpcional" name="mAutOpcional" class="select2" data-width="100%"></select>
                        </label>
                        <table id="tblAutOpcional" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                            <thead>
                                <tr>
                                    <th width="10px"></th>
                                    <th>Autorizador</th>
                                    <th width="10px"></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                            </tfoot>
                        </table>

						<!--
                        <table id="tblAutorizadoresReq" class="display browse" cellspacing="0" width="100%" data-page-length="10">
                            <thead>
                                <tr>
                                    <th colspan="3">Autorizadores Default</th>
                                </tr>
                                <tr>
                                    <th width="10px"></th>
                                    <th>Autorizador</th>
                                    <th width="10px"></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                            </tfoot>
                        </table>
						-->
                    </div>
                      <div class="modal-footer account-controles">
                        <button id="EnviarAutorizadores" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Enviar</button>
                    </div>
                </div>
            </div>
        </div>



    </section>

    <!-- App functions and actions -->
    <script src="js/app.min.js"></script>
    <script src="js/js.js"></script>
    <script src="js/Autorizaciones.js?1q1q11q"></script>
    <script src="js/AutorizaInformes.js?1q1q1x1q"></script>

   

</asp:Content>
