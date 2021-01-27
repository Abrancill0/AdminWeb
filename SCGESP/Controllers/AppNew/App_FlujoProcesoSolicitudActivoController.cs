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
using SCGESP.Clases;

namespace SCGESP.Controllers.AppNew
{
    public class App_FlujoProcesoSolicitudActivoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string AfVrfValeResguardo { get; set; }
        }
        //Parametros Salida
        public class ObtieneParametrosSalida
        {

            public string AfVrfValeResguardo { get; set; }
            public string AfVrfOrden { get; set; }
            public string AfVrfEstatus { get; set; }
            public string AfVrfEstatusNombre { get; set; }
            public string AfVrfResponsable { get; set; }
            public string AfVrfResponsableNombre { get; set; }
            public string AfVrfAplica { get; set; }
            public string AfVrfComentario { get; set; }
            public string AfVrfFolioEstatus { get; set; }
            public string AfVrfProceso { get; set; }
        }
        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        //public XmlDocument Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120995,
                Operacion = 1,
            };

            entrada.agregaElemento("AfVrfValeResguardo", Datos.AfVrfValeResguardo);
            // entrada.agregaElemento("estatus", 2);

            DataTable DTListaAdministrativos = new DataTable();

            try
            {
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    DTListaAdministrativos = respuesta.obtieneTabla("Catalogo");

                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    foreach (DataRow row in DTListaAdministrativos.Rows)
                    {
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {

                            AfVrfValeResguardo = Convert.ToString(row["AfVrfValeResguardo"]),
                            AfVrfOrden = Convert.ToString(row["AfVrfOrden"]),
                            AfVrfEstatus = Convert.ToString(row["AfVrfEstatus"]),
                            AfVrfEstatusNombre = Convert.ToString(row["AfVrfEstatusNombre"]),
                            AfVrfResponsable = Convert.ToString(row["AfVrfResponsable"]),
                            AfVrfResponsableNombre = Convert.ToString(row["AfVrfResponsableNombre"]),
                            AfVrfAplica = Convert.ToString(row["AfVrfAplica"]),
                            AfVrfComentario = Convert.ToString(row["AfVrfComentario"]),
                            AfVrfFolioEstatus = Convert.ToString(row["AfVrfFolioEstatus"]),
                            AfVrfProceso = Convert.ToString(row["AfVrfProceso"]),
                         
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