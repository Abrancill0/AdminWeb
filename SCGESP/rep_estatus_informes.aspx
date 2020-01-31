<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rep_estatus_informes.aspx.cs" Inherits="SCGESP.rep_informes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<style type="text/css">
		input::-webkit-input-placeholder {
			font-size: 10px !important;
		}

		input:-moz-placeholder {
			font-size: 10px !important;
		}

		input:-ms-input-placeholder {
			font-size: 10px !important;
		}

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
		}

			table.tblGastos thead {
				background: #6B5C4F;
				font-size: 12px;
				color: #ffffff;
				font-weight: bold;
			}

				table.tblGastos thead tr th {
					background: #6B5C4F;
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
				border-left: #000 solid 1px;
			}

			table.tblGastos tfoot {
				background: #DAE875;
				border: #000 solid 1px;
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
				Reporte Estatus Informes
        <a href="#" onclick='cerrarPanel(".panel")' style="color: #FFF; border-left: 1px solid #FFF"><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<div class="table-responsive">
					<table class="filtro">
						<tr>
							<td style="text-align: center; padding: 10px 20px">Fechas:
							</td>
							<td>
								<select id="tipoFecha" name="tipoFecha" class="select2" data-width="230px">
									<option value="*">--- Todas ---</option>
									<option value="periodo">Periodo de Informe</option>
									<option value="registro">Registro de Informe</option>
								</select>
							</td>
							<td style="text-align: center; padding: 10px 20px">De:
							</td>
							<td>
								<div class='input-group date' id='datetimepicker1' style="width: 140px">
									<input class="form-control reporte2" readonly="readonly" name="repde" id="repde" type="text" style="text-align: center;" />
									<i class="form-group__bar"></i>
									<span class="input-group-addon" style="width: 50px">
										<span class="zmdi zmdi-calendar zmdi-hc-2x"></span>
									</span>
								</div>
							</td>
							<td style="text-align: center; padding: 10px 20px">A:
							</td>
							<td>
								<div class='input-group date' id='datetimepicker2' style="width: 140px">
									<input class="form-control reporte2" readonly="readonly" name="repa" id="repa" type="text" style="text-align: center;" />
									<i class="form-group__bar"></i>
									<span class="input-group-addon" style="width: 50px">
										<span class="zmdi zmdi-calendar zmdi-hc-2x"></span>
									</span>
								</div>
							</td>
							<td rowspan="2" style="padding-left: 10px">
								<button id="btn-consultar" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-search"></span>Consultar</button>
							</td>
						</tr>
						<tr>
							<td style="text-align: center; padding: 10px 20px">Estatus:
							</td>
							<td>
								<select id="estatus" name="estatus" class="select2" data-width="230px">
									<option value="*">--- Todos ---</option>
								</select>
							</td>
							<td style="text-align: center; padding: 10px 20px" colspan="2">Responsable:
							</td>
							<td colspan="2">
								<select id="uResponsable" name="uResponsable" class="select2" data-width="230px">
									<option value="*">--- Todos ---</option>
								</select>
							</td>
						</tr>
					</table>
				</div>

				<hr />

				<div class="table-responsive">
					<table id="tblInformes" class="display browse" cellspacing="0" width="100%" data-page-length="-1">
						<thead>
							<tr style="text-align: center;">
								<th width="50px" style="text-align: center;">Requisici&oacute;n</th>
								<th width="40px" style="text-align: center;">Informe</th>
								<th width="40px" style="text-align: center;">Del</th>
								<th width="40px" style="text-align: center;">Al</th>
								<th width="250px">Justificaci&oacute;n</th>
								<th width="100px" style="text-align: center;">Importe<br />
									Autorizado</th>
								<th width="100px" style="text-align: center;">Responsable</th>
								<th width="100px" style="text-align: center;">Estatus</th>
								<th width="100px" style="text-align: center;">Usuario Actual</th>
								<th width="40px" style="text-align: center;">Recibido</th>
								<th width="40px" style="text-align: center;">Ver</th>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
				</div>

			</div>

		</div>

		<!-- Modal Comprobante -->
		<div class="modal fade" id="verInforme" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
			<div class="modal-dialog modal-xlg" role="document">
				<div class="modal-content">
					<div class="modal-header titulo-modal">
						<a class="center-block	">Ver </a>
						<a href="#" data-dismiss="modal" aria-label="Close" style="color: #FFF; border-left: 1px solid #FFF">&nbsp;<i class="zmdi zmdi-close"></i>Cerrar&nbsp;</a>

					</div>
					<div class="modal-body">
						<div class="card">
							<div class="card-header" style="margin: 0px; padding: 2px;">
								<div class="row">
									<div id="cabeceraInforme" class="col-xs-12 col-md-6 col-lg-8">
										<!--informe-cabecera-template-->
									</div>

									<div id="importesInforme" class="col-md-6 col-lg-4">
										<!--importes-informe-template-->
									</div>
								</div>
							</div>
							<div class="card-text">
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
		</div>


	</section>

	<!--Informe cabecera-->
	<script id="informe-cabecera-template" type="text/x-handlebars-template">

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
					<p class="valor">{{ e_estatus }} / {{ bandeja_usuario }} / {{ a_fsolicitud }}</p>
				</td>
			</tr>
		</table>
	</script>
	<!--Importes informe-->
	<script id="importes-informe-template" type="text/x-handlebars-template">

		<table cellspacing="0" width="100%">
			<tr>
				<td colspan="2" align="center"><b>Importes Requisici&oacute;n</b></td>
			</tr>
			<tr>
				<td class="concepto-importe">Autorizado: </td>
				<td>
					<span style='font-size: 16px; display: block; background-color: #007aff !important;' class='label label-primary text-right'>{{ montoRequisicion }}
					</span>
				</td>
			</tr>
			<tr>
				<td class="concepto-importe">Gastado: </td>
				<td>
					<span style='font-size: 16px; display: block; background-color: #ffa000 !important;' class='label label-danger text-right'>{{ montoGastado }}
					</span></td>
			</tr>
			<tr>
				<td class="concepto-importe">Por Comprobar: </td>
				<td>
					<span style='font-size: 16px; display: block; background-color: transparent !important; color: #000;' class='label label-success text-right'>{{ disponible }}
					</span>
				</td>
			</tr>
			<tr>
				<td class="concepto-importe">Decrementado: </td>
				<td><span style='font-size: 16px; display: block; background-color: transparent !important; color: #000;' class='label label-warning text-right'>{{ decrementado }}
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
						<a href='{{ xml }}' data-toggle='tooltip' target="_blank" class='btn btn-success btn-sm' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="pdf text-center" style='width: 30px;'>{{#if pdf}}
						<a href='{{ pdf }}' data-toggle='tooltip' target="_blank" class='btn btn-success btn-sm verPDF' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="img text-center" style='width: 30px;'>{{#if img}}
						<a href='{{ img }}' data-toggle='tooltip' target="_blank" class='btn btn-success btn-sm verIMG' aria-disabled='false' role='button'>
							<span class="glyphicon glyphicon-eye-open"></span>
						</a>
				{{/if}}
			</td>
			<td class="monto_comprobado text-right" style='width: 120px;' title="Importe con comprobante">{{ monto_comprobado }}</td>
			<td class="dentro_politica text-right" style='width: 150px;' title="Importe aceptable">{{ dentro_politica }}</td>
			<td class="fuera_politica text-right" style='width: 120px;' title="Importe fuera de política">{{ fuera_politica }}</td>
			<td class="no_deducible text-right" style='width: 100px;' title="Importe no deducible">{{ no_deducible }}</td>
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
		</tr>
	</script>

	<script type="text/javascript" src="js/handlebars-v4.0.11.js"></script>
	<script type="text/javascript">
		var UsuarioActivo = localStorage.getItem("cosa");
		var EmpeladoActivo = localStorage.getItem("cosa2");
		rangoFechas("repde", "repa", "reporte2", "");

		$("#repde").val(FechaMasMenos(fechaActual(), "30", "d", "-"));
		$("#repa").val(fechaActual());

		var tblInformes = crearTablaReportes("#tblInformes", 0, "desc", false, ['excel', 'pdf', 'print']);
		$(".dt-buttons").css(
			{
				"display": "block"
			}
		);
		$(".dt-buttons a").removeClass().addClass("btn btn-primary px-md-5").attr("role", "button").css(
			{
				"margin-right": "10px"
			}
		);
		$("#btn-consultar").click(function () {
			consultarInformes();
		});

		selectUsuarios();
		selectEstatusInforme();

		function selectUsuarios() {
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/SelectUsuarios",
				data: {},
				dataType: "json",
				beforeSend: function () {
					$("#uResponsable").empty();
					$("#uResponsable").append("<option value='*'>--- Todos ---</option>");
				},
				success: function (result) {
					$.each(result, function (key, value) {
						$("#uResponsable").append("<option value='" + value.Usuario + "'>" + value.Nombre + " (" + value.Usuario + ")" + "</option>");
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function selectEstatusInforme() {
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/SelectEstatusInforme",
				data: {},
				dataType: "json",
				beforeSend: function () {
					$("#estatus").empty();
					$("#estatus").append("<option value='*'>--- Todos ---</option>");
				},
				success: function (result) {
					$.each(result, function (key, value) {
						$("#estatus").append("<option value='" + value.Estatus + "'>" + value.Estatus + "</option>");
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
		function consultarInformes() {
			var repde = $("#repde").val().split("-");
			var repa = $("#repa").val().split("-");
			var aDatos = {
				'TipoFecha': $("#tipoFecha").val(),
				'RepDe': repde[2] + "-" + repde[1] + "-" + repde[0],
				'RepA': repa[2] + "-" + repa[1] + "-" + repa[0],
				'Estatus': $("#estatus").val(),
				'UResponsable': $("#uResponsable").val()
			};
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/rep_informes_estatus",
				data: aDatos,
				dataType: "json",
				beforeSend: function () {
					cargando();
				},
				success: function (result) {
					console.log(result);
					$("#tblInformes tbody").empty();
					tblInformes.rows()
						.remove()
						.draw();

					//823 4900 opc 1 o 2
					var f = new Date();
					if (valorVacio(result) === false) {
						$.each(result, function (key, value) {
							var btnVer = "<a href='#' onClick='VerInforme(" + value.i_id + ")' class='btn btn-success btn-sm'><i class='zmdi zmdi-eye zmdi-hc-lg' style='padding: 3px 0px'></i> Ver</a>";
							var newTr = [
								value.r_idrequisicion,
								value.i_ninforme,
								value.i_finicio,
								value.i_ffin,
								value.i_nmb,
								formatNumber.new((value.r_montorequisicion * 1).toFixed(2), "$ "),
								value.responsable,
								value.e_estatus,
								value.bandeja_usuario,
								value.a_fsolicitud,
								btnVer
							];
							tblInformes.row.add(newTr).draw(false);
						});
					} else {
						$.notify("Sin información.", { globalPosition: 'top center', className: 'error' });
					}
				},
				complete: function () {
					cargado();
					try {
						$("#tblInformes tbody tr").each(function () {
							$(this)[0].cells[5].className = "text-right";
						});
					} catch (e) {
						//e
					}
				},
				error: function (result) {
					cargado();
					console.log(result);
				}
			});
		}

		function VerInforme(IdInforme) {
			$("#verInforme").modal({
				show: true
			});

			var datos = { "id": IdInforme, "idinforme": IdInforme };

			let importesInformeTemplate = Handlebars.compile($("#importes-informe-template").html());
			let cabeceraInformeTemplate = Handlebars.compile($("#informe-cabecera-template").html());

			let gastosInformeTemplate = Handlebars.compile($("#gastos-informe-template").html());
			let totalGastosInformeTemplate = Handlebars.compile($("#total-gastos-informe-template").html());

			let okInforme = false;

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
					informe.i_comentarioaut = (informe.i_comentarioaut);
					informe.p_nmb = (informe.p_nmb);
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
					okInforme = true;
				},
				error: function (result) {
					console.log(result);
				}
			});

			if (okInforme) {
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
								classTr: colorRow,
								valores_justificacion: []//valores_edit_justificacion(result_justificacion, value.g_nombreCategoria)
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

		}
		function DatosRequisicion(RmReqId) {
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
				},
				success: function (result) {
					resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
					var fInicio = ((datoEle(resultado.RmReqFechaRequerida)).split("T"))[0];
					var fFin = ((datoEle(resultado.RmReqFechaFinal)).split("T"))[0];

					if (valorVacio(fFin)) {
						fFin = fInicio;
					}
					datos['FInicio'] = fInicio;
					datos['FFin'] = fFin;
					datos['RmReqJustificacion'] = resultado.RmReqJustificacion;
					fInicio = formatFecha(fInicio, "dd-mm-yyyy");
					fFin = formatFecha(fFin, "dd-mm-yyyy");
					localStorage.removeItem('fechasReq');
					localStorage.setItem('fechasReq', JSON.stringify({ 'fInicio': fInicio, 'fFin': fFin }));
					fechasReq = JSON.parse(localStorage.getItem("fechasReq"));
				},
				error: function (result) {
					console.log("error", result);
				}
			});
			return resultado;
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
			hoy.setHours(0, 0, 0, 0); // Lo iniciamos a 00:00 horas

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
			strJson = strJson.replace(/\\/g, '\\');
			try {
				JsonStr = JSON.parse(strJson);
			} catch (e) {
				try {
					var gJson = JSON.stringify(eval('(' + strJson + ')'));
					var JSONObj = JSON.parse(gJson);
					JsonStr = JSONObj;
				} catch (e) {
					try {
						JsonStr = JSONize(strJson);
					} catch (e) {
						try {
							JsonStr = JSON.parse(JSONize(strJson))
						} catch (e) {
							var errorString = strJson;
							var jsonValidString = JSON.stringify(eval("(" + errorString + ")"));
							var JSONObj = JSON.parse(jsonValidString);
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
				.replace(/([\$\w]+)\s*:/g, function (_, $1) { return '"' + $1 + '":' })
				// replacing single quote wrapped ones to double quote 
				.replace(/'([^']+)'/g, function (_, $1) { return '"' + $1 + '"' })
		}
		function quitar_punto_final(cadena) {
			var cadena_final = $.trim(cadena);
			if (cadena_final.substr(-1) === ".") {
				cadena_final = cadena_final.substr(0, cadena_final.length - 1);
			}
			return cadena_final;
		}
	</script>


</asp:Content>
