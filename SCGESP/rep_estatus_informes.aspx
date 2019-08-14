<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="rep_estatus_informes.aspx.cs" Inherits="SCGESP.rep_informes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<section class="content">
		<div class="panel panel-primary">
			<div class="panel-heading">
				Reporte Estatus Informes
        <a href="#" onclick='cerrarPanel(".panel")' style="color:#FFF;border-left:1px solid #FFF "><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">
				<div class="table-responsive">
				<table class="filtro">
					<tr>
						<td style="text-align:center; padding: 10px 20px">
							Fechas:
						</td>
						<td>
							<select id="tipoFecha" name="tipoFecha" class="select2" data-width="230px">
								<option value="*">--- Todas ---</option>
								<option value="periodo">Periodo de Informe</option>
								<option value="registro">Registro de Informe</option>
							</select>
						</td>
						<td style="text-align:center; padding: 10px 20px">
							De:
						</td>
						<td>
							<div class='input-group date' id='datetimepicker1' style="width:140px">
								<input class="form-control reporte2" readonly="readonly" name="repde" id="repde" type="text" style="text-align:center;" />
								<i class="form-group__bar"></i>
								<span class="input-group-addon" style="width:50px">
									<span class="zmdi zmdi-calendar zmdi-hc-2x"></span>
								</span>
							</div>
						</td>
						<td style="text-align:center; padding: 10px 20px">
							A:
						</td>
						<td>
								<div class='input-group date' id='datetimepicker2' style="width:140px">
									<input class="form-control reporte2" readonly="readonly" name="repa" id="repa" type="text" style="text-align:center;" />
									<i class="form-group__bar"></i>
									<span class="input-group-addon" style="width:50px">
										<span class="zmdi zmdi-calendar zmdi-hc-2x"></span>
									</span>
								</div>
						</td>
						<td rowspan="2" style="padding-left:10px">
							<button id="btn-consultar" type="button" class="btn btn-primary"><span class="glyphicon glyphicon-search"></span> Consultar</button>
						</td>
					</tr>
					<tr>
						<td style="text-align:center; padding: 10px 20px">
							Estatus:
						</td>
						<td>
							<select id="estatus" name="estatus" class="select2" data-width="230px">
								<option value="*">--- Todos ---</option>
							</select>							
						</td>
						<td style="text-align:center; padding: 10px 20px" colspan="2">
							Responsable:
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
                        <tr style="text-align:center;">
                            <th width="50px" style="text-align:center;">Requisici&oacute;n</th>
                            <th width="40px" style="text-align:center;">Informe</th>
							<th width="40px" style="text-align:center;">Del</th>
							<th width="40px" style="text-align:center;">Al</th>
                            <th width="250px">Justificaci&oacute;n</th>
                            <th width="100px" style="text-align:center;">Importe<br /> Autorizado</th>
                            <th width="100px" style="text-align:center;">Responsable</th>
							<th width="100px" style="text-align:center;">Estatus</th>
							<th width="100px" style="text-align:center;">Usuario Actual</th>
							<th width="40px" style="text-align:center;">Recibido</th>
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

					if (valorVacio(result) === false) {
						$.each(result, function (key, value) {
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
								value.a_fsolicitud
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

	</script>


</asp:Content>
