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
    public class App_FlujoProcesoCambioPresupuestoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPtiAnio { get; set; }
            public string PrPtiFolio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string Proceso { get; set; }
            public string IdResponsable { get; set; }
            public string Responsable { get; set; }
            public string Terminado { get; set; }
            public string Usuario { get; set; }
            public string Importe { get; set; } 
            public string Alterno { get; set; }
           
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120623,
                Operacion = 17,
            };


            entrada.agregaElemento("PrPtiAnio", Datos.PrPtiAnio);
            entrada.agregaElemento("PrPtiFolio", Datos.PrPtiFolio);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();


            try
            {

                if (respuesta.Resultado == "1")
            {
                DTLista = respuesta.obtieneTabla("FlujoTipoPresupuesto");

                int NumOCVobo = DTLista.Rows.Count;

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                foreach (DataRow row in DTLista.Rows)
                {
                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        Proceso =Convert.ToString(row["Proceso"]),
                        IdResponsable=Convert.ToString(row["IdResponsable"]),
                        Responsable = Convert.ToString(row["Responsable"]),
                        Terminado =Convert.ToString(row["Terminado"]),
                        Usuario = Convert.ToString(row["Usuario"]),
                        Importe = Convert.ToString(row["Importe"]),
                        Alterno = Convert.ToString(row["Alterno"]),
                      
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
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }
    }
}
