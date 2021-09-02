using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using SCGESP.Clases;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System;

namespace SCGESP.Controllers
{
    public class App_RechazaOCController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string RmOcoId { get; set; }
            public string RmOcoRequisicion { get; set; }
            public string RmOcoComentario { get; set; }
        }



        public JObject Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "AdminApp",  //Datos.Origen; 
                Transaccion = 120768,
                Operacion = 14 //rechazar requisiciones
            };
            entrada.agregaElemento("RmOcoId", Datos.RmOcoId);
            entrada.agregaElemento("RmOcoComentarios", Datos.RmOcoComentario);
            entrada.agregaElemento("RmOcoRequisicion", Datos.RmOcoRequisicion);
            

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

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


