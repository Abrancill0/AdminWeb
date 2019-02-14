using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class SolicitudCambioPresupuestoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPtiAnio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PrPtiFolio { get; set; } // Folio del Cambio
            public string PrPtiEstatus { get; set; } // Estatus del Cambio
            public string PrPtiFechaAlta { get; set; } // Fecha de alta del cambio
            public string PrPtiReferencia { get; set; } // Referencia del cambio
            public string PrPtiComentario { get; set; } // Comentarios del cambio
            public string PrPtiTotal { get; set; }
            public string PrPtiTipoNombre { get; set; }


        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120623,
                Operacion = 1,
            };

            entrada.agregaElemento("proceso", "2");
            entrada.agregaElemento("PrPtiAnio", Datos.PrPtiAnio);
 
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
                        PrPtiFolio = Convert.ToString(row["PrPtiFolio"]),
                        PrPtiEstatus = Convert.ToString(row["PrPtiEstatus"]),
                        PrPtiFechaAlta = Convert.ToString(row["PrPtiFechaAlta"]),
                        PrPtiReferencia = Convert.ToString(row["PrPtiReferencia"]),
                        PrPtiComentario = Convert.ToString(row["PrPtiComentario"]),
                        PrPtiTipoNombre = Convert.ToString(row["PrPtiTipoNombre"]),
                        PrPtiTotal = string.IsNullOrEmpty(Convert.ToString(row["PrPtiTotal"])) ? "0" : Convert.ToString(row["PrPtiTotal"])
                    };
                    lista.Add(ent);
                }
                return lista;
            }
            else
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    PrPtiComentario = Convert.ToString("no encontro ningun registro"),
                    
                };
                lista.Add(ent);
                
                return lista;
            }

            }
            catch (Exception ex)
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {

                    PrPtiComentario = ex.ToString()

                };
                lista.Add(ent);
                
                return lista;
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
