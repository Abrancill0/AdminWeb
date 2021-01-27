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
    public class App_TraspasosAutorizarController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PrTraId { get; set; } //No.Traspaso
            public string PrTraEstatus { get; set; } //Estatus del Traspaso
            public string PrTraFecha { get; set; } //Fecha del Traspaso
            public string PrTraReferencia { get; set; } //Referencia del Traspaso
            public string PrTraComentario { get; set; } //Comentarios del Traspaso
            public string PrTraTotal { get; set; } // Total
            public string PrTraEstatusNombre { get; set; }
            public string PrTraEstatusSiguienteNombre { get; set; }
            public string MesesFuturos { get; set; }

        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120697,
                Operacion = 16,
            };

            entrada.agregaElemento("proceso", "2");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();

            try
            {

                if (respuesta.Resultado == "1")
                {
                    DTLista = respuesta.obtieneTabla("Catalogo");

                    int NumOCVobo = DTLista.Rows.Count;

                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    foreach (DataRow row in DTLista.Rows)
                    {

                        string mesesfuturos = "";

                        try
                        {
                            mesesfuturos = Convert.ToString(row["MesesFuturos"]);
                        }
                        catch (Exception ex)
                        {

                        }
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {
                            PrTraId = Convert.ToString(row["PrTraId"]),
                            PrTraEstatus = Convert.ToString(row["PrTraEstatus"]),
                            PrTraFecha = Convert.ToString(row["PrTraFecha"]),
                            PrTraReferencia = Convert.ToString(row["PrTraReferencia"]),
                            PrTraComentario = Convert.ToString(row["PrTraComentario"]),
                            PrTraEstatusNombre = Convert.ToString(row["PrTraEstatusNombre"]),
                            PrTraEstatusSiguienteNombre = Convert.ToString(row["PrTraEstatusSiguienteNombre"]),
                            PrTraTotal = string.IsNullOrEmpty(Convert.ToString(row["PrTraTotal"])) ? "0" : Convert.ToString(row["PrTraTotal"]),
                            MesesFuturos = mesesfuturos

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
