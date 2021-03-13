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
    public class App_SolicitudesCambioCentroAutorizarController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string FiCscSolicitud { get; set; } //No.Solicitud
            public string FiCscTipo { get; set; } //Tipo Solicitud (Alta, Cambio)
            public string FiCscEstatus { get; set; } //Estatus de la solicitud
            public string FiCscEstatusNombre { get; set; } //nombre del estatus de la solicitud
            public string FiCscSolicitante { get; set; } //Solicitante de alta/cambio
            public string FiCscSolicitanteNombre { get; set; }  //nombre del solicitante
            public string FiCscCentro { get; set; } //departamento/area/centro
            public string FiCscNombre { get; set; } //nombre del departamento/area/centro
            public string FiCscResponsable { get; set; } //responsible
            public string FiCscResponsableNombre { get; set; } //nombre del responsible
            public string FiCscMontoMinimo { get; set; } //monto mínimo
            public string FiCscMontoMaximo { get; set; }//monto máximo
            public string FiCscEstatusSiguiente { get; set; }//estatus siguiente de la solicitud
            public string FiCscEstatusSiguienteNombre { get; set; } //nombre del estatus siguiente de la solicitud
            public string FiCscEmpleadoObligado { get; set; }//empleado obligado a autorizar
            public string FiCscEmpleadoObligadoNombre { get; set; }//nombre del empleado obligado a autorizar
            public string FICenResponsable { get; set; }//responsible actual
            public string FiCenResponsableNombre { get; set; }//responsible actual
            public string FiCenMontoMinimo { get; set; }//monto mínimo actual
            public string FiCenMontoMaximo { get; set; } //monto máximo actual
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120402,
                Operacion = 1,
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
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {
                            FiCscSolicitud = Convert.ToString(row["FiCscSolicitud"]),
                            FiCscTipo = Convert.ToString(row["FiCscTipo"]),
                            FiCscEstatus = Convert.ToString(row["FiCscEstatus"]),
                            FiCscEstatusNombre = Convert.ToString(row["FiCscEstatusNombre"]),
                            FiCscSolicitante = Convert.ToString(row["FiCscSolicitante"]),
                            FiCscSolicitanteNombre = Convert.ToString(row["FiCscSolicitanteNombre"]),
                            FiCscCentro = Convert.ToString(row["FiCscCentro"]),
                            FiCscNombre = Convert.ToString(row["FiCscNombre"]),
                            FiCscResponsable = Convert.ToString(row["FiCscResponsable"]),
                            FiCscResponsableNombre = Convert.ToString(row["FiCscResponsableNombre"]),
                            FiCscMontoMinimo = string.IsNullOrEmpty(Convert.ToString(row["FiCscMontoMinimo"])) ? "0" : Convert.ToString(row["FiCscMontoMinimo"]),
                            FiCscMontoMaximo = string.IsNullOrEmpty(Convert.ToString(row["FiCscMontoMaximo"])) ? "0" : Convert.ToString(row["FiCscMontoMaximo"]),
                            FiCscEstatusSiguiente = Convert.ToString(row["FiCscEstatusSiguiente"]),
                            FiCscEstatusSiguienteNombre = Convert.ToString(row["FiCscEstatusSiguienteNombre"]),
                            FiCenResponsableNombre = string.IsNullOrEmpty(Convert.ToString(row["FiCenResponsableNombre"])) ? "N/A" : Convert.ToString(row["FiCenResponsableNombre"]),
                            // FiCscEmpleadoObligadoNombre = Convert.ToString(row["FiCscEmpleadoObligadoNombre"]),
                            FICenResponsable = Convert.ToString(row["FICenResponsable"]),
                            FiCenMontoMinimo = string.IsNullOrEmpty(Convert.ToString(row["FiCenMontoMinimo"])) ? "0" : Convert.ToString(row["FiCenMontoMinimo"]),
                            FiCenMontoMaximo = string.IsNullOrEmpty(Convert.ToString(row["FiCenMontoMaximo"])) ? "0" : Convert.ToString(row["FiCenMontoMaximo"]),

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
