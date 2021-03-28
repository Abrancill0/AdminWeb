using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Ele.Generales;
using Newtonsoft.Json.Linq;

namespace SCGESP.Controllers
{
    public class AltaAlternoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string RmUaaUsuario { get; set; }
            public string RmUaaUsuarioAlterno { get; set; }
            public string RmUaaFechaInicial { get; set; }
            public string RmUaaFechaFinal { get; set; }
            public string RmUaaSoloComprobaciones { get; set; }
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.RmUaaUsuario,
                Origen = "AdminAPP",
                Transaccion = 120795,
                Operacion = 2,
            };

            entrada.agregaElemento("RmUaaUsuario", Datos.RmUaaUsuario);
            entrada.agregaElemento("RmUaaUsuarioAlterno", Datos.RmUaaUsuarioAlterno);
            entrada.agregaElemento("RmUaaFechaInicial", Datos.RmUaaFechaInicial);
            entrada.agregaElemento("RmUaaFechaFinal", Datos.RmUaaFechaFinal);
            entrada.agregaElemento("RmUaaSoloComprobaciones", Datos.RmUaaSoloComprobaciones);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
              
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
