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
    //AutorizarSolicitudCambio
    public class App_AutorizaCambioPrespupuestoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPtiAnio { get; set; }
            public string PrPtiFolio { get; set; }
            public string PrPtiEstatus { get; set; }
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public DocumentoSalida Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120623,
                Operacion = 13,
            };
            
            entrada.agregaElemento("PrPtiAnio", Datos.PrPtiAnio);
            entrada.agregaElemento("PrPtiFolio", Datos.PrPtiFolio);
            entrada.agregaElemento("PrPtiEstatus", Datos.PrPtiEstatus);

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
