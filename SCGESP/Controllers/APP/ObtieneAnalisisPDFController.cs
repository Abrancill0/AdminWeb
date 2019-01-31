using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class ObtieneAnalisisPDFController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string RmReqId { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PDF { get; set; } //PDF
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120760,
                Operacion = 21,
            };

            entrada.agregaElemento("RmReqId", Datos.RmReqId);
            entrada.agregaElemento("proceso", "9");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
  
            try
            {

                if (respuesta.Resultado == "1")
            {
                
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    string result = "";
                    string format = ".pdf";
                    string path = HttpContext.Current.Server.MapPath("/PDF/Analisis/");
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss");

                    byte[] data = Convert.FromBase64String(respuesta.Valores.InnerText);

                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + name + format;
                    File.WriteAllBytes(rutacompleta, data);
                    result = "PDF/Analisis/" + name + format;

                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        PDF = result
                    };
                    lista.Add(ent);
               
                return lista;
            }
            else
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    PDF = Convert.ToString("no encontro ningun registro"),
                    
                };
                lista.Add(ent);
                
                return lista;
            }

            }
            catch (Exception ex)
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {

                    PDF = ex.ToString()

                };
                lista.Add(ent);
                
                return lista;
            }

        }

        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }
    }
}
