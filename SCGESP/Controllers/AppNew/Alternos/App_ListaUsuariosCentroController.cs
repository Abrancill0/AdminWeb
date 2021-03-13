using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System.Data;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace SCGESP.Controllers.AppNew
{
    public class App_ListaUsuariosCentroController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Empleado { get; set; }
        }

        public class ParametrosSalidaResult
        {
            public string Empleado { get; set; }
            public string Nombre { get; set; }
            public string Usuario { get; set; }

        }

        public JObject Post(Datos Datos)
        {
            try
            {
               
                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = Datos.Usuario,
                    Origen = "AdminApp",
                    Transaccion = 120037,
                    Operacion = 17
                };

                entrada.agregaElemento("GrEmpId", Datos.Empleado);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Empleados");

                    List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        ParametrosSalidaResult ent = new ParametrosSalidaResult
                        {
                            Empleado = Convert.ToString(row["Empleado"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            Usuario = Convert.ToString(row["Usuario"]),
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
                    CambiaContrasena = false,
                });


                return Resultado;
            }


        }

        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp
            {
                Timeout = -1
            };
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}
