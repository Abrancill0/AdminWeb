<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rep_por_categorias.aspx.cs" Inherits="SCGESP.rep_por_categorias" %>

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
				Reporte Por Categorías
				<a href="#" onclick='cerrarPanel(".panel")' style="color: #FFF; border-left: 1px solid #FFF"><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<a class="btn btn-primary btn-md float-left" href="/reportes" style="background-color: #706259" role="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Regresar</a>

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
							<td style="text-align: center; padding: 10px 20px">Categoría:
							</td>
							<td colspan="2">
								<select id="categoria" name="categoria" class="select2" data-width="100%">
									<option value="*">--- Todas ---</option>
								</select>
							</td>
							<td style="text-align: center; padding: 10px 20px">Responsable:
							</td>
							<td colspan="2">
								<select id="uResponsable" name="uResponsable" class="select2" data-width="100%">
									<option value="*">--- Todos ---</option>
								</select>
							</td>

						</tr>
					</table>
				</div>

				<hr />
				<table class="display" cellspacing="0" width="100%" data-page-length="-1">
					<tr>
						<td width="33%" style="vertical-align: top; padding: 0px 1px">
							<table id="tblTotalTipoGasto" class="display browse" cellspacing="0" width="100%" data-page-length="-1">
								<thead>
									<tr style="text-align: center;">
										<th width="50%" style="text-align: center;">Tipo de Gasto</th>
										<th width="50%" style="text-align: center;">Gastado</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
						</td>
						<td width="33%" style="vertical-align: top; padding: 0px 1px">
							<table id="tblTotalDepartamento" class="display browse" cellspacing="0" width="100%" data-page-length="-1">
								<thead>
									<tr style="text-align: center;">
										<th width="50%" style="text-align: center;">Departamento</th>
										<th width="50%" style="text-align: center;">Gastado</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
						</td>
						<td width="33%" style="vertical-align: top; padding: 0px 1px">
							<table id="tblTotalCategorias" class="display browse" cellspacing="0" width="100%" data-page-length="-1">
								<thead>
									<tr style="text-align: center;">
										<th width="50%" style="text-align: center;">Categorías</th>
										<th width="50%" style="text-align: center;">Gastado</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>

						</td>
					</tr>
				</table>
				<hr />
				<div class="table-responsive">
					<table class="filtro w-50 float-right">
						<tr>
							<td style="text-align: right; padding: 10px 50px">Total Gastado:
							</td>
							<td class="h2" style="text-align: right; padding: 10px 5px" id="totalGastado">$ 0.00
							</td>
						</tr>
					</table>
				</div>
				<div class="table-responsive">
					<table id="tblInformesCategoria" class="display browse" cellspacing="0" width="100%" data-page-length="-1">
						<thead>
							<tr style="text-align: center;">
								<th width="50px" style="text-align: center;">Requisici&oacute;n</th>
								<th width="150px" style="text-align: center;">Solicitante</th>
								<th width="100px" style="text-align: center;">Tipo de gasto</th>
								<th width="100px" style="text-align: center;">Departamento</th>
								<th width="100px">Categoría</th>
								<th width="100px" style="text-align: center;">Gastado</th>
								<th width="150px" style="text-align: center;">Periodo</th>
								<th width="250px" style="text-align: center;">Justificación</th>
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

		var tblInformesCategoria = crearTablaReportes("#tblInformesCategoria", 0, "desc", false, ['excel', 'pdf', 'print']);
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
			ConsultarCategoriasInformes();
		});

		selectUsuarios();
		SelectCategorias();
		function SelectCategorias() {
			$.ajax({
				async: false,
				type: "POST",
				url: "/api/SelectCategorias",
				data: {},
				dataType: "json",
				beforeSend: function () {
					$("#categoria").empty();
					$("#categoria").append("<option value='*'>--- Todas ---</option>");
				},
				success: function (result) {
					$.each(result, function (key, value) {
						$("#categoria").append("<option value='" + value.Categoria + "'>" + value.Categoria + "</option>");
					});
				},
				error: function (result) {
					console.log(result);
				}
			});
		}
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
		function ConsultarCategoriasInformes() {
			var repde = $("#repde").val().split("-");
			var repa = $("#repa").val().split("-");
			var UsuarioIdEmpleado = ($("#uResponsable").val()).split("|");
			var Categoria = $("#categoria").val();
			var aDatos = {
				'TipoFecha': $("#tipoFecha").val(),
				'RepDe': repde[2] + "-" + repde[1] + "-" + repde[0],
				'RepA': repa[2] + "-" + repa[1] + "-" + repa[0],
				'UResponsable': UsuarioIdEmpleado[0],
				'IdEmpleado': UsuarioIdEmpleado[1],
				'UsuarioActivo': UsuarioActivo,
				'Categoria': Categoria
			};
			$.ajax({
				async: true,
				type: "POST",
				url: "/api/ReportePorCategorias",
				data: aDatos,
				dataType: "json",
				beforeSend: function () {
					cargando();
				},
				success: function (result) {
					console.log(result);
					$("#tblInformesCategoria tbody").empty();

					var f = new Date();
					if (valorVacio(result) === false) {
						var totalGastado = 0;
						var TotalCategorias = [];
						var TotalDepartamento = [];
						var TotalTipoGasto = [];
						var ValCategorias = [];
						var ValDepartamento = [];
						var ValTipoGasto = [];
						$.each(result, function (key, value) {
							var total = (value.Total * 1);
							var newTr = "<tr>";
							newTr += "<td>" + value.Requisicion + "</td>";
							newTr += "<td>" + value.NombreResponsabe + "</td>";
							newTr += "<td>" + value.TipoGasto + "</td>";
							newTr += "<td>" + value.Departamento + "</td>";
							newTr += "<td>" + value.Categoria + "</td>";
							newTr += "<td>" + formatNumber.new(total.toFixed(2), "$ ") + "</td>";
							newTr += "<td>" + value.Periodo + "</td>";
							newTr += "<td>" + value.Justificacion + "</td>";
							newTr += "</tr>";
							$("#tblInformesCategoria tbody").append(newTr);
							totalGastado += total;
							ValDepartamento.push({
								"label": value.Departamento,
								"valor": total
							});
							ValTipoGasto.push({
								"label": value.TipoGasto,
								"valor": total
							});
							ValCategorias.push({
								"label": value.Categoria,
								"valor": total
							});
						});
						//console.log(ValDepartamento, ValTipoGasto, ValCategorias);

						TotalDepartamento = groupBy(ValDepartamento, 'label');
						TotalTipoGasto = groupBy(ValTipoGasto, 'label');
						TotalCategorias = groupBy(ValCategorias, 'label');
						$("#tblTotalDepartamento tbody, #tblTotalTotalTipoGasto tbody, #tblTotalCategorias tbody").empty();
						for (var item in TotalDepartamento) {
							var newTr = "<tr>";
							newTr += "<td>" + item + "</td>";
							newTr += "<td class='text-right'>" + formatNumber.new(TotalDepartamento[item].valor.toFixed(2), "$ ") + "</td>";
							newTr += "</tr>";
							$("#tblTotalDepartamento tbody").append(newTr);
						}
						for (var item in TotalTipoGasto) {
							var newTr = "<tr>";
							newTr += "<td>" + item + "</td>";
							newTr += "<td class='text-right'>" + formatNumber.new(TotalTipoGasto[item].valor.toFixed(2), "$ ") + "</td>";
							newTr += "</tr>";
							$("#tblTotalTipoGasto tbody").append(newTr);
						}
						for (var item in TotalCategorias) {
							var newTr = "<tr>";
							newTr += "<td>" + item + "</td>";
							newTr += "<td class='text-right'>" + formatNumber.new(TotalCategorias[item].valor.toFixed(2), "$ ") + "</td>";
							newTr += "</tr>";
							$("#tblTotalCategorias tbody").append(newTr);
						}

						$("#totalGastado").empty().append(formatNumber.new(totalGastado.toFixed(2), "$ "));
					} else {
						$.notify("Sin información.", { globalPosition: 'top center', className: 'error' });
					}
				},
				complete: function () {
					cargado();
					try {
						$("#tblInformesCategoria tbody tr").each(function () {
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
		var groupBy = function (miarray, prop) {
			return miarray.reduce(function (groups, item) {
				var val = item[prop];
				groups[val] = groups[val] || { label: item.label, valor: 0 };
				groups[val].valor += item.valor;
				return groups;
			}, {});
		}
	</script>

</asp:Content>
