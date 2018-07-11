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
    public class CargarExcelBancoClosedXMLController : ApiController
    {
        public class ListResult
        {
            public bool ArchivoOk { get; set; }
            public string Descripcion { get; set; }
            public string Embosado { get; set; }
            public string Nombre { get; set; }
            public string Nomina { get; set; }
            public IEnumerable<ListExcelResult> RowExcel { get; set; }
        }
        public class ListArchivoResult
        {
            public bool ArchivoOk { get; set; }
            public string Archivo { get; set; }
            public string Descripcion { get; set; }
        }
        public class ListExcelResult
        {
            public string Tarjeta { get; set; }
            public string Tipo { get; set; }
            public string Fecha { get; set; }
            public string Descripcion { get; set; }
            public double Importe { get; set; }
        }
        public class Parametros
        {
            public string Usuario { get; set; }
            public string Archivo { get; set; }
            public string ArchivoNmb { get; set; }
            public string ArchivoExt { get; set; }
        }
        public IEnumerable<ListResult> PostObtieneDatos(Parametros Datos)
        {
            IEnumerable<ListArchivoResult> CargaExcel;
            CargaExcel = PostSaveArchivo(Datos.Archivo, Datos.ArchivoNmb, Datos.ArchivoExt);

            foreach (var item in CargaExcel)
            {
                if (item.ArchivoOk == true)
                {
                    return LeerExcel(item.Archivo, item.ArchivoOk, item.Descripcion);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public IEnumerable<ListResult> LeerExcel(string archivo, bool archivook, string descripcion)
        {

            //Save the uploaded Excel file.
            string filePath = archivo;
            List<ListResult> respuesta = null;
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Loop through the Worksheet rows.
                int i = 1;
                bool movOk = false;
                string RowEmbosado = "", RowNombre = "", RowNomina = "";
                List<ListExcelResult> RowsExcel = new List<ListExcelResult>();
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (i == 3 && RowEmbosado == "")
                        RowEmbosado = row.Cell("B").GetString();
                    if (i == 4 && RowNombre == "")
                        RowNombre = row.Cell("B").GetString();
                    if (i == 5 && RowNomina == "")
                        RowNomina = row.Cell("B").GetString();

                    if (i > 8)
                    {
                        try
                        {
                            string RowTarjeta = "";
                            RowTarjeta = row.Cell("A").GetString().Replace("'", "");
                            if (RowTarjeta != "")
                            {
                                var ImporteOk = row.Cell("E").Value.ToString();
                                if (ImporteOk != "") {
                                    double RowImporte = Convert.ToDouble(ImporteOk);
                                    if (RowImporte > 0)
                                    {
                                        string RowDescripcion = row.Cell("D").GetString();
                                        if (RowDescripcion.Trim() != "MOV.REVERSION RECARGA EFECTIVO" &&
                                            RowDescripcion.Trim() != "")
                                        {
                                            string RowTipo = row.Cell("B").GetString();
                                            string RowFecha = row.Cell("C").GetString();//.ToString("dd/MM/yyyy");
                                            ListExcelResult ColsExcel = new ListExcelResult
                                            {
                                                Tarjeta = RowTarjeta,
                                                Tipo = RowTipo,
                                                Fecha = RowFecha,
                                                Descripcion = RowDescripcion,
                                                Importe = RowImporte
                                            };
                                            RowsExcel.Add(ColsExcel);
                                            movOk = true;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            ListExcelResult ColsExcel = new ListExcelResult
                            {
                                Tarjeta = "Error",
                                Tipo = "Row",
                                Fecha = null,
                                Descripcion = err.ToString(),
                                Importe = 0
                            };
                            RowsExcel.Add(ColsExcel);
                        }
                    }

                    i++;
                }

                bool ArcOk = movOk == true ? archivook : movOk;
                ListResult valores = new ListResult
                {
                    ArchivoOk = ArcOk,
                    Descripcion = descripcion,
                    Embosado = RowEmbosado,
                    Nombre = RowNombre,
                    Nomina = RowNomina,
                    RowExcel = RowsExcel
                };

                respuesta = new List<ListResult>
                {
                    valores
                };
            }
            return respuesta;
        }
        public IEnumerable<ListArchivoResult> PostSaveArchivo(string Based64BinaryString, string ArchivoNmb, string ArchivoExt)
        {
            List<ListArchivoResult> resultado = new List<ListArchivoResult>();
            try
            {
                if (ArchivoExt == "xlsx")
                {
                    string path = HttpContext.Current.Server.MapPath("/");
                    path = path + "temp";
                    string name = ArchivoNmb + "_" + DateTime.Now.ToString("hhmmss");

                    string str = Based64BinaryString.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", " ");//Excel check

                    byte[] data = Convert.FromBase64String(str);

                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + "\\" + name + "." + ArchivoExt;
                    File.WriteAllBytes(rutacompleta, data);

                    ListArchivoResult archivoOK = new ListArchivoResult
                    {
                        ArchivoOk = true,
                        Archivo = rutacompleta,
                        Descripcion = "El Archivo se Cargo Correctamente."
                    };
                    resultado.Add(archivoOK);
                }
                else
                {
                    ListArchivoResult archivoOK = new ListArchivoResult
                    {
                        ArchivoOk = false,
                        Archivo = "",
                        Descripcion = "Error formato no valido."
                    };
                    resultado.Add(archivoOK);
                }

            }
            catch (Exception ex)
            {
                ListArchivoResult archivoOK = new ListArchivoResult
                {
                    ArchivoOk = false,
                    Archivo = "",
                    Descripcion = "Error al cargar Archivo. " + Convert.ToString(ex)
                };
                resultado.Add(archivoOK);
            }
            return resultado;
        }

        public static void DeleteArchivo(string archivo)
        {
            try
            {
                string curFile = @archivo;
                File.Delete(curFile);
            }
            catch (Exception ex)
            {
                var error = ex;
            }

        }
    }
}
