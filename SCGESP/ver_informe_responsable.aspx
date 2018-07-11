﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ver_informe_responsable.aspx.cs" Inherits="SCGESP.ver_informe_responsable" %>

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
			vertical-align: middle;
		}

		.valor:first-letter {
			text-transform: uppercase;
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
		}

			table.tblGastos thead {
				background: #337AB7;
				font-size: 12px;
				color: #ffffff;
				font-weight: bold;
				text-transform: uppercase;
			}

				table.tblGastos thead tr th {
					background: #337AB7;
					padding: 5px 5px;
					font-size: 12px;
					font-weight: bold;
				}

			table.tblGastos tbody {
				font-size: 10px;
				text-transform: uppercase;
			}

			table.tblGastos td {
				font-size: 11px;
				font-weight: bold;
				text-transform: uppercase;
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

		#tblGastos tbody tr td {
			text-transform: lowercase;
		}

			#tblGastos tbody tr td:first-letter, #tblGastos tbody tr td a:first-letter {
				text-transform: uppercase;
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
				Requisición / Informe de gastos (*Modo Prueba)
        <a href="#" onclick='cerrarPanel(".panel")' class='btn btn-danger btn-xs'><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<table>
					<tr>
						<td style="width: 130px">
							<a class="btn btn-primary btn-md" href="/Informes" role="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Regresar</a>
						</td>
						<td style="vertical-align: middle; text-align: left; padding-left: 50px;">
							<!--Opciones-->
							<!--a id="aedit" visible="0" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-edit"></span>Editar</!--a>
							<a id="acancela" class="btn btn-danger btn-md" href="#" role="button"><span class="glyphicon glyphicon-floppy-remove"></span>Cancela</a>
							<a id="aguarda" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-floppy-disk"></span>Guardar</a-->
							<a id="aenvia" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-send"></span>&nbsp;Enviar a Validación</a>
							<a id="aexportarxls" class="btn btn-primary btn-md" href="#" role="button" onclick=""><span class="glyphicon glyphicon-export"></span>&nbsp;Excel</a>
							<!--a id="averhorag" class="btn btn-primary btn-md" href="#" role="button" data-placement='top' data-html='true'
								title="<div style='width: 170px;'>Ver la hora en que se realizo el Gasto.</div>" aria-hidden='true'><span class="zmdi zmdi-time"></span>Ver Hora Gasto</a-->
							<a id="aagregarg" class="btn btn-primary btn-md" href="#" role="button"><span class="glyphicon glyphicon-plus"></span>&nbsp;Agregar Gasto</a>
							<a id="aconfrontar" class="btn btn-primary btn-md" href="#" role="button"><i class="zmdi zmdi-swap"></i>&nbsp;Confrontar</a>
							<a id="arefresh" class="btn btn-primary btn-md" href="#" role="button"><i class="zmdi zmdi-refresh"></i>&nbsp;Actualizar</a>
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

							<div id="importesInforme" class="hidden-xs col-md-6 col-lg-4">
								<!--importes-informe-template-->
							</div>
						</div>
						<div class="row" style="padding: 0px 5px;">
							<table id="tblGastos" class="tblGastos display nowrap" cellspacing="0" data-page-length="-1">
								<thead>
									<tr>
										<th style='width: 10px;'></th>
										<th style="width: 50px;">No. De<br />
											Cargo</th>
										<th style='width: 50px;'>Día</th>
										<th style="width: 150px;">Categoria</th>
										<th style="width: 230px;" width="230px">Justificaci&oacute;n</th>
										<th style='width: 120px;'>Monto</th>
										<th style='width: 30px;'>XML</th>
										<th style='width: 30px;'>PDF</th>
										<th style='width: 30px;'>IMG</th>
										<th style='width: 120px;' title="Importe con comprobante">Monto<br />
											Comprobado</th>
										<th style='width: 50px;'>Editar</th>
										<th style='width: 50px;'>Eliminar</th>
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
	</section>

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
	<!-- Modal editar Gasto a Informe -->
	<!--data-modal-color="bluegray"-->
	<div class="modal fade" id="mEditarGastoInf" data-modal-color="gray" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
		<div class="modal-dialog" role="document">
			<div class="modal-content">
				<div class="modal-header titulo-modal">
					<span>Editar Gasto</span>
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal" aria-label="Close"><i class="zmdi zmdi-close"></i>Cerrar</button>
				</div>
				<div class="modal-body">
					<input type="hidden" id="idGasto" value="" />
					<input type="hidden" id="gasto" value="" />
					<div id="inpustGasto" class="form-horizontal" role="form" style="color: black;">
						<div class="form-group" style="margin: 5px 0px">
							<label for="fechagasto" class="col-lg-3 control-label bold">Fecha Gasto:</label>
							<div class="col-lg-9">
								<input type='text' id='fechagasto' name='fechagasto' readonly class='form-control input-mask fechagasto' style="background-color: white;" />
								<i class="form-group__bar"></i>
							</div>
						</div>
						<div class="form-group" style="margin: 5px 0px">
							<label for="categoria" class="col-lg-3 control-label bold">Categor&iacute;a:</label>
							<div class="col-lg-9">
								<select id='categoria' class="form-control" name='categoria' style="width: 100%">
								</select>
								<i class="form-group__bar"></i>
							</div>
						</div>
						<div class="form-group" style="margin: 5px 0px">
							<label for="justificacion" class="col-lg-3 control-label bold">Justificaci&oacute;n:</label>
							<div class="col-lg-9">
								<input type='text' id='justificacion' name='justificacion' class='form-control input-mask' />
								<label id="text_ayuda" style="width: 100%; display: block">
									<small>*Justificar Gasto.</small>
								</label>
								<i class="form-group__bar"></i>
							</div>
						</div>
						<div id="comensales_gasto" class="form-group" style="margin: 5px 0px">
							<label for="comensales" class="col-lg-3 control-label bold">Comensales:</label>
							<div class="col-lg-9">
								<input type='text' id='comensales' name='comensales' class='form-control input-mask' placeholder="Comensal 1, Comensal 2, Comensal N..." />
								<i class="form-group__bar"></i>
							</div>
						</div>
						<div class="form-group" style="margin: 5px 0px">
							<label for="monto" class="col-lg-3 control-label bold">Monto:</label>
							<div class="col-lg-9">
								<input type="number" id='monto' name='monto' class='form-control input-mask' onkeypress="return justNumbers(event)" valueold="" max="0" />
								<i class="form-group__bar"></i>
							</div>
						</div>
						<div class="form-group" style="margin: 5px 0px">
							<label for="monto" class="col-lg-3 control-label bold">Comprobante:</label>
							<div class="col-lg-9">
								<div class="row">
									<div class="col-md-4">
										<label for="monto" class="col-1 control-label bold">XML:</label>
										<input id='filexml' accept='.xml' name='filexml' type='file' />
									</div>
									<div class="col-md-4">
										<label for="monto" class="col-1 control-label bold">PDF:</label>
										<input id='filepdf' accept='.pdf' name='filepdf' type='file' />
									</div>
									<div class="col-md-4">
										<label for="monto" class="col-1 control-label bold">IMG:</label>
										<input id='fileotro' accept='image/*' name='fileotro' type='file' />
									</div>
								</div>
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

		<table id="tblCabeceraInforme" class="filtro text-left" style="text-align: left;" border="1">
			<tr>
				<td style="width: 150px">Requisici&oacute;n:</td>
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
			{{#if i_conciliacionbancos}}
				<tr>
					<td>Confrontación:</td>
					<td>
						<span style='font-size: 11px; padding: 2px 30px;' class='label label-success'><span class='glyphicon glyphicon-ok'></span></span>
					</td>
				</tr>
			{{/if}}
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
		<tr id="{{ idgasto }}" class="{{ classTr }}">
			<td class="text-center" style='width: 10px;'>{{#if btnEditar}}
				{{#if btnAdicional}}
				
					<div class='dropdown'>
						<button type='button' class='btn btn-info' data-toggle='dropdown'><i class='zmdi zmdi-more-vert'></i></button>
						<div class='dropdown-menu' style='padding: 5px;'>
							<a href="#" onclick="gastoAjuste(1, '{{ datosGastoAdi }}')" class='dropdown-item' style='margin: 0px; padding: 10px 0px;'>Agregar otros gastos y/o propina al Gasto</a>

							{{#if xml}}
				<label for='fileXmlAdicional{{ idgasto }}' class='dropdown-item' style='margin: 0px; padding: 10px 0px;'>
					Cargar factura adicional
				<input id='fileXmlAdicional{{ idgasto }}' accept='.xml' onchange="gastoAjuste(2, '{{ datosGastoAdi }}')" name='fileXmlAdicional{{ idgasto }}' type='file' class='hidden' />
				</label>
							{{/if}}

						</div>
					</div>
				{{/if}}
				{{/if}}
			</td>
			<td class="text-left" style="vertical-align: middle; width: 50px;">
				<h5><span class="ngasto label label-success">{{ ngasto }}</span></h5>
			</td>
			<td class="dia" style='width: 50px;'>{{ dia }}</td>
			<td class="categoria" style="width: 150px;">{{ categoria }}
			</td>
			<td class="justificacion" width="230px">
				<p style="width: 230px;" class="valor">{{ justificacion }}</p>
			</td>
			<td class="monto text-right" style='width: 120px;'>{{ monto }}</td>
			<td class="xml text-center" style='width: 30px;'>{{#if xml}}
						<a dirxml='{{ xml }}' data-toggle='tooltip' onclick="verComprobante('{{ xml }}', 'XML', '{{ idgasto }}')" class='btn btn-success btn-sm' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="pdf text-center" style='width: 30px;'>{{#if pdf}}
						<a dirpdf='{{ pdf }}' data-toggle='tooltip' onclick="verComprobante('{{ pdf }}', 'PDF', '{{ idgasto }}')" class='btn btn-success btn-sm verPDF' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="img text-center" style='width: 30px;'>{{#if img}}
						<a dirimg='{{ img }}' data-toggle='tooltip' onclick="verComprobante('{{ img }}', 'OTRO', '{{ idgasto }}')" class='btn btn-success btn-sm verIMG' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="monto_comprobado text-right" style='width: 120px;' title="Importe con comprobante">{{ monto_comprobado }}</td>
			<td class="text-center" style='width: 50px;'>{{#if btnEditar}}
					<a class="btn btn-primary btn-sm" onclick="editar_gasto('{{ gasto }}')" role="button"><span class="glyphicon glyphicon-edit"></span></a>
				{{/if}}
			</td>
			<td class="text-center" style='width: 50px;'>{{#if btnEditar}}
					<a class="btn btn-danger btn-sm" onclick="eliminar_gasto('{{ gasto }}')" role="button"><span class="glyphicon glyphicon-trash"></span></a>
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
			<td></td>
			<td class="text-right">{{ total_gastado }}</td>
			<td></td>
			<td></td>
			<td></td>
			<td class="text-right">{{ monto_comprobado }}</td>
			<td></td>
			<td></td>
		</tr>
	</script>

	<script type="text/javascript" src="js/app.min.js"></script>
	<script type="text/javascript" src="js/js.js"></script>
	<script type="text/javascript" src="js/handlebars-v4.0.11.js"></script>
	<script type="text/javascript">

		var UsuarioActivo = localStorage.getItem("cosa");
		var EmpeladoActivo = localStorage.getItem("cosa2");
		var fechasReq = JSON.parse(localStorage.getItem("fechasReq"));
		var f = new Date();
		var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
		// compile handlebars templates and store them for use later

		let importesInformeTemplate = Handlebars.compile($("#importes-informe-template").html());
		let cabeceraInformeTemplate = Handlebars.compile($("#informe-cabecera-template").html());
		let gastosInformeTemplate = Handlebars.compile($("#gastos-informe-template").html());
		let totalGastosInformeTemplate = Handlebars.compile($("#total-gastos-informe-template").html());
		var IdInforme = 0;
		IdInforme = url.get("item") * 1;
		$(":file").filestyle({
			input: false,
			text: "Seleccionar"
		});
		selectInforme(IdInforme);
		browseGastos(IdInforme);
		function selectInforme(IdInforme) {
			$("#RmRdeRequisicion, #idinforme, #estatus, #ConfBanco, #disAnticipo").val(0);
			$("#formaPagoInforme, #usuResponsable").val("");

			var datos = { "id": IdInforme };

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

					var uresponsable = informe.i_uresponsable;
					var reembolso = informe.reembolso * 1;
					informe.del = valorVacio(informe.del) ? "" : formatFecha(new Date(informe.del), "dd/mm/yyyy");
					informe.al = valorVacio(informe.al) ? "" : formatFecha(new Date(informe.al), "dd/mm/yyyy");
					informe.i_comentarioaut = (informe.i_comentarioaut).toLowerCase();
					informe.p_nmb = (informe.p_nmb).toLowerCase();
					informe.i_uresponsable = $.trim(informe.i_uresponsable);
					informe.i_comentarioaut = (informe.i_comentarioaut).replace("adminerp", "AdminERP")
						.replace("adminweb", "AdminWeb").replace("adminapp", "AdminApp");
					informe.p_nmb = (informe.p_nmb).replace("adminerp", "AdminERP")
						.replace("adminweb", "AdminWeb").replace("adminapp", "AdminApp");
					informe['comentario_rechazo'] = (informe.i_comentarioaut).split(";;");
					if (informe.i_conciliacionbancos > 0)
						informe.confrontacionOK = "OK";
					else
						informe.confrontacionOK = "";

					var requisicion = DatosRequisicion(informe.r_idrequisicion); //DatosRequisicion(informe.r_idrequisicion);

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

					$('#cabeceraInforme').empty();
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
					$("#importesInforme").empty();
					$("#importesInforme").append(importesInformeTemplate(importesInforme));

					habilitaControlesInfo(IdInforme, informe.i_estatus, informe.i_conciliacionbancos);
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function browseGastos(IdInforme) {
			var datos = {
				"idinforme": IdInforme,
				"idproyecto": 0
			};
			var estatus = $("#estatus").val() * 1;
			var ConfBanco = $("#ConfBanco").val() * 1;
			$("#tblGastos tbody").empty();
			var total = 0, totalmonto = 0, totalaceptable = 0, totalnoaceptable = 0, totalnodeducible = 0;
			var listGastos = [];
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
					$("#tblGastos tbody").empty();
					$.each(result, function (key, value) {
						var fg = (value.g_fgasto).split('-');
						var fgasto = formatFecha(fg[2] + '-' + fg[1] + '-' + fg[0], 'dd/mmm');
						var fechagasto = formatFecha(fg[2] + '-' + fg[1] + '-' + fg[0], 'dd-mm-yyyy');
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

						var datosGastoAdi = [];
						var tipoajuste = value.tipoajuste * 1;
						var btnAdicional = false;
						if (tipoajuste === 0) {
							btnAdicional = true;
							if (estatus > 2 || ConfBanco === 1)
								btnAdicional = false;
							datosGastoAdi = {
								'IdInforme': IdInforme, 'IdGasto': value.g_id,
								'FGasto': value.g_fgasto, 'HGasto': value.g_hgasto,
								'Concepto': value.g_concepto, 'Negocio': value.g_negocio,
								'FormaPago': value.g_formapago, 'IdCategoria': value.g_categoria,
								'Categoria': value.g_nombreCategoria, 'IvaCategoria': value.g_ivaCategoria,
								'TGastado': (value.g_total * 1), 'TComprobar': (value.MONTO * 1),
								'ConciliacionBanco': value.g_conciliacionbancos, 'IdMovBanco': value.g_idmovbanco,
								'Observaciones': value.g_observaciones, 'UGasto': value.g_ugasto,
								'ValMaxPropina': (value.g_total * 1), 'DirXML': dir_xml
							};
						}

						var btnEditar = true;
						if (estatus > 2 || ConfBanco === 1)
							btnEditar = false;

						var gasto = {
							idgasto: value.g_id,
							ngasto: value.orden,
							dia: fgasto,
							fechagasto: fechagasto,
							categoria: value.g_nombreCategoria,
							idcategoria: value.g_categoria,
							justificacion: value.g_concepto,
							monto: formatNumber.new((value.g_total).toFixed(2), "$ "),
							nmbcomensales: value.nmbcomensales,
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
							btnEditar: true,
							classTr: colorRow,
							btnAdicional: btnAdicional,
							tipoajuste: tipoajuste
						}
						gasto['gasto'] = JSON.stringify(gasto);
						gasto['datosGastoAdi'] = JSON.stringify(datosGastoAdi);

						total += value.g_total * 1;
						totalmonto += monto;
						totalaceptable += importeaceptable;
						totalnoaceptable += importenoaceptable;
						totalnodeducible += importenodeducible;

						$("#tblGastos tbody").append(gastosInformeTemplate(gasto));
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
									btnAdicional: false,
									tipoajuste: tipoajuste
								};
								$("#tblGastos tbody").append(gastosInformeTemplate(gasto));
							}

						});

						if (tipoajuste === 0) {
							listGastos.push(datosGastoAdi);
						}

						colorRow = colorRow === "rowBlanco" ? "rowGris" : "rowBlanco";
					});

					localStorage.setItem('listGastos', JSON.stringify(listGastos));
				},
				complete: function () {

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
		$(".fechagasto").datepicker({
			dateFormat: "dd-mm-yy",
			dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
			dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
			monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
			monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
			changeMonth: true,
			changeYear: true,
			minDate: fechasReq.fInicio,
			maxDate: fechasReq.fFin,
			onSelect: function (selectedDate) {
				//
			},
			beforeShow: function (input, inst) {
				var calendar = inst.dpDiv;
				setTimeout(function () {
					calendar.css({
						'z-index': 2000
					});
					var hoy = new Date();
					var fInicio1 = (fechasReq.fInicio).split("-");
					var fFin1 = (fechasReq.fFin).split("-");
					var fInicio = new Date(fInicio1[2] + "-" + fInicio1[1] + "-" + fInicio1[0]);
					var fFin = new Date(fFin1[2] + "-" + fFin1[1] + "-" + fFin1[0]);

					hoy.setHours(0, 0, 0, 0);
					fInicio.setHours(0, 0, 0, 0); // Lo iniciamos a 00:00 horas
					fFin.setHours(0, 0, 0, 0); // Lo iniciamos a 00:00 horas

					if (hoy < fInicio || hoy > fFin) {
						if (valorVacio($("#fechagasto").val())) {
							$("#fechagasto").val(fechasReq.fInicio);
						}
					}

				}, 0);

			}
		});
		function DatosRequisicion(RmReqId) {
			var datos = { 'Usuario': UsuarioActivo, 'RmReqId': RmReqId, 'Empleado': EmpeladoActivo };
			var resultado = [];
			localStorage.removeItem('fechasReq');

			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ConsultaRequisicionIDCabecera',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
				},
				success: function (result) {
					resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
					console.log(resultado);
					var fInicio = ((datoEle(resultado.RmReqFechaRequerida)).split("T"))[0];
					var fFin = ((datoEle(resultado.RmReqFechaFinal)).split("T"))[0];

					if (valorVacio(fFin)) {
						fFin = fInicio;
					}
					datos['FInicio'] = fInicio;
					datos['FFin'] = fFin;
					datos['RmReqJustificacion'] = resultado.RmReqJustificacion;
					actualizaInfReq(datos);
					fInicio = formatFecha(fInicio, "dd-mm-yyyy");
					fFin = formatFecha(fFin, "dd-mm-yyyy");
					localStorage.setItem('fechasReq', JSON.stringify({ 'fInicio': fInicio, 'fFin': fFin }));
				},
				error: function (result) {
					console.log("error", result);
				}
			});
			return resultado;
		}
		function habilitaControlesInfo(id, estatus, conciliacionOk) {
			$("#aexportarxls").hide();
			$("#aterminar").hide();
			$("#apago").hide();
			$("#aexportarxls").show();
			$("#aagregarg").hide();

			if (estatus <= 2) {

				$("#aagregarg").show();
				$("#aenvia").show();
				$("#aedit").show();
			} else if (estatus === 3 || estatus === 7 || estatus === 5 || estatus === 8) {//3 = por autorizar, 7 = autorizado parcial, 5 = autorizado
				$("#aenvia").hide();
				$("#aedit").hide();
				$("#aguarda").hide();
				$("#acancela").hide();

				$("#auautoriza").show();

				if (estatus === 5)
					$("#aterminar").show();
				else
					$("#aterminar").hide();
			}
			if (estatus === 2 && conciliacionOk === 1) {
				$("#aenvia").show();

				if (estatus === 2) {
					$("#aconfrontar, #cancelarConfrontacion").show();
					$("#confrontarInforme").hide();
				} else {
					$("#aconfrontar").hide();
				}
			}
			else if (estatus === 2 && conciliacionOk === 0) {
				$("#aenvia, #cancelarConfrontacion, #confrontarInforme").hide();
				$("#aconfrontar").show();
			}

			if (conciliacionOk === 1) {
				$("#aagregarg").hide();
			}

			if (conciliacionOk === 0) {
				$("#aenvia").hide();
			}
		}
		function verComprobante(dircomp, comprobante, idgasto) {

			var IdInforme = $("#idinforme").val() * 1;
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
			var ConfBanco = $("#ConfBanco").val() * 1;
			var btnEliCom = "";
			if (ConfBanco === 0) {
				btnEliCom = "<button type='button' class='btn btn-danger btn-xs' onclick='eliminarGastoCom(\"" + comprobante + "\", " + IdInforme + ", " + idgasto + ")'><i class='zmdi zmdi-delete zmdi-hc-2x'></i> Eliminar</button>";
			};

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
						controles = "<button type='button' class='btn btn-warning btn-xs' onclick='imprimirXML()'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
						controles += btnEliCom;
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
				"#controles_compPDF".AsHTML(btnEliCom);
				$("#compPDF").show();
				$("#compPDF").attr("data", "/" + dircomp);
			}
			if (comprobante === "OTRO") {
				$("#verComprobante").modal('show');
				dircomp = "/" + dircomp;
				var controles = "";
				controles += "<button type='button' class='btn btn-warning btn-xs' dir='" + dircomp + "' onclick='imprimirImg()'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
				controles += btnEliCom;
				var f = new Date();
				var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
				$("#controles_compOTRO").append(controles);
				$("#compOTRO").show();
				$("#compOTRO").attr("src", dircomp + "?" + fh);
			}

		}
		function eliminarGastoCom(comprobante, idinforme, idgasto) {
			var datos = {
				'id': idgasto,
				'idinforme': idinforme,
				'comprobante': comprobante
			};

			var botones = [];

			botones[0] = {
				text: "Si", click: function () {
					cargando();
					$.ajax({
						async: false,
						type: "POST",
						url: "/api/EliminaComprobantes",
						data: datos,
						dataType: "json",
						beforeSend: function () {
							//
						},
						success: function (result) {
							//console.log(result);

							if (comprobante.toLowerCase === "xml")
								selectInforme(idinforme);

							browseGastos(idinforme);

							$.notify("Comprobante de Gasto Borrado.", { globalPosition: 'top center', className: 'success' });


						},
						error: function (result) {
							$.notify("Error al Eliminar", { globalPosition: 'top center', className: 'error' });
						}
					});
					cargado();
					$(this).dialog("close");
					$("#verComprobante").modal('hide');
				}
			};
			botones[1] = {
				text: "No", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Eliminar Comprobante?", botones, "Eliminar Comprobante.", "#verComprobante");
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
		$("#aagregarg").click(function () {
			var gasto = {
				idgasto: 0,
				ngasto: 0,
				dia: 0,
				fechagasto: "",
				categoria: "",
				idcategoria: 0,
				justificacion: "",
				monto: 0,
				nmbcomensales: "",
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
				btnEditar: true,
				classTr: "",
				btnAdicional: false,
				tipoajuste: 0
			}
			gasto['gasto'] = "";
			gasto['datosGastoAdi'] = "";
			$("#gasto").val(JSON.stringify(gasto));
			$(":file").filestyle('clear');
			menucategorias();
			var disponible = $("#disAnticipo").val() * 1;
			$("#mEditarGastoInf").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			"#mEditarGastoInf .modal-header.titulo-modal span".AsHTML("Agregar Gasto");
			
			$("#comensales_gasto").hide();
			if ((gasto.categoria.toLowerCase()).indexOf("alimenta") > -1 || (gasto.categoria.toLowerCase()).indexOf("sesion") > -1) {
				$("#comensales_gasto").show();
			} else {
				gasto.nmbcomensales = "";
			}

			textAyudaJustificacion("", "");

			$("#fechagasto").val("");
			$("#idGasto").val("");
			$("#categoria").val("");
			$("#justificacion").val("");
			$("#comensales").val("");
			$("#monto").val("");
			$("#monto").attr("max", (disponible));
		});
		function editar_gasto(datos_gasto) {
			$(":file").filestyle('clear');
			menucategorias();
			var disponible = $("#disAnticipo").val() * 1;
			$("#mEditarGastoInf").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			"#mEditarGastoInf .modal-header.titulo-modal span".AsHTML("Editar Gasto");
			var gasto = JSON.parse(datos_gasto);

			$("#comensales_gasto").hide();
			if ((gasto.categoria.toLowerCase()).indexOf("alimenta") > -1 || (gasto.categoria.toLowerCase()).indexOf("sesion") > -1) {
				$("#comensales_gasto").show();
			} else {
				gasto.nmbcomensales = "";
			}

			textAyudaJustificacion(gasto.categoria, gasto.justificacion);

			$("#fechagasto").val(gasto.fechagasto);
			$("#idGasto").val(gasto.idgasto);
			$("#gasto").val(JSON.stringify(gasto));
			$("#categoria").val(gasto.idcategoria);
			$("#justificacion").val(gasto.justificacion);
			$("#comensales").val(gasto.nmbcomensales);
			$("#monto").val(gasto.num_monto);
			$("#monto").attr("max", (gasto.num_monto + disponible));
			$("#monto").attr("valueOld", gasto.num_monto);

		}
		function eliminar_gasto(datos_gasto) {
			var gasto = JSON.parse(datos_gasto);
			var ConfBanco = $("#ConfBanco").val() * 1;
			var IdInforme = $("#idinforme").val() * 1;
			if (ConfBanco === 1) {
				$.notify("No puede eliminar el gasto el informe ya esta confrontado", { globalPosition: 'top center', className: 'error' });
				return false;
			}

			var datos = {
				"id": gasto.idgasto,
				"idinforme": IdInforme
			};
			var nombre = gasto.justificacion;
			var botones = [];
			botones[0] = {
				text: "Si", click: function () {
					$(this).dialog("close");
					$.ajax({
						async: true,
						type: "POST",
						url: "/api/DeleteGasto",
						data: JSON.stringify(datos), //checar con hector{'dirxml': dirxml, 'dirpdf': dirpdf, 'dirotros': dirotros},
						contentType: 'application/json; charset=utf-8',
						dataType: 'json',
						success: function (result) {
							$.notify("Gasto [" + nombre + "] Borrado.", { globalPosition: 'top center', className: 'success' });
							selectInforme(IdInforme);
							browseGastos(IdInforme);
						},
						error: function (result) {
							$.notify("Error al Eliminar", { globalPosition: 'top center', className: 'error' });
						}
					});
				}
			};
			botones[1] = {
				text: "No", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Eliminar Gasto:<br /><b>" + nombre + "</b>?", botones, "Eliminar Gasto.");

		}
		$("#btnGuardaGasto").click(function () {
			var gasto = JSON.parse($("#gasto").val());
			var tipoajuste = gasto.tipoajuste * 1;
			var ugasto = localStorage.getItem("cosa");
			var IdInforme = $("#idinforme").val() * 1;
			var IdGasto = gasto.idgasto * 1; // $("#idGasto").val() * 1;
			var error = 0;
			var total = $("#monto").val() * 1;
			var formaPago = $("#formaPagoInforme").val();
			var fechagasto = $("#fechagasto").val();
			var categoria = $("#categoria").val() * 1;
			var CategoriaSelect = document.getElementById("categoria");
			var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
			var datosCat = [];
			var ivacategoria = 0;
			try {
				datosCat = elementoCat.options[opIndex].dataset;
				ivacategoria = datosCat.grmativa;
			} catch (err) {
				datosCat = [];
				ivacategoria = 0;
			}
			var justificacion = $.trim($("#justificacion").val());
			var hrg = horaActual("hh:mm");
			var comensales = $.trim($("#comensales").val());
			var comensalesObligatorios = false;
			var ncomensales = 0;

			if (((NombreCategoria.toLowerCase()).indexOf("alimenta") > -1 || (NombreCategoria.toLowerCase()).indexOf("sesion") > -1) && tipoajuste === 0) {
				comensalesObligatorios = true;
			}
			if (!validarNumero($("#monto").val())) {//($.trim($("#filexml" + id).val()) != "" || xml != "") &&
				$.notify("Ingresa un Importe valido.", { position: "top center" }, "error");
				error = 1;
			}
			var totalmaximo = parseFloat($("#monto").attr('max'));
			if (total > totalmaximo) {
				$.notify("El importe *Capturado* (gastado) no puede ser mayor al importe *Por Comprobar* (disponible).", { position: "top center" }, "error");
				var valorOld = parseFloat($("#monto").attr('valueOld'));
				$("#monto").val(valorOld);
				$("#monto").focus().select();
				error = 1;
			}
			if (categoria === 0) {//($.trim($("#filexml" + id).val()) != "" || xml != "") &&
				$.notify("Selecciona una categoria.", { position: "top center" }, "error");
				error = 1;
			}
			if (justificacion === "") {
				$.notify("Indica justificación del gasto.", { position: "top center" }, "error");
				error = 1;
			}
			if (total === "" && total <= 0) {
				$.notify("Indica Importe gastado.", { position: "top center" }, "error");
				error = 1;
			}
			if (comensales === "" && comensalesObligatorios === true) {
				$.notify("Los comensales son obligatorios.", { position: "top center" }, "error");
				error = 1;
			}
			if (comensales !== "" && comensalesObligatorios === true) {
				try {
					ncomensales = (comensales.split(",")).length;
				} catch (err) {
					ncomensales = 1;
				}
			}
			if (comensalesObligatorios === false) {
				comensales = "";
				ncomensales = 0;
			}
			if (error === 1) {
				return false;
			}

			var datos = {
				"id": IdGasto,
				"idinforme": IdInforme,
				"idproyecto": 0,
				"fgasto": fechagasto,
				"hgasto": hrg,
				"concepto": justificacion,
				"negocio": justificacion,
				"total": total,
				"formapago": formaPago,
				"categoria": categoria,
				"observaciones": justificacion,
				"nombreCategoria": NombreCategoria,
				"ivaCategoria": ivacategoria,
				"ncomensales": ncomensales,
				"nmbcomensales": comensales,
				"ugasto": ugasto,
				"rfc": "-",
				"contacto": "-",
				"telefono": "-",
				"correo": "-",
				"fileotros": ""
			};
			
			var apiEjecutar = IdGasto > 0 ? "UpdateGasto" : "InsertGasto";
			console.log(datos);
			$.ajax({
				async: false,
				type: "POST",
				url: ("/api/" + apiEjecutar),
				data: (datos), //data,
				dataType: "json",
				cache: false,
				beforeSend: function () {
					//cargando();
				},
				success: function (result) {
					console.log("gasto guardado->",result);
					if (IdGasto > 0) {
						$.notify("Gasto Actualizado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
					} else {
						$.notify("Gasto Guardado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
					}

					if (IdGasto === 0 && apiEjecutar === "InsertGasto") {
						try {
							IdGasto = result[0]['IdGasto'] * 1;
						} catch (err) {
							IdGasto = 0;
						}
					}
					if (IdGasto > 0) {
						actualizarGastoComXML(IdGasto, IdInforme);
						actualizarGastoComPDF(IdGasto, IdInforme);
						actualizarGastoComOTRO(IdGasto, IdInforme);
					}

					$("#mEditarGastoInf").modal('hide');

					

				},
				complete: function () {
					//cargado();
					selectInforme(IdInforme);
					browseGastos(IdInforme);
				},
				error: function (result) {
					//cargado();
					$.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
				}
			});

		});
		$("#aenvia").click(function () {
			var ConfBanco = $("#ConfBanco").val() * 1;
			if (ConfBanco === 0) {
				Seguridad.alerta("No puedes enviar a autorización.<br />Informe NO Confrontado.");
				return false;
			}
			/*var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
			if (vComensalesObjetivo.estatus === false) {
				Seguridad.alerta("No puedes enviar a autorización el informe.<br />" + vComensalesObjetivo.mensaje);
				return false;
			}*/
			var valida = validaExistenComprobantes();
			var req = DatosRequisicion();
			//console.log(req);
			//RmReqImporteComprobar, RmReqEstatus
			var totalg = $("#totalg").val() * 1;//monto a comprobar informe
			var RmReqImporteComprobar = req.RmReqImporteComprobar * 1;//monto a comprobar requisicion
			var estatus = req.RmReqEstatus * 1;
			var estatusObligatorioReq = "Fondo Retirado";
			var estatusActualReq = req.RmReqEstatusNombre;
			if (valida.NoGastosConComprobante != valida.NoGastos) {
				Seguridad.alerta("Todos los gastos deben contener almenos un comprobante.");
			} else {
				if (estatus === 52) {
					if (valida.NoGastosNoDeducible > 0) {
						confGastosNoDeducibles(totalg, RmReqImporteComprobar);
					} else if (totalg.toFixed(2) != RmReqImporteComprobar.toFixed(2)) {
						//ValidacionMensajeMontos();
						Seguridad.alerta("No puedes enviar a autorización el Informe.<br />" +
							"Existen diferencias entre el importe gastado (" + formatNumber.new((totalg * 1).toFixed(2), "$ ") + ") " +
							"y el importe comprobado (" + formatNumber.new((RmReqImporteComprobar * 1).toFixed(2), "$ ") + ") en la requisicion.");
					}
					else {
						enviarAAutorizacion();
					}
				} else {
					Seguridad.alerta("No puedes enviar a autorización el Informe.<br />Tu requisición necesita estar en estatus <b>'" +
						estatusObligatorioReq + "'</b><br />" +
						"Estatus Actual de la requisición <b>'" + estatusActualReq + "'</b>.");
				}
			}
		});
		function confGastosNoDeducibles(totalg, RmReqImporteComprobar) {
			var botones = [];
			botones[0] = {
				text: "Aceptar", click: function () {
					$(this).dialog("close");
					if (totalg.toFixed(2) != RmReqImporteComprobar.toFixed(2)) {
						//ValidacionMensajeMontos();

						Seguridad.alerta("No puedes enviar a autorización el Informe.<br />" +
							"Existen diferencias entre el importe gastado (" + formatNumber.new((totalg * 1).toFixed(2), "$ ") + ") " +
							"y el importe comprobado (" + formatNumber.new((RmReqImporteComprobar * 1).toFixed(2), "$ ") + ") en la requisicion.");
					}
					else {
						enviarAAutorizacion();
					}
				}
			};
			botones[1] = {
				text: "Cancelar", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Existen Gastos sin comprobante Fiscal,<br />estos gastos se clasificaran como no deducibles.", botones, "Enviar a Autorización.");
		}
		function enviarAAutorizacion() {
			var idinforme = $("#idinforme").val();

			var datos =
				{
					"idinforme": idinforme
				};

			var botones = [];
			botones[0] = {
				text: "Aceptar", click: function () {
					$.ajax({
						async: true,
						type: "POST",
						url: "/api/EnviaAutorizacion",
						data: JSON.stringify(datos),
						contentType: 'application/json; charset=utf-8',
						dataType: "json",
						beforeSend: function () {

							cargando();
						},
						success: function (result) {
							$.notify("Informe enviado correctamente.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
							setTimeout(function () {
								window.location.href = "/Informes?" + fh;
							}, 1000);

						},
						complete: function () {

							cargado();
						},
						error: function (result) {
							cargado();
							console.log(result);
						}
					});
					$(this).dialog("close");
				}
			};
			botones[1] = {
				text: "Cancelar", click: function () {
					$(this).dialog("close");
				}
			};

			var botones1 = [];

			botones1[0] = {
				text: "Si", click: function () {
					$(this).dialog("close");
					Seguridad.confirmar("Enviar a Autorización?<br />Una vez enviado ya no podras hacer ningun cambio.", botones, "Enviar a Autorización.");
				}
			};
			botones1[1] = {
				text: "No", click: function () {
					$(this).dialog("close");
				}
			};

			var totalg = $("#totalg").val() * 1;
			var montog = $("#montog").val() * 1;

			Seguridad.confirmar("Enviar a Autorización?<br />Una vez enviado ya no podras hacer ningun cambio.", botones, "Enviar a Autorización.");
		}
		function validaExistenComprobantes() {
			var resultado = [];

			var idinforme = $("#idinforme").val();

			var datos =
				{
					"idinforme": idinforme
				};
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/ValidaExistenComprobantes",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: "json",
				success: function (result) {
					resultado = result;
				},
				error: function (result) {
					console.log(result);
				}
			});

			return resultado;

		}
		/*
		funciones adicional por gasto - propina - facturas adicionales
		*/
		function gastoAjuste(tipo, datos_gasto) {
			var datos = JSON.parse(datos_gasto);
			console.log("adicional", datos);
			var ucrea = localStorage.getItem("cosa");
			var ImportePropina = 0;
			datos['Tipo'] = tipo;
			datos['UCrea'] = ucrea;
			datos['BinXML'] = "";
			datos['NombreArc'] = "";
			datos['ExtFile'] = "";
			if (tipo === 1) {
				var botones = [];
				botones[0] = {
					text: "Si", click: function () {
						var error = 0;
						ImportePropina = $("#importePropina").val() * 1;
						var importePropinaMax = $("#importePropina").attr("max") * 1;
						var afecta = ($("#ImpGastadoPro").is(':checked') || $("#ImpComprobadoPro").is(':checked')) ? true : false;
						if (ImportePropina <= 0) {
							error = 1;
							$.notify("El importe debe ser mayor a cero (0).", { position: "top center", className: "error" });
						}
						if (ImportePropina > importePropinaMax) {
							error = 1;
							$.notify("El importe no puede ser mayor a lo gastado.", { position: "top center", className: "error" });
						}
						if (afecta === false) {
							error = 1;
							$.notify("El importe debe afectar al importe gastado y/o comprobado.", { position: "top center", className: "error" });
						}
						if (error === 0) {
							$(this).dialog("close");
							datos['Observaciones'] = "Gasto: " + datos['Concepto'];
							datos['Concepto'] = "Otro Gasto / Propina";
							if ($("#ImpGastadoPro").is(':checked')) {
								datos['TGastado'] = ImportePropina;
								datos['AfectaImpGastado'] = 1;
							}
							else {
								datos['TGastado'] = 0;
								datos['AfectaImpGastado'] = 0;
							}

							if ($("#ImpComprobadoPro").is(':checked')) {
								datos['TComprobar'] = ImportePropina;
								datos['AfectaImpComprobado'] = 1;
							}
							else {
								datos['TComprobar'] = 0;
								datos['AfectaImpComprobado'] = 0;
							}

							agregaAjusteGasato(datos);
						} else {
							return false;
						}
					}
				};
				botones[1] = {
					text: "No", click: function () {
						$(this).dialog("close");
					}
				};
				var gasto = datos.Concepto + " / " + formatNumber.new((datos.TGastado * 1).toFixed(2), "$ ");
				var txtImporte = "<div class='input-group'>";
				txtImporte += "<span class='input-group-addon' style='width: 140px;'>Por un importe de $</span>";
				txtImporte += "<div class='form-group'>";
				txtImporte += "<input type='number' id='importePropina' name='importePropina' placeholder='0.00' min='0' max='" + datos.TGastado + "' class='form-control'>";
				txtImporte += "<i class='form-group__bar'></i> ";
				txtImporte += "<small>(Politica de Propina 10%)</small>";
				txtImporte += "</div>";
				txtImporte += "</div>";

				var chkAfectaa = "<div id='chkAfectaImpPro' class='hidden'>";
				chkAfectaa += "<b>Afecta al importe:</b><br /><label class='custom-control custom-checkbox'>";
				chkAfectaa += "<input type='checkbox' checked id='ImpGastadoPro' class='custom-control-input'>";
				chkAfectaa += "<span class='custom-control-indicator'></span>";
				chkAfectaa += "<span class='custom-control-description'>Gastado</span>";
				chkAfectaa += "</label>";
				chkAfectaa += "<label class='custom-control custom-checkbox'>";
				chkAfectaa += "<input type='checkbox' checked id='ImpComprobadoPro' class='custom-control-input'>";
				chkAfectaa += "<span class='custom-control-indicator'></span>";
				chkAfectaa += "<span class='custom-control-description'>Comprobado</span>";
				chkAfectaa += "</label>";
				chkAfectaa += "</div>";

				Seguridad.confirmar("Agregar otro gasto / propina al gasto: <b>" + gasto + "</b>" + txtImporte + chkAfectaa, botones, "Agregar Gasto Adicional.");
			} else if (tipo === 2) {

				var file = $("#fileXmlAdicional" + datos.IdGasto).get(0).files[0];
				var r = new FileReader();
				var nombre = file.name;
				var extFile = (nombre.substring(nombre.lastIndexOf(".") + 1)).toLowerCase();
				if (extFile === "xml") {

					var botones = [];
					botones[0] = {
						text: "Si", click: function () {
							$(this).dialog("close");

							r.onload = function () {
								var binXML = r.result;
								var afecta = ($("#ImpGastadoFac").is(':checked') || $("#ImpComprobadoFac").is(':checked')) ? true : false;
								datos['Observaciones'] = "Gasto: " + datos['justificacion'];
								datos['Concepto'] = "Factura Adicional";

								if ($("#ImpGastadoFac").is(':checked')) {
									datos['AfectaImpGastado'] = 1;
								}
								else {
									datos['AfectaImpGastado'] = 0;
								}

								if ($("#ImpComprobadoFac").is(':checked')) {
									datos['AfectaImpComprobado'] = 1;
								}
								else {
									datos['AfectaImpComprobado'] = 0;
								}

								datos['BinXML'] = binXML;
								datos['NombreArc'] = nombre;
								datos['ExtFile'] = extFile;
								nombre = nombre.replace("." + extFile, "");
								agregaAjusteGasato(datos);
								$("#fileXmlAdicional" + datos.idgasto).clearInputs();
							};
							r.readAsDataURL(file);

						}
					};
					botones[1] = {
						text: "No", click: function () {
							$(this).dialog("close");
							$("#fileXmlAdicional" + datos.IdGasto).clearInputs();
						}
					};
					var gasto = datos.Concepto + " / " + formatNumber.new((datos.TGastado * 1).toFixed(2), "$ ");

					var chkAfectaa = "<div id='chkAfectaImpFac' class='hidden'>";
					chkAfectaa += "<b>Afecta al importe:</b><br /><label class='custom-control custom-checkbox'>";
					chkAfectaa += "<input type='checkbox' checked id='ImpGastadoFac' class='custom-control-input'>";
					chkAfectaa += "<span class='custom-control-indicator'></span>";
					chkAfectaa += "<span class='custom-control-description'>Gastado</span>";
					chkAfectaa += "</label>";
					chkAfectaa += "<label class='custom-control custom-checkbox'>";
					chkAfectaa += "<input type='checkbox' checked id='ImpComprobadoFac' class='custom-control-input'>";
					chkAfectaa += "<span class='custom-control-indicator'></span>";
					chkAfectaa += "<span class='custom-control-description'>Comprobado</span>";
					chkAfectaa += "</label>";
					chkAfectaa += "</div>";

					Seguridad.confirmar("Agregar factura adcional:<br /><b>" + nombre + "</b><br />al gasto: <b>" + gasto + "</b>" + chkAfectaa, botones, "Agregar Factura.");

				} else {
					$.notify("Archivo invalido, prueba con otro.", { globalPosition: 'top center', className: 'error', autoHideDelay: 4000 });
					$("#fileXmlAdicional" + datos.IdGasto).clearInputs();
				}
			}
		}
		function agregaAjusteGasato(datos) {
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/AgregarAjusteGasto",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (result) {
					if (result.AgregadoOk === true) {
						$.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
						selectInforme(IdInforme);
						browseGastos(IdInforme);
					} else {
						console.log(result);
						$.notify(result.Descripcion, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });
					}
				},
				complete: function () {
				},
				error: function (result) {
					console.log(result);
					$.notify("Error al agregar.", { globalPosition: 'top center', className: 'error' });
				}
			});
		}

		function recuperagastoxml(id, idinforme, bitxml) {

			var datos = {
				"id": id,
				"idinforme": idinforme,
				"idproyecto": 0,
				"dir": bitxml
			};

			$.ajax({
				async: false,
				type: "POST",
				url: "/api/UpdateGastoXML",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				processData: false,
				success: function (result) {
					var error = 0;

					if (result === 'El UUID ingresado ya existe') {
						error = 1;
						$.notify('El Xml cargado ya existe en la base de datos,favor de verificar', { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });
					};

					if (result === 'No se puede guardar el comprobante, el importe es igual o mayor a $ 2000.00 y la forma de pago es efectivo.') {
						$.notify(result, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });

						error = 1;
					};

					if (result === 'Gasto Actualizado,la forma de pago capturada no coincide con la del XML') {
						$.notify(result, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });

						error = 1;
					};


					$("#filexml").filestyle('clear');

					if (error === 0) {
						$.notify("Comprobante XML cargado y gasto Actualizado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
					}

				},
				complete: function (){
					selectInforme(idinforme);
					browseGastos(idinforme);
				},
				error: function (result) {
					console.log("error", result);
					$("#filexml").filestyle('clear');
					$.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
				}
			});

		}
		function actualizarGastoComXML(id, idinforme) {

			if (valorVacio($("#filexml").val()))
				return false

			var inputFileXML = document.getElementById("filexml");
			var fileXML = inputFileXML.files[0];

			var formapagogasto = $("#formaPagoInforme").val();

			var error = 0;

			var extFileXML = (($("#filexml").val()).substring(($("#filexml").val()).lastIndexOf(".") + 1)).toLowerCase();
			if ($.trim($("#filexml" + id).val()) !== "") {
				if (extFileXML != "xml") {
					$.notify("Solo se permiten archivos XML.", { position: "top center" }, "error");
					$.filestyle('clear');
					error = 1;
				}
			}
			var c = $("#categoria").val();
			var categoria = c * 1;

			if (categoria === 0) {
				$.notify("Seleciona una categoria.", { position: "top center" }, "error");
				$("#filexml").filestyle('clear');
				error = 1;
			}

			if (error === 1) {
				return false;
			}

			var bitxml;

			var file = $("#filexml").get(0).files[0];
			var r = new FileReader();
			r.onload = function () {
				bitxml = r.result;
				recuperagastoxml(id, idinforme, bitxml)
			};
			r.readAsDataURL(file);

		}
		function RecuperaDatos(id, idinforme, binimagePDF) {
			var datos = {
				"id": id,
				"idinforme": idinforme,
				"dir": binimagePDF,
				"Valida": "1"
			};

			$.ajax({
				async: false,
				type: "POST",
				url: "/api/UpdateGastoPDFOtros",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				success: function (result) {
					$.notify("Gasto Actualizado [Comprobante PDF Cargado].", { globalPosition: 'top center', className: 'success' });
				},
				complete: function () {
					selectInforme(idinforme);
					browseGastos(idinforme);
				},
				error: function (result) {
					console.log(result);
					$.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });

				}
			});

		}
		function actualizarGastoComPDF(id, idinforme) {

			if (valorVacio($("#filepdf").val()))
				return false

			var inputFilePDF = document.getElementById("filepdf");
			var filePDF = inputFilePDF.files[0];

			var sizeByte = inputFilePDF.files[0].size;
			var siezekiloByte = parseInt(sizeByte / 1024);

			var error = 0;

			var extFilePDF = (($("#filepdf").val()).substring(($("#filepdf").val()).lastIndexOf(".") + 1)).toLowerCase();
			if ($.trim($("#filepdf" + id).val()) !== "") {
				if (extFilePDF !== "pdf") {
					$("#filepdf").notify("Solo se permiten archivos PDF.", { position: "top center" }, "error");
					$("#filepdf").filestyle('clear');
					error = 1;
				}
			}

			if (siezekiloByte > 2000) {
				$("#filepdf").notify("No se permiten archivos mayores a 2MB.", { position: "top center" }, "error");
				$("#filepdf").filestyle('clear');
				error = 1;
			}


			if (error === 1) {
				return false;
			}

			var binimagePDF;

			var file = $('#filepdf').get(0).files[0];
			var r = new FileReader();
			r.onload = function () {
				binimagePDF = r.result;
				RecuperaDatos(id, idinforme, binimagePDF)

			};
			r.readAsDataURL(file);


		}
		function recuperadatosotros(id, idinforme, bitImg) {

			var datos = {
				"id": id,
				"idinforme": idinforme,
				"dir": bitImg,
				"Valida": "0"
			};

			$.ajax({
				async: false,
				type: "POST",
				url: "/api/UpdateGastoPDFOtros",
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				success: function () {
					$.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
				},
				complete: function () {
					selectInforme(idinforme);
					browseGastos(idinforme);
				},
				error: function (result) {
					console.log(result);
					$.notify("Error al Guardar Comprobante.", { globalPosition: 'top center', className: 'error' });
					//Seguridad.bitacora("", 5, idproyecto + "," + idinforme + "," + id, "Error: Al cargar comprobante para el gasto (imagen). " + JSON.stringify(result), 0);
				}
			});

		}
		function actualizarGastoComOTRO(id, idinforme) {

			if (valorVacio($("#fileotro").val()))
				return false

			var inputFileOTRO = document.getElementById("fileotro");
			var fileOTRO = inputFileOTRO.files[0];

			var sizeByte = inputFileOTRO.files[0].size;
			var siezekiloByte = parseInt(sizeByte / 1024);

			var error = 0;

			var extFileOTRO = (($("#fileotro").val()).substring(($("#fileotro").val()).lastIndexOf(".") + 1)).toLowerCase();
			if ($.trim($("#fileotro").val()) !== "") {
				var extensiones_permitidas = new Array("gif", "jpg", "png", "jpeg");
				var permitida = false;
				for (var i = 0; i < extensiones_permitidas.length; i++) {
					if (extensiones_permitidas[i] === extFileOTRO) {
						permitida = true;
						break;
					}
				}
				if (permitida === false) {
					$.notify("Solo se permiten archivos de tipo Imagen.", { position: "top center" }, "error");
					$("#fileotro").filestyle('clear');
					error = 1;
				}
			}

			if (siezekiloByte > 2000) {
				$.notify("No se permiten archivos mayores a 2MB.", { position: "top center" }, "error");
				$("#fileotro").filestyle('clear');
				error = 1;
			}

			if (error === 1) {
				return false;
			}

			var bitImg;

			var file = $('#fileotro').get(0).files[0];
			var r = new FileReader();
			r.onload = function () {
				bitImg = r.result;
				recuperadatosotros(id, idinforme, bitImg);
			};
			r.readAsDataURL(file);
		}

		$("#categoria").change(function () {
			var CategoriaSelect = document.getElementById("categoria");
			var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
			$("#comensales_gasto").hide();
			if ((NombreCategoria.toLowerCase()).indexOf("alimenta") > -1 || (NombreCategoria.toLowerCase()).indexOf("sesion") > -1) {
				$("#comensales_gasto").show();
			}
			var justificacion = $.trim($("#justificacion").val());
			textAyudaJustificacion(NombreCategoria, justificacion);
		});
		$("#justificacion").change(function () {
			var CategoriaSelect = document.getElementById("categoria");
			var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
			var justificacion = $.trim($(this).val());
			textAyudaJustificacion(NombreCategoria, justificacion);
		});
		function textAyudaJustificacion(categoria, justificacion) {
			$("#text_ayuda").hide();
			var ayuda = "Justificar Gasto.";
			if ((categoria.toLowerCase()).indexOf("hospeda") > -1) {
				ayuda = "Noches de Hospedaje.";
			} else if ((categoria.toLowerCase()).indexOf("autobus") > -1 ||
				(categoria.toLowerCase()).indexOf("autobús") > -1 ||
				(categoria.toLowerCase()).indexOf("autob") > -1) {
				ayuda = "Ida o Vuelta.";
			} else if ((categoria.toLowerCase()).indexOf("caseta") > -1) {
				ayuda = "¿Cuál?";
			} else if ((categoria.toLowerCase()).indexOf("uber") > -1) {
				ayuda = "Origen y Destino.";
			} else if ((categoria.toLowerCase()).indexOf("estacionamiento") > -1) {
				ayuda = "Motivo.";
			} else if ((categoria.toLowerCase()).indexOf("alimenta") > -1) {
				ayuda = "¿Desayuno, Comida o Cena?";
			} else if ((categoria.toLowerCase()).indexOf("sesion") > -1) {
				ayuda = "Favor de indicar el negocio captado, renovado o el motivo de su gasto de representación, Ademas de el nombre de los comensales.";
			}
			"#text_ayuda small".AsHTML("*" + ayuda);
			$("#justificacion").attr("placeholder", ayuda);
			if (!valorVacio(justificacion)) {
				$("#text_ayuda").show();
			}

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
		$("#arefresh").click(function () {
			location.reload();
		});
		//inicio excel
		$("#aexportarxls").click(function () {
			var idinforme = $("#idinforme").val() * 1;
			cargando();
			if (idinforme > 0) {
				var datos = {
					'IdInforme': idinforme
				};

				var informe = selectInformeExcel(datos.IdInforme);
				if (informe.ok === true) {
					datos['NoInforme'] = informe.datos.i_ninforme;
					datos['NmbSolicitante'] = informe.datos.responsable;
					var requisicion = SelectRequisicionExcel(informe.datos.r_idrequisicion);
					if (requisicion.ok === true) {
						datos['TipoReq'] = datoEle(requisicion.datos.RmReqTipoRequisicionNombre);
						datos['Departamento'] = "";
						datos['Puesto'] = "";
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
		function actualizaInfReq(datos) {
			datos['IdInforme'] = url.get("item") * 1;
			console.log(datos);
			$.ajax({
				async: true,
				type: "POST",
				url: '/api/ActualizaInfReq',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
				},
				success: function (result) {
					//result
				},
				error: function (result) {
					console.log("error", result);
				}
			});


		}

		//confrontación
		$("#aconfrontar").click(function () {
			//if ($("#tabsConfrontar").tabs()) {
			//  $("#tabsConfrontar").tabs("destroy").tabs();
			//}
			var IdInforme = $("#idinforme").val() * 1;
			var valida = validaExistenComprobantes();

			if (valida.NoGastosConComprobante != valida.NoGastos) {
				Seguridad.alerta("Para Confrontar, todos los gastos deben contener almenos un comprobante.");
				return false;
			}
			/*var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
			if (vComensalesObjetivo.estatus === false) {
				Seguridad.alerta("No puedes confrontar el informe.<br />" + vComensalesObjetivo.mensaje);
				return false;
			}*/

			browseGastos(IdInforme);
			$("#filebanco:file").filestyle({
				input: false,
				buttonName: "btn-success",
				buttonText: "&nbsp; Cargar Movimientos*"
			});
			var datos = JSON.parse(localStorage.getItem("listGastos"));
			var importeMin = 99999;
			var importeMax = 0;

			var FechaMin = "";
			var FechaMax = "";
			var tarjeta = "";
			$.each(datos, function (key, value) {
				importeMax = value.TGastado > importeMax ? value.TGastado : importeMax;
				importeMin = value.TGastado < importeMin ? value.TGastado : importeMin;

				tarjeta = $.trim($("#formaPagoInforme").val());// value.FormaPago;

				var f1 = (value.FGasto).split("-");
				var fgasto = new Date(f1[2] + "-" + f1[1] + "-" + f1[0]);
				if (FechaMax === "" && FechaMin === "") {
					FechaMax = f1[2] + "-" + f1[1] + "-" + f1[0];
					FechaMin = f1[2] + "-" + f1[1] + "-" + f1[0];
				} else {
					var fMin = new Date(FechaMin);
					var fMax = new Date(FechaMax);
					FechaMax = fgasto.getTime() > fMax.getTime() ? f1[2] + "-" + f1[1] + "-" + f1[0] : FechaMax;
					FechaMin = fgasto.getTime() < fMin.getTime() ? f1[2] + "-" + f1[1] + "-" + f1[0] : FechaMin;
				}
			});

			FechaMax = formatFecha(FechaMax, 'dd-mm-yyyy');
			FechaMin = formatFecha(FechaMin, 'dd-mm-yyyy');

			$("#msnmb").empty();

			rangoFechas("repde2", "repa2", "reporte2", "BuscarMovBancariosParaConfrontar('" + tarjeta + "')");
			$("#repde2, #repde2 + span span").datepicker("destroy");

			var fechafin = FechaMax;
			var fechaini = FechaMin;
			$("input#repde2").val(fechaini);
			$("input#repa2").val(fechafin);


			$("#repa2").datepicker("option", "minDate", fechaini);

			$("#importede").val(Math.ceil(importeMin));
			$("#importea").val(Math.ceil(importeMax));


			$("#tabsConfrontar").tabs();
			$("#confrontacion").modal({
				show: true,
				keyboard: false,
				backdrop: "static"
			});
			BuscarMovBancariosParaConfrontar(tarjeta);
			$("#confrontacion").css({ 'z-index': 2000 });
		});
		$("#importede, #importea").change(function () {
			BuscarMovBancariosParaConfrontar("");
		});
		$("#btnBuscarMovBanco").click(function () {
			var IdInforme = $("#idinforme").val() * 1;
			browseGastos(IdInforme);
			setTimeout(function () {
				BuscarMovBancariosParaConfrontar("");
			}, 2000);

		});
		$("#confrontarInforme").click(function () {
			var ConfBanco = $("#ConfBanco").val() * 1;
			if (ConfBanco === 0) {
				var IdInforme = $("#idinforme").val() * 1;
				var ImporteRequisicion = $("#RmRdeRequisicion").val() * 1;
				var ImporteMovBanco = $("#totalMovBanco").val() * 1;
				var ImporteGastado = $("#totalg").val() * 1;
				/*var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
				if (vComensalesObjetivo.estatus === false) {
					Seguridad.alerta("No puedes confrontar el informe.<br />" + vComensalesObjetivo.mensaje, "#tabConfrontacion");
					return false;
				}*/
				var valida = validaExistenComprobantes();
				if (valida.NoGastosConComprobante != valida.NoGastos) {
					Seguridad.alerta("Para Confrontar, todos los gastos deben contener almenos un comprobante.", "#tabConfrontacion");
					return false;
				}

				if (ImporteGastado.toFixed(2) !== ImporteMovBanco.toFixed(2)) {
					Seguridad.alerta("No se puede confrontar el informe.<br />" +
						"El importe gastado y confrontado son diferentes.<br />" +
						"Importe a confrontar: <b>" + formatNumber.new(ImporteMovBanco.toFixed(2), "$ ") + "</b><br />" +
						"Importe gastado: <b>" + formatNumber.new(ImporteGastado.toFixed(2), "$ ") + "</b>",
						"#tabConfrontacion");
					return false;
				}

				var ImporteFondeo = ImporteRequisicion - ImporteMovBanco;
				ImporteFondeo = ImporteFondeo.toFixed(2);

				var msnFondeo = "Importe a confrontar: <b>" + formatNumber.new(ImporteMovBanco.toFixed(2), "$ ") + "</b><br />" +
					"Importe requisición: <b>" + formatNumber.new(ImporteRequisicion.toFixed(2), "$ ") + "</b><br />" +
					"Importe gastado: <b>" + formatNumber.new(ImporteGastado.toFixed(2), "$ ") + "</b><br />" +
					//"Al confrontar tu informe se solicitara un retiro por: <b>" + formatNumber.new(ImporteFondeo, "$ ") + "</b> (solo en caso necesario)<br />" +
					"Al confrontar ya no podras modificar tus gastos.";

				var botones = [];
				botones[0] = {
					text: "Si", click: function () {
						var datos = {
							'IdInforme': IdInforme,
							'ImporteRequisicion': ImporteRequisicion,
							'ImporteMovBanco': ImporteMovBanco,
							'ImporteGastado': ImporteGastado,
							'ImporteFondeo': ImporteFondeo
						};
						confrontarInforme(datos);
						$(this).dialog("close");
					}
				};
				botones[1] = {
					text: "No", click: function () {
						$(this).dialog("close");
					}
				};
				Seguridad.confirmar("Confrontar Informe?<br />" + msnFondeo, botones, " Confrontar Informe.", "#tabConfrontacion");
			} else {
				Seguridad.alerta("El informe ya fue confrontado.", "#tabConfrontacion");
			}
		});
		function confrontarInforme(datos) {
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ConfrontarInforme',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					//console.log(result)
					if (result.ConfrontacionOk === true) {
						$.notify(result.Descripcion, { position: "top center", className: "success" });
						selectInforme(datos.IdInforme);
						browseGastos(datos.IdInforme);
						$("#confrontacion").modal("hide");
					} else {
						$.notify(result.Descripcion, { position: "top center", className: "error" });
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
		}
		function BuscarMovBancariosParaConfrontar(tarjeta) {
			var IdInforme = $("#idinforme").val() * 1;
			var repde = $("#repde2").val();
			var repa = $("#repa2").val();
			var importede = $("#importede").val() * 1;
			var importea = $("#importea").val() * 1;

			tarjeta = $.trim(tarjeta);
			if (valorVacio(tarjeta)) {
				var datos = JSON.parse(localStorage.getItem("listGastos"));
				tarjeta = $.trim($("#formaPagoInforme").val());// datos[0]['FormaPago'];
			}
			repde = repde.split("-");
			repa = repa.split("-");

			var repde1 = repde[2] + "-" + repde[1] + "-" + repde[0];
			var repa1 = repa[2] + "-" + repa[1] + "-" + repa[0];
			var ConfBanco = $("#ConfBanco").val() * 1;
			var datos = {
				'RepDe': repde1,
				'RepA': repa1,
				'ImporteDe': importede,
				'ImporteA': importea,
				'Tarjeta': tarjeta,
				'IdInforme': IdInforme
			};
			$.ajax({
				async: true,
				type: "POST",
				url: '/api/ConsultaMovBancariosParaConfrontar',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargando();
				},
				success: function (result) {
					//console.log(result);
					$("#tblMovBanco tbody").empty();
					var i = 1;
					if (valorVacio(result))
						$("#confrontarInforme").hide();
					else
						$("#confrontarInforme").show();
					var tImporte = 0;
					$.each(result, function (key, value) {
						value["IdInf"] = IdInforme;
						var opc = menuOpcionesConfrontar(value);
						tImporte += value.Importe * 1;
						var row = "<tr>";
						row += "<td>" + value.Banco + "</td>";
						row += "<td>" + value.Descripcion + "</td>";
						row += "<td>" + value.Fecha + "</td>";
						row += "<td style='text-align: right'>" + formatNumber.new((value.Importe).toFixed(2), "$ ") + "</td>";
						row += "<td align='center'>" + opc.opciones + "</td>";
						row += "</tr>";
						$("#tblMovBanco tbody").append(row);
						if (opc.menu === true)
							$("#confrontarInforme").hide();
					});
					$("#totalMovBanco").val(tImporte);
					"#tdTotalMovBanco".AsHTML(formatNumber.new(tImporte.toFixed(2), "$ "));
				},
				complete: function () {

					if (ConfBanco === 1) {
						$("#cancelarConfrontacion").show();
					}
					$("#tblMovSinGasto tbody tr").each(function () {
						try {
							//$(this)[0].cells[5].className = "text-right";
						} catch (err) {
						}
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function menuOpcionesConfrontar(datos) {
			if (datos.Conciliacion === 0) {
				var menu = "<div class='dropdown'>";
				menu += "<button class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Opciones</button>";
				menu += "<div class='dropdown-menu'>";
				menu += "<a href='#' onclick='confLigarGasto(" + JSON.stringify(datos) + ")' class='dropdown-item'>Ligar a Gasto</a>";
				menu += "<a href='#' onclick='confAgregarGastoMovBanco(" + JSON.stringify(datos) + ")' class='dropdown-item'>Agregar como un nuevo Gasto</a>";
				menu += "</div>";
				menu += "</div>";
				var opciones = {
					'opciones': menu,
					'menu': true
				};
				return opciones;
			} else {
				var gasto = selGastoStorage(datos);
				gasto = "<h6><label class='label' style='font-size: 11px; margin:3px; color:black'>Gasto: " + gasto + "</label></h6>";
				var opciones = {
					'opciones': gasto,
					'menu': false
				};
				return opciones;
			}

		}
		function selGastoStorage(datos) {
			var listGastos = JSON.parse(localStorage.getItem("listGastos"));
			var gasto = "";
			$.each(listGastos, function (key, value) {
				if (datos.IdGasto === value.IdGasto) {
					gasto = value.Concepto + " / " + value.Negocio;
				}
			});
			return gasto;
		}
		function confLigarGasto(datos) {
			var datosMovBan = datos;
			var listGastos = JSON.parse(localStorage.getItem("listGastos"));
			var opcionGasto = "<ol style='list-style-type: none;'>";
			var checked = "checked";
			var nopciones = 0;
			$.each(listGastos, function (key, value) {
				if ((value.IdMovBanco * 1) === 0) {
					var total = formatNumber.new((value.TGastado).toFixed(2), "$ ");
					opcionGasto += "<li><label><input type='radio' name='radioGasto' " + checked + " value='" + JSON.stringify(value) + "'> " + value.Concepto + " <b>" + total + "</b></label></li>";
					checked = "";
					nopciones++;
				}
			});
			if (nopciones === 0) {
				Seguridad.alerta("No existen gastos para ligar.",
					"#tabConfrontacion");
				return false;
			}

			opcionGasto += "</ol>";
			var botones = [];
			botones[0] = {
				text: "Aceptar", click: function () {
					var datosGastos = $("input:radio[name='radioGasto']:checked").val();
					datosGastos = JSON.parse(datosGastos);
					ligarGasto(datosMovBan, datosGastos);
					$(this).dialog("close");
				}
			};
			botones[1] = {
				text: "Cancelar", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Selecciona un gasto:" + opcionGasto, botones, "Ligar Gasto.", "#tabConfrontacion");
		}
		function ligarGasto(datosMovBan, datosGastos) {
			//console.log("info mov banco => ", datosMovBan, "Info gasto => ", datosGastos);

			cargando();
			var datos = {
				'IdMovBanco': datosMovBan.IdMovBanco,
				'IdInforme': datosMovBan.IdInf,
				'IdGasto': datosGastos.IdGasto,
				'Importe': datosMovBan.Importe
			};

			crearLigaGasto(datos);
			//obtenerGastosInforme(datosMovBan.IdInf, 0, 2);
			browseGastos(datosMovBan.IdInf);
			BuscarMovBancariosParaConfrontar(datosMovBan.Tarjeta);
			cargado();
		}
		function crearLigaGasto(datos) {
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/RelacionaGastoMovBanco',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
				},
				success: function (result) {
					var tipomsn = result.RelacionOk === true ? "success" : "error";
					$.notify(result.Descripcion, { position: "top center", className: tipomsn });
				},
				complete: function () {

				},
				error: function (result) {
				}
			});
		}
		function confAgregarGastoMovBanco(datos) {

			var disponible = $("#disAnticipo").val() * 1;

			if (datos.Importe > disponible) {
				Seguridad.alerta("No puedes agregar el gasto.<br />" +
					"El importe del gasto es mayor al importe disponible (por comprobar):<br />" +
					"Imp. disponible: <b>" + formatNumber.new(disponible.toFixed(2), "$ ") + "</b><br />" +
					"Imp. gasto: <b>" + formatNumber.new((datos.Importe).toFixed(2), "$ ") + "</b>",
					"#tabConfrontacion");
				return false;
			}

			var datosMovBan = datos;
			var botones = [];
			botones[0] = {
				text: "Aceptar", click: function () {
					agregarGastoMovBanco(datosMovBan);
					$(this).dialog("close");
				}
			};
			botones[1] = {
				text: "Cancelar", click: function () {
					$(this).dialog("close");
				}
			};
			var gasto = datosMovBan.Descripcion + " / " + formatNumber.new((datosMovBan.Importe).toFixed(2), "$ ");
			Seguridad.confirmar("Agregar movimiento bancario como un gasto:<br />" + gasto, botones, "Agregar Gasto.", "#tabConfrontacion");
		}
		function agregarGastoMovBanco(datosMovBan) {

			var elementoCat = $("#categoria")[0];
			var Categoria = elementoCat.options[0].value;
			var NombreCategoria = elementoCat.options[0].text;
			var datosCat = elementoCat.options[0].dataset;
			var ivacategoria = datosCat.grmativa;

			var ugasto = localStorage.getItem("cosa");
			var datos = {
				"idinforme": datosMovBan.IdInf,
				"idproyecto": 0,
				"ugasto": ugasto,
				"concepto": datosMovBan.Descripcion,
				"negocio": datosMovBan.Banco,
				"observaciones": "Movimiento Bancario",
				"rfc": "",
				"contacto": "",
				"telefono": "",
				"fgasto": (datosMovBan.Fecha).replace("/", "-"),
				"hgasto": horaActual("hh:mm"),
				"total": datosMovBan.Importe,
				"formapago": datosMovBan.Tarjeta,
				"categoria": Categoria,
				"correo": "",
				"fileotros": "",
				"nombreCategoria": NombreCategoria,
				"ivaCategoria": ivacategoria,
				"ncomensales": 0,
				"nmbcomensales": ""
			};
			//console.log("datos insert=>", datos);
			cargando();
			$.ajax({
				type: "POST",
				url: "/api/InsertGasto",
				data: JSON.stringify(datos), //checar con hector{'dirxml': dirxml, 'dirpdf': dirpdf, 'dirotros': dirotros},
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				beforeSend: function () {

				},
				success: function (result) {
					$.notify("Gasto Guardado.", { globalPosition: 'top center', className: 'success' });
					var datosLiga = {
						'IdMovBanco': datosMovBan.IdMovBanco,
						'IdInforme': datosMovBan.IdInf,
						'IdGasto': result[0].IdGasto,
						'Importe': datosMovBan.Importe
					};
					crearLigaGasto(datosLiga);
				},
				complete: function () {
					cargado();
					//consultaInfoGastos(datosMovBan.IdInf, 2, 1);
					selectInforme(datosMovBan.IdInf);
					browseGastos(datosMovBan.IdInf);
					BuscarMovBancariosParaConfrontar(datosMovBan.Tarjeta);
				},
				error: function (result) {
					console.log(result);
					$.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
				}
			});

		}
		//confrontación carga excel
		function preparaCarga() {
			$("#trIntrucciones").show();
			$("#trMovimientoEdoCuenta").hide();
		}
		function cargaBanco() {
			var IdInforme = $("#idinforme").val() * 1;
			cargando();
			var banco = $("#banco").val();
			if (!valorVacio(banco)) {
				resetInformeConfrontado();
				var file = $("#filebanco").get(0).files[0];
				var r = new FileReader();
				var nombre = file.name;
				var extFile = (nombre.substring(nombre.lastIndexOf(".") + 1)).toLowerCase();
				if (extFile === "xlsx") {
					r.onload = function () {
						var binimage = r.result;
						nombre = nombre.replace("." + extFile, "");
						guardarExcelBanco(banco, nombre, extFile, binimage, IdInforme);
					};
					r.readAsDataURL(file);
				} else {
					$("#filebanco").notify("El formato del archivo no es valido.", { globalPosition: 'top center', className: 'error' });
					$("#filebanco").filestyle('clear');
				}

			} else {
				$("#banco").notify("Seleciona un banco.", { globalPosition: 'top center', className: 'error' });
				$("#filebanco").filestyle('clear');
			}
		}
		function guardarExcelBanco(banco, nombre, extFile, binimage, IdInforme) {
			var datos = {
				'Usuario': encriptaDesencriptaEle(UsuarioActivo, 0),
				'ArchivoNmb': nombre,
				'ArchivoExt': extFile,
				'Archivo': binimage
			};
			var tablaMov = "";
			var total = 0;
			$.ajax({
				async: true,
				type: "POST",
				url: '/api/CargarExcelBancoClosedXML', // CargarExcelBanco,
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					$("#filebanco").filestyle('clear');
					$("#tdTarjeta, #tdNombre, #tdNomina").empty();
					$("#tblMovimientos tbody").empty().remove();
					tablaMov = $("#tblMovimientos").DataTable()
					tablaMov.destroy();
					tablaMov = crearTablaReportes("#tblMovimientos", 0, "desc", false, '');
				},
				success: function (result) {
					if (!valorVacio(result)) {
						var resultado = result[0];
						if (resultado.ArchivoOk === true) {
							"#tdNombre".AsHTML(resultado.Nombre);
							"#tdNomina".AsHTML(resultado.Nomina);
							$("#tblMovimientos").append("<tbody>");
							var nmovimientos = 0;
							try {
								nmovimientos = resultado.RowExcel.length;
							} catch (err) {
								nmovimientos = 0;
							}
							if (nmovimientos > 0) {
								var i = 0;
								var Tarjeta = "", Descripcion = "", Fecha = "", Importe = 0, Tipo = "";
								$.each(resultado.RowExcel, function (key, value) {
									if (value.Fecha !== null) {
										value['Nombre'] = resultado.Nombre;
										value['Nomina'] = resultado.Nomina;
										value['Embosado'] = resultado.Embosado;
										value['IdInforme'] = IdInforme;
										Tarjeta = value.Tarjeta;
										Descripcion = value.Descripcion;
										Fecha = ((value.Fecha).split(" "))[0];
										Importe = value.Importe;
										total += Importe;
										//Tipo = value.Tipo;
										//var fc = 'chkMovimiento($(this))';
										var inpChk = chk("movBanco" + i, "movBanco", "checked", "", JSON.stringify(value), "18", "success", "danger", "");
										var spanDuplicado = "<span id='tdDuplicado" + i + "' align='center'></span>";
										var spanFechaDuplicado = "<span id='tdFDuplicado" + i + "' align='center'></span>";
										var spanChk = "<span id='tdChk" + i + "' align='center'>" + inpChk + "</span>";
										var newRow = [
											Tarjeta,
											Fecha,
											Descripcion,
											formatNumber.new(Importe.toFixed(2), "$ "),
											//spanDuplicado,
											//spanFechaDuplicado,
											spanChk
										];
										tablaMov.row.add(newRow).draw(false);
										i++;
									}
								});
								"#tdTarjeta".AsHTML(Tarjeta);
							}
							$("#tblMovimientos").append("</tbody>");
							//$("#totalMovBanco").val()
							"#tdTotalConfrontar".AsHTML(formatNumber.new(total.toFixed(2), "$ "));
							$("#trIntrucciones").hide();
							$("#trMovimientoEdoCuenta").show();



						} else {
							$.notify("Error al cargar archivo.", { position: "top", className: 'error' });
						}
					}
				},
				complete: function () {
					cargado();
					$("#tblMovimientos tbody tr").each(function () {
						$(this)[0].cells[3].className = "text-right";
					});
					validaDuplicados();
				},
				error: function (result) {
					cargado();
					console.log("error", result);
				}
			});
		}
		function validaDuplicados() {
			$("input:checkbox[name=movBanco]").each(function () {
				var id = ($(this)[0].id).replace("movBanco", "");
				var datos = JSON.parse($(this).val());
				datos['Banco'] = "TOKA";
				datos['IdChk'] = id;
				var duplicado = movBancoEsDuplicado(datos);
				datos['Duplicado'] = duplicado.Duplicado === "No" ? 0 : 1;
				datos['IdMovimiento'] = duplicado.IdMovimiento;
				datos['Usuario'] = encriptaDesencriptaEle(UsuarioActivo, 0);
				("#tdDuplicado" + id).AsHTML(duplicado.Duplicado);
				("#tdFDuplicado" + id).AsHTML(datoEle(duplicado.Fecha));

				$(this).val(JSON.stringify(datos));
			});
		}
		function movBancoEsDuplicado(datos) {
			var resultado = [];
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/MovBancoEsDuplicado',
				data: JSON.stringify(datos),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					resultado = result[0];

				},
				complete: function () {
					//cargado();
				},
				error: function (result) {
					//cargado();
					resultado = {
						'Duplicado': "No",
						'Fecha': null,
						'IdMovimiento': 0
					};
					console.log("error", result);
				}
			});
			return resultado;
		}
		$("#guardarMBanco").click(function () {
			var elementosSel = false;
			var IdInforme = $("#idinforme").val() * 1;
			//resetInformeConfrontado();
			$("input:checkbox[name=movBanco]").each(function () {
				if ($(this).is(':checked')) {
					elementosSel = true;
					var datos = $(this).val(); //JSON.parse($(this).val());
					var resultado = guardarMBanco(datos);
					if (resultado.Exito.GuardadoOk === true) {
						var idchk = resultado.DatosGuardados.IdChk;
						("#tdChk" + idchk).AsHTML("<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span> Cargado</span>");
					} else {
						("#tdChk" + idchk).AsHTML("<span style='font-size: 11px' class='label label-danger'><span class='glyphicon glyphicon-remove'></span> Cargado</span>");
					}
				}
			});

			if (elementosSel === false) {
				$.notify("Seleccionar movimiento(s).", { position: "top center", className: 'error' });
			} else {
				obtenerGastosInforme(IdInforme, 0, 2);
				$.notify("Información guardada.", { position: "top center", className: 'success' });
			}
		});
		function resetInformeConfrontado() {
			var IdInforme = $("#idinforme").val() * 1;
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/ResetInformeConfrontado',
				data: JSON.stringify({ 'IdInforme': IdInforme }),
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					BuscarMovBancariosParaConfrontar("");
				},
				error: function (result) {
					//cargado();
					console.log("error", result);
					resultado = null;
				}
			});
		}
		function guardarMBanco(datos) {
			var IdInforme = $("#idinforme").val() * 1;
			var resultado = [];
			$.ajax({
				async: false,
				type: "POST",
				url: '/api/GuardarMovBanco',
				data: datos,
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//cargado();
				},
				success: function (result) {
					resultado = {
						'DatosGuardados': JSON.parse(datos),
						'Exito': result
					};
				},
				complete: function () {
					//cargado();
				},
				error: function (result) {
					//cargado();
					console.log("error", result);
					resultado = null;
				}
			});
			return resultado;
		}
		$("#cancelarConfrontacion").click(function () {
			var IdInforme = $("#idinforme").val() * 1;
			var botones = [];
			botones[0] = {
				text: "Si", click: function () {
					$(this).dialog("close");
					resetInformeConfrontado();
					consultaInfoGastos(IdInforme, 2, 1);
				}
			};
			botones[1] = {
				text: "No", click: function () {
					$(this).dialog("close");
				}
			};
			Seguridad.confirmar("Cancelar la confrontación del informe?", botones, " Cancelar Confrontación.", "#tabConfrontacion");
		});
	</script>

</asp:Content>
