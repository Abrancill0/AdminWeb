<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ver_informe_autorizacion.aspx.cs" Inherits="SCGESP.ver_informe_autorizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<style type="text/css">
		table {
			font-size: 12px;
			width: 100%;
			vertical-align: top;
		}

		#tblCabeceraInforme {
			vertical-align: top;
		}

		.valor {
			color: black;
			font-size: 13px;
			font-weight: bold;
			text-align: left;
			word-wrap: break-word !important;
		}

		#tblGastos td p.valor {
			font-size: 12px;
			border: none;
		}

		#tblGastos td {
			vertical-align: top;
		}

		.valor:first-letter {
			text-transform: uppercase !important;
		}

		.concepto-importe {
			text-align: right;
			padding: 5px 5px;
			font-weight: bold;
		}

		table.tblGastos {
			border: #000 solid 1px;
			color: #000;
			word-wrap: break-word;
			vertical-align: top;
		}

			table.tblGastos thead {
				background: #337AB7;
				font-size: 12px;
				color: #ffffff;
				font-weight: bold;
			}

				table.tblGastos thead tr th {
					background: #337AB7;
					padding: 5px 5px;
					font-size: 12px;
					font-weight: bold;
				}

			table.tblGastos tbody {
				font-size: 10px;
			}

			table.tblGastos td {
				font-size: 11px;
				font-weight: bold;
			}

			table.tblGastos tbody tr {
				border-right: #000000 solid 1px;
				border-left: #000000 solid 1px;
			}

				table.tblGastos tbody tr td {
					padding: 2px;
					border-right: #ffffff solid 1px;
					color: #000000;
					padding: 1px 4px;
				}

			table.tblGastos tbody tr {
				border-right: #000 solid 1px;
			}

			table.tblGastos tfoot {
				background: #DAE875;
				border: #000 solid 1px;
				text-transform: uppercase;
				color: #000;
			}

				table.tblGastos tfoot tr td {
					background: #DAE875;
					font-size: 14px;
					padding: 0px 4px;
				}

		#tblGastos thead {
			text-transform: uppercase;
		}

			#tblGastos thead th {
				border: 1px solid black;
				text-align: center;
			}

		.rowGris {
			background-color: #e0e0e0;
		}

		.rowBlanco {
			background-color: #ffffff;
		}
	</style>
	<section class="content">
		<div class="panel panel-primary">
			<div class="panel-heading">
				Requisición / Informe de gastos por autorizar
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<table>
					<tr>
						<td style="width: 130px">
							<a class="btn btn-primary btn-md" href="/autorizaciones" role="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Regresar</a>
							<!--<a class="btn btn-primary btn-md" href="#" onclick="location.reload();" role="button"><span class="zmdi zmdi-refresh"></span>&nbsp;Actualizar</a-->
						</td>
						<td style="vertical-align: middle; text-align: left; padding-left: 50px;">
							<!--Opciones-->
							<input id="HFUsuariovobo" type="hidden" />
							<a id="aexportarxls" class="btn btn-primary btn-md" href="#" role="button" onclick=""><span class="glyphicon glyphicon-export"></span>&nbsp;Excel</a>
							<a id="aenvia" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-envelope"></span>&nbsp;Enviar a Comprobación</a>
							<a id="aautoriza" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-envelope"></span>&nbsp;Autorizar</a><!--Regresar Comentario-->
							<a id="arechaza" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-remove"></span>&nbsp;Rechazar</a>
							<a id="autorizadores" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-user"></span>&nbsp;Enviar a VoBo</a>

						</td>
					</tr>
				</table>
				<div class="card" style="margin: 0px; padding: 0px;">
					<div class="card-header card-info" role="tab" style="margin: 0px; padding: 2px 5px;">
					</div>
					<div class="card-block" style="margin: 0px; padding: 2px;">
						<div class="row">
							<div id="cabeceraInforme" class="col-xs-12 col-md-6 col-lg-8">
								<!--informe-cabecera-template-->
							</div>

							<div id="importesInforme" class="col-md-6 col-lg-4">
								<!--importes-informe-template-->
							</div>
						</div>
						<div class="row" style="padding: 0px 5px;">
							<table id="tblGastos" class="tblGastos display nowrap" cellspacing="0" data-page-length="-1">
								<thead>
									<tr>
										<th style="width: 50px;">No. De<br />
											Cargo</th>
										<th style='width: 110px;'>Día</th>
										<th style="width: 150px;">Categor&iacute;a</th>
										<th style="width: 230px;" width="230px">Justificaci&oacute;n</th>
										<th style='width: 120px;'>Monto</th>
										<th style='width: 30px;'>XML</th>
										<th style='width: 30px;'>PDF</th>
										<th style='width: 30px;'>IMG</th>
										<th style='width: 120px;' title="Importe con comprobante">Monto<br />
											Comprobado</th>
										<th style='width: 150px;' title="Importe aceptable">Dentro de<br />
											pol&iacute;tica</th>
										<th style='width: 120px;' title="Importe fuera de política">Fuera de<br />
											pol&iacute;tica</th>
										<th style='width: 100px;' title="Importe no deducible">No Deducible</th>
										<th style='width: 80px;'>Editar</th>
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

							<table id="tblConceptosXML" class="display tblGastos imprimir" cellspacing="0" width="100%">
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
						<div id="print_compOTRO" style="width: 100%; height: 100%; padding: 10px;">
							<img id="compOTRO" src="" alt="Comprobante" class="img-thumbnail" />
						</div>

					</div>
					<div class="modal-footer">
						<!--button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button-->
					</div>
				</div>
			</div>
		</div>
		<!-- Modal editar Gasto a Informe -->
		<!--data-modal-color="bluegray"-->
		<div class="modal fade" id="mEditarGastoInf" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						Editar Gasto
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
					</div>
					<div class="modal-body">
						<input type="hidden" id="idGasto" value="" />
						<input type="hidden" id="gasto" value="" />
						<div id="inpustGasto" style="color: black;">
							<h6>Fecha Gasto: <span class="valor dia">fecha</span></h6>
							<label for="categoria" style="width: 100%;">
								<span>Categor&iacute;a: </span>
								<div class="input-group" style="background-color: #ffffff">
									<span></span>
									<div class="form-group">
										<select id='categoria' name='categoria' style="width: 100%">
										</select>
										<i class="form-group__bar"></i>
									</div>
								</div>
							</label>
							<h6>Justificaci&oacute;n: <span class="valor justificacion">justificacion</span></h6>
							<h6>Monto: <span class="valor monto">$ 0.00</span></h6>
							<h6>Monto Comprobado: <span class="valor monto_comprobado">$ 0.00</span></h6>
							<h6>Dentro de Pol&iacute;tica: <span class="valor dentro_politica">$ 0.00</span></h6>
							<label for="fuera_politica" style="width: 100%;">
								<span>Fueta de Pol&iacute;tica:</span>
								<div class="input-group" style="background-color: #ffffff">
									<span></span>
									<div class="form-group">
										<input type="number" id='fuera_politica' name='fuera_politica' class='form-control' onkeypress='return justNumbers(event);' style='width: 100%;' />
										<i class="form-group__bar"></i>
									</div>
								</div>
							</label>
							<label for="no_deducible" style="width: 100%;">
								<span>No Deducible:</span>
								<div class="input-group" style="background-color: #ffffff">
									<span></span>
									<div class="form-group">
										<input type="number" id='no_deducible' name='no_deducible' class='form-control' onkeypress='return justNumbers(event);' style='width: 100%;' />
										<i class="form-group__bar"></i>
									</div>
								</div>
							</label>

						</div>
					</div>
					<div class="modal-footer">
						<button type="button" id="btnGuardaGasto" class="btn btn-primary">
							<span class="glyphicon glyphicon-floppy-saved"></span>Guardar</button>
					</div>
				</div>
			</div>
		</div>
		<!--Autorizadores-->
		
		<div id="tabAutoriza" class="modal fade" tabindex="1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						VoBo
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
					</div>
					<div class="modal-body">
						<label id="lblAutOpcional">
							Usuarios:
                            <select id="mAutOpcional" name="mAutOpcional" class="select2" data-width="100%"></select>
						</label>
						<table id="tblAutOpcional" class="display tblGastos" cellspacing="0" width="100%" data-page-length="10">
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
					</div>
					<div class="modal-footer account-controles">
						<button id="EnviarAutorizadores" type="button" class="btn btn-primary"><span class="zmdi zmdi-mail-send"></span>&nbsp;Enviar a VoBo</button>
					</div>
				</div>
			</div>
		</div>
		<!-- Modal editar Gasto a Informe -->
		<!--data-modal-color="bluegray"-->
		<div class="modal fade" id="modal_alerta" data-modal-color="gray" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						<div id="titulo_modal_alert" style="width: 100%; padding: 1px 10px;">
						</div>
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
					</div>
					<div class="modal-body">
						<div id="contenido_modal_alert" style="width: 100%; color: black">
						</div>
					</div>
					<div class="modal-footer">
						<div id="footer_modal_alert" style="width: 100%">
						</div>
					</div>
				</div>
			</div>
		</div>

		<!--Informe cabecera-->
		<script id="informe-cabecera-template" type="text/x-handlebars-template">

			<input type="hidden" id="RmRdeRequisicion" value="{{ r_idrequisicion }}" />
			<input type="hidden" id="idinforme" name="idinforme" value="{{ i_id }}" />
			<input type="hidden" id="estatus" name="estatus" value="{{ i_estatus }}" />
			<input type="hidden" id="ConfBanco" name="ConfBanco" value="{{ i_conciliacionbancos }}" />
			<input type="hidden" id="formaPagoInforme" name="formaPagoInforme" value="{{ i_tarjetatoka }}" />
			<input type="hidden" id="usuResponsable" name="usuResponsable" value="{{ i_uresponsable }}" />
			<input type="hidden" id="esvobo" name="esvobo" value="{{ esvobo }}" />
			<input type="hidden" id="esvobo_2" name="esvobo_2" value="{{ esvobo_2 }}" />

			<table id="tblCabeceraInforme" class="filtro text-left" style="text-align: left;" border="1" style="width:100%">
				<tr>
					<td style="width: 180px">Requisici&oacute;n:</td>
					<td>
						<table style="width: 250px">
							<tr>
								<td style="width: 80px">
									<p class="valor">{{ r_idrequisicion }}</p>
								</td>
								<td style="padding: 0px 5px; width: 90px;">Informe:</td>
								<td style="width: 80px">
									<p class="valor">{{ i_ninforme }}</p>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>Empleado:</td>
					<td>
						<p class="valor">{{ responsable }}</p>
					</td>
				</tr>
				<tr>
					<td style="vertical-align: top">Justificaci&oacute;n:</td>
					<td>
						<p class="valor">{{ p_nmb }}</p>
					</td>
				</tr>
				<tr>
					<td>Inicio:</td>
					<td>
						<table style="width: 250px">
							<tr>
								<td style="width: 80px">
									<p class="valor">{{ del }}</p>
								</td>
								<td style="padding: 0px 5px; width: 90px;">Vigencia:</td>
								<td style="width: 80px">
									<p class="valor">{{ al }}</p>
								</td>
							</tr>
						</table>

					</td>
				</tr>
				<tr>
					<td>Viaje:</td>
					<td style="width: 100%">
						<table id="tblTipoRequisicion">
							<tr>
								<td style="width: 80px;">{{#if esViaje}}
									<span style='font-size: 11px; display: block' class='label label-success'><i class='glyphicon glyphicon-ok'></i></span>
									{{/if}}
								</td>
								<td style="width: 50px;">Otros:</td>
								<td style="width: 80px;">{{#if esOtros}}
									<span style='font-size: 11px; display: block' class='label label-success'><i class='glyphicon glyphicon-ok'></i></span>
									{{/if}}
								</td>
								<td style="width: 150px;">Sesión de Trabajo:</td>
								<td style="width: 80px;">{{#if esSesion}}
									<span style='font-size: 11px; display: block' class='label label-success'><i class='glyphicon glyphicon-ok'></i></span>
									{{/if}}
								</td>
							</tr>
						</table>
					</td>
				</tr>
				{{#if i_comentarioaut}}
				<tr>
					<td style="vertical-align: top">Rechazo Por:</td>
					<td>
						<p class="valor">
							{{#each comentario_rechazo}}
								{{#if this}}
									{{this}}<br />
								{{/if}}
							{{/each}}
						</p>
					</td>
				</tr>
				{{/if}}
				{{#if comentario_1}}
				<tr>
					<td style="vertical-align: top">Solicitud<span style="color:#ffffff;">_</span>VoBo:</td>
					<td>
						<p class="valor">{{ comentario_1 }}</p>
					</td>
				</tr>
				{{/if}}
				{{#if comentario_2}}
				<tr>
					<td style="vertical-align: top">VoBo:</td>
					<td>
						<p class="valor">{{ comentario_2 }}</p>
					</td>
				</tr>
				{{/if}}

				{{#if comentario_3}}
				<tr>
					<td style="vertical-align: top">Comentario<span style="color:#ffffff;">_</span>Validación:</td>
					<td>
						<p class="valor">{{ comentario_3 }}</p>
					</td>
				</tr>
				{{/if}}
                {{#if comentario_4}}
				<tr>
					<td style="vertical-align: top">Comentario<span style="color:#ffffff;">_</span>Autorización:</td>
					<td>
						<p class="valor">{{ comentario_4 }}</p>
					</td>
				</tr>
				{{/if}}
            
				<tr>
					<td>Confrontación:</td>
					<td>
						<span style='font-size: 11px; padding: 2px 30px;' class='label label-success'><span class='glyphicon glyphicon-ok'></span></span>
					</td>
				</tr>
				<tr>
					<td>Estatus:</td>
					<td>
						<p class="valor">{{ e_estatus }}</p>
					</td>
				</tr>
			</table>
		</script>
		<!--Importes informe-->
		<script id="importes-informe-template" type="text/x-handlebars-template">

			<input type="hidden" id="montoRequisicion" name="montoRequisicion" value="{{ num_montoRequisicion }}" />
			<input type="hidden" id="montoGastado" name="montoGastado" value="{{ num_montoGastado }}" />
			<input type="hidden" id="disAnticipo" name="disAnticipo" value="{{ num_disponible }}" />
			<input type="hidden" id="decrementado" name="decrementado" value="{{ num_decrementado }}" />
			<input type="hidden" id="totalg" name="totalg" value="{{ num_montoGastado }}" />
			<input type="hidden" id="montog" name="montog" value="{{ num_montog }}" />

			<table cellspacing="0" width="100%">
				<tr>
					<td colspan="2" align="center"><b>Importes Requisici&oacute;n</b></td>
				</tr>
				<tr>
					<td class="concepto-importe">Autorizado: </td>
					<td>
						<span style='font-size: 16px; display: block;' class='label label-primary text-right'>{{ montoRequisicion }}
						</span>
					</td>
				</tr>
				<tr>
					<td class="concepto-importe">Gastado: </td>
					<td>
						<span style='font-size: 16px; display: block;' class='label label-danger text-right'>{{ montoGastado }}
						</span></td>
				</tr>
				<tr>
					<td class="concepto-importe">Por Comprobar: </td>
					<td>
						<span style='font-size: 16px; display: block;' class='label label-success text-right'>{{ disponible }}
						</span>
					</td>
				</tr>
				<tr>
					<td class="concepto-importe">Decrementado: </td>
					<td><span style='font-size: 16px; display: block;' class='label label-warning text-right'>{{ decrementado }}
					</span></td>
				</tr>
			</table>
		</script>
		<!--Gastos informe-->
		<script id="gastos-informe-template" type="text/x-handlebars-template">
			<tr id="{{ idgasto}}" class="{{ classTr }}">
				<td class="text-left" style="vertical-align: middle; width: 50px;">
					<h5><span class="ngasto label label-success">{{ ngasto }}</span></h5>
				</td>
				<td class="dia" style='width: 110px;'>{{ dia }}</td>
				<td class="categoria" style="width: 150px;">{{ categoria }}
				</td>
				<td class="justificacion" width="230px">
					<p style="width: 230px;" class="valor">{{ justificacion }}</p>
				</td>
				<td class="monto text-right" style='width: 120px;'>{{ monto }}</td>
				<td class="xml text-center" style='width: 30px;'>{{#if xml}}
						<a dirxml='{{ xml }}' data-toggle='tooltip' onclick="verComprobante('{{ xml }}', 'XML')" class='btn btn-success btn-sm' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
					{{/if}}
				</td>
				<td class="pdf text-center" style='width: 30px;'>{{#if pdf}}
						<a dirpdf='{{ pdf }}' data-toggle='tooltip' onclick="verComprobante('{{ pdf }}', 'PDF')" class='btn btn-success btn-sm verPDF' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
					{{/if}}
				</td>
				<td class="img text-center" style='width: 30px;'>{{#if img}}
						<a dirimg='{{ img }}' data-toggle='tooltip' onclick="verComprobante('{{ img }}', 'OTRO')" class='btn btn-success btn-sm verIMG' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
					{{/if}}
				</td>
				<td class="monto_comprobado text-right" style='width: 120px;' title="Importe con comprobante">{{ monto_comprobado }}</td>
				<td class="dentro_politica text-right" style='width: 150px;' title="Importe aceptable">{{ dentro_politica }}</td>
				<td class="fuera_politica text-right" style='width: 120px;' title="Importe fuera de política">{{ fuera_politica }}</td>
				<td class="no_deducible text-right" style='width: 100px;' title="Importe no deducible">{{ no_deducible }}</td>
				<td class="text-center" style='width: 80px;'>{{#if btnEditar}}
					<a class="btn btn-primary btn-sm" onclick="editar_gasto('{{ gasto }}')" role="button"><span class="glyphicon glyphicon-edit"></span></a>
					{{/if}}
				</td>
			</tr>
		</script>
		<!--Total gastos informe-->
		<script id="total-gastos-informe-template" type="text/x-handlebars-template">
			<tr>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td class="text-right">{{ total_gastado }}</td>
				<td></td>
				<td></td>
				<td></td>
				<td class="text-right">{{ monto_comprobado }}</td>
				<td class="text-right">{{ dentro_politica }}</td>
				<td class="text-right">{{ fuera_pilitica }}</td>
				<td class="text-right">{{ no_deducible }}</td>
				<td></td>
			</tr>
		</script>
		<script id="modal-alerta-titulo" type="text/x-handlebars-template">
			{{ titulo }}
		</script>
		<script id="modal-confirma-comprobacion" type="text/x-handlebars-template">
			<div id='divChkVoBo' style="color: black">
				<label class='custom-control custom-checkbox'>
					<input type='checkbox' id='ChkVoBo' class='custom-control-input'>
					<span class='custom-control-indicator'></span>
					<span class='custom-control-description'>Enviar a revisión.</span>
				</label>
			</div>
		</script>
        
		<script id="modal-confirma-comprobacion-msn" type="text/x-handlebars-template">
            <div class="form-group">
                <label>Comentarios:
                </label>
                <input type="text" id="comentariosValidacion" name="comentariosValidacion" style="width: 100%" class="form-control">
                <i class="form-group__bar"></i>
            </div>
			<div id='divChkVoBo' style="color: black">
				<label class='custom-control custom-checkbox'>
					<input type='checkbox' id='ChkVoBo' class='custom-control-input'>
					<span class='custom-control-indicator'></span>
					<span class='custom-control-description'>Enviar a revisión.</span>
				</label>
			</div>
		</script>

		<script id="modal-confirma-rechazo" type="text/x-handlebars-template">
			<table cellpadding='0' style="width: 100%" cellspacing='0' border='0'>
				<tr>
					<td>Comentario de rechazo: </td>
				</tr>
				<tr>
					<td>
						<input id='ComentariosRechazo' data-toggle='tooltip' data-placement='top' style="width: 100%;" data-html='true' aria-hidden='true' type='text' title='Comentarios de rechazo' />
					</td>
				</tr>
			</table>
		</script>
		<script id="modal-confirma-solicitar-vobo" type="text/x-handlebars-template">
			Enviar informe a Visto Bueno
			<table cellpadding='0' style="width: 100%;" cellspacing='0' border='0'>
				<tr>
					<td>Comentario:</td>
				</tr>
				<tr>
					<td>
						<input id='comentarioEnvioVoBo' data-toggle='tooltip' data-placement='top' style="width: 100%;" data-html='true' aria-hidden='true' type='text' title='Comentarios de envio a VoBo.' /></td>
				</tr>
			</table>
		</script>
		<script id="modal-confirma-regresa-vobo" type="text/x-handlebars-template">
			<table cellpadding='0' width='100%' cellspacing='0' border='0'>
				<tr>
					<td>Comentario:</td>
				</tr>
				<tr>
					<td>
						<input id='comentarioVoBo' data-toggle='tooltip' data-placement='top' style="width: 100%;" data-html='true' aria-hidden='true' type='text' title='Comentarios a VoBo.' />
					</td>
				</tr>
			</table>
		</script>
		<script id="modal-botones" type="text/x-handlebars-template">
			{{#if btn1}}
						<button type="button" class="btn btn-{{ tipo1 }}" onclick="{{ function1 }}">
							<span class="{{ icono1 }}"></span>&nbsp;{{ label1 }}</button>
			{{/if}}
						{{#if btn2}}
						<button type="button" class="btn btn-{{ tipo2 }}" onclick="{{ function2 }}">
							<span class="{{ icono2 }}"></span>&nbsp;{{ label2 }}</button>
			{{/if}}
						{{#if btn3}}
						<button type="button" class="btn btn-{{ tipo3 }}" onclick="{{ function3 }}">
							<span class="{{ icono3 }}"></span>&nbsp;{{ label3 }}</button>
			{{/if}}
						<button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close">
							<i class="zmdi zmdi-close"></i>&nbsp;Cancelar</button>
		</script>

	</section>

	<script type="text/javascript" src="js/app.min.js"></script>
	<script type="text/javascript" src="js/js.js"></script>
	<script type="text/javascript" src="js/handlebars-v4.0.11.js"></script>
	<script type="text/javascript">
		var UsuarioActivo = localStorage.getItem("cosa");
		var EmpeladoActivo = localStorage.getItem("cosa2");
		var f = new Date();
		var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
		// compile handlebars templates and store them for use later

		let importesInformeTemplate = Handlebars.compile($("#importes-informe-template").html());
		var IdInforme = 0;
		IdInforme = url.get("item") * 1;

		selectInforme(IdInforme);
		browseGastos(IdInforme);
		function selectInforme(IdInforme) {
			let cabeceraInformeTemplate = Handlebars.compile($("#informe-cabecera-template").html());
			$("#RmRdeRequisicion, #idinforme, #estatus, #ConfBanco, #disAnticipo").val(0);
			$("#formaPagoInforme, #usuResponsable").val("");

			var datos = {
				"id": IdInforme
			};

			$.ajax({
				async: false,
				type: 'POST',
				url: '/api/SelectInforme',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				beforeSend: function () {
					//cargando();
				},
				success: function (result) {
					var informe = result[0];
					//informe.del = valorVacio(informe.del) ? "" : formatFecha(new Date(informe.del), "dd/mm/yyyy");
					//informe.al = valorVacio(informe.al) ? "" : formatFecha(new Date(informe.al), "dd/mm/yyyy");
					informe.i_comentarioaut = (informe.i_comentarioaut);
					informe.p_nmb = (informe.p_nmb);
					informe.i_uresponsable = $.trim(informe.i_uresponsable);
					informe.i_comentarioaut = (informe.i_comentarioaut).replace("adminerp", "AdminERP")
						.replace("adminweb", "AdminWeb").replace("adminapp", "AdminApp");
					informe['comentario_rechazo'] = (informe.i_comentarioaut).split(";;");
					informe.p_nmb = (informe.p_nmb).replace("adminerp", "AdminERP")
						.replace("adminweb", "AdminWeb").replace("adminapp", "AdminApp");
					if (informe.i_conciliacionbancos > 0)
						informe.confrontacionOK = "OK";
					else
						informe.confrontacionOK = "";

					var requisicion = DatosRequisicion(informe.r_idrequisicion);

					var esSesion = "", esViaje = "", esOtros = "";
					var RmReqTipoRequisicionNombre = (requisicion.RmReqTipoRequisicionNombre).toLowerCase();
					if (RmReqTipoRequisicionNombre.indexOf("sesi") > -1) {
						esSesion = "OK";
					} else if (RmReqTipoRequisicionNombre.indexOf("viaje") > -1) {
						esViaje = "OK";
					} else {
						esOtros = "OK";
					}
					informe.esSesion = esSesion;
					informe.esViaje = esViaje;
                    informe.esOtros = esOtros;

					$('#cabeceraInforme').append(cabeceraInformeTemplate(informe));

					var RmReqTotal = requisicion.RmReqTotal * 1;
					var RmReqImporteComprobar = datoEle(requisicion.RmReqImporteComprobar) * 1;
					var RmReqImporteDecrementado = 0;
					var RmReqEstatus = requisicion.RmReqEstatus * 1;
					if (RmReqImporteComprobar > 0) {
						RmReqImporteDecrementado = RmReqTotal - RmReqImporteComprobar;
					} else if (RmReqImporteComprobar === 0 && RmReqEstatus === 53) {
						RmReqImporteDecrementado = RmReqTotal;
					}

					var disponible = RmReqTotal - informe.i_totalg;
					disponible = disponible - RmReqImporteDecrementado;

					var importesInforme = {
						montoRequisicion: formatNumber.new(RmReqTotal.toFixed(2), "$ "),
						montoGastado: formatNumber.new((informe.i_totalg).toFixed(2), "$ "),
						disponible: formatNumber.new(disponible.toFixed(2), "$ "),
						decrementado: formatNumber.new(RmReqImporteDecrementado.toFixed(2), "$ "),
						num_montoRequisicion: RmReqTotal.toFixed(2),
						num_montoGastado: informe.i_totalg,
						num_disponible: disponible,
						num_decrementado: RmReqImporteDecrementado,
						num_montog: informe.i_total
					};

					$("#importesInforme").append(importesInformeTemplate(importesInforme));

					habilitaControlesInfo(0, IdInforme, informe.i_estatus, informe.i_uresponsable,
						informe.DesactivaControl,
						informe.hAutorizarComprobar,
						informe.esvobo, informe.esvobo_2,
						informe.idinforme_2,
						informe.autorizador_final);
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function browseGastos(IdInforme) {
			let gastosInformeTemplate = Handlebars.compile($("#gastos-informe-template").html());
			let totalGastosInformeTemplate = Handlebars.compile($("#total-gastos-informe-template").html());
			var esvobo_2 = $("#esvobo_2").val() * 1;
			var datos = {
				"idinforme": IdInforme,
				"idproyecto": 0
			};
			$("#tblGastos tbody").empty();
			var total = 0, totalmonto = 0, totalaceptable = 0, totalnoaceptable = 0, totalnodeducible = 0;
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/browseGastosInforme",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				success: function (result) {
					var colorRow = "rowGris";
					var habBtnEditar = true;

					if (esvobo_2 === 1) {
						habBtnEditar = false;
					}

					$.each(result, function (key, value) {
						var fg = (value.g_fgasto).split('-');
						var fgasto = formatFecha(fg[2] + '-' + fg[1] + '-' + fg[0], 'dd/mmm');
						var nmbComensales = (value.nmbcomensales).split(",");

						var monto = value.MONTO * 1;
						var importeaceptable = value.importeaceptable * 1;
						var importenoaceptable = value.importenoaceptable * 1;
						var importenodeducible = value.importenodeducible * 1;
						var dir_xml = $.trim(datoEle(value.g_dirxml));
						var dir_pdf = $.trim(datoEle(value.g_dirpdf));
						var dir_img = $.trim(datoEle(value.g_dirotros));

						if (valorVacio(dir_xml) && valorVacio(dir_pdf) && valorVacio(dir_img)) {
							monto = 0;
							importeaceptable = 0;
						}
						var tipoajuste = value.tipoajuste * 1;
						var result_justificacion = conceptos_adicionales(value.g_concepto, value.g_nombreCategoria, tipoajuste);

						var justificacion_text_1 = "";
						try {
							justificacion_text_1 = result_justificacion[0][1];
						} catch (e) {
							justificacion_text_1 = value.g_concepto;
						}

						var gasto = {
							idgasto: value.g_id,
							ngasto: value.orden,
							dia: fgasto,
							categoria: value.g_nombreCategoria,
							idcategoria: value.g_categoria,
							justificacion: justificacion_text_1,
							monto: formatNumber.new((value.g_total).toFixed(2), "$ "),
							num_monto: value.g_total,
							xml: dir_xml,
							pdf: dir_pdf,
							img: dir_img,
							monto_comprobado: formatNumber.new(monto.toFixed(2), "$ "),
							num_monto_comprobado: monto,
							dentro_politica: formatNumber.new(importeaceptable.toFixed(2), "$ "),
							num_dentro_politica: importeaceptable,
							fuera_politica: formatNumber.new(importenoaceptable.toFixed(2), "$ "),
							num_fuera_politica: importenoaceptable,
							no_deducible: formatNumber.new(importenodeducible.toFixed(2), "$ "),
							num_no_deducible: importenodeducible,
							gasto: "",
							btnEditar: habBtnEditar,
							classTr: colorRow,
							valores_justificacion: valores_edit_justificacion(result_justificacion, value.g_nombreCategoria)
						}
						gasto['gasto'] = JSON.stringify(gasto);

						total += value.g_total * 1;
						totalmonto += monto;
						totalaceptable += importeaceptable;
						totalnoaceptable += importenoaceptable;
						totalnodeducible += importenodeducible;

						$("#tblGastos tbody").append(gastosInformeTemplate(gasto));
						$.each(result_justificacion, function (key, value) {
							if (key > 0) {
								gasto = {
									idgasto: "",
									ngasto: "",
									dia: "",
									categoria: value[0],
									idcategoria: 0,
									justificacion: value[1],
									monto: "",
									num_monto: 0,
									xml: "",
									pdf: "",
									img: "",
									monto_comprobado: "",
									num_monto_comprobado: 0,
									dentro_politica: "",
									num_dentro_politica: 0,
									fuera_politica: "",
									num_fuera_politica: 0,
									no_deducible: "",
									num_no_deducible: 0,
									gasto: "",
									btnEditar: false,
									classTr: colorRow,
									btnAdicional: false,
									tipoajuste: tipoajuste,
									valores_justificacion: []
								};
								$("#tblGastos tbody").append(gastosInformeTemplate(gasto));
							}
						});
						$.each(nmbComensales, function (key, value) {
							var ncomensal = key + 1;
							var nmbcomensal = $.trim(value);
							if (nmbcomensal !== "") {
								gasto = {
									idgasto: "",
									ngasto: "",
									dia: "",
									categoria: "  Comensal " + ncomensal,
									idcategoria: 0,
									justificacion: nmbcomensal,
									monto: "",
									num_monto: 0,
									xml: "",
									pdf: "",
									img: "",
									monto_comprobado: "",
									num_monto_comprobado: 0,
									dentro_politica: "",
									num_dentro_politica: 0,
									fuera_politica: "",
									num_fuera_politica: 0,
									no_deducible: "",
									num_no_deducible: 0,
									gasto: "",
									btnEditar: false,
									classTr: colorRow,
									valores_justificacion: []
								};
								$("#tblGastos tbody").append(gastosInformeTemplate(gasto));
							}

						});
						colorRow = colorRow === "rowBlanco" ? "rowGris" : "rowBlanco";
					});
				},
				complete: function () {
					$("#tblGastos tbody tr td").each(function (key, value) {
						if (!valorVacio(value['innerText']))
							value['innerText'] = (value['innerText']).replace(/\\"/g, '\"');
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
			var totalesGasto = {
				total_gastado: formatNumber.new(total.toFixed(2), "$ "),
				monto_comprobado: formatNumber.new(totalmonto.toFixed(2), "$ "),
				dentro_politica: formatNumber.new(totalaceptable.toFixed(2), "$ "),
				fuera_pilitica: formatNumber.new(totalnoaceptable.toFixed(2), "$ "),
				no_deducible: formatNumber.new(totalnodeducible.toFixed(2), "$ ")
			};
			$("#tblGastos tfoot").empty();
			$("#tblGastos tfoot").append(totalGastosInformeTemplate(totalesGasto));
		}
		function valores_edit_justificacion(justificacion, categoria) {
			var datos = [];
			var njustificacion = justificacion.length;
			if (njustificacion === 1) {
				datos.push({ "justificacion": justificacion[0][1] });
			} else {
				$.each(justificacion, function (key, value) {
					var campo = valorVacio(value[2]) ? "" : value[2];
					var valor = value[1];
					var valores = "{\"" + campo + "\": \"" + valor + "\"}";
					valores = StrToJSON(valores);//JSON.parse(valores);
					datos.push(valores);
				});
			}
			return datos;
		}
		function conceptos_adicionales(justificacion, categoria, tipoajuste) {
			justificacion = justificacion.replace(/\"/g, "");
			var list_justificacion = [];
			if (tipoajuste > 0) {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
				return list_justificacion;
			}
			var hoy = new Date();
			var fechaFormulario = new Date('2018-08-15');
			// Comparamos solo las fechas => no las horas!!
			hoy.setHours(0,0,0,0); // Lo iniciamos a 00:00 horas

			if (hoy <= fechaFormulario) {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
				return list_justificacion;
			}

			if ((categoria.toLowerCase()).indexOf("hospeda") > -1) {
				//justificacion_huespedes_alimentos, justificacion_noches
				var datos = justificacion.split("Noches de hospedaje: ");
				var huespedes_alimentos = quitar_punto_final($.trim(datos[0]));
				list_justificacion.push(["", huespedes_alimentos, "justificacion_huespedes_alimentos"]);
				list_justificacion.push(["Noches de hospedaje: ", $.trim(datos[1]), "justificacion_noches"]);
			} else if ((categoria.toLowerCase()).indexOf("autobus") > -1 ||
				(categoria.toLowerCase()).indexOf("autobús") > -1 ||
				(categoria.toLowerCase()).indexOf("autob") > -1) {
				var regreso = "";
				if ((justificacion.toLowerCase()).indexOf("ida:") > -1) {
					var ida = justificacion.replace("Ida: ", "");
					if ((ida.toLowerCase()).indexOf("regreso:") > -1) {
						var regreso1 = ida.split("Regreso: ");
						ida = regreso1[0].replace(" y ", "");
						regreso = regreso1[1];
					}
					list_justificacion.push(["", "", "justificacion"]);
					if ($.trim(ida) !== "") {
						ida = quitar_punto_final($.trim(ida));
						list_justificacion.push(["Ida", ida, "justificacion_autobus_ida"]);
					}
					if ($.trim(regreso) !== "") {
						regreso = quitar_punto_final($.trim(regreso));
						list_justificacion.push(["Regreso", regreso, "justificacion_autobus_regreso"]);
					}
				} else if ((justificacion.toLowerCase()).indexOf("regreso:") > -1) {
					list_justificacion.push(["", "", "justificacion"]);
					regreso = quitar_punto_final($.trim(justificacion.replace("Regreso: ", "")));
					list_justificacion.push(["Regreso", regreso, "justificacion_autobus_regreso"]);
				}
				console.log(list_justificacion);
			} else if ((categoria.toLowerCase()).indexOf("caseta") > -1) {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
			} else if ((categoria.toLowerCase()).indexOf("uber") > -1 || (categoria.toLowerCase()).indexOf("taxi") > -1) {
				var datos1 = justificacion.replace("Origen: ", "");
				var datos2 = datos1.split(" Destino: ");
				var uber_taxi_origen = quitar_punto_final($.trim(datos2[0]));
				var uber_taxi_destino = quitar_punto_final($.trim(datos2[1]));
				list_justificacion.push(["", "", "justificacion"]);
				list_justificacion.push(["Origen", uber_taxi_origen, "justificacion_uber_taxi_origen"]);
				list_justificacion.push(["Destino", uber_taxi_destino, "justificacion_uber_taxi_destino"]);
			} else if ((categoria.toLowerCase()).indexOf("estacionamiento") > -1) {
				var datos = justificacion.split("Duración: ");
				var horas = "";
				var dias = "";
				if ((datos[1].toLowerCase()).indexOf("día(s)") > -1) {
					if ((datos[1].toLowerCase()).indexOf("hora(s)") > -1) {
						var datos2 = datos[1].split("día(s) y ");
						dias = datos2[0];
						horas = datos2[1].replace(" hora(s)", "");
					} else {
						dias = datos[1].replace(" día(s)", "");
					}

				} else {
					horas = datos[1].replace(" hora(s)", "");
				}

				var estacionamiento = quitar_punto_final($.trim(datos[0]));

				list_justificacion.push(["", estacionamiento, "justificacion_estacionamiento"]);
				if (dias !== "") {
					list_justificacion.push(["Día(s)", $.trim(dias), "justificacion_estacionamiento_dias"]);
				}
				if (horas !== "") {
					list_justificacion.push(["Hora(s)", $.trim(horas), "justificacion_estacionamiento_horas"]);
				}
			} else if ((categoria.toLowerCase()).indexOf("otro") > -1 && (categoria.toLowerCase()).indexOf("viaje") > -1) {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
			} else if ((categoria.toLowerCase()).indexOf("traslado") > -1 && (categoria.toLowerCase()).indexOf("cobranza") > -1) {
				var datos1 = justificacion.split(" Origen: ");
				var datos2 = datos1[1].split(" Destino: ");

				var traslado_cobranza = quitar_punto_final($.trim(datos1[0]));
				var traslado_cobranza_origen = quitar_punto_final($.trim(datos2[0]));
				var traslado_cobranza_destino = quitar_punto_final($.trim(datos2[1]));

				list_justificacion[0] = datos1[0];
				list_justificacion.push(["", traslado_cobranza, "justificacion_traslado_cobranza"]);
				list_justificacion.push(["Origen", traslado_cobranza_origen, "justificacion_traslado_cobranza_origen"]);
				list_justificacion.push(["Destino", traslado_cobranza_destino, "justificacion_traslado_cobranza_destino"]);
			} else if ((categoria.toLowerCase()).indexOf("traslado") > -1 &&
				(categoria.toLowerCase()).indexOf("cabina") > -1 &&
				(categoria.toLowerCase()).indexOf("siniestro") > -1) {
				var datos1 = justificacion.split(" Origen: ");
				var datos2 = datos1[1].split(" Destino: ");
				var traslado_cabina_siniestro = quitar_punto_final($.trim(datos1[0]));
				var traslado_cabina_siniestro_origen = quitar_punto_final($.trim(datos2[0]));
				var traslado_cabina_siniestro_destino = quitar_punto_final($.trim(datos2[1]));
				list_justificacion.push(["", traslado_cabina_siniestro, "justificacion_traslado_cabina_siniestro"]);
				list_justificacion.push(["Origen", traslado_cabina_siniestro_origen, "justificacion_traslado_cabina_siniestro_origen"]);
				list_justificacion.push(["Destino", traslado_cabina_siniestro_destino, "justificacion_traslado_cabina_siniestro_destino"]);
			} else if ((categoria.toLowerCase()).indexOf("premio") > -1 &&
				(categoria.toLowerCase()).indexOf("cuaderno") > -1 &&
				(categoria.toLowerCase()).indexOf("incentivo") > -1) {
				var datos = justificacion.split(" Agente: ");
				var premio_cuaderno_incentivo = quitar_punto_final($.trim(datos[0]));
				var premio_cuaderno_incentivo_agente = quitar_punto_final($.trim(datos[1]));
				list_justificacion.push(["", premio_cuaderno_incentivo, "justificacion_premio_cuaderno_incentivo"]);
				list_justificacion.push(["Agente", premio_cuaderno_incentivo_agente, "justificacion_premio_cuaderno_incentivo_agente"]);
			} else if (((categoria.toLowerCase()).indexOf("alimenta") > -1 || (categoria.toLowerCase()).indexOf("sesion") > -1)) {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
			} else {
				list_justificacion.push(["", $.trim(justificacion), "justificacion"]);
			}
			return list_justificacion;
		}
		function StrToJSON(strJson) {
			var JsonStr = [];
			strJson = strJson.replace(/\\/g,'\\');
			try {
				JsonStr = JSON.parse(strJson);
			} catch (e) {
				try {
					var gJson = JSON.stringify(eval('(' + strJson + ')'));
					var JSONObj=JSON.parse(gJson);
					JsonStr = JSONObj;
				} catch (e) {
					try {
						JsonStr = JSONize(strJson);
					} catch (e) {
						try {
							JsonStr = JSON.parse(JSONize(strJson)) 
						} catch (e) {
							var errorString= strJson;
							var jsonValidString = JSON.stringify(eval("(" + errorString+ ")"));
 							var JSONObj=JSON.parse(jsonValidString);
							JsonStr = JSONObj;
						}
					}
				}
			}
			return JsonStr;
		}
function JSONize(str) {
  return str
    // wrap keys without quote with valid double quote
    .replace(/([\$\w]+)\s*:/g, function(_, $1){return '"'+$1+'":'})    
    // replacing single quote wrapped ones to double quote 
    .replace(/'([^']+)'/g, function(_, $1){return '"'+$1+'"'})         
}
		function quitar_punto_final(cadena) {
			var cadena_final = $.trim(cadena);
			if (cadena_final.substr(-1) === ".") {
				cadena_final = cadena_final.substr(0, cadena_final.length - 1);
			}
			return cadena_final;
		}
		function valores_edit_justificacion(justificacion, categoria) {
			var datos = [];
			var njustificacion = justificacion.length;
			if (njustificacion === 1) {
				datos.push({ "justificacion": justificacion[0][1] });
			} else {
				$.each(justificacion, function (key, value) {
					var campo = valorVacio(value[2]) ? "" : value[2];
					var valor = value[1];
					var valores = "{\"" + campo + "\": \"" + valor + "\"}";
					valores = StrToJSON(valores);//JSON.parse(valores);
					datos.push(valores);
				});
			}
			return datos;
		}
		function DatosRequisicion(RmRdeRequisicion) {
			var RmReqId = RmRdeRequisicion * 1;
			var datos = { 'Usuario': UsuarioActivo, 'RmReqId': RmReqId, 'Empleado': EmpeladoActivo };
			var resultado = [];
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ConsultaRequisicionIDCabecera',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//
				},
				success: function (result) {
					resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
				},
				error: function (result) {
					console.log("error", result);
				}
			});
			return resultado;
		}
		function habilitaControlesInfo(idproyecto, id, estatus, uresponsable, DesactivaControl, hAutorizarComprobar, esvobo, esvobo_2, idinforme_2, autorizador_final) {
			$("#aenvia, #aautoriza, #autorizadores").hide();
			if (estatus === 3 || estatus === 7 || estatus === 5 || estatus === 4 || estatus === 8) {//3 = por autorizar, 7 = autorizado parcial, 5 = autorizado
				if (hAutorizarComprobar === "comprobar") {
					$("#aenvia").show();//envia a comprobacion

					if (DesactivaControl === 0) {
						"#aenvia".AsHTML('<span class="glyphicon glyphicon-envelope"></span>&nbsp;Enviar a Comprobaci&oacute;n');
					} else {
						"#aenvia".AsHTML('<span class="glyphicon glyphicon-envelope"></span>&nbsp;Enviar a Revisi&oacute;n');
					}


				} else if (hAutorizarComprobar === "autorizar") {
					$("#aautoriza").show();//autoriza
				}
				$("#arechaza").show();//rechaza

				if ((DesactivaControl * 1) === 1) {
					$("#autorizadores").hide();
				}
				else {
					$("#autorizadores").show();
				}
			}
			if (esvobo_2 === 1) {
				$("#aautoriza").show();//autoriza
				$("#arechaza").hide();//rechaza
			}
			console.log(autorizador_final);

			if (autorizador_final === 1) {
				"#aenvia".AsHTML('<span class="glyphicon glyphicon-envelope"></span>&nbsp;Enviar a Comprobaci&oacute;n');
			}

		}
		function verComprobante(dircomp, comprobante) {

			$("#ModalLabelComprobante").empty();
			$("#ModalLabelComprobante").append("Comprobante: " + comprobante);
			$("#compPDF").removeAttr("data");
			$("#print_compXML").hide();
			$("#compPDF").hide();
			$("#divPDF").hide();
			$("#compOTRO").removeAttr("src");
			$("#compOTRO").hide();
			$("#controles_compOTRO").empty();
			$("#controles_compXML").empty();
			$("#qrxml").removeAttr("src");

			if (comprobante === "XML") {
				$("#print_compXML").show();
				$.ajax({
					async: false,
					type: "GET",
					contentType: false,
					url: "/" + dircomp,
					dataType: "xml",
					success: function (response) {
						//console.log(response);
						$("#verComprobante").modal('show');
						var controles = "";
						controles = "&nbsp;<a type='button' class='btn btn-warning btn-xs' href='#'  onclick='imprimirXML()' title='Imprimir'><i class='zmdi zmdi-print zmdi-hc-2x'></i> </a>";
						controles += "&nbsp;<a type='button' class='btn btn-warning btn-xs' href='" + dircomp + "' target='_blank' title='Descargar'><i class='zmdi zmdi-download zmdi-hc-2x'></i> </a>";
						$("#controles_compXML").append(controles);

						var xmljson = xmlToJson(response);
						//console.log(xmljson);
						verFacturaJSONenHTML(xmljson, 0, 0, 0);
					},
					error: function (result) {
						console.log("error", result);
						$.notify("Error: Al consultar XML.", { globalPosition: 'top center', className: 'error' });
					}
				});
				if (dircomp === "") {
					$("#print_compXML").empty();
				}
			}

			if (comprobante === "PDF") {
				$("#verComprobante").modal('show');
				$("#divPDF").show();
				//"#controles_compPDF".AsHTML(btnEliCom);
				$("#controles_compPDF").empty();
				$("#compPDF").show();
				$("#compPDF").attr("data", "/" + dircomp);
			}
			if (comprobante === "OTRO") {
				$("#verComprobante").modal('show');
				dircomp = "/" + dircomp;
				var controles = "";
				controles += "&nbsp;<a type='button' class='btn btn-warning btn-xs' href='#' dir='" + dircomp + "' onclick='imprimirImg()' title='Imprimir'><i class='zmdi zmdi-print zmdi-hc-2x'></i> </button> ";
				controles += "&nbsp;<a type='button' class='btn btn-primary btn-xs' href='#' dir='" + dircomp + "' onclick='rotarImg(\"" + dircomp + "\", -90)' title='Rotar a la izquierda'><i class='zmdi zmdi-rotate-left zmdi-hc-2x'></i> </a>";
				controles += "&nbsp;<a type='button' class='btn btn-primary btn-xs' href='#' onclick='rotarImg(\"" + dircomp + "\", 90)' title='Rotar a la derecha'><i class='zmdi zmdi-rotate-right zmdi-hc-2x'></i> </a>";
				controles += "&nbsp;<a type='button' class='btn btn-warning btn-xs' href='" + dircomp + "' target='_blank' title='Descargar'><i class='zmdi zmdi-download zmdi-hc-2x'></i> </a>";
				var f = new Date();
				var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
				$("#controles_compOTRO").append(controles);
				$("#compOTRO").show();
				$("#compOTRO").attr("src", dircomp + "?" + fh);
			}

		}
		function rotarImg(imagen, angulo) {
			var datos = {
				'Imagen': imagen,
				'Angulo': angulo
			};
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/RotarImagen",
				data: datos,
				dataType: "json",
				beforeSend: function () {
					//
				},
				success: function (result) {
					var f = String(new Date().getTime()) + "-" + Math.random();
					$("#compOTRO").attr("src", imagen + "?" + f);
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function imprimirImg() {
			$("#print_compOTRO").printArea({
				mode: "iframe",
				standard: "html5",
				popTitle: 'relatorio',
				popClose: false,
				extraCss: 'css/app.css',
				extraHead: '',
				retainAttr: ["id", "class", "style"],
				printDelay: 500, // tempo de atraso na impressao
				printAlert: true,
				printMsg: 'Aguarde'
			});
		}
		function imprimirXML() {
			$("#print_compXML").printArea({
				mode: "iframe",
				standard: "html5",
				popTitle: 'relatorio',
				popClose: false,
				extraCss: 'css/app.css',
				extraHead: '',
				retainAttr: ["id", "class", "style"],
				printDelay: 500, // tempo de atraso na impressao
				printAlert: true,
				printMsg: 'Aguarde'
			});
		}
		function editar_gasto(datos_gasto) {
			menucategorias();
			$("#mEditarGastoInf").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			var gasto = StrToJSON(datos_gasto);//JSON.parse(datos_gasto);
			console.log(gasto);

			"#inpustGasto .dia".AsHTML(gasto.dia);
			$("#idGasto").val(gasto.idgasto);
			$("#gasto").val(JSON.stringify(gasto));
			$("#categoria").val(gasto.idcategoria);
			"#inpustGasto .justificacion".AsHTML(gasto.justificacion);
			"#inpustGasto .monto".AsHTML(gasto.monto);
			"#inpustGasto .monto_comprobado".AsHTML(gasto.monto_comprobado);
			"#inpustGasto .dentro_politica".AsHTML(gasto.dentro_politica);
			$("#fuera_politica").val(gasto.num_fuera_politica);
			$("#no_deducible").val(gasto.num_no_deducible);
		}
		function menucategorias() {
			var IdRequisicion = $("#RmRdeRequisicion").val();
			var datos = {
				"Requisicion": IdRequisicion,
				"Valida": 0,
				'Usuario': UsuarioActivo,
				'Empleado': EmpeladoActivo
			};
			$("#categoria").empty();
			var i = 0;
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/ConsultaMaterial2",
				data: datos,
				dataType: "json",
				beforeSend: function () {
					$("#categoria").append("<option value='0' data-informacion=''>- Categoria -</option>");
				},
				success: function (result) {
					var stResultado = result.Salida.Resultado;
					if (stResultado === "1") {
						var resultado = result.Salida.Tablas.Materiales.NewDataSet.Materiales;
						var nres = 0;
						try {
							nres = resultado.length;
						} catch (err) {
							nres = 0;
						}
						if (nres > 0) {
							$.each(resultado, function (key, value) {
								var GrMatId = datoEle(value.GrMatId);
								var GrMatNombre = datoEle(value.GrMatNombre);
								var GrMatPrecio = datoEle(value.GrMatPrecio) * 1;
								var GrMatIva = datoEle(value.GrMatIva) * 1;
								var GrMatGrupo = datoEle(value.GrMatGrupo);
								var GrMatUnidadMedida = datoEle(value.GrMatUnidadMedida);
								var option = "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";
								$("#categoria").append(option);
							});
						} else {
							var GrMatId = datoEle(resultado.GrMatId);
							var GrMatNombre = datoEle(resultado.GrMatNombre);
							var GrMatPrecio = datoEle(resultado.GrMatPrecio) * 1;
							var GrMatIva = datoEle(resultado.GrMatIva) * 1;
							var GrMatGrupo = datoEle(resultado.GrMatGrupo);
							var GrMatUnidadMedida = datoEle(resultado.GrMatUnidadMedida);
							var option = "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";
							$("#categoria").append(option);
						}
					}
				},
				error: function (result) {
					console.log(result);
				}
			});
		}

		$("#aenvia").click(function () {
			var idinforme = $("#idinforme").val();
			var RmRdeRequisicion = $("#RmRdeRequisicion").val();
			var UsuarioActivo = localStorage.getItem("cosa");
			var req = DatosRequisicion(RmRdeRequisicion);
			var datos =
				{
					"idinforme": idinforme,
					"Usuario": UsuarioActivo
				};

			var totalg = $("#totalg").val() * 1;//monto a comprobar informe
			var RmReqImporteComprobar = datoEle(req.RmReqImporteComprobar) * 1;//monto a comprobar requisicion
			var estatus = req.RmReqEstatus * 1;
			var estatusObligatorioReq = "Fondo Retirado";
			var estatusActualReq = req.RmReqEstatusNombre;

			if (estatus === 52) {
				if (totalg.toFixed(2) === RmReqImporteComprobar.toFixed(2)) {
					confEnviarAAutorizacion(datos);
				} else {
					//confEnviarAAutorizacion(datos);
					Seguridad.alerta("No puedes enviar el Informe.<br />El importe Gastado del Informe (" +
						formatNumber.new(totalg.toFixed(2), "$ ") + ") debe ser igual al importe comprobado en la requisición " +
						"(" + formatNumber.new(RmReqImporteComprobar.toFixed(2), "$ ") + ").");
				}
			} else {
				Seguridad.alerta("No puedes enviar el Informe.<br />Tu requisición necesita estar en estatus <b>'" +
					estatusObligatorioReq + "'</b><br />" +
					"Estatus Actual de la requisición <b>'" + estatusActualReq + "'</b>.");
			}

		});
		function confEnviarAAutorizacion(datos) {
			let confirmaComprobacion = Handlebars.compile($("#modal-confirma-comprobacion-msn").html());
			let alertaTitulo = Handlebars.compile($("#modal-alerta-titulo").html());
			let botones = Handlebars.compile($("#modal-botones").html());

			var boton = {
				btn1: true,
				label1: "Enviar",
				tipo1: "primary",
				function1: "enviarComprobarRevisar()",
				icono1: " zmdi zmdi-mail-send",
				btn2: false,
				btn3: false
			}

			$('#titulo_modal_alert').empty().append(alertaTitulo({ titulo: 'Enviar Informe' }));
			$("#contenido_modal_alert").empty().append("Desea enviar el informe?<br />" + confirmaComprobacion());
			$("#footer_modal_alert").empty().append(botones(boton));

			$("#modal_alerta").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
            });
            setTimeout(function () { $("#comentariosValidacion").focus(); }, 500);
			habilitaChkVoBo();
		}
		function enviarComprobarRevisar() {
			var idinforme = $("#idinforme").val();
            var req = DatosRequisicion();
            var comentariosValidacion = $.trim($("#comentariosValidacion").val());
            var nmbemp = localStorage.getItem("nmbemp");
            var fecha = fechaActual() + " " + horaActual("hh:mm");
            var comentariode = ". (Validado por: " + nmbemp + " / " + fecha + ")";
            comentariosValidacion += comentariosValidacion !== "" ? comentariode : "";

			var datos =
				{
					"idinforme": idinforme,
					"comentariosValidacion": comentariosValidacion,
					"Usuario": UsuarioActivo
            };
			if ($("#ChkVoBo").is(":checked")) {
				//console.log("enviado a vobo");
				enviarVoBo();
			} else {
				//console.log("enviado a adminerp");
				enviarAAutorizacion(datos);
			}
		}
		function habilitaChkVoBo() {
			var UsuarioEncriptadoActivo = localStorage.getItem("cosa");
			var UsuariDesencriptadoActivo = encriptaDesencriptaEle(UsuarioEncriptadoActivo, 0);
			var idinforme = $("#idinforme").val() * 1;
			$.ajax({
				async: true,
				type: 'POST',
				url: '/api/VistoBueno',
				data: JSON.stringify({ 'c_accion': 'VOBO', 'idinforme': idinforme, 'usuarioActual': UsuariDesencriptadoActivo }),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (result) {

					var resultado = result[0];

					if (resultado.Resultado == 'OK') {
						$("#divChkVoBo").show();

						c_usuario_nombre = resultado.c_usuario_nombre;
						c_correo = resultado.c_correo;
						c_usuario = resultado.c_usuario;
						c_valor_default = resultado.c_valor_default * 1;
						c_chk_bloqueado = resultado.c_chk_bloqueado * 1;
						c_duracion_ini = resultado.c_duracion_ini;
						c_duracion_fin = resultado.c_duracion_fin;

						if (c_valor_default === 1) {
							$("#ChkVoBo").attr("checked", true);
						} else {
							$("#ChkVoBo").removeAttr("checked");
						}

						if (c_chk_bloqueado === 1) {
							$("#ChkVoBo").attr("disabled", true);
						} else {
							$("#ChkVoBo").removeAttr("disabled");
						}

						$("#HFUsuariovobo").val(c_usuario);

					}
					else {
						$("#divChkVoBo").hide();
						$("#ChkVoBo").removeAttr("checked");
						$("#ChkVoBo").removeAttr("disabled");
					}


				},
				error: function (result) {
					console.log(result);
				}

			});
		}
		function enviarVoBo() {
			var usuarioActual = localStorage.getItem("cosa");;
			var usuariovobo = $("#HFUsuariovobo").val();
            var idinforme = $("#idinforme").val();
            var comentariosValidacion = $.trim($("#comentariosValidacion").val());
            var nmbemp = localStorage.getItem("nmbemp");
            var fecha = fechaActual() + " " + horaActual("hh:mm");
            var comentariode = ". (Validado por: " + nmbemp + " / " + fecha + ")";
            comentariosValidacion += comentariosValidacion !== "" ? comentariode : "";

			var datos = {
				"usuarioActual": usuarioActual,
				"usuariovobo": usuariovobo,
                "idinforme": idinforme,
                "comentariosValidacion": comentariosValidacion
			};

			$.ajax({
				async: true,
				type: 'POST',
				url: '/api/Enviovobo',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				beforeSend: function () {
					var ocultarModal = $('#modal_alerta').is(':visible');
					if (ocultarModal)
						$("#modal_alerta").modal('hide');
					cargando();
				},
				success: function (result) {
                    console.log(result);
					$.notify("Se ha enviado correctamente a revisión.", { globalPosition: 'top center', className: 'success' });
					//$("#verInformeGastos").modal("hide");
					setTimeout(function () {
						//window.location.href = "/Autorizaciones?" + fh;
					}, 1000);
				},
				error: function (result) {
					cargado();
					$.notify("Error al enviar a revisión.", { globalPosition: 'top center', className: 'error' });
					console.log(result);
				}
			});
		}
		function enviarAAutorizacion(datos) {
/*
cargando();
				var respuesta = enviarAAutorizacion(datos);
				//if (respuesta.stResultado === 1) {
				if (respuesta.stResultado === 1) {
					$.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'success' });
					setTimeout(function () {
						window.location.href = "/Autorizaciones?" + fh;
					}, 2000);
				} else {
					//"Error al enviar comprobacion,favor de verificar"
					$.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'error' });
				}
				cargado();
*/
            var respuesta = [];
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/Comprobacion",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					cargando()
					var ocultarModal = $('#modal_alerta').is(':visible');
					if (ocultarModal)
						$("#modal_alerta").modal("hide");
					//$("#verInformeGastos").modal('hide');
				},
				success: function (result) {
					//var stResultado = result.Salida.Resultado * 1;
					//if (stResultado === 1) {

					var stResultado = result;
					if (stResultado === 'OK') {
						respuesta = {
							'stResultado': 1,
							'descripcion': 'El informe se envio a comprobación.'
						};
$.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'success' });
setTimeout(function () {
						window.location.href = "/Autorizaciones?" + fh;
cargado();
					}, 2000);
					} else {
						var error = stResultado; //datoEle(result.Salida.Errores.Error.Descripcion);
						if (valorVacio(error)) {
							error = "Error el informe no se pudo enviar.";
						}
						respuesta = {
							'stResultado': 0,
							'descripcion': error
						};
					$.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'error' });
					}
					
				},
				error: function (result) {
					console.log(result);
					cargado();
					respuesta = {
						'stResultado': 0,
						'descripcion': result
					};
$.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'error' });
				}
			});
			return respuesta;
		}

		$("#arechaza").click(function () {
			let confirmaRechazo = Handlebars.compile($("#modal-confirma-rechazo").html());
			let alertaTitulo = Handlebars.compile($("#modal-alerta-titulo").html());
			let botones = Handlebars.compile($("#modal-botones").html());

			var boton = {
				btn1: true,
				label1: "Rechazar",
				tipo1: "primary",
				function1: "rechazar()",
				icono1: " zmdi zmdi-mail-send",
				btn2: false,
				btn3: false
			}

			$('#titulo_modal_alert').empty().append(alertaTitulo({ titulo: 'Rechazar Informe' }));
			$("#contenido_modal_alert").empty().append(confirmaRechazo());
			$("#footer_modal_alert").empty().append(botones(boton));

			$("#modal_alerta").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			
			setTimeout(function () {
				$("#ComentariosRechazo").focus();
			},500);
		});
		function rechazar() {
			var idinforme = $("#idinforme").val();
			var UsuarioActivo = localStorage.getItem("cosa");
			var nmbemp = localStorage.getItem("nmbemp");

			var Comentarios = $.trim($("#ComentariosRechazo").val());
			if (Comentarios === "") {
				$.notify("Se requiere un comentario de rechazo.", { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
				return false;
			}
			var fecha = fechaActual() + " " + horaActual("hh:mm");
			var datos =
				{
					"idinforme": idinforme,
					"comentarioaut": Comentarios + ". (Rechazado por: " + nmbemp + " / " + fecha + ")",
					"usuario": UsuarioActivo
				};
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/RechazarInforme",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					cargando();
					var ocultarModal = $('#modal_alerta').is(':visible');
					if (ocultarModal)
						$("#modal_alerta").modal('hide');
				},
				success: function (result) {
					$.notify("El informe se ha rechazado correctamente", { globalPosition: 'top center', className: 'success', autoHideDelay: 8000 });
					setTimeout(function () {
						window.location.href = "/Autorizaciones?" + fh;
					}, 2000);
				},
				complete: function () {
					
				}
			});

		}

		$("#aautoriza").click(function () {
			let ConfirmaRegresaVobo = Handlebars.compile($("#modal-confirma-regresa-vobo").html());
			let alertaTitulo = Handlebars.compile($("#modal-alerta-titulo").html());
			let botones = Handlebars.compile($("#modal-botones").html());

			var boton = {
				btn1: true,
				label1: "Enviar Mis Comentarios",
				tipo1: "primary",
				function1: "regresarComentarioVoBo()",
				icono1: " zmdi zmdi-mail-send",
				btn2: false,
				btn3: false
			}

			$('#titulo_modal_alert').empty().append(alertaTitulo({ titulo: 'Enviar Mis Comentarios' }));
			$("#contenido_modal_alert").empty().append(ConfirmaRegresaVobo());
			$("#footer_modal_alert").empty().append(botones(boton));

			$("#modal_alerta").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			setTimeout(function () {
				$("#comentarioVoBo").focus();
			},500);
		});
		function regresarComentarioVoBo() {
			var idinforme = $("#idinforme").val();
			var idrequisicion = $("#RmRdeRequisicion").val();
			var UsuarioActivo = localStorage.getItem("cosa");
			var nmbemp = localStorage.getItem("nmbemp");
			var datos = {
				"idinforme": idinforme,
				"idrequisicion": idrequisicion,
				"Usuario": UsuarioActivo
			};
			var Comentarios = $.trim($("#comentarioVoBo").val());
			if (Comentarios === "") {
				$.notify("Se requiere un comentario de VoBo.", { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
				return false;
			} 
			var fecha = fechaActual() + " " + horaActual("hh:mm");
			var usuario_fecha = ". (Enviado por: " + nmbemp + " / " + fecha + ")";
			datos['comentario'] = Comentarios + usuario_fecha;
			datos['comentario_respuesta'] = Comentarios;
			datos['usuario_fecha_responde'] = nmbemp + " / " + fecha;
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/AutorizaInforme",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					cargando();
					var ocultarModal = $('#modal_alerta').is(':visible');
					if (ocultarModal)
						$("#modal_alerta").modal("hide");
				},
				success: function (result) {
					$.notify("Los comentarios fueron enviados correctamente.", { globalPosition: 'top center', className: 'success' });
					setTimeout(function () {
						window.location.href = "/Autorizaciones?" + fh;
						cargado();
					}, 1000);
				},
				error: function () {
					setTimeout(function () {
						$.notify("Error al enviar comentarios.", { globalPosition: 'top center', className: 'error' });
						cargado();
						$("#modal_alerta").modal({
							show: true,
							keyboard: false,
							backdrop: "static"
						});
					}, 600);
				}
			});
		}

		$("#btnGuardaGasto").click(function () {
			var IdGasto = $("#idGasto").val();
			var IdInforme = $("#idinforme").val();
			var gasto = StrToJSON($("#gasto").val());//JSON.parse($("#gasto").val());
			var catActualizada = actualizarCtaGasto(IdGasto, IdInforme);
			var noAcepActualizada = CambioNoAceptable(IdInforme, IdGasto, gasto);
			var noDecActualizada = CambioNoDeducible(IdInforme, IdGasto, gasto);
			//console.log(catActualizada, noAcepActualizada, noDecActualizada);
			if (catActualizada === 1 && noAcepActualizada === 1 && noDecActualizada === 1) {
				browseGastos(IdInforme);
				$.notify("Gasto Actualizado.", { globalPosition: 'top center', className: 'success' });
				$("#mEditarGastoInf").modal('hide');
			}
		});
		function actualizarCtaGasto(IdGasto, IdInforme) {
			var Categoria = $("#categoria").val();
			var CategoriaSelect = document.getElementById("categoria");
			var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
			var ok = 0;
			var datos = {
				'IdInforme': IdInforme,
				'IdGasto': IdGasto,
				'Categoria': Categoria,
				'NombreCategoria': NombreCategoria
			};
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/ActualizarCtaGasto",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				success: function (result) {
					ok = 1;
				},
				error: function (result) {
					console.log(result);
					ok = 0;
					$.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
				}
			});
			return ok;
		}
		function CambioNoAceptable(idinforme, idgasto, gasto) {
			var importeNoAceptable = $("#fuera_politica").val() * 1;
			var importeNoDeducible = $("#no_deducible").val() * 1;
			var monto = gasto.num_monto;
			var ok = 0;

			if ((importeNoAceptable <= monto && importeNoAceptable >= 0) &&
				(importeNoDeducible <= monto && importeNoDeducible >= 0)) {
				var datos = {
					'IdInforme': idinforme,
					'IdGasto': idgasto,
					'ImporteAceptable': 0,
					'Monto': monto,
					'ImporteNoAceptable': importeNoAceptable,
					'ImporteNoDeducible': importeNoDeducible
				};
				$.ajax({
					async: false,
					type: "POST",
					url: "/api/GastoImporteNoAceptable",
					data: JSON.stringify(datos),
					contentType: 'application/json; charset=utf-8',
					dataType: 'json',
					cache: false,
					success: function () {
						ok = 1;
						//$.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
					},
					error: function (result) {
						console.log(result);
						ok = 0;
						$.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
					}
				});
			} else {
				ok = 0;
				$.notify("El importe fuera de politica debe ser mayor o igual a 0 y menor o igual al importe a comprobar.", { globalPosition: 'top center', className: 'error' });
			}
			return ok;
		}
		function CambioNoDeducible(idinforme, idgasto, gasto) {
			var importeNoAceptable = $("#fuera_politica").val() * 1;
			var importeNoDeducible = $("#no_deducible").val() * 1;
			var monto = gasto.num_monto;
			var ok = 0;

			if ((importeNoAceptable <= monto && importeNoAceptable >= 0) &&
				(importeNoDeducible <= monto && importeNoDeducible >= 0)) {
				var datos = {
					'IdInforme': idinforme,
					'IdGasto': idgasto,
					'ImporteAceptable': 0,
					'Monto': monto,
					'ImporteNoAceptable': importeNoAceptable,
					'ImporteNoDeducible': importeNoDeducible
				};
				$.ajax({
					async: false,
					type: "POST",
					url: "/api/GastoImporteNoAceptable",
					data: JSON.stringify(datos),
					contentType: 'application/json; charset=utf-8',
					dataType: 'json',
					cache: false,
					success: function () {
						ok = 1;
						//$.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
					},
					error: function (result) {
						ok = 0;
						$.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
					}
				});
			} else {
				ok = 0;
				$.notify("El importe no deducible debe ser mayor o igual a 0 y menor o igual al importe a comprobar.", { globalPosition: 'top center', className: 'error' });
			}
			return ok;
		}

		$("#autorizadores").click(function () {

			$("#tabAutoriza").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			if (localStorage.getItem('autOpcInf')) {
				localStorage.removeItem('autOpcInf');
			}
			//SelectProcesosRequisicion();
			selectUsuarios([]);

		});
		function selectUsuarios(listAutDefault) {
			var listUsuarios = [];
			var UsuariosLista = false;
			$.ajax({
				async: true,
				type: "POST",
				url: '/api/ConsultaCatalogoUsuarios',
				contentType: 'application/json; charset=utf-8',
				data: JSON.stringify({ 'Usuario': UsuarioActivo }),
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					var exito = result.Salida.Resultado * 1;
					if (exito === 1) {
						var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
						var nusuarios = 0;
						try {
							nusuarios = resultado.length;
						} catch (err) {
							nusuarios = 0;
						}
						if (nusuarios > 0) {
							$.each(resultado, function (key, value) {
								if ($.inArray($.trim(value.SgUsuId), listAutDefault) === -1 &&
									!valorVacio($.trim(value.SgUsuId)) &&
									value.SgUsuActivo === "true" &&
									!valorVacio(datoEle(value.SgUsuEmpleado))) {
									listUsuarios.push(value);
									UsuariosLista = true;
								}
							});
						} else {
							if ($.inArray($.trim(resultado.SgUsuId), listAutDefault) === -1 &&
								!valorVacio($.trim(resultado.SgUsuId)) &&
								resultado.SgUsuActivo === "true" &&
								!valorVacio(datoEle(resultado.SgUsuEmpleado))) {
								listUsuarios.push(resultado);
								UsuariosLista = true;
							}
						}
					} else {
						$.notify("Error: Al consultar Usuarios.", { globalPosition: 'top center', className: 'error' });
					}
				},
				complete: function () {
					//cargado();
					//selectEmpleado(listUsuarios);
					$("#mAutOpcional").empty();
					$("#tblAutOpcional").hide();
					if (UsuariosLista === true) {
						$("#mAutOpcional").append("<option value=''> - Seleccionar Para Agregar - </option>");
						$.each(listUsuarios, function (key, value) {
							var usu = $.trim(value.SgUsuId);
							var nmb = $.trim(value.SgUsuNombre);
							var option = "<option value='" + usu + "'>" + nmb + "</option>";
							$("#mAutOpcional").append(option);
						});
						$("#mAutOpcional").select2({
							dropdownParent: $("#lblAutOpcional")
						});
					}
				},
				error: function (result) {
					//cargado();
					console.log("error", result);
				}
			});
		}
		$("#mAutOpcional").change(function () {
			var usuario = $("#mAutOpcional").val();
			var mAutOpcional = $("#mAutOpcional");
			var nombre = mAutOpcional[0].options[mAutOpcional[0].selectedIndex].text;
			var autOpc = [];
			if (!localStorage.getItem('autOpcInf')) {
				localStorage.setItem('autOpcInf', []);
			} else {
				autOpc = StrToJSON(localStorage.getItem('autOpcInf'));//JSON.parse(localStorage.getItem('autOpcInf'));
			}
			var usuValido = validaUsuarioSel(usuario, autOpc);
			if (usuValido.ok === true) {
				autOpc.push({ 'usuario': usuario, 'nombre': nombre });
			} else {
				$("#mAutOpcional").notify(usuValido.descripcion, { globalPosition: 'top center', className: 'error' });
			}
			localStorage.setItem('autOpcInf', JSON.stringify(autOpc));
			tablaAutOpc(autOpc);
		});
		function tablaAutOpc(autOpc) {
			$("#tblAutOpcional tbody").empty();
			var i = 1;
			$.each(autOpc, function (key, value) {
				if (!valorVacio(datoEle(value.usuario)) && !valorVacio(datoEle(value.nombre))) {
					var inputs = "<input type='hidden' id='usuOpc" + value.usuario + "' name='usuOpc' value='" + value.usuario + "' />";
					inputs += "<button type='button' class='btn btn-danger btn-sm glyphicon glyphicon-trash DelGas' onclick='eliminarUsuario(\"" + value.usuario + "\", \"" + value.nombre + "\")'></button>"
					var row = "<tr>";
					row += "<td>" + i + "</td>";
					row += "<td>" + value.nombre + "</td>";
					row += "<td>" + inputs + "</td></tr>";
					$("#tblAutOpcional tbody").append(row);
					i++;
				}
			});
			if (i > 1)
				$("#tblAutOpcional").show();

			$("#tblAutOpcional").hide();
		}
		function eliminarUsuario(usuario, nombre) {
			var botones = [];
			botones[0] = {
				text: "Si", click: function () {
					$(this).dialog("close");
					$("#tblAutOpcional tbody").empty();
					var autOpc = StrToJSON(localStorage.getItem('autOpcInf'));//JSON.parse(localStorage.getItem('autOpcInf'));
					$.each(autOpc, function (key, value) {
						if (value.usuario === usuario) {
							autOpc[key] = { 'usuario': '', 'nombre': '' };
						}
					});
					localStorage.setItem('autOpcInf', JSON.stringify(autOpc));
					tablaAutOpc(autOpc);
				}
			};
			botones[1] = {
				text: "No", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Elimina usuario: <b>" + nombre + "</b><br />como autorizador del informe?", botones, " Elimina Usuario Autorizador.", "#tblAutOpcional");
		}
		function validaUsuarioSel(usuario, autOpc) {
			var seleccionado = [];
			if ($("#usuResponsable").val() !== usuario) {
				seleccionado['ok'] = true;
				seleccionado['descripcion'] = "Usuario agregado como autorizador.";
				$.each(autOpc, function (key, value) {
					if (value.usuario === usuario) {
						$("#mAutOpcional").val("");
						seleccionado['ok'] = false;
						seleccionado['descripcion'] = "No puedes agregar el mismo usuario más de una vez.";
						return seleccionado;
					}
				});
			} else {
				$("#mAutOpcional").val("");
				seleccionado['ok'] = false;
				seleccionado['descripcion'] = "El responsable del informe no puede ser quien de el VoBo.";
				return seleccionado;
			}
			return seleccionado;
		}
		$("#EnviarAutorizadores").click(function () {
			
			$("#tabAutoriza").modal("hide");
			let confirmaSolicitarVobo = Handlebars.compile($("#modal-confirma-solicitar-vobo").html());
			let alertaTitulo = Handlebars.compile($("#modal-alerta-titulo").html());
			let botones = Handlebars.compile($("#modal-botones").html());

			var boton = {
				btn1: true,
				label1: "Enviar",
				tipo1: "primary",
				function1: "solicitarVoBo()",
				icono1: " zmdi zmdi-mail-send",
				btn2: false,
				btn3: false
			};

			$('#titulo_modal_alert').empty().append(alertaTitulo({ titulo: 'Enviar a VoBo' }));
			$("#contenido_modal_alert").empty().append(confirmaSolicitarVobo());
			$("#footer_modal_alert").empty().append(botones(boton));

			setTimeout(function () {
				$("#modal_alerta").modal({
					show: true,
					keyboard: false,
					backdrop: "static"
				});
				$("#modal_alerta").css({ 'z-index': 2000 });
				setTimeout(function () {
					$("#comentarioEnvioVoBo").focus();
				},500);
			}, 600);
			
		});
		function solicitarVoBo() {
			var idinforme = $("#idinforme").val()
			var RmReqId = $("#RmRdeRequisicion").val();
			var nmbemp = localStorage.getItem("nmbemp");
			var autorizadores = [];
			if (valorVacio($("#mAutOpcional").val())) {
				$.notify("Se requiere seleccionar un usuario.", { globalPosition: 'top center', className: 'error' });
				return false;
			}
			autorizadores.push($("#mAutOpcional").val());
			if (autorizadores.length > 0) {
				var Comentarios = $.trim($("#comentarioEnvioVoBo").val());
				if (Comentarios === "") {
					$.notify("Se requiere un comentario para envio a VoBo.", { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
					return false;
				} else {
					var ocultarModal = $('#modal_alerta').is(':visible');
					if (ocultarModal)
						$("#modal_alerta").modal("hide");

					ocultarModal = $('#tabAutoriza').is(':visible');
					if (ocultarModal)
						$("#tabAutoriza").modal("hide");
				}
				var fecha = fechaActual() + " " + horaActual("hh:mm");
				var usuario_fecha = ". (Enviado por: " + nmbemp + " / " + fecha + ")";
				var datos = {
					'RmReqId': RmReqId,
					'Usuario': UsuarioActivo,
					'Empleado': EmpeladoActivo,
					'idinforme': idinforme,
					'autorizadores': autorizadores,
					'comentario': Comentarios + usuario_fecha
				};

				$.ajax({
					async: true,
					type: "POST",
					url: '/api/EnviaAutorizadores',
					data: JSON.stringify(datos),
					contentType: 'application/json; charset=utf-8',
					dataType: 'json',
					cache: false,
					beforeSend: function () {
						cargando();
					},
					success: function (result) {
						cargado();
						$.notify("El informe se ha enviado a VoBo", { globalPosition: 'top center', className: 'success' });
						setTimeout(function () {
							window.location.href = "/Autorizaciones?" + fh;
						}, 1000);
					}
				});
			} else {
				cargado();
				setTimeout(function () {
					$("#tabAutoriza").modal({
						show: true,
						keyboard: false,
						backdrop: "static"
					});
				}, 600)
				$.notify("Se requiere seleccionar almenos a un usuario.", { globalPosition: 'top center', className: 'error' });
			}
		}

//inicio excel
		$("#aexportarxls").click(function () {
			var idinforme = $("#idinforme").val() * 1;
			var IdRequisicion = $("#RmRdeRequisicion").val();
			cargando();
			if (idinforme > 0) {
				var datos = {
					'IdInforme': idinforme,
					'IdRequisicion': IdRequisicion
				};

				var informe = selectInformeExcel(datos.IdInforme);
				if (informe.ok === true) {
					datos['NoInforme'] = informe.datos.i_ninforme;
					datos['NmbSolicitante'] = informe.datos.responsable;
					var requisicion = SelectRequisicionExcel(informe.datos.r_idrequisicion);
					var empleado = SelectEmpleado(requisicion.datos.RmReqSolicitante);
					if (requisicion.ok === true) {
						datos['TipoReq'] = datoEle(requisicion.datos.RmReqTipoRequisicionNombre);
						datos['RmReqCentro'] = datoEle(requisicion.datos.RmReqCentro);
						datos['RmReqUsuarioAlta'] = $.trim(datoEle(requisicion.datos.RmReqUsuarioAlta));
						datos['Departamento'] = ""; //datoEle(empleado);
						datos['Puesto'] = datoEle(empleado.GrEmpPuestoNombre);
						datos['Area'] = "";
						datos['Oficina'] = datoEle(requisicion.datos.RmReqOficinaNombre);
						datos['Centro'] = datoEle(requisicion.datos.RmReqCentroNombre);
					}
					generaExcel(datos);
				} else {
					console.log("error");
					$.notify("Error al generar excel.", { globalPosition: 'top center', className: 'error', autoHideDelay: 3000 });
				}
			} else {
				$.notify("Error al generar excel.", { globalPosition: 'top center', className: 'error', autoHideDelay: 3000 });
			}
			cargado();

		});

		function SelectEmpleado(GrEmpID) {
			var resultado = [];
//UsuarioActivo;EmpeladoActivo;
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ConsultaEmpleadoID',
				data: JSON.stringify({ 'GrEmpID': GrEmpID, 'Usuario': UsuarioActivo }),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					//console.log(result);
					var exito = result.Salida.Resultado * 1;
					if (exito === 1) {
						resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
					} else {
						$.notify("Error: Al consultar Empleado.", { globalPosition: 'top center', className: 'error' });
					}

				},
				complete: function () {
					//cargado();
				},
				error: function (result) {
					//cargado();
					console.log("error", result);
				}
			});
			return resultado;
		}

		function selectInformeExcel(id) {
			var datos = {
				"id": id
			};
			var datosInf = [];
			$.ajax({
				async: false,
				type: 'POST',
				url: '/api/SelectInforme',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (result) {
					datosInf['ok'] = true;
					datosInf['datos'] = result[0];
				},
				error: function (result) {
					console.log(result);
					datosInf['ok'] = false;
					datosInf['datos'] = "Error al consultar informe";
				}
			});
			return datosInf;
		}
		function SelectRequisicionExcel(id) {
			var datos = { 'Usuario': UsuarioActivo, 'RmReqId': id };
			var datosReq = [];
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ConsultaRequisicionIDCabecera',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				success: function (result) {
					var stResultado = result.Salida.Resultado;
					if (stResultado === "1") {
						var resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
						datosReq['ok'] = true;
						datosReq['datos'] = resultado;
					} else {
						datosReq['ok'] = false;
						datosReq['datos'] = "Error al consultar requisicion";
					}
				},
				error: function (result) {
					console.log(result);
					datosReq['ok'] = false;
					datosReq['datos'] = "Error al consultar requisicion";
				}
			});
			return datosReq;
		}
		function generaExcel(datos) {
			var rutaEli = "";
			; $.ajax({
				async: false,
				type: 'POST',
				url: '/api/ExportarExcel',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (result) {
					//console.log("SUCCESS", result)
					var rutas = result.split(",");
					var rutades1 = rutas[1];
					var rDescarga = rutades1.replace("api/ExportarExcel", "temp/") + rutas[2];
					rutaEli = rutas[0];
					window.location = rDescarga;
				},
				complete: function () {
					setTimeout(function () {
						if (rutaEli !== "") {
							eliminaExcel(rutaEli);
						}
					}, 4000);
				},
				error: function (result) {
					console.log(result)
				}
			});
		}
		function eliminaExcel(rutaEli) {
			$.ajax({
				async: true,
				type: 'POST',
				url: '/api/EliminaExcel',
				data: JSON.stringify({ 'RutaExcel': rutaEli }),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (result) {

				},
				error: function (result) {
					console.log(result)
				}
			});
		}

	</script>
</asp:Content>
