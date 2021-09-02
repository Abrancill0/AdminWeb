using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Ele.Generales;
using Newtonsoft.Json.Linq;
using SCGESP.Clases;

namespace SCGESP.Controllers.AppNew
{
    public class App_PendientesEleController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }

        }

        public class ObtieneParametrosSalida
        {
            public string Proceso { get; set; }
            public string ProcesoNombre { get; set; }
            public string Registros { get; set; }
        }

        public class AdminWebPendientesSalida
        {
            public string Tipo { get; set; }
            public int Numero { get; set; }

        }


        public JObject Post(ParametrosEntrada Datos)
        {
            try
            {               
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = Datos.Usuario,
                    Origen = "AdminApp",  //Datos.Origen; 
                    Transaccion = 100004, //usuario
                    Operacion = 18
                };
                entrada.agregaElemento("SgUsuId", Datos.Usuario);
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTLista = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTLista = respuesta.obtieneTabla("Pendientes");

                    int NumOCVobo = DTLista.Rows.Count;

                    foreach (DataRow row in DTLista.Rows)
                    {
                        ObtieneParametrosSalida ent1 = new ObtieneParametrosSalida
                        {
                            Proceso = Convert.ToString(row["Proceso"]),
                            ProcesoNombre = Convert.ToString(row["ProcesoNombre"]),
                            Registros = Convert.ToString(row["Registros"]),

                        };
                        lista.Add(ent1);
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
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }
    }
}
