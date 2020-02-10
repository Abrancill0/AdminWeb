<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rep_desglose_estatus_informes.aspx.cs" Inherits="SCGESP.rep_desglose_estatus_informes" %>

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
				Reporte Desglose De Estatus Por Informes
				<a href="#" onclick='cerrarPanel(".panel")' style="color: #FFF; border-left: 1px solid #FFF"><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<a class="btn btn-primary btn-md float-left" href="javascript:window.history.back();" style="background-color: #706259" role="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Regresar</a>

				<div class="table-responsive">
					<table class="filtro w-75">
						<tr>
							<td style="text-align: center; padding: 10px 20px">Fechas:
							</td>
							<td>
								<select id="tipoFecha" name="tipoFecha" class="select2" data-width="100%">
									<option value="*">--- Todas ---</option>
									<option value="periodo">Registro de Gastos</option>
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
							<td style="text-align: center; padding: 10px 20px">No. Requisición:</td>
							<td>
								<input type="text" id='IdRequisicion' name='IdRequisicion' class='form-control input-mask' style="width: 100%" />
							</td>
							<td style="text-align: center; padding: 10px 20px">No. Informe:</td>
							<td>
								<input type="text" id='NoInforme' name='NoInforme' class='form-control input-mask' style="width: 100%" />
							</td>
							<td style="text-align: center; padding: 10px 20px">Responsable:</td>
							<td>
								<select id="uResponsable" name="uResponsable" class="select2" data-width="100%">
									<option value="*">--- Todos ---</option>
								</select>
							</td>
						</tr>
						<tr>
							<td colspan="6" style="text-align: right; padding: 10px 20px">
								<label class='custom-control custom-checkbox'>
									<input type='checkbox' checked id='VerEstatusAdminERP' class='custom-control-input'>
									<span class='custom-control-indicator'></span>
									<span class='custom-control-description'>Ver estatus AdminERP</span>
								</label>
							</td>
						</tr>
					</table>
				</div>


				<hr />

				<div class="table-responsive">
					<table id="tblDesgloseEstatusInformes" class="display" cellspacing="0" width="100%" data-page-length="-1">
						<thead>
							<tr style="text-align: center;">
								<th style="text-align: center;"></th>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
				</div>

			</div>
		</div>
	</section>

	<script type="text/javascript">
		var UsuarioActivo = localStorage.getItem("cosa");
		var EmpeladoActivo = localStorage.getItem("cosa2");
		rangoFechas("repde", "repa", "reporte2", "");

		$("#repde").val(FechaMasMenos(fechaActual(), "30", "d", "-"));
		$("#repa").val(fechaActual());

		$("#btn-consultar").click(function () {
			ConsultarDesgloseEstatusInformes();
		});

		selectUsuarios();
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
						$("#uResponsable").append("<option value='" + value.Usuario + "|" + value.IdEmpleado + "'>" + value.Nombre + " (" + value.Usuario + " - " + value.IdEmpleado + ")" + "</option>");
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
		}

		function ConsultarDesgloseEstatusInformes() {
			var repde = $("#repde").val().split("-");
			var repa = $("#repa").val().split("-");
			var UsuarioIdEmpleado = ($("#uResponsable").val()).split("|");
			var IdRequisicion = $("#IdRequisicion").val() * 1;
			var NoInforme = $("#NoInforme").val() * 1;
			var VerEstatusAdminERP = 1;
			VerEstatusAdminERP = $("#VerEstatusAdminERP").is(':checked') ? 1 : 0;

			var aDatos = {
				'TipoFecha': $("#tipoFecha").val(),
				'RepDe': repde[2] + "-" + repde[1] + "-" + repde[0],
				'RepA': repa[2] + "-" + repa[1] + "-" + repa[0],
				'UResponsable': UsuarioIdEmpleado[0],
				'IdEmpleado': UsuarioIdEmpleado[1],
				'UsuarioActivo': UsuarioActivo,
				'Estatus': "",
				'IdRequisicion': IdRequisicion,
				'NoInforme': NoInforme,
				'VerEstatusAdminERP': VerEstatusAdminERP
			};
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/ReporteDesgloseEstatusInformes",
				data: aDatos,
				dataType: "json",
				beforeSend: function () {
					cargando();
				},
				success: function (result) {
					console.log(result);
					$("#tblDesgloseEstatusInformes tbody").empty();

					var f = new Date();
					if (valorVacio(result) === false) {
						$.each(result, function (key, estatus) {
							var newTr = "<tr><td>";

							newTr += "<div class='card border rounded'>";
							newTr += "<div class='card-header border rounded panel-heading h3 p-1'>Requisición: " + estatus.Requisicion + " - Informe: " + estatus.Informe + "</div>";
							newTr += "<div class='card-body border pt-1'>";
							newTr += "<h5 class='card-title'>";
							newTr += "<small class='float-right'><b>Responsable:</b> " + estatus.NombreResponsabe + "</small>";
							newTr += "<b>Justificación:</b> " + estatus.Justificacion;
							newTr += "</h5>";

							newTr += "<div class='container border'>";

							newTr += "<div class='row align-items-start text-bold bold h5 p-1'>";
							newTr += "<div class='col'>Estatus</div>";
							newTr += "<div class='col'>Usuario</div>";
							newTr += "<div class='col'>Fecha</div>";
							newTr += "</div>";

							$.each(estatus.Estatus, function (k, est) {
								newTr += "<div class='row align-items-start'>";
								newTr += "<div class='col'>" + est.Estatus + "</div>";
								newTr += "<div class='col'>" + est.NombreUsurio + "</div>";
								newTr += "<div class='col'>" + est.Fecha + "</div>";
								newTr += "</div>";
							});
							

							newTr += "</div>";


							newTr += "</div>";
							newTr += "</div>";

							newTr += "</td></tr>";

							$("#tblDesgloseEstatusInformes tbody").append(newTr);
						});
					} else {
						$.notify("Sin información.", { globalPosition: 'top center', className: 'error' });
					}
				},
				complete: function () {
					cargado();
				},
				error: function (result) {
					cargado();
					console.log(result);
				}
			});
		}
	</script>

</asp:Content>
