using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Web;
using System.IO;

namespace SCGESP.Controllers.AppNew
{
    public class App_OCPPDFV3Controller : ApiController
    {
       
        public class datos
        {
          
            public string Usuario { get; set; }
            public string PrPtiAnio { get; set; }
            public string PrPtiFolio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PDF { get; set; } //PDF
        }

        public JObject Post(datos Datos)
        {
            try
            {
                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = Datos.Usuario,
                    Origen = "AdminAPP",
                    Transaccion = 120623,
                    Operacion = 22,
                };

                entrada.agregaElemento("PrPtiAnio", Datos.PrPtiAnio);
                entrada.agregaElemento("PrPtiFolio", Datos.PrPtiFolio);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTLista = new DataTable();

                string Mensaje = "";
                int Estatus = 0;

                if (respuesta.Resultado == "1")//&& idUsuario.Trim() != ""
                {


                    DataTable DT = new DataTable();

                    string cosa = respuesta.obtieneValor("ArchivoPdf");
                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    string result = "";
                    string format = ".pdf";
                    string path = HttpContext.Current.Server.MapPath("/PDF/SolicitudCambio/");
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss");

                    byte[] data = Convert.FromBase64String(cosa);


                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + name + format;
                    File.WriteAllBytes(rutacompleta, data);
                    result = "PDF/SolicitudCambio/" + name + format;


                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        PDF = result
                    };
                    lista.Add(ent);

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "OK",
                        estatus = 1,
                        Result = lista

                    });



                    return Resultado;

                }
                else
                {

                    //  < Error >< Concepto > SgUsuClaveAcceso </ Concepto >< Descripcion > CONTRASEÑA INVÁLIDA </ Descripcion ></ Error >
                    Mensaje = respuesta.Errores.InnerText;
                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Errores = Salida.Element("Errores");
                    XElement Error = Errores.Element("Error");
                    XElement Descripcion = Error.Element("Descripcion");
                    Estatus = 0;

                    string resultado2 = respuesta.Errores.InnerText;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Descripcion.Value,
                        estatus = Estatus,
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

