using System;
using System.Collections.Generic;
using System.Data;
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
    //AutorizaTraspaso
    public class App_AutorizarTraspasoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrTraId { get; set; }
            public string PrTraEstatus { get; set; }
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120697,
                Operacion = 13,
            };

            entrada.agregaElemento("PrTraId", Datos.PrTraId);
            entrada.agregaElemento("PrTraEstatus", Datos.PrTraEstatus);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();

            try
            {

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
            catch (Exception ex)
            {

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
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
