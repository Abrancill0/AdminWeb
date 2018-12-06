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
    public class AutorizaSolicitudCambioCentroController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string FiCscSolicitud { get; set; }
            public string FiCscEstatus { get; set; }
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public DocumentoSalida Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120402,
                Operacion = 10,
            };

            entrada.agregaElemento("FiCscSolicitud", Datos.FiCscSolicitud);
            entrada.agregaElemento("FiCscEstatus", Datos.FiCscEstatus);

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
