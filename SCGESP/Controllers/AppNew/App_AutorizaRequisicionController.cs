using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace SCGESP.Controllers
{
    public class App_AutorizaRequisicionController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string RmReqId { get; set; }
            public string RmReqEstatus { get; set; }
        }

        public class AutorizaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public JObject Post(datos Datos)
        {

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120760,
                Operacion = 10 //autorizar requisiciones
            };
            entrada.agregaElemento("RmReqId", Datos.RmReqId);
            entrada.agregaElemento("RmReqEstatus", Datos.RmReqEstatus);
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                //< Salida >< Resultado > 0 </ Resultado >< Valores />< Tablas />< Errores >< Error >< Concepto > RmReqId </ Concepto >< Descripcion > Usuario mdribarra no es alterno del usuario obligado imartinez, no puede autorizar</ Descripcion ></ Error ></ Errores ></ Salida >

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = "OK",
                    estatus = 1
                });


                return Resultado;
            }
            else
            {
                XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                XElement Salida = doc.Element("Salida");
                XElement Errores = Salida.Element("Errores");
                XElement Error = Errores.Element("Error");
                XElement Descripcion = Error.Element("Descripcion");

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = Descripcion.Value,
                    estatus = 0
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

