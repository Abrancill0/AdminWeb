using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class EliminaExcelController : ApiController
    {
        public class Parametros
        {
            public string RutaExcel { get; set; }
        }
        public string PostEliminar(Parametros Datos)
        {
            string resultado = "";
            try
            {
                string curFile = Datos.RutaExcel;
                File.Delete(curFile);
                resultado = "Eliminado";
            }
            catch (Exception ex)
            {
                var error = ex;
                resultado = "No Eliminado: " + error.ToString();
            }
            return resultado;
        }
    }    
}
