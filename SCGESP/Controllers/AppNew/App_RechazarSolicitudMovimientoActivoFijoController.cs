using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class App_RechazarSolicitudMovimientoActivoFijoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string AfVarID { get; set; }
            public string AfVreMotivoRechazo { get; set; }
        }
        //Parametros Salida
        public class ObtieneParametrosSalida
        {
        }
        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        //public XmlDocument Post(ParametrosEntrada Datos)
        public DocumentoSalida Post(ParametrosEntrada Datos)
        {
           // string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120341,
                Operacion = 10,
            };

            entrada.agregaElemento("AfVarID", Datos.AfVarID);
            entrada.agregaElemento("AfVreMotivoRechazo", Datos.AfVreMotivoRechazo);
          
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