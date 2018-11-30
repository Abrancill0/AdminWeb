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
    public class RechazarTraspasoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }

            public string PrTraId { get; set; }
            public string PrTraEstatus { get; set; }
            public string PrTraMotivoRechazo { get; set; }
            
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public DocumentoSalida Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120402,
                Operacion = 14,
            };

            entrada.agregaElemento("PrTraId", Datos.PrTraId);
            entrada.agregaElemento("PrTraEstatus", Datos.PrTraEstatus);
            entrada.agregaElemento("MotivoRechazo", Datos.PrTraMotivoRechazo);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();

            try
            {

                if (respuesta.Resultado == "1")
                {
                    return respuesta;
                }
                else
                {
                    //var errores = respuesta.Errores;

                    return respuesta;
                }
            }
            catch (Exception ex)
            {


                return respuesta;
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
