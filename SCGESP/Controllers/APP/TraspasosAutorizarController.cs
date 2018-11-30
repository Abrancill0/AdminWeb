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
    public class TraspasosAutorizarController : ApiController
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
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120697,
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
                        PrTraId = Convert.ToString(row["PrTraId"]), 
                        PrTraEstatus = Convert.ToString(row["PrTraEstatus"]),
                        PrTraFecha = Convert.ToString(row["PrTraFecha"]), 
                        PrTraReferencia = Convert.ToString(row["PrTraReferencia"]),
                        PrTraComentario = Convert.ToString(row["PrTraComentario"]),
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
                    PrTraComentario = Convert.ToString("no encontro ningun registro"),
                    
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

                    PrTraComentario = ex.ToString()

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
