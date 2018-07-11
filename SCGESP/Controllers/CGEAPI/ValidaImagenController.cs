using SCGESP.Clases;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;
//using static SCGESP.Controllers.EleAPI.LoginEleController;

namespace SCGESP.Controllers.CGEAPI
{
    public class ValidaImagenController : ApiController
    {
        public class ListResult
        {
            public bool Existe { get; set; }
            public string Img { get; set; }
        }

        public class ParametrosRutaImg
        {
            public string Ruta { get; set; }
            public string Archivo { get; set; }
            public string ImgDefault { get; set; }
            public string Ext { get; set; }
        }

        public IEnumerable<ListResult> PostRutaImg(ParametrosRutaImg Datos)
        {
            string Archivo = Datos.Archivo.ToLower();
            string raiz = HttpContext.Current.Server.MapPath("/");
            string curFile = raiz + Datos.Ruta + Archivo;
            bool ok = File.Exists(curFile); // ? "1" : "0";
            string rutaImg = Datos.Ruta.Replace("\\", "/") + Archivo;
            string imagen = ok ? rutaImg : Datos.ImgDefault;
            List<ListResult> resultado = new List<ListResult>();

            ListResult imgOK = new ListResult
            {
                Existe = ok,
                Img = imagen
            };
            resultado.Add(imgOK);

            return resultado;
        }


    }
}
