<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="reportes.aspx.cs" Inherits="SCGESP.reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<section class="content">
		<div class="panel panel-primary">
			<div class="panel-heading">
				Reportes
        <a href="#" onclick='cerrarPanel(".panel")' style="color: #FFF; border-left: 1px solid #FFF"><i class="zmdi zmdi-close"></i>Cerrar</a>
			</div>
			<div class="panel-body">

				<div class="row m-3">
					<div class="col col-12 col-md-12">
						<div class="row">
							<div class="col col-md-6">
								<a href="/rep_estatus_informes">
									<div class="card card-info rounded m-3">
										<div class="card-header border">
											<div class="card-title text-center h3 bold" style="color: white !important;">
												Reporte estatus informes
											</div>
										</div>
										<div class="card-body text-center h1" style="color: white !important;">
											<i class='zmdi zmdi-format-list-bulleted' style="font-size: 120px"></i>
										</div>
									</div>
								</a>
							</div>
							<div class="col col-md-6">
								<a href="/rep_por_categorias">
								<div class="card card-info rounded m-3">
									<div class="card-header border">
										<div class="card-title text-center h3 bold" style="color: white !important;">
											Reporte por categorías
										</div>
									</div>
									<div class="card-body text-center h1" style="color: white !important;">
										<i class='zmdi zmdi-format-indent-increase' style="font-size: 120px"></i>
									</div>
								</div>
									</a>
							</div>
						</div>

						<div class="row">
							<div class="col col-md-12 w-100">
								<a href="/rep_desglose_estatus_informes">
								<div class="card card-info rounded mt-3 w-50" style="margin: 0 auto;">
									<div class="card-header border">
										<div class="card-title text-center h3 bold" style="color: white !important;">
											Reporte desglose de estatus por informe
										</div>
									</div>
									<div class="card-body text-center h1" style="color: white !important;">
										<i class='zmdi zmdi-sort-amount-asc' style="font-size: 120px"></i>
									</div>
								</div>
									</a>
							</div>
						</div>
					</div>


				</div>
			</div>
		</div>
	</section>

</asp:Content>
