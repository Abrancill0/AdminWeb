using ClosedXML.Excel;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class ExportarExcelController : ApiController
    {
        public class ParametrosGastoInforme
        {
            //Datos informe
            public int IdInforme { get; set; }
            public int NoInforme { get; set; }
            public string NmbSolicitante { get; set; }
            //Datos requisicion
            public string Puesto { get; set; }
            public string TipoReq { get; set; }
            public string Departamento { get; set; }
            public string Area { get; set; }
            public string Oficina { get; set; }
            public string Centro { get; set; }

        }

        public class ObtieneInformeResult
        {

            public string Fecha { get; set; } 
            public string DescripcionGasto { get; set; }
            public string Serie { get; set; }
            public string Folio { get; set; }
            public string Proveedor { get; set; }
            public string Justificacion { get; set; }
            public string Objetivo { get; set; }
            public double g_total { get; set; }
            public string nmbcomensales { get; set; }
        }
        
        public string Post(ParametrosGastoInforme Datos)
        {
            SqlCommand comando = new SqlCommand("ExcelExport");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.IdInforme;// Datos.idinforme;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Comprobacion de Gastos");

            //encabezado
            worksheet.Range("A2:H2").SetValue("Formato de Comprobación de Gastos")
                .Merge()
                .Style
                    .Font.SetFontSize(16)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.FromArgb(89, 89, 89))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Range("A4:B4").SetValue("Nombre:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            worksheet.Range("C4:D4").SetValue(Datos.NmbSolicitante)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);


            worksheet.Range("A5:B5").SetValue("Puesto:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            worksheet.Range("C5:D5").SetValue(Datos.Puesto)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("A6:B6").SetValue("Tipo De Requisición:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            worksheet.Range("C6:D6").SetValue(Datos.TipoReq)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("F1:H1").SetValue((DateTime.Now.ToString("dddd, dd De MMMM De yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"))).ToLower())
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range("E3:F3").SetValue("Folio:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Range("G3:H3").SetValue(Datos.NoInforme)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("E4:F4").SetValue("Departamento:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Range("G4:H4").SetValue(Datos.Departamento)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("E5:F5").SetValue("Area:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Range("G5:H5").SetValue(Datos.Area)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("E6:F6").SetValue("Oficina:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Range("G6:H6").SetValue(Datos.Oficina)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            worksheet.Range("E7:F7").SetValue("Centro:")
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Range("G7:H7").SetValue(Datos.Centro)
                .Merge()
                .Style
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.Black)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            //detalle
            int nFilaEncabezados = 9;
            worksheet.Cell(nFilaEncabezados, 1).Value = "Fecha";
            worksheet.Cell(nFilaEncabezados, 2).Value = "Factura";
            worksheet.Cell(nFilaEncabezados, 3).Value = "Justificacion de Gasto";
            worksheet.Cell(nFilaEncabezados, 4).Value = "Comensales";
			worksheet.Cell(nFilaEncabezados, 5).Value = "Total";
			worksheet.Cell(nFilaEncabezados, 6).Value = "XML";
			worksheet.Cell(nFilaEncabezados, 7).Value = "PDF";
			worksheet.Cell(nFilaEncabezados, 8).Value = "IMG";

			var Rango = worksheet.Range("A" + nFilaEncabezados + ":H" + nFilaEncabezados);

            Rango.Style.Fill.BackgroundColor = XLColor.FromArgb(128, 0, 0);
            Rango.Style.Font.Bold = true;
            Rango.Style.Font.FontColor = XLColor.White;

            int ContadorIni = nFilaEncabezados + 1;
            int Contador = ContadorIni;
            if (DT.Rows.Count > 0)
            {
				String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
				String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

				foreach (DataRow row in DT.Rows)
                {
                    worksheet.Cell(Contador, 1).Value = (row["Fecha"]);
                    worksheet.Cell(Contador, 2).Value = (Convert.ToString(row["Serie"]) + "-" + Convert.ToString(row["Folio"]));
                    worksheet.Cell(Contador, 3).Value = (row["Justificacion"]);
                    worksheet.Cell(Contador, 4).Value = (row["nmbcomensales"]);
                    worksheet.Cell(Contador, 5)
                        .SetValue(row["importe"])
                        .Style.NumberFormat.SetFormat("$ #,##0.#00");
					if (row["g_dirxml"].ToString() != "")
					{
						//XLHyperlink LINK = new XLHyperlink(strUrl + row["g_dirxml"].ToString(), "XML");
						//worksheet.Hyperlinks.Add(LINK)
						worksheet.Cell(Contador, 6)
							.SetFormulaA1("=HYPERLINK(\"" + strUrl + row["g_dirxml"].ToString() + "\",\"Ver\")");
					}
					if (row["g_dirpdf"].ToString() != "")
					{
						worksheet.Cell(Contador, 7)
							.SetFormulaA1("=HYPERLINK(\"" + strUrl + row["g_dirpdf"].ToString() + "\",\"Ver\")");
					}
					if (row["g_dirotros"].ToString() != "")
					{
						worksheet.Cell(Contador, 8)
							.SetFormulaA1("=HYPERLINK(\"" + strUrl + row["g_dirotros"].ToString() + "\",\"Ver\")");
					}


					Contador += 1;


                }
                
            }

            worksheet.Cell("D" + Contador).SetValue("Total");
            worksheet.Cell("E" + Contador)
                .SetFormulaA1("=SUM(E"+ ContadorIni + ":E" + (Contador - 1) + ")")
                .Style
                    .Border.SetTopBorder(XLBorderStyleValues.Medium)
                    .NumberFormat.SetFormat("$ #,##0.#00");

            worksheet.Range("D" + Contador + ":E" + Contador)
                .Style
                    .Fill.SetBackgroundColor(XLColor.FromArgb(128, 0, 0))
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.White)
                    .Font.SetFontSize(12);


            string nmbExcel = "Comprobante de Gastos (No_Inf_" + Datos.NoInforme + ").xlsx";
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string RutaCompleta = HttpContext.Current.Server.MapPath("/temp/") + nmbExcel;
            workbook.SaveAs(RutaCompleta);


            return RutaCompleta + "," + url + "," + nmbExcel;
        }
    }
}
