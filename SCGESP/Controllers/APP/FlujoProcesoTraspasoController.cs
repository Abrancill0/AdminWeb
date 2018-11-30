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
    public class FlujoProcesoTraspasoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrTraId { get; set; }
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
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120697,
                Operacion = 17,
            };

            entrada.agregaElemento("PrTraId", Datos.PrTraId);
            
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();
            
            try
            {

                if (respuesta.Resultado == "1")
            {
                DTLista = respuesta.obtieneTabla("FlujoTraspaso");

                int NumOCVobo = DTLista.Rows.Count;

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                foreach (DataRow row in DTLista.Rows)
                {
                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        Proceso = Convert.ToString(row["Proceso"]),
                        IdResponsable = Convert.ToString(row["IdResponsable"]),
                        Responsable = Convert.ToString(row["Responsable"]),
                        Terminado = Convert.ToString(row["Terminado"]),
                        Usuario = Convert.ToString(row["Usuario"]),
                        Importe = Convert.ToString(row["Importe"]),
                        Alterno = Convert.ToString(row["Alterno"]),
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
                    Responsable = Convert.ToString("no encontro ningun registro"),
                    
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

                    Responsable = ex.ToString()

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
