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
    public class App_DetalleCambioPresupuestoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPdeAnio { get; set; }
            public string PrPdeFolio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PrPdeFolio { get; set; } //Folio del Cambio
            public string PrPdeConsecutivo { get; set; } // Consecutivo
            public string PrPdeCentroNombre { get; set; } //Departameneto
            public string PrPdeOficinaNombre { get; set; } //Oficina
            public string PrPdeSubramoNombre { get; set; } //Subramo
            public string PrPdeCuentaNombre { get; set; } //Cuenta
            public string PrPdeCuenta { get; set; }
            public string PrPdeAfectacion { get; set; } //tipo de afectación( 1= aumenta, -1 = desminuye)
            public string PrPdeValorTotal { get; set; } //Total del movimiento en el año(Te muestra además una columna por cada mes pero yo creo que con sólo mostrar la anual es suficiente)
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120625,
                Operacion = 1,
            };
            
            entrada.agregaElemento("PrPdeAnio", Datos.PrPdeAnio);
            entrada.agregaElemento("PrPdeFolio", Datos.PrPdeFolio);
           
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
                        PrPdeFolio = Convert.ToString(row["PrPdeFolio"]),
                        PrPdeConsecutivo = Convert.ToString(row["PrPdeConsecutivo"]),
                        PrPdeCentroNombre = Convert.ToString(row["PrPdeCentroNombre"]),
                        PrPdeOficinaNombre = Convert.ToString(row["PrPdeOficinaNombre"]),
                        PrPdeSubramoNombre = Convert.ToString(row["PrPdeSubramoNombre"]),
                        PrPdeCuentaNombre = Convert.ToString(row["PrPdeCuentaNombre"]),
                        PrPdeCuenta = Convert.ToString(row["PrPdeCuenta"]),
                        PrPdeAfectacion = Convert.ToString(row["PrPdeAfectacion"]),
                        PrPdeValorTotal = Convert.ToString(row["PrPdeValorTotal"]),  
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
