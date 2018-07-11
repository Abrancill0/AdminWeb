using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class CargarExcelBancoController : ApiController
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
            public decimal Importe { get; set; }
        }
        public class ParametrosExcelBanco
        {
            public string Usuario { get; set; }
            public string Archivo { get; set; }
            public string ArchivoNmb { get; set; }
            public string ArchivoExt { get; set; }
        }
        public IEnumerable<ListResult> PostObtieneDatos(ParametrosExcelBanco Datos)
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
            try
            {
                Application xlApp;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                Range range;
                var misValue = Type.Missing;//System.Reflection.Missing.Value;

                // abrir el documento
                xlApp = new Application();
                xlWorkBook = xlApp.Workbooks.Open(archivo, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue);

                // seleccion de la hoja de calculo
                // get_item() devuelve object y numera las hojas a partir de 1
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // seleccion rango activo
                range = xlWorkSheet.UsedRange;

                // leer las celdas
                int rows = range.Rows.Count;
                int cols = range.Columns.Count;
                string RowEmbosado, RowNombre, RowNomina;
                RowEmbosado = (range.Cells[3, 2] as Range).Value2.ToString();
                RowNombre = (range.Cells[4, 2] as Range).Value2.ToString();
                RowNomina = (range.Cells[5, 2] as Range).Value2.ToString();
                bool iniciaRow = false, movOk = false;
                //CultureInfo ci = new CultureInfo("es-MX");
                List<ListExcelResult> RowsExcel = new List<ListExcelResult>();
                for (int row = 2; row <= rows; row++)
                {
                    string RowTarjeta = "";
                    //try
                    //{
                        RowTarjeta = (range.Cells[row, 1] as Range).Value2.ToString().Replace("'", "");
                        if (iniciaRow == true && RowTarjeta != "")
                        {
                            var ImporteOk = (range.Cells[row, 5] as Range).Value2;
                            if (ImporteOk != null)
                            {
                                var RowImporte = (range.Cells[row, 5] as Range).Value2.ToString();
                                RowImporte = Convert.ToDecimal(RowImporte);
                                if (RowImporte > 0)
                                {
                                    string RowDescripcion = (range.Cells[row, 4] as Range).Value2.ToString();
                                    if (RowDescripcion.Trim() != "MOV.REVERSION RECARGA EFECTIVO" &&
                                        RowDescripcion.Trim() != "")
                                    {
                                        var RowTipo = (range.Cells[row, 2] as Range).Value2.ToString();
                                        string FechaExcel = (range.Cells[row, 3] as Range).Value2.ToString();
                                        double date = double.Parse(FechaExcel);
                                        string RowFecha = DateTime.FromOADate(date).AddDays(-1).ToString("dd/MM/yyyy");
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
                        else if (RowTarjeta.Trim() == "TARJETA")
                        {
                            iniciaRow = true;
                        }
                    //}
                    //catch (Exception err)
                    //{
                    //    string error = err.ToString();
                    //}
                }
                // cerrar
                xlWorkBook.Close(false, misValue, misValue);
                xlApp.Quit();
                // liberar
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

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

                List<ListResult> respuesta = new List<ListResult>
            {
                valores
            };

                DeleteArchivo(archivo);//eliminar el archivo

                return respuesta;
            }
            catch (Exception err) {
                ListResult valores = new ListResult
                {
                    ArchivoOk = false,
                    Descripcion = err.ToString(),
                    Embosado = "",
                    Nombre = "",
                    Nomina = "",
                    RowExcel = null
                };
                List<ListResult> respuesta = new List<ListResult>
            {
                valores
            };
                return respuesta;
            }
        }

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to release the object(object:{0})", obj.ToString());
            }
            finally
            {
                obj = null;
                GC.Collect();
            }
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
