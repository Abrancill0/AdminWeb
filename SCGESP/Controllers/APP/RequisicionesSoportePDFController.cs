using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class RequisicionesSoportePDFController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string RmRdoRequisicion { get; set; }
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
                Transaccion = 120859,
                Operacion = 16
            };

//            elementos: "RmRdoRequisicion".- número de requisición
//        "RmRdoTipoDocumento".- 53(fijo)
//te regresa una tabla de un renglón que tiene:
//            una columna que se llama "Archivo"(string en formato base 64)
//    una columna que se llama "NombreArchivo"(string con el nombre del archivo)
//para generar el archivo yo uso lo siguiente:

            entrada.agregaElemento("RmRdoTipoDocumento",53);
            entrada.agregaElemento("RmRdoRequisicion", Datos.RmRdoRequisicion);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
  
            try
            {

                if (respuesta.Resultado == "1")
            {
                
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    string result = "";
                    string format = ".pdf";
                    string path = HttpContext.Current.Server.MapPath("/PDF/AutorizaTraspasos/");
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss");


                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Tablas = Salida.Element("Tablas");
                    XElement ConsultaAdicional = Tablas.Element("ConsultaAdicional1");
                    XElement NewDataSet = ConsultaAdicional.Element("NewDataSet");
                    XElement ConsultaAdicional1 = NewDataSet.Element("ConsultaAdicional1");
                    XElement Archivo = ConsultaAdicional1.Element("Archivo");


                    byte[] data = Convert.FromBase64String(Archivo.Value);

                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + name + format;
                    File.WriteAllBytes(rutacompleta, data);
                    result = "PDF/AutorizaTraspasos/" + name + format;

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
