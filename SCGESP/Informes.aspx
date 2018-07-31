<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Informes.aspx.cs" Inherits="SCGESP.Informes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<section class="content">
		<div class="panel panel-primary">
			<div class="panel-heading">
				Mis Requisiciones de Viaje
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">

				<div id="AlertInfoDisp" class="alert alert-danger" hidden="hidden">
					<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
					<span id="infoDisp"></span>
					<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
				</div>

				<form id="frmFiltrosInforme" name="informes" class="form-inline" action="#">
					<table id="filtro">
						<tr>
							<td>&nbsp;
                                <a id="btnBuscar" class="btn btn-primary" href="#" role="button" onclick="ObtenerInformes()"><i class="zmdi zmdi-refresh-alt"></i>Actualizar</a>
							</td>

					</table>
				</form>
				<br />
				<div id="divListInformes" style="padding: 0px; width: 100%; margin: 0 auto; white-space: nowrap; display: block;">
					<table id="tblProyectos" class="display browse" style="overflow-x: auto; white-space: nowrap;" cellspacing="0" data-page-length="10">
						<thead>
							<tr>
								<th width="70px">Requisici&oacute;n</th>
								<th width="70px">Informe</th>
								<th width="200px">Justificaci&oacute;n</th>
								<th width="100px" title="Importe solicitado en la requisición">Importe<br />Autorizado</th>
								<th width="100px">Empleado</th>
								<th width="70px">Estatus</th>
								<th width="20px"></th>
								<th width="20px"></th>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
				</div>
			</div>
		</div>


		<!-- Modal Crear informe -->
		<div class="modal fade" id="frmInforme" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">

						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
					</div>
					<div class="modal-body">
						<form id="informe" name="informe" class="form-inline" action="#">
							<table id="filtro" style="width: 100%">
								<tr>
									<td>Nombre:</td>
									<td>
										<div class="form-group">
											<input type="text" id="proyecto" name="proyecto" style="width: 100%" class="form-control" placeholder="Nombre Informe">
											<i class="form-group__bar"></i>
										</div>
									</td>
									<td>Motivo:</td>
									<td>
										<div class="form-group">
											<input type="text" id="motivo" name="motivo" style="width: 100%" class="form-control" placeholder="Motivo...">
											<i class="form-group__bar"></i>
										</div>
									</td>
								</tr>
								<tr>
									<td>Mes:</td>
									<td colspan="3">
										<div class="form-group" style="width: 50%">
											<select id="mes" name="mes" class="select2" data-width="100%" data-minimum-results-for-search="Infinity">
											</select>
										</div>
									</td>
								</tr>
								<tr>
									<td>Tipo:</td>
									<td colspan="3">
										<select id="tipoInf" name="tipoInf" class="select2" data-width="150px">
										</select>
									</td>
								</tr>
								<tr>
									<td>Responsable:</td>
									<td colspan="3">
										<select id="responsable" name="responsable" class="select2" data-width="200px">
										</select>

									</td>
								</tr>
							</table>
						</form>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
						<button type="button" class="btn btn-primary" onclick="GuardarInforme('I')"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal Agregar Gasto a Informe -->
		<div class="modal fade" id="mAgregarGastoInf" data-modal-color="bluegray" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						Nuevo Gasto
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
					</div>
					<div class="modal-body">
						<div id="preInputGasto" style="display: block; position: absolute; margin: 0px 40%; top: 50%; z-index: 100;">
							<img src="img/loader.gif" class="img-responsive" width="50px" style="margin: 0 auto;" />
						</div>
						<div id="inpustGasto">
							<div class="form-group form-group--float text-left" style="margin-bottom: 10px;">
								<input type='text' id='fgasto' name='fgasto' class='form-control input-mask' data-mask="00-00-0000" onkeypress="return bloqueaTeclado(event)" onchange="$(this).focus();" />
								<label class="bold">Fecha Gasto</label>
								<i class="form-group__bar"></i>
							</div>

							<label class="bold" style="font-weight: bold; display: block; margin: 0px 0px 0px 35px;">Hora Gasto</label>
							<div class="text-left input-group bootstrap-timepicker timepicker" style="margin: 0px; padding: 0px;">
								<input id="hgasto" type="text" class="form-control input-small input-mask" on data-mask="00:00">
								<i class='form-group__bar'></i>
							</div>

							<div class="form-group form-group--float text-left">
								<input type='text' id='concepto' name='concepto' class='form-control' style='width: 100%;' />
								<label class="bold">Justificaci&oacute;n</label>
								<i class="form-group__bar"></i>
							</div>
							<div class="form-group form-group--float text-left">
								<input type='text' id='negocio' name='negocio' class='form-control' style='width: 100%;' />
								<label class="bold">Proveedor</label>
								<i class="form-group__bar"></i>
							</div>
							<div class="form-group form-group--float text-left" style="margin-bottom: 10px;">
								<input type='text' id='totalng' name='totalng' class='form-control' onkeypress='return justNumbers(event);' style='width: 100%;' />
								<label class="bold">$ Monto</label>
								<i class="form-group__bar"></i>
							</div>

							<label for="categoria" style="width: 100%;">
								<span style="font-weight: bold; margin: 0px 0px 0px 35px;">Forma de Pago</span>
								<div class="input-group" style="background-color: #ffffff">
									<span class="input-group-addon"
										style="background: url('img/iconos/FormaPago.png') no-repeat; width: 35px"></span>
									<div class="form-group">
										<select id='formapago' name='formapago' data-width='100%' class="select2">
											<option value='0'>- Forma de Pago -</option>
										</select>
										<i class="form-group__bar"></i>
									</div>
								</div>
							</label>
							<label for="categoria" style="width: 100%;">
								<span style="font-weight: bold; margin: 0px 0px 0px 35px;">Categoria</span>
								<div class="input-group" style="background-color: #ffffff">
									<span class="input-group-addon"
										style="background: url('img/iconos/Cuenta.png') no-repeat; width: 35px"></span>
									<div class="form-group">
										<select id='categoria' name='categoria' data-width='100%' class="select2">
											<!--option value='0'>- categoria -</option-->
										</select>
										<i class="form-group__bar"></i>
									</div>
								</div>
							</label>
							<div class="form-group form-group--float text-left" style="margin-bottom: 10px;">
								<input type='text' id='observaciones' name='observaciones' class='form-control' style='width: 100%;' />
								<label class="bold">Objetivo</label>
								<i class="form-group__bar"></i>
							</div>
							<div class="form-group form-group--float text-left" style="margin-bottom: 10px;">
								<input type='text' id='comensalesInsert' name='comensalesInsert' class='form-control' style='width: 100%;' />
								<label class="bold">Comensales <small>(Comensal 1, Comensal 2, Comensal N...)<//small></label>
								<i class="form-group__bar"></i>
							</div>
							<!--
							<div class="form-group text-left">
								<span style="font-weight: bold; margin: 0px 0px 0px 35px;">No. Comensales</span>
								<div class="input-group mb-3 form-group form-group--float text-left" style="margin-top: 0px; margin-bottom: 10px;">
									<div class="input-group-prepend">
										<button class="btn btn-danger" type="button" onclick="masmenoscomensal(-1)" style="padding: 10px 10px;"><i class="zmdi zmdi-minus zmdi-hc-lg"></i></button>
									</div>
									<input type="number" id='ncomensales' name='ncomensales' min="0" max="50" step="1" class='form-control' placeholder="No. Comensales" />
									<i class="form-group__bar"></i>
									<div class="input-group-append">
										<button class="btn btn-success" type="button" onclick="masmenoscomensal(1)" style="padding: 10px 10px;"><i class="zmdi zmdi-plus zmdi-hc-lg"></i></button>
									</div>
								</div>
								<div id="inpComensales" style="width: 100%">
								</div>
							</div>
							-->
							<div class="row btn-demo">
								<div class="col-md-3">
									<input type='file' id='fileotro' name='fileotro' accept='image/*' data-btnclass="btn-primary" />
								</div>
								<div class="col-md-9">
									<a id="btnInpDA" class="btn btn-primary"
										onclick="if ($(this).attr('datos-adicionales') === 'false') {
                                           $('#inpDA').show('slow');
                                           $(this).attr('datos-adicionales', 'true');
                                            creaInputsComensales([]);
                                       } else {
                                           $('#inpDA').hide('slow');
                                           $(this).attr('datos-adicionales', 'false');
                                       }"
										datos-adicionales="false">Datos Adcionales
									</a>
								</div>
							</div>
							<div class="collapse" id="inpDA">

								<div class="form-group form-group--float text-left">
									<input type='text' id='irfc' name='irfc' class='form-control' maxlength='14' style='width: 100%;' />
									<label class="bold">R.F.C.</label>
									<i class="form-group__bar"></i>
								</div>
								<div class="form-group form-group--float text-left">
									<input type='text' id='icontacto' name='icontacto' class='form-control' style='width: 100%;' />
									<label class="bold">Contacto</label>
									<i class="form-group__bar"></i>
								</div>
								<div class="form-group form-group--float text-left">
									<input type='text' id='itelefono' name='itelefono' class='form-control' style='width: 100%;' />
									<label class="bold">Telefono</label>
									<i class="form-group__bar"></i>
								</div>
								<div class="form-group form-group--float text-left">
									<input type='text' id='icorreo' name='icorreo' class='form-control' style='width: 100%;' />
									<label class="bold">Correo</label>
									<i class="form-group__bar"></i>
								</div>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<button type="button" id="btnGuardaGasto" class="btn btn-primary">
							<span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
					</div>
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
						<div id="print_compOTRO" style="width: 100%; height: 100%">
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
							<table style="width: 100%; border: hidden">
								<tr>
									<td style="width: 100px;">No. Comensales</td>
									<td colspan="3">
										<div class="input-group mb-3 form-group form-group--float text-left" style="margin-top: 0px; margin-bottom: 10px;">
											<div class="input-group-prepend">
												<button class="btn btn-danger btnmasmenos" type="button" onclick="masmenoscomensalda(-1)" style="padding: 10px 10px;"><i class="zmdi zmdi-minus zmdi-hc-lg"></i></button>
											</div>
											<input type="number" id='ncomensalesda' name='ncomensalesda' min="0" max="50" step="1" class='form-control' placeholder="No. Comensales" style="width: 20%" />
											<i class="form-group__bar"></i>
											<div class="input-group-append">
												<button class="btn btn-success btnmasmenos" type="button" onclick="masmenoscomensalda(1)" style="padding: 10px 10px;"><i class="zmdi zmdi-plus zmdi-hc-lg"></i></button>
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td>Nmb. Comensales</td>
									<td colspan="3">
										<div id="inpComensalesda" style="width: 100%"></div>
									</td>
								</tr>
								<tr>
									<td>R.F.C</td>
									<td>
										<input type="hidden" id="idgasto" name="idgasto" />
										<input type="hidden" id="idinforme" name="idinforme" />
										<input type="hidden" id="idproyecto" name="idproyecto" />
										<input type="hidden" id="estatusInformeDA" name="estatusInformeDA" />
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
							</table>
						</form>

					</div>
					<div class="modal-footer">

						<button id="btnDA" type="button" class="btn btn-primary" onclick="GuardarDatosAdicionales()"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Modal Confrontacion -->
		<div class="modal fade" data-modal-color="bluegray" id="confrontacion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog modal-lg" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
						Confrontaci&oacute;n
					</div>
					<div class="modal-body">

						<div id="tabsConfrontar">
							<ul>
								<li><a href="#tabConfrontacion">Confrontacion</a></li>
								<li><a href="#tabCargaEdoCuenta" onclick="preparaCarga()">Carga Edo.Cuenta</a></li>
							</ul>
							<div id="tabConfrontacion">
								<span id="msnmb"></span>
								<button id="confrontarInforme" type="button" class="btn btn-primary centerxy"><i class="zmdi zmdi-thumb-up"></i>Confrontar Informe</button>
								<button id="cancelarConfrontacion" type="button" class="btn btn-danger centerxy"><i class="zmdi zmdi-thumb-down"></i>Cancelar Confrontación</button>
								<form id="frmMovBanco" name="frmMovBanco" class="form-inline" action="#" style="width: 100%;">
									<input class="form-control reporte2" name="repde2" id="repde2" type="hidden" style="width: 80px;" />
									<input class="form-control reporte2" name="repa2" id="repa2" type="hidden" style="width: 80px;" />
									<input type="hidden" id="importede" name="importede" class="form-control text-right" style="width: 80px;" />
									<input type="hidden" id="importea" name="importea" class="form-control text-right" style="width: 80px;" />
									<a id="btnBuscarMovBanco" class="btn btn-primary" href="#" role="button" onclick="ObtenerInformes()"><i class="zmdi zmdi-refresh-alt"></i>Actualizar</a>
								</form>
								<div id="scrolltblMovBanco">
									<table id="tblMovBanco" class="display browse" cellspacing="0" width="100%">
										<thead>
											<tr>
												<th>Banco</th>
												<th>Descripción</th>
												<th width="70px">Fecha</th>
												<th width="70px">Monto $</th>
												<th width="70px"></th>
											</tr>
										</thead>
										<tbody></tbody>
										<tfoot>
											<tr>
												<td></td>
												<td></td>
												<td>Total</td>
												<td id="tdTotalMovBanco" style="text-align: right"></td>
												<td></td>
											</tr>
										</tfoot>
									</table>
									<input type="hidden" id="totalMovBanco" value="0" />
								</div>
							</div>
							<div id="tabCargaEdoCuenta">
								<table class="filtro">
									<tr>
										<td style="width: 100px;">Banco:</td>
										<td style="width: 200px;">
											<select id="banco" name="banco" class="select2" data-width="90%">
												<option value="toka">TOKA</option>
											</select>
										</td>
										<td>
											<input id="filebanco" name="filebanco" onchange="cargaBanco()" type="file" accept=".xlsx" />
										</td>
									</tr>
									<tr id="trIntrucciones">
										<td colspan="4">*<i>Para usar esta opción debes seguir los siguientes pasos:</i>
											<ol>
												<li>Descargar Estado de Cuenta que proporciona TOKA.</li>
												<li>Click en el botón "Cargar Movimientos*"</li>
												<li>Buscar y seleccionar el archivo Excel que contiene los movimientos (Estado de Cuenta TOKA).</li>
												<li>Click en el botón "Abrir".</li>
												<li>Si no hay errores en el contenido del Excel, los movimientos serán procesados y se te pedirá una confirmación para guardarlos.</li>
											</ol>
											<i>Nota:</i>
											<ul>
												<li>No alterar el archivo de Excel.
                                    <ul>
										<li>No cambiar la extensión del archivo (xlsx).</li>
										<li>No cambiar de posición las columnas.</li>
										<li>No cambiar el formato de las columnas.</li>
										<li>No eliminar el encabezado de las columnas.</li>
										<li>No agregar más columnas solo se guardaran las ya definidas en el Excel.</li>
									</ul>
												</li>
											</ul>
											<a href="temp/layout_edo_cuenta.xlsx">Descargar Ejemplo</a>
											<br />
											<img src="img/ejemploEdoCtaTOKA.PNG" width="50%" />
										</td>
									</tr>
									<tr id="trMovimientoEdoCuenta">
										<td colspan="4">

											<table class="filtro text-left" style="width: 99%; text-align: left">
												<tr>
													<td width="110px">No. Tarjeta</td>
													<td id="tdTarjeta"></td>
												</tr>
												<tr>
													<td>Nombre:</td>
													<td id="tdNombre"></td>
												</tr>
												<tr>
													<td>Nomina:</td>
													<td id="tdNomina"></td>
												</tr>
											</table>

											<span id="msnbanco">Guardar los siguientes movimientos para el banco</span>
											<table id="tblMovimientos" class="display browse" cellspacing="0" style="width: 100%" data-page-length="100">
												<thead>
													<tr>
														<th>No. Tarjeta</th>
														<th width="70px">Fecha</th>
														<th>Concepto</th>
														<th width="70px">Imp. $</th>
														<!--th title="Indica si el movimiento ya existe en la BD.">Dup.?</th-->
														<!--th title="Fecha de cuando el movimiento se cargo por ultima vez.">F.Carga</th-->
														<th title="Confirma si deseas guardar el movimiento.">Conf.?</th>
													</tr>
												</thead>
												<tbody>
												</tbody>
												<tfoot>
													<tr>
														<td></td>
														<td></td>
														<td>Total:</td>
														<td id="tdTotalConfrontar" style="text-align: right"></td>
														<td></td>
													</tr>
												</tfoot>
											</table>
											<button id="guardarMBanco" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
										</td>
									</tr>
								</table>
							</div>
						</div>

					</div>
					<div class="modal-footer">
					</div>
				</div>
			</div>
		</div>

		<!-- Modal ver Informe -->
		<div class="modal fade" id="verInformeGastos" tabindex="1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog modal-xlg" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						<!--style="height: 50px;"-->
						Requisicion de Viaje
                <table cellspacing="0" width="80%">
					<tr>
						<td valign="middle">
							<!--Opciones-->
							<a id="aedit" visible="0" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-edit"></span>Editar</a>
							<a id="acancela" class="btn btn-danger btn-md" href="#" role="button"><span class="glyphicon glyphicon-floppy-remove"></span>Cancela</a>
							<a id="aguarda" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-floppy-disk"></span>Guardar</a>
							<%--<a id="arelaciona" class="btn btn-primary btn-md" href="#" role="button"><span class="zmdi zmdi-arrow-merge"></span> Relacionar Gastos</a>--%>
							<a id="aenvia" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-send"></span>Enviar a Autorización</a>

							<a id="aexportarxls" class="btn btn-primary btn-md" href="#" role="button" onclick=""><span class="glyphicon glyphicon-export"></span>Excel</a>

							<a id="averhorag" class="btn btn-primary btn-md" href="#" role="button" data-placement='top' data-html='true'
								title="<div style='width: 170px;'>Ver la hora en que se realizo el Gasto.</div>" aria-hidden='true'><span class="zmdi zmdi-time"></span>Ver Hora Gasto</a>

							<a id="aagregarg" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-plus"></span>Agregar Gasto</a>

							<a id="aconfrontar" class="btn btn-primary btn-md" href="#" role="button"><i class="zmdi zmdi-swap"></i>Confrontar</a>

							<a id="arefresh" class="btn btn-primary btn-md" href="#" role="button"><i class="zmdi zmdi-refresh"></i>Actualizar</a>

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
                            <div id="infoInforme" class="collapse in" aria-expanded="true" style="margin: 0px; padding: 0px;">
                                <div class="card-block" style="margin: 0px; padding: 2px;">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6">
                                            <div id="verinforme" name="informe" class="form-inline">
                                                <input type="hidden" id="id_informe" name="id_informe" />
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
                                                        <td colspan="3">
                                                            <input type="hidden" id="idinforme" name="idinforme" />
                                                            <input type="hidden" id="idproyecto" name="idproyecto" />
                                                            <input type="hidden" id="estatus" name="estatus" />
                                                            <input type="hidden" id="ConfBanco" name="ConfBanco" />
                                                            <input type="hidden" id="inputCabeceraFormaPago" name="inputCabeceraFormaPago" />
															<input id='totalg' type='hidden' value=''/>
															<input id='montog' type='hidden' value=''/>
                                                            <div class="form-group">
                                                                <input type="text" id="proyecto" readonly="readonly" name="proyecto" style="width: 100%" class="form-control" placeholder="Nombre Informe">
                                                                <i class="form-group__bar"></i>
                                                            </div>
                                                            <label id="lblproyecto"></label>
                                                        </td>
                                                        <!--td width="70px">Motivo:</td>
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
                                                        

                                                    </tr>
                                                    <tr>
                                                        <td width="110px">Forma de Pago:</td>
                                                        <td>
                                                            <label id="lblCabeceraFormaPago"></label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdcomAut" colspan="4"></td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdConXML" colspan="4"></td>
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
                        <input id="HFMontoRequisicion" type="hidden" />
                        <!--Lista de gastos del informe-->
                        <!--div id="gastos" class="row inner-content scrollbar-dynamic" style="width: 100%; margin: 0px; padding: 0px;"-->
<div id="divListGastos">
				<form id='nuevoGasto' name='nuevoGasto' style="padding: 0px; width: 100%; margin: 0 auto; white-space: nowrap;" class='form-inline' action='#' method="POST" enctype='multipart/form-data'>                                
					<table id="tblGastos" class="browse display nowrap" style="overflow-x: auto; white-space: nowrap;" cellspacing="0" data-page-length="-1">
                                    <!--data-page-length="25"-->
                                    <thead style="width: 100%">
                                        <tr style="width: 100%">
                                            <th style='width: 10px;'></th>
                                            <th style='width: 10px;'>#</th>
                                            <th style='width: 80px;'>Día</th>
                                            <th style='width: 40px;'>Hora</th>
                                            <th>Justificaci&oacute;n</th>
                                            <th>Proveedor</th>
                                            <th title="Importe registrado por gasto desde la APP/WEB">Capturado</th>
                                            <th>F. de Pago</th>
                                            <th>XML</th>
                                            <th>PDF</th>
                                            <th>IMG</th>
                                            <th title="Importe del gasto con algun comprobante (XML/PDF/IMAGEN)">Comprobado</th>
                                            <th>Categoria</th>                                        
                                            <th width="30px">Comensales</th>
                                            <th width="200px">Objetivo</th>    
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot>
                                    </tfoot>
                                </table>

                                <div id="inputnuevogasto"></div>
                            </form>
</div>
                        <!--/div-->
				</div>
			</div>
		</div>
		</div>
	</section>

	<!-- App functions and actions -->
	<script src="js/app.min.js"></script>
	<script src="js/js.js"></script>
	<script src="js/informe.js?2xx"></script>
	<script src="js/gastos.js?2xx"></script>
	<script>
		$("#anuevo").click(function () {
			$("#frmInforme").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});

			var fecha = fechaActual();
			fecha = "01" + fecha.substr(2, 10);
			OpcionesMenuMes(fecha, "i");
			$("#mes").val(fecha);

			$("#tipoInf, #responsable, #mes").select2({
				dropdownParent: $("#informe")
			});
		});

	</script>

</asp:Content>
