using Ele.Generales;
using Newtonsoft.Json.Linq;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace SCGESP.Controllers.AppNew
{
    public class App_ConsultaDocumentosRequisicionController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string RmRdoRequisicion { get; set; }
        }

        public class RequisicionDetalleResult
        {
            public int RmRdoRequisicion { get; set; }
            public int RmRdoTipoDocumento { get; set; }
            public string RmRdoTipoDocumentoNombre { get; set; }
            public string RmRdoArchivo { get; set; }
        }

        public JObject Post(datos Datos)
        {
            try
            {

                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = Datos.Usuario;
                entrada.Origen = "AdminApp";  //Datos.Origen; 
                entrada.Transaccion = 120859;
                entrada.Operacion = 1;

                entrada.agregaElemento("RmRdoRequisicion", Datos.RmRdoRequisicion);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    List<RequisicionDetalleResult> lista = new List<RequisicionDetalleResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        RequisicionDetalleResult ent = new RequisicionDetalleResult
                        {
                            RmRdoRequisicion = Convert.ToInt32(row["RmRdoRequisicion"]),
                            RmRdoTipoDocumento = Convert.ToInt32(row["RmRdoTipoDocumento"]),
                            RmRdoTipoDocumentoNombre = Convert.ToString(row["RmRdoTipoDocumentoNombre"]),
                            RmRdoArchivo = Convert.ToString(row["RmRdoArchivo"])
                        };
                        lista.Add(ent);
                    }


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


                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Errores = Salida.Element("Errores");
                    XElement Error = Errores.Element("Error");
                    XElement Descripcion = Error.Element("Descripcion");
                    

                    string resultado2 = respuesta.Errores.InnerText;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Descripcion.Value,
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


