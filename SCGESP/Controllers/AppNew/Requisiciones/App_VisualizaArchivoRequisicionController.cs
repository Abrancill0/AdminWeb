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
using Newtonsoft.Json.Linq;

namespace SCGESP.Controllers.AppNew
{
    public class App_VisualizaArchivoRequisicionController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string RmRdoRequisicion { get; set; }
            public string  RmRdoTipoDocumento { get; set; }
            public string ExtencionDocumentos { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PDF { get; set; } //PDF
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = Datos.Usuario;
            entrada.Origen = "AdminApp";  //Datos.Origen; 
            entrada.Transaccion = 120859;
            entrada.Operacion = 16;

            entrada.agregaElemento("RmRdoRequisicion", Datos.RmRdoRequisicion);
            entrada.agregaElemento("RmRdoTipoDocumento", Datos.RmRdoTipoDocumento);
           
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
  
            try
            {

                if (respuesta.Resultado == "1")
            {
                   
                   // string Archivo = respuesta.obtieneValor("NombreArchivo");


                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Tablas = Salida.Element("Tablas");
                    XElement ConsultaAdicional1 = Tablas.Element("ConsultaAdicional1");
                    XElement NewDataSet = ConsultaAdicional1.Element("NewDataSet");
                    XElement ConsultaAdicional11 = NewDataSet.Element("ConsultaAdicional1");
                    XElement NombreArchivo = ConsultaAdicional11.Element("NombreArchivo");
                    XElement Archivo = ConsultaAdicional11.Element("Archivo");


                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    string result = "";
                    string format = Datos.ExtencionDocumentos;
                    string path = HttpContext.Current.Server.MapPath("/PDF/ArchivosComunes/");
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss");

                    byte[] data = Convert.FromBase64String(Archivo.Value);

                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + name + format;
                    File.WriteAllBytes(rutacompleta, data);
                    result = "PDF/ArchivosComunes/" + name + format;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "OK",
                        estatus = 1,
                        PDF = result

                    });

                    return Resultado;

            }
            else
            {
                
                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "No se encontro informacion",
                        estatus = 0,
                      
                    });

                    return Resultado;
            }

            }
            catch (Exception ex)
            {
              JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 0,
                });

                return Resultado;
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
